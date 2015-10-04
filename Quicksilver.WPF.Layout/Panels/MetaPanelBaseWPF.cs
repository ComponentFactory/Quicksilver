// *****************************************************************************
// 
//  © Component Factory Pty Ltd 2011. All rights reserved.
//	The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, 17/267 Nepean Hwy, 
//  Seaford, Vic 3198, Australia and are supplied subject to licence terms.
// 
//  Version 1.0.8.0 	www.ComponentFactory.com
// *****************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;

namespace ComponentFactory.Quicksilver.Layout
{
    /// <summary>
    /// Base class of panel that has polymorphic layout/animate strategy.
    /// </summary>
    public partial class MetaPanelBase : Panel
    {
        #region Static Fields
        private static PropertyInfo _piIDB;
        #endregion

        #region Instance Fields
        private MetaElementCollection _children;
        private bool _ignoreVisualChange;
        #endregion

        #region Public
        /// <summary>
        /// Adds the provided object to the logical tree of this element.
        /// </summary>
        /// <param name="child">Child element to be added.</param>
        public void LogicalChildAdd(object child)
        {
            AddLogicalChild(child);
        }

        /// <summary>
        /// Removes the provided object from the logical tree of this element.
        /// </summary>
        /// <param name="child">Child element to be removed.</param>
        public void LogicalChildRemove(object child)
        {
            RemoveLogicalChild(child);
        }
        #endregion

        #region Protected
        /// <summary>
        /// Create the collection used to hold child elements.
        /// </summary>
        /// <param name="logicalParent">The logical parent element of the collection to be created.</param>
        /// <returns>An ordered collection of elements that have the specified logical parent.</returns>
        protected override UIElementCollection CreateUIElementCollection(FrameworkElement logicalParent)
        {
            // Use overriden version of the collection that does not remove children from 
            // the visual tree until they have finished being removed by optional animation.
            _children = new MetaElementCollection(this, logicalParent);
            _children.InternalIsItemsHost = IsItemsHost;

            // We need to know whenever an element is added or removed, to keep element state in sync
            _children.UIElementsAdded += new EventHandler<UIElementsEventArgs>(OnUIElementsAdded);
            _children.UIElementsRemove += new EventHandler<UIElementsEventArgs>(OnUIElementsRemove);
            return _children;
        }

        /// <summary>
        /// Gets the number of visual child elements within this element.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get
            {
                if (_children == null)
                    return 0;
                else
                    return _children.CompoundCount;
            }
        }

        /// <summary>
        /// Return child at the specified index from a collection of child elements.
        /// </summary>
        /// <param name="index">Zero-based index of the requested child element.</param>
        /// <returns>Requested child element.</returns>
        protected override Visual GetVisualChild(int index)
        {
            if (index >= _children.ExternalCount)
                return _children.GetInternalChild(index - _children.ExternalCount);
            else
                return _children[index];
        }

        /// <summary>
        /// Measure the layout size required to arrange all elements.
        /// </summary>
        /// <param name="availableSize">Available size that can be given to elements.</param>
        /// <returns>Size the layout determines it needs based on child element sizes.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            // To ensure correct operation when transitioning between being an items host and 
            // not being an items host (and vica versa) we need to ask for the InternalChildren 
            // property. The base class implementation will automatically handle creating the 
            // item container generator and pulling it down again.
            int c = InternalChildren.Count;

            // Delegate calculation to the panel layout instance
            if (_children != null)
            {
                // Use default layout so that any children not handled by the layout definitions is still processed
                _defaultLayout.MeasureChildren(string.Empty, this, _stateDict, _children.CompoundElements, availableSize);

                // Ask the layout strategy to measure the elements for us
                return Layouts.MeasureChildren(LayoutId, this, _stateDict, _children.CompoundElements, availableSize);
            }
            else
                return new Size();
        }

        /// <summary>
        /// Indicates that the IsItemsHost property value has changed.
        /// </summary>
        /// <param name="oldIsItemsHost">The old property value.</param>
        /// <param name="newIsItemsHost">The new property value.</param>
        protected override void OnIsItemsHostChanged(bool oldIsItemsHost, bool newIsItemsHost)
        {
            // Let base class do its own stuff
            base.OnIsItemsHostChanged(oldIsItemsHost, newIsItemsHost);

            // Update the element collection to reflect item host functionality
            if (_children != null)
                _children.InternalIsItemsHost = newIsItemsHost;
        }

        /// <summary>
        /// Invoked when the VisualCollection of a visual object is modified.
        /// </summary>
        /// <param name="visualAdded">The Visual that was added to the collection.</param>
        /// <param name="visualRemoved">The Visual that was removed from the collection.</param>
        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            if (!_ignoreVisualChange)
            {
                // Let base class do its own stuff
                base.OnVisualChildrenChanged(visualAdded, visualRemoved);

                // When an items host we do not get the UIElementAdded/Removed events 
                // for changing the element state dictionary. So we do it here instead.
                if (IsItemsHost)
                {
                    if (visualAdded is UIElement)
                    {
                        UIElement elementAdded = (UIElement)visualAdded;
                        _stateDict.Add(elementAdded, new MetaElementState(elementAdded));
                    }
                    
                    if (visualRemoved is UIElement)
                    {
                        UIElement elementRemoved = (UIElement)visualRemoved;
                        MetaElementState elementState = _stateDict[elementRemoved];

                        // If item has finished its removal
                        if (elementState.Status == MetaElementStatus.Removing)
                        {
                            // Base class already removed it as a visual/logical child so just remove dictionary entry
                            _stateDict.Remove(elementRemoved);
                        }
                        else
                        {
                            // Item needs marking so it removal animates
                            elementState.Status = MetaElementStatus.Removing;
                            elementState.TargetChanged = true;
                            
                            // Prevent reentrancy from trying to process the element being added back again
                            _ignoreVisualChange = true;

                            // Add into the internal collection and add back as a visual child
                            _children.InternalAdd(elementRemoved);
                            _ignoreVisualChange = false;
                        }
                    }
                }
            }
        }
        #endregion

        #region Internal
        internal bool IsDataBound
        {
            get
            {
                if (_piIDB == null)
                {
                    _piIDB = typeof(Panel).GetProperty("IsDataBound",
                                                        BindingFlags.Instance |
                                                        BindingFlags.GetProperty |
                                                        BindingFlags.NonPublic);
                }

                return (bool)_piIDB.GetValue(this, null);
            }
        }
        #endregion

        #region Private
        private void PlatformConstructor()
        {
            _ignoreVisualChange = false;
        }

        private void ImplementClipToBounds(Size finalSize)
        {
            // Do nothing as WPF implement ClipToBounds as a property on Panel
        }

        private ICollection MetaChildren
        {
            get
            {
                // In WPF we need to return the compound collection that includes the internal and 
                // external set of children all aggregated together to look like a single collection.
                return _children.CompoundElements;
            }
        }

        private void RemoveChildElement(UIElement element)
        {
            _children.InternalRemove(element);
        }

        private void OnUIElementsAdded(object sender, UIElementsEventArgs e)
        {
            foreach (UIElement element in e.Elements)
                _stateDict.Add(element, new MetaElementState(element));
        }

        private void OnUIElementsRemove(object sender, UIElementsEventArgs e)
        {
            foreach (UIElement element in e.Elements)
            {
                MetaElementState elementState = _stateDict[element];
                elementState.Status = MetaElementStatus.Removing;
                elementState.TargetChanged = true;
            }

            // If we are animating the removal of an element then it might not cause a
            // measure to occur because we do not actually remove it from the visual
            // collection. So force measure here so animation will be started.
            InvalidateMeasure();
        }
        #endregion
    }
}

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
using System.Net;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ComponentFactory.Quicksilver.Layout
{
    /// <summary>
    /// Base class of panel that has polymorphic layout/animate strategy.
    /// </summary>
    public partial class MetaPanelBase : Panel
    {
        #region Dependancy Properties
        /// <summary>
        /// Identifies the ClipToBounds dependency property.
        /// </summary>
        public static readonly DependencyProperty ClipToBoundsProperty;
        #endregion

        #region Instance Fields
        private RectangleGeometry _clipRectGeometry;
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets a value that determines if children are clipped to the bounds of the MetaPanelBase.
        /// </summary>
        public bool ClipToBounds
        {
            get { return (bool)GetValue(ClipToBoundsProperty); }
            set { SetValue(ClipToBoundsProperty, value); }
        }

        /// <summary>
        /// Adds the provided object to the logical tree of this element.
        /// </summary>
        /// <param name="child">Child element to be added.</param>
        public void LogicalChildAdd(object child)
        {
            // Silverlight does not have a logical tree. The visual tree is traversed by using
            // the Parent property on the FrameworkElement but it is impossible to set this value
            // and so we cannot hook up the element.
        }

        /// <summary>
        /// Removes the provided object from the logical tree of this element.
        /// </summary>
        /// <param name="child">Child element to be removed.</param>
        public void LogicalChildRemove(object child)
        {
            // See 'LogicalChildAdd'
        }
        #endregion

        #region Protected
        /// <summary>
        /// Measure the layout size required to arrange all elements.
        /// </summary>
        /// <param name="availableSize">Available size that can be given to elements.</param>
        /// <returns>Size the layout determines it needs based on child element sizes.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            // The collection of children might have changed, so resync
            ResyncStateDictionary();

            // Use default layout so that any children not handled by the layout definitions is still processed
            _defaultLayout.MeasureChildren(string.Empty, this, _stateDict, Children, availableSize);

            // Ask the layout strategy to measure the elements for us
            return Layouts.MeasureChildren(LayoutId, this, _stateDict, Children, availableSize);
        }
        #endregion

        #region Private
        private void PlatformConstructor()
        {
        }

        private void ImplementClipToBounds(Size finalSize)
        {
            if (ClipToBounds)
            {
                if (_clipRectGeometry == null)
                    _clipRectGeometry = new RectangleGeometry();

                _clipRectGeometry.Rect = new Rect(Utility.PointZero, finalSize);
                Clip = _clipRectGeometry;
            }
            else
                Clip = null;
        }

        private ICollection MetaChildren
        {
            get
            {
                // In Silverlight we simply return the actual child collection of elements.
                return Children;
            }
        }

        private void RemoveChildElement(UIElement element)
        {
            Children.Remove(element);
        }

        private void ResyncStateDictionary()
        {
            MetaElementStateDict newStateDict = new MetaElementStateDict();

            // Transfer existing state to new dictionary and create new state for new elements
            foreach (UIElement element in Children)
            {
                if (_stateDict.ContainsKey(element))
                    newStateDict.Add(element, _stateDict[element]);
                else
                    newStateDict.Add(element, new MetaElementState(element));
            }

            // Any removed elements will not have state in new dictionary
            _stateDict = newStateDict;
        }
        #endregion
    }
}

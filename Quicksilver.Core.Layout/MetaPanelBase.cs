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

namespace ComponentFactory.Quicksilver.Layout
{
    /// <summary>
    /// Base class of panel that has polymorphic layout/animate strategy.
    /// </summary>
    public abstract partial class MetaPanelBase : Panel, ILogicalParent
    {
        #region Dependancy Properties
        /// <summary>
        /// Identifies the LayoutId dependency property.
        /// </summary>
        public static readonly DependencyProperty LayoutIdProperty;

        /// <summary>
        /// Identifies the AnimateId dependency property.
        /// </summary>
        public static readonly DependencyProperty AnimateIdProperty;

        /// <summary>
        /// Identifies the IsAnimatingProperty dependency property.
        /// </summary>
        public static readonly DependencyProperty IsAnimatingProperty;

        /// <summary>
        /// Identifies the DisableOnElementCount dependency property.
        /// </summary>
        public static readonly DependencyProperty DisableOnElementCountProperty;
        #endregion

        #region Instance Fields
        private LayoutCollection _layoutCollection;
        private AnimateDefinitions _animateCollection;
        private MetaElementStateDict _stateDict;
        private MetaElementStateList _removeList;
        private StretchLayout _defaultLayout;
        private NullAnimate _defaultAnimate;
        private long _lastTicks;
        private long _nextTicks;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the IsAnimating property has changed in value.
        /// </summary>
        public event EventHandler IsAnimatingChanged;
        #endregion

        #region Identity
        static MetaPanelBase()
        {
            LayoutIdProperty = DependencyProperty.Register("LayoutId", 
                                                           typeof(string),
                                                           typeof(MetaPanelBase), 
                                                           new PropertyMetadata(string.Empty,
                                                           new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            AnimateIdProperty = DependencyProperty.Register("AnimateId",
                                                            typeof(string),
                                                            typeof(MetaPanelBase),
                                                            new PropertyMetadata(string.Empty,
                                                            new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            IsAnimatingProperty = DependencyProperty.Register("IsAnimating",
                                                              typeof(bool),
                                                              typeof(MetaPanelBase),
                                                              new PropertyMetadata(false,
                                                              new PropertyChangedCallback(OnIsAnimatingChanged)));

            DisableOnElementCountProperty = DependencyProperty.Register("DisableOnElementCount",
                                                                        typeof(int),
                                                                        typeof(MetaPanelBase),
                                                                        new PropertyMetadata(int.MaxValue));

            #if SILVERLIGHT
            ClipToBoundsProperty = DependencyProperty.Register("ClipToBounds",
                                                               typeof(bool),
                                                               typeof(MetaPanelBase),
                                                               new PropertyMetadata(false,
                                                               new PropertyChangedCallback(OnNeedMeasureOnChanged)));
            #endif
        }

        /// <summary>
        /// Initialize a new instance of the MetaPanelBase class.
        /// </summary>
        public MetaPanelBase()
        {
            // List used to collect entries that are to be removed from panel
            _removeList = new MetaElementStateList();

            // Use dictionary to associate each element with its animation state
            _stateDict = new MetaElementStateDict();

            // Create and monitor changes in the layout collection
            _layoutCollection = new LayoutCollection(this);
            _layoutCollection.NeedMeasure += new EventHandler(OnNeedMeasure);
            _layoutCollection.CollectionChanged += new EventHandler(OnNeedMeasure);
            MonitorExtendElement(_layoutCollection);

            // Create and monitor changes in the animate collection
            _animateCollection = new AnimateDefinitions(this);
            _animateCollection.NeedMeasure += new EventHandler(OnNeedMeasure);
            MonitorExtendElement(_animateCollection);

            // Default layout/animate to be applied as the default in case collections are empty or not applied
            _defaultLayout = new StretchLayout();
            _defaultAnimate = new NullAnimate();

            // Let the Silverlight/WPF specific construction take place
            PlatformConstructor();
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets or sets the identifier to used for layout.
        /// </summary>
        public string LayoutId
        {
            get { return (string)GetValue(LayoutIdProperty); }
            set { SetValue(LayoutIdProperty, value); }
        }

        /// <summary>
        /// Gets or sets the identifier to used for layout.
        /// </summary>
        public string AnimateId
        {
            get { return (string)GetValue(AnimateIdProperty); }
            set { SetValue(AnimateIdProperty, value); }
        }

        /// <summary>
        /// Gets a value indicating if the panel is currently animating the child elements.
        /// </summary>
        public bool IsAnimating
        {
            get { return (bool)GetValue(IsAnimatingProperty); }
            protected set { SetValue(IsAnimatingProperty, value); }
        }

        /// <summary>
        /// Gets and sets the element count that when exceeded causes animation to be disabled.
        /// </summary>
        public int DisableOnElementCount
        {
            get { return (int)GetValue(DisableOnElementCountProperty); }
            set { SetValue(DisableOnElementCountProperty, value); }
        }

        /// <summary>
        /// Perform remove animation for the child and then remove from the children.
        /// </summary>
        /// <param name="element">Element to be removed.</param>
        public void RemoveChild(UIElement element)
        {
            // Check the element is in the collection
            if (_stateDict.ContainsKey(element))
            {
                // If the element is not already marked to be removed...
                MetaElementState elementState = _stateDict[element];
                if (elementState.Status != MetaElementStatus.Removing)
                {
                    // Mark element to be removed
                    elementState.Status = MetaElementStatus.Removing;
                    elementState.TargetChanged = true;

                    // Need to measure to force remove animation
                    InvalidateMeasure();
                }
            }
        }

        /// <summary>
        /// Perform remove animation for all the child elements and then remove from children.
        /// </summary>
        public void ClearChildren()
        {
            // Check the state of each child in turn
            foreach (KeyValuePair<UIElement, MetaElementState> pair in _stateDict)
            {
                if (pair.Value.Status != MetaElementStatus.Removing)
                {
                    // Mark element to be removed
                    pair.Value.Status = MetaElementStatus.Removing;
                    pair.Value.TargetChanged = true;
                }
            }

            // Need to measure to force remove animation
            InvalidateMeasure();
        }
        #endregion

        #region Protected
        /// <summary>
        /// Gets and sets the collection of layout strategies used for child elements.
        /// </summary>
        protected LayoutCollection Layouts
        {
            get { return _layoutCollection; }
        }

        /// <summary>
        /// Gets and sets the collection of animate strategies used for child elements.
        /// </summary>
        protected AnimateDefinitions Animates
        {
            get { return _animateCollection; }
        }

        /// <summary>
        /// Position child elements according to already provided size.
        /// </summary>
        /// <param name="finalSize">Size that layout should use to arrange child elements.</param>
        /// <returns>Size used by the panel.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            // Implement any ClipToBounds property setting
            ImplementClipToBounds(finalSize);

            // Grab the collection of appropriate elements
            ICollection elements = MetaChildren;

            // Ask the panel layout to calculate the target display rectangles for elements
            Layouts.TargetChildren(LayoutId, this, _stateDict, elements, finalSize);

            // Ask the chain of animation handlers to update animation state and elements
            bool needAnimating = AnimateChildren(_stateDict, elements);

            // Faster perf to only set the IsAnimating when we notice a change
            if (needAnimating != IsAnimating)
                IsAnimating = needAnimating;

            // Finally ask the panel layout to arrange elements in animation determined positions
            return Layouts.ArrangeChildren(LayoutId, this, _stateDict, elements, finalSize);
        }

        /// <summary>
        /// Invoked when a property change requires a measure to occur.
        /// </summary>
        /// <param name="d">Owning object.</param>
        /// <param name="e">Details of property that has changed.</param>
        protected static void OnNeedMeasureOnChanged(DependencyObject d,
                                                     DependencyPropertyChangedEventArgs e)
        {
            MetaPanelBase sender = (MetaPanelBase)d;
            sender.OnNeedMeasure(sender, EventArgs.Empty);
        }
        #endregion

        #region Private
        private bool IsAnimationAllowed
        {
            get
            {
                return (MetaChildren.Count < DisableOnElementCount);
            }
        }

        private bool AnimateChildren(MetaElementStateDict stateDict, ICollection elements)
        {
            // Find number of milliseconds elapsed since last animation calculation, note that if not 
            // currently animating then the elapsed time since the last animation cycle must be 0.
            double elapsedMilliseconds = (!IsAnimating || (_lastTicks == -1) ? 0 : (_nextTicks - _lastTicks) / (double)(10000));

            if (IsAnimationAllowed)
            {
                // Ask the chain of animation classes to apply any required movement changes
                Animates.ApplyAnimation(AnimateId, this, stateDict, elements, elapsedMilliseconds);
            }

            // Post-process on animation state
            bool moreAnimation = false;
            foreach (MetaElementState elementState in stateDict.Values)
            {
                // At end of animation cycle the new/remove values must have been calculated
                elementState.NewCalculating = false;
                if (elementState.Status == MetaElementStatus.Removing)
                    elementState.RemoveCalculated = true;

                // Has the element finished being removed?
                if (elementState.AnimateComplete && (elementState.Status == MetaElementStatus.Removing))
                    _removeList.Add(elementState);
                else
                {
                    // If the element has not finished being animated
                    if (!elementState.AnimateComplete)
                    {
                        // Always reset to being completed, ready for next cycle
                        elementState.AnimateComplete = true;
                        moreAnimation = true;
                    }
                    else
                    {
                        // Has element finished being 'new'?
                        if (elementState.Status == MetaElementStatus.New)
                            elementState.Status = MetaElementStatus.Existing;

                        // Animation is completed so ensure the current rect is same as the target
                        // rect. This ensures that if there are no animation classes actually doing
                        // size/position animation then the child elements actually do get positioned.
                        elementState.CurrentRect = elementState.TargetRect;
                    }
                }

                // Reset the target changed flag
                elementState.TargetChanged = false;

                // Never allow the current rect to be empty
                if (elementState.CurrentRect.IsEmpty)
                    elementState.CurrentRect = elementState.TargetRect;
            }

            // Process the removal list
            foreach (MetaElementState elementState in _removeList)
            {
                RemoveChildElement(elementState.Element);
                stateDict.Remove(elementState.Element);
                moreAnimation = true;
            }

            // Must clear list to prevent dangling references to the removed UIElement instances
            _removeList.Clear();

            return moreAnimation;
        }

        private void MonitorExtendElement(object element)
        {
            if (element != null)
            {
                MeasureElement extend = element as MeasureElement;
                if (extend != null)
                {
                    LogicalChildAdd(extend);
                    extend.NeedMeasure += new EventHandler(OnNeedMeasure);
                }
            }
        }

        private void UnmonitorExtendElement(object element)
        {
            if (element != null)
            {
                MeasureElement extend = element as MeasureElement;
                if (extend != null)
                {
                    extend.NeedMeasure -= new EventHandler(OnNeedMeasure);
                    LogicalChildRemove(extend);
                }
            }
        }

        private void OnNeedMeasure(object sender, EventArgs e)
        {
            InvalidateMeasure();
        }

        private void OnRendering(object sender, EventArgs e)
        {
            RenderingEventArgs rea = (RenderingEventArgs)e;

            // The first rendering call does not have a 'last' ticks count
            if (_lastTicks == -1)
                _lastTicks = rea.RenderingTime.Ticks;
            else
                _lastTicks = _nextTicks;

            // Record the time when the next frame will be displayed
            _nextTicks = rea.RenderingTime.Ticks;

            InvalidateMeasure();
        }

        private static void OnIsAnimatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MetaPanelBase metaPanelBase = (MetaPanelBase)d;

            // By always requesting a panel measure when the Rendering event is
            // fired we ensure that the panel invokes the animation instances and
            // the child elements are updated until the animation ends.
            if ((bool)e.NewValue)
            {
                // Reset the last ticks as we do not want to use the duration since the
                // last time animation finished, which might have been a long time ago
                metaPanelBase._lastTicks = -1;

                // This event is fired just before rendering of each display frame
                CompositionTarget.Rendering += new EventHandler(metaPanelBase.OnRendering);
            }
            else
                CompositionTarget.Rendering -= new EventHandler(metaPanelBase.OnRendering);

            // Raise the changed event
            EventHandler handler = metaPanelBase.IsAnimatingChanged;
            if (handler != null)
                handler(metaPanelBase, EventArgs.Empty);
        }
        #endregion
    }
}

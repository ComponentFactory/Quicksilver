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
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

namespace ComponentFactory.Quicksilver.Layout
{
    /// <summary>
    /// Positions elements in sequential order from left to right and line break when reaching the far edge.
    /// </summary>
    public class WrapLayout : Layout
    {
        #region Dependancy Properties
        /// <summary>
        /// Identifies the ItemWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemWidthProperty;

        /// <summary>
        /// Identifies the ItemHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty;

        /// <summary>
        /// Identifies the Orientation dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty;

        /// <summary>
        /// Identifies the Break dependency property.
        /// </summary>
        public static readonly DependencyProperty BreakAfterProperty;
        #endregion

        #region Identity
        static WrapLayout()
        {
            ItemWidthProperty = DependencyProperty.Register("ItemWidth", 
                                                            typeof(double), 
                                                            typeof(WrapLayout), 
                                                            new PropertyMetadata(double.NaN, 
                                                            new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            ItemHeightProperty = DependencyProperty.Register("ItemHeight",
                                                             typeof(double),
                                                             typeof(WrapLayout),
                                                             new PropertyMetadata(double.NaN,
                                                             new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            OrientationProperty = DependencyProperty.Register("Orientation",
                                                             typeof(Orientation),
                                                             typeof(WrapLayout),
                                                             new PropertyMetadata(Orientation.Horizontal,
                                                             new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            BreakAfterProperty = DependencyProperty.RegisterAttached("BreakAfter",
                                                                     typeof(bool),
                                                                     typeof(WrapLayout),
                                                                     new PropertyMetadata(false,
                                                                     new PropertyChangedCallback(OnWrapChanged)));
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets or sets a value that specifies the width of all items that are contained within a WrapPanel.
        /// </summary>
        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the height of all items that are contained within a WrapPanel.
        /// </summary>
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the dimension in which child content is arranged.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Sets the BreakAfter attached property of the provided element.
        /// </summary>
        /// <param name="element">Target element.</param>
        /// <param name="value">BreakAfter value.</param>
        public static void SetBreakAfter(UIElement element, bool value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            else
                element.SetValue(BreakAfterProperty, value);
        }

        /// <summary>
        /// Gets the BreakAfter attached property of the provided element.
        /// </summary>
        /// <param name="element">Target element.</param>
        /// <returns>BreakAfter value.</returns>
        public static bool GetBreakAfter(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            else
                return (bool)element.GetValue(BreakAfterProperty);
        }

        /// <summary>
        /// Measure the layout size required to arrange all elements.
        /// </summary>
        /// <param name="layoutId">Identifier of the layout to be used.</param>
        /// <param name="metaPanel">Reference to owning panel instance.</param>
        /// <param name="stateDict">Dictionary of per-element state.</param>
        /// <param name="elements">Collection of elements to be measured.</param>
        /// <param name="availableSize">Available size that can be given to elements.</param>
        /// <returns>Size the layout determines it needs based on child element sizes.</returns>
        public override Size MeasureChildren(string layoutId,
                                             MetaPanelBase metaPanel,
                                             MetaElementStateDict stateDict,
                                             ICollection elements,
                                             Size availableSize)
        {
            // Only apply if we match the incoming layout identifier
            if (string.IsNullOrEmpty(Id) || Id.Equals(layoutId))
            {
                // Cache the dependancy properties for faster perf
                double itemWidth = ItemWidth;
                double itemHeight = ItemHeight;
                Orientation orientation = Orientation;

                // Cache if the defined sizes are valid
                bool itemWidthDefined = !double.IsNaN(ItemWidth);
                bool itemHeightDefined = !double.IsNaN(itemHeight);

                // Find size we provide to each child for measuring against, we use item sizes if defined
                Size elementSize = new Size(itemWidthDefined ? itemWidth : double.PositiveInfinity,
                                            itemHeightDefined ? itemHeight : double.PositiveInfinity);

                // Measure each element in turn
                bool breakAfter = false;
                double lineMax = 0;
                double lineOffset = 0;
                Size measureSize = new Size();
                foreach (UIElement element in elements)
                {
                    element.Measure(elementSize);

                    // We ignore items being removed
                    if (stateDict[element].Status != MetaElementStatus.Removing)
                    {
                        // Decide on the actual size we will allocate to the element
                        Size childSize = new Size(itemWidthDefined ? itemWidth : element.DesiredSize.Width,
                                                  itemHeightDefined ? itemHeight : element.DesiredSize.Height);

                        // Calculation depends on orientation
                        if (orientation == Orientation.Horizontal)
                        {
                            // Does this element overflow the line?
                            if (breakAfter || (((lineOffset + childSize.Width) > availableSize.Width) && !measureSize.IsEmpty))
                            {
                                // Total measured size is equal to the widest line and tallest item per line
                                measureSize.Width = Math.Max(measureSize.Width, lineOffset);
                                measureSize.Height += lineMax;

                                // Start at the left edge of the next line
                                lineOffset = 0;
                                lineMax = 0;
                            }

                            // Position the child on the current line
                            lineOffset += childSize.Width;
                            lineMax = Math.Max(lineMax, childSize.Height);
                        }
                        else
                        {
                            // Does this element overflow the line?
                            if (breakAfter || (((lineOffset + childSize.Height) > availableSize.Height) && !measureSize.IsEmpty))
                            {
                                // Total measured size is equal to the tallest line and widest item per line
                                measureSize.Height = Math.Max(measureSize.Height, lineOffset);
                                measureSize.Width += lineMax;

                                // Start at the top edge of the next line
                                lineOffset = 0;
                                lineMax = 0;
                            }

                            // Position the child on the current line
                            lineOffset += childSize.Height;
                            lineMax = Math.Max(lineMax, childSize.Width);
                        }

                        // Do we perform a line break after this element
                        breakAfter = WrapLayout.GetBreakAfter(element);
                    }
                }

                // Remember to take into account the last line
                if (orientation == Orientation.Horizontal)
                {
                    // Total measured size is equal to the widest line and tallest item per line
                    measureSize.Width = Math.Max(measureSize.Width, lineOffset);
                    measureSize.Height += lineMax;
                }
                else
                {
                    // Total measured size is equal to the tallest line and widest item per line
                    measureSize.Height = Math.Max(measureSize.Height, lineOffset);
                    measureSize.Width += lineMax;
                }

                // Return minimum size needed to contain all the elements according to their desired sizes
                return measureSize;
            }
            else
                return Size.Empty;
        }

        /// <summary>
        /// Calculate target state for each element based on layout algorithm.
        /// </summary>
        /// <param name="layoutId">Identifier of the layout to be used.</param>
        /// <param name="metaPanel">Reference to owning panel instance.</param>
        /// <param name="stateDict">Dictionary of per-element state.</param>
        /// <param name="elements">Collection of elements to be arranged.</param>
        /// <param name="finalSize">Size that layout should use to arrange child elements.</param>
        public override void TargetChildren(string layoutId,
                                            MetaPanelBase metaPanel,
                                            MetaElementStateDict stateDict,
                                            ICollection elements,
                                            Size finalSize)
        {
            // Only apply if we match the incoming layout identifier
            if (string.IsNullOrEmpty(Id) || Id.Equals(layoutId))
            {
                // Cache the dependancy properties for faster perf
                double itemWidth = ItemWidth;
                double itemHeight = ItemHeight;
                Orientation orientation = Orientation;

                // Cache if the defined sizes are valid
                bool itemWidthDefined = !double.IsNaN(ItemWidth);
                bool itemHeightDefined = !double.IsNaN(itemHeight);

                // Find size we provide to each child for measuring against, we use item sizes if defined
                Size elementSize = new Size(itemWidthDefined ? itemWidth : double.PositiveInfinity,
                                            itemHeightDefined ? itemHeight : double.PositiveInfinity);

                // Measure each element in turn
                int current = 0;
                double lineOffset = 0;
                double lineMax = 0;
                double positionOffset = 0;
                bool breakAfter = false;
                List<UIElement> targets = new List<UIElement>();
                foreach (UIElement element in elements)
                {
                    // We ignore items being removed
                    if (stateDict[element].Status != MetaElementStatus.Removing)
                    {
                        // Decide on the actual size we will allocate to the element
                        Size childSize = new Size(itemWidthDefined ? itemWidth : element.DesiredSize.Width,
                                                  itemHeightDefined ? itemHeight : element.DesiredSize.Height);

                        // Calculation depends on orientation
                        if (orientation == Orientation.Horizontal)
                        {
                            // Does this element overflow the line?
                            if (breakAfter || (((lineOffset + childSize.Width) > finalSize.Width) && (current > 0)))
                            {
                                // Create position targets for items on this line
                                TargetLine(stateDict, targets,
                                           positionOffset, lineMax,
                                           itemWidthDefined, itemHeightDefined,
                                           itemWidth, itemHeight,
                                           finalSize, orientation);

                                // Move positioning down by height of the line
                                positionOffset += lineMax;

                                // Start at the left edge of the next line
                                lineOffset = 0;
                                lineMax = 0;
                                targets.Clear();
                            }

                            // Position the child on the current line
                            lineOffset += childSize.Width;
                            lineMax = Math.Max(lineMax, childSize.Height);
                        }
                        else
                        {
                            // Does this element overflow the line?
                            if (breakAfter || (((lineOffset + childSize.Height) > finalSize.Height) && (current > 0)))
                            {
                                // Create position targets for items on this line
                                TargetLine(stateDict, targets,
                                           positionOffset, lineMax,
                                           itemWidthDefined, itemHeightDefined,
                                           itemWidth, itemHeight,
                                           finalSize, orientation);

                                // Move positioning down by height of the line
                                positionOffset += lineMax;

                                // Start at the left edge of the next line
                                lineOffset = 0;
                                lineMax = 0;
                                targets.Clear();
                            }

                            // Position the child on the current line
                            lineOffset += childSize.Height;
                            lineMax = Math.Max(lineMax, childSize.Width);
                        }

                        targets.Add(element);
                        current++;
                    
                        // Do we perform a line break after this element
                        breakAfter = WrapLayout.GetBreakAfter(element);
                    }
                }

                // Remember to take into account the last line
                TargetLine(stateDict, targets,
                           positionOffset, lineMax,
                           itemWidthDefined, itemHeightDefined,
                           itemWidth, itemHeight,
                           finalSize, orientation);
            }
        }
        #endregion

        #region Private
        private void TargetLine(MetaElementStateDict stateDict,
                                List<UIElement> targets,
                                double positionOffset,
                                double lineMax,
                                bool itemWidthDefined,
                                bool itemHeightDefined,
                                double itemWidth,
                                double itemHeight,
                                Size finalSize,
                                Orientation orientation)
        {
            double lineOffset = 0;
            foreach (UIElement element in targets)
            {
                // Decide on the actual size we will allocate to the element
                Size childSize = new Size(itemWidthDefined ? itemWidth : element.DesiredSize.Width,
                                          itemHeightDefined ? itemHeight : element.DesiredSize.Height);

                Rect newTargetRect;

                // Updating the elements target rectangle depends on orientation
                if (orientation == Orientation.Horizontal)
                {
                    newTargetRect = new Rect(lineOffset, positionOffset, childSize.Width, lineMax);
                    lineOffset += childSize.Width;
                }
                else
                {
                    newTargetRect = new Rect(positionOffset, lineOffset, lineMax, childSize.Height);
                    lineOffset += childSize.Height;
                }

                // Store the new target rectangle
                if (!stateDict[element].TargetRect.Equals(newTargetRect))
                {
                    stateDict[element].TargetChanged = true;
                    stateDict[element].TargetRect = newTargetRect;
                }
            }
        }

        private static void OnWrapChanged(DependencyObject d,
                                          DependencyPropertyChangedEventArgs e)
        {
            UIElement element = d as UIElement;
            if (element != null)
            {
                MetaPanelBase parent = VisualTreeHelper.GetParent(element) as MetaPanelBase;
                if (parent != null)
                    parent.InvalidateMeasure();
            }
        }
        #endregion
    }
}

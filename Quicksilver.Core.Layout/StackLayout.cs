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
    /// Arranges child elements into a single line that can be oriented horizontally or vertically. 
    /// </summary>
    public class StackLayout : Layout
    {
        #region Dependancy Properties
        /// <summary>
        /// Identifies the Orientation dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty;
        #endregion

        #region Identity
        static StackLayout()
        {
            OrientationProperty = DependencyProperty.Register("Orientation",
                                                              typeof(Orientation),
                                                              typeof(StackLayout),
                                                              new PropertyMetadata(Orientation.Vertical,
                                                              new PropertyChangedCallback(OnNeedMeasureOnChanged)));
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets or sets a value that indicates the dimension by which child elements are stacked.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
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
                Orientation orientation = Orientation;

                // Give children as much space as they want in the opposite direction to the stacking
                if (orientation == Orientation.Horizontal)
                    availableSize.Width = double.PositiveInfinity;
                else
                    availableSize.Height = double.PositiveInfinity;

                // Measure each element in turn
                Size measureSize = new Size();
                foreach (UIElement element in elements)
                {
                    element.Measure(availableSize);

                    // We ignore items being removed
                    if (stateDict[element].Status != MetaElementStatus.Removing)
                    {
                        // Accumulate in the orientation direction, but track maximum value in the opposite
                        if (orientation == Orientation.Horizontal)
                        {
                            measureSize.Width += element.DesiredSize.Width;
                            measureSize.Height = Math.Max(measureSize.Height, element.DesiredSize.Height);
                        }
                        else
                        {
                            measureSize.Width = Math.Max(measureSize.Width, element.DesiredSize.Width);
                            measureSize.Height += element.DesiredSize.Height;
                        }
                    }
                }

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
                Orientation orientation = Orientation;

                // Calculate the target rectangle for each element
                double offset = 0;
                foreach (UIElement element in elements)
                {
                    // We ignore items being removed
                    if (stateDict[element].Status != MetaElementStatus.Removing)
                    {
                        Rect newTargetRect;

                        // Updating the elements target rectangle depends on orientation
                        if (orientation == Orientation.Horizontal)
                        {
                            newTargetRect = new Rect(offset, 0, element.DesiredSize.Width, Math.Max(finalSize.Height, element.DesiredSize.Height));
                            offset += element.DesiredSize.Width;
                        }
                        else
                        {
                            newTargetRect = new Rect(0, offset, Math.Max(finalSize.Width, element.DesiredSize.Width), element.DesiredSize.Height);
                            offset += element.DesiredSize.Height;
                        }

                        // Store the new target rectangle
                        if (!stateDict[element].TargetRect.Equals(newTargetRect))
                        {
                            stateDict[element].TargetChanged = true;
                            stateDict[element].TargetRect = newTargetRect;
                        }
                    }
                }
            }
        }
        #endregion
    }
}

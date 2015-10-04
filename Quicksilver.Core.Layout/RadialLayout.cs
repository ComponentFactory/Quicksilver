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
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

namespace ComponentFactory.Quicksilver.Layout
{
    /// <summary>
    /// Arranges child elements in a radial pattern.
    /// </summary>
    public class RadialLayout : Layout
    {
        #region Dependancy Properties
        /// <summary>
        /// Identifies the StartAngle dependency property.
        /// </summary>
        public static readonly DependencyProperty StartAngleProperty;

        /// <summary>
        /// Identifies the EndAngle dependency property.
        /// </summary>
        public static readonly DependencyProperty EndAngleProperty;

        /// <summary>
        /// Identifies the Circle dependency property.
        /// </summary>
        public static readonly DependencyProperty CircleProperty;
        #endregion

        #region Instance Fields
        private double _count;
        private double _maxLength;
        #endregion

        #region Identity
        static RadialLayout()
        {
            StartAngleProperty = DependencyProperty.Register("StartAngle", 
                                                             typeof(double), 
                                                             typeof(RadialLayout), 
                                                             new PropertyMetadata(0.0, 
                                                             new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            EndAngleProperty = DependencyProperty.Register("EndAngle",
                                                            typeof(double),
                                                            typeof(RadialLayout),
                                                            new PropertyMetadata(360.0,
                                                            new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            CircleProperty = DependencyProperty.Register("Circle",
                                                         typeof(bool),
                                                         typeof(RadialLayout),
                                                         new PropertyMetadata(false,
                                                         new PropertyChangedCallback(OnNeedMeasureOnChanged)));
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets or sets the starting angle in degress.
        /// </summary>
        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ending angle in degress.
        /// </summary>
        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets if a circle is used or an ellipse that best fits the available space.
        /// </summary>
        public bool Circle
        {
            get { return (bool)GetValue(CircleProperty); }
            set { SetValue(CircleProperty, value); }
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
            // Measure each element in turn
            _count = 0;
            Size maxSize = new Size();
            foreach (UIElement element in elements)
                if (element != null)
                {
                    element.Measure(Utility.SizeInfinity);

                    // We ignore items being removed
                    if (stateDict[element].Status != MetaElementStatus.Removing)
                    {
                        // We ignore items that are collapsed
                        if (element.Visibility != Visibility.Collapsed)
                        {
                            // Find the widest and tallest element
                            maxSize.Width = Math.Max(maxSize.Width, element.DesiredSize.Width);
                            maxSize.Height = Math.Max(maxSize.Height, element.DesiredSize.Height);

                            // Count number of valid elements
                            _count++;
                        }
                    }
                }

            // Max length is the diagonal length of biggest element
            _maxLength = Math.Sqrt((maxSize.Width * maxSize.Width) + (maxSize.Height * maxSize.Height));

            // Always use the provided size
            return availableSize;
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
            if (_count > 0)
            {
                // Each child is an equal angle around the radius
                double angleCurrent = StartAngle;
                double diff = (EndAngle - StartAngle);
                double angleDelta = diff / ((diff == (double)360) ? _count : _count - 1);

                // Calculate the radius separately for each dimension
                double radiusX = (finalSize.Width - _maxLength * 2) / 2;
                double radiusY = (finalSize.Height - _maxLength * 2) / 2;

                // Do we force into being a circle?
                if (Circle)
                {
                    // Always reduce to using the smallest direction
                    radiusX = Math.Min(radiusX, radiusY);
                    radiusY = radiusX;
                }

                // Rotate around the center point
                Point center = new Point(finalSize.Width / 2, finalSize.Height / 2);

                // Calculate the target rectangle for each element
                foreach (UIElement element in elements)
                {
                    // We ignore items being removed
                    if (stateDict[element].Status != MetaElementStatus.Removing)
                    {
                        // We ignore items that are collapsed
                        if (element.Visibility != Visibility.Collapsed)
                        {
                            // Rotate around the center point using the accumulated angle
                            double childX = center.X + Math.Cos(2 * Math.PI * angleCurrent / 360) * radiusX;
                            double childY = center.Y + Math.Sin(2 * Math.PI * angleCurrent / 360) * radiusY;
                            angleCurrent += angleDelta;

                            // Position the element at
                            Size desiredSize = element.DesiredSize;
                            Rect newTargetRect = new Rect(childX - desiredSize.Width / 2,
                                                          childY - desiredSize.Height / 2,
                                                          desiredSize.Width,
                                                          desiredSize.Height);

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
        }
        #endregion
    }
}

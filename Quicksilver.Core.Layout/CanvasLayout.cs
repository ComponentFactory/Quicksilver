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
    /// Arranges child elements in fixed positions as provided by Canvas attached properties.
    /// </summary>
    public class CanvasLayout : Layout
    {
        #region Dependancy Properties
        /// <summary>
        /// Identifies the Left dependency property.
        /// </summary>
        public static readonly DependencyProperty LeftProperty;

        /// <summary>
        /// Identifies the Right dependency property.
        /// </summary>
        public static readonly DependencyProperty RightProperty;

        /// <summary>
        /// Identifies the Top dependency property.
        /// </summary>
        public static readonly DependencyProperty TopProperty;

        /// <summary>
        /// Identifies the Bottom dependency property.
        /// </summary>
        public static readonly DependencyProperty BottomProperty;
        #endregion

        #region Identity
        static CanvasLayout()
        {
            LeftProperty = DependencyProperty.RegisterAttached("Left", 
                                                               typeof(double), 
                                                               typeof(CanvasLayout),
                                                               new PropertyMetadata(double.NaN,
                                                               new PropertyChangedCallback(OnCanvasChanged)));

            RightProperty = DependencyProperty.RegisterAttached("Right", 
                                                                typeof(double), 
                                                                typeof(CanvasLayout),
                                                                new PropertyMetadata(double.NaN,
                                                                new PropertyChangedCallback(OnCanvasChanged)));
            
            TopProperty = DependencyProperty.RegisterAttached("Top", 
                                                              typeof(double), 
                                                              typeof(CanvasLayout),
                                                              new PropertyMetadata(double.NaN,
                                                              new PropertyChangedCallback(OnCanvasChanged)));
            
            BottomProperty = DependencyProperty.RegisterAttached("Bottom", 
                                                                 typeof(double), 
                                                                 typeof(CanvasLayout),
                                                                 new PropertyMetadata(double.NaN,
                                                                 new PropertyChangedCallback(OnCanvasChanged)));
        }
        #endregion

        #region Public
        /// <summary>
        /// Sets the Left attached property of the provided element.
        /// </summary>
        /// <param name="element">Target element.</param>
        /// <param name="value">Left value.</param>
        public static void SetLeft(UIElement element, double value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            else
                element.SetValue(LeftProperty, value);
        }

        /// <summary>
        /// Gets the Left attached property of the provided element.
        /// </summary>
        /// <param name="element">Target element.</param>
        /// <returns>Left value.</returns>
        #if !SILVERLIGHT       
        [AttachedPropertyBrowsableForChildren]
        #endif
        [TypeConverter(typeof(LengthConverter))]
        public static double GetLeft(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            else
                return (double)element.GetValue(LeftProperty);
        }

        /// <summary>
        /// Sets the Right attached property of the provided element.
        /// </summary>
        /// <param name="element">Target element.</param>
        /// <param name="value">Right value.</param>
        public static void SetRight(UIElement element, double value)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            else
                element.SetValue(RightProperty, value);
        }

        /// <summary>
        /// Gets the Right attached property of the provided element.
        /// </summary>
        /// <param name="element">Target element.</param>
        /// <returns>Right value.</returns>
        #if !SILVERLIGHT       
        [AttachedPropertyBrowsableForChildren]
        #endif
        [TypeConverter(typeof(LengthConverter))]
        public static double GetRight(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            else
                return (double)element.GetValue(RightProperty);
        }

        /// <summary>
        /// Sets the Top attached property of the provided element.
        /// </summary>
        /// <param name="element">Target element.</param>
        /// <param name="length">Top value.</param>
        public static void SetTop(UIElement element, double length)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            else
                element.SetValue(TopProperty, length);
        }

        /// <summary>
        /// Gets the Top attached property of the provided element.
        /// </summary>
        /// <param name="element">Target element.</param>
        /// <returns>Top value.</returns>
        #if !SILVERLIGHT       
        [AttachedPropertyBrowsableForChildren]
        #endif
        [TypeConverter(typeof(LengthConverter))]
        public static double GetTop(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            else
                return (double)element.GetValue(TopProperty);
        }

        /// <summary>
        /// Sets the Bottom attached property of the provided element.
        /// </summary>
        /// <param name="element">Target element.</param>
        /// <param name="length">Bottom value.</param>
        public static void SetBottom(UIElement element, double length)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            else
                element.SetValue(BottomProperty, length);
        }

        /// <summary>
        /// Gets the Bottom attached property of the provided element.
        /// </summary>
        /// <param name="element">Target element.</param>
        /// <returns>Bottom value.</returns>
        #if !SILVERLIGHT       
        [AttachedPropertyBrowsableForChildren]
        #endif
        [TypeConverter(typeof(LengthConverter))]
        public static double GetBottom(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            else
                return (double)element.GetValue(BottomProperty);
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
                // Measure each element in turn
                foreach (UIElement element in elements)
                    stateDict[element].Element.Measure(Utility.SizeInfinity);
            }

            // Return an empty size
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
                // Calculate the target rectangle for each element
                foreach (UIElement element in elements)
                {
                    // We ignore items being removed
                    if (stateDict[element].Status != MetaElementStatus.Removing)
                    {
                        // Default to being the desired size but at position zero,zero
                        Rect newTargetRect = new Rect(0, 0, element.DesiredSize.Width, element.DesiredSize.Height);

                        // Use the Left or Right value provided as attached properties of the element
                        double left = CanvasLayout.GetLeft(element);
                        if (!double.IsNaN(left))
                            newTargetRect.X = left;
                        else
                        {
                            double right = CanvasLayout.GetRight(element);
                            if (!double.IsNaN(right))
                                newTargetRect.X = (finalSize.Width - element.DesiredSize.Width) - right;
                        }

                        // Use the Top or Bottom value provided as attached properties of the element
                        double top = CanvasLayout.GetTop(element);
                        if (!double.IsNaN(top))
                            newTargetRect.Y = top;
                        else
                        {
                            double bottom = CanvasLayout.GetBottom(element);
                            if (!double.IsNaN(bottom))
                                newTargetRect.Y = (finalSize.Height - element.DesiredSize.Height) - bottom;
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

        #region Private
        private static void OnCanvasChanged(DependencyObject d,
                                            DependencyPropertyChangedEventArgs e)
        {
            UIElement element = d as UIElement;
            if (element != null)
            {
                MetaPanel parent = VisualTreeHelper.GetParent(element) as MetaPanel;
                if (parent != null)
                    parent.InvalidateMeasure();
            }
        }
        #endregion
    }
}

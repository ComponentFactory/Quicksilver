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
    /// Arranges child elements against the panel edges. 
    /// </summary>
    public partial class DockLayout : Layout
    {
        #region Dependancy Properties
        /// <summary>
        /// Identifies the LastChildFill dependency property.
        /// </summary>
        public static readonly DependencyProperty LastChildFillProperty;

        /// <summary>
        /// Identifies the Dock dependency property.
        /// </summary>
        public static readonly DependencyProperty DockProperty;
        #endregion

        #region Instance Fields
        private int _validCount;
        #endregion

        #region Public
        /// <summary>
        /// Sets the value of the Dock attached property to a specified element.
        /// </summary>
        /// <param name="element">The element to which the attached property is written.</param>
        /// <param name="dock">The new Dock value.</param>
        public static void SetDock(UIElement element, Dock dock)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            element.SetValue(DockProperty, dock);
        }

        /// <summary>
        /// Gets the value of the Dock attached property for a specified UIElement. 
        /// </summary>
        /// <param name="element">The element from which the property value is read.</param>
        /// <returns>The Dock property value for the element.</returns>
        #if !SILVERLIGHT
        [AttachedPropertyBrowsableForChildren]
        #endif
        public static Dock GetDock(UIElement element)
        {
            if (element == null)
                throw new ArgumentNullException("element");

            return (Dock)element.GetValue(DockProperty);
        }

        /// <summary>
        /// Gets or sets a value that indicating if the last element is used to fill the remainder space.
        /// </summary>
        public bool LastChildFill
        {
            get { return (bool)GetValue(LastChildFillProperty); }
            set { SetValue(LastChildFillProperty, value); }
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
                _validCount = 0;
                Size usedSize = new Size();
                Size maxSize = new Size();
                foreach (UIElement element in elements)
                {
                    // Allow element to have all the remainder size available
                    Size elementSize = new Size(Math.Max(0.0, (double)(availableSize.Width - usedSize.Width)),
                                                Math.Max(0.0, (double)(availableSize.Height - usedSize.Height)));

                    element.Measure(elementSize);

                    // We ignore items being removed
                    if (stateDict[element].Status != MetaElementStatus.Removing)
                    {
                        // Discover the used size and the maximum required size for opposite direction
                        Size desiredSize = element.DesiredSize;
                        switch (GetDock(element))
                        {
                            case Dock.Left:
                            case Dock.Right:
                                maxSize.Height = Math.Max(maxSize.Height, usedSize.Height + desiredSize.Height);
                                usedSize.Width += desiredSize.Width;
                                break;
                            case Dock.Top:
                            case Dock.Bottom:
                                maxSize.Width = Math.Max(maxSize.Width, usedSize.Width + desiredSize.Width);
                                usedSize.Height += desiredSize.Height;
                                break;
                        }

                        _validCount++;
                    }
                }

                return new Size(Math.Max(maxSize.Width, usedSize.Width),
                                Math.Max(maxSize.Height, usedSize.Height));
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
                double x = 0.0;
                double y = 0.0;
                double right = 0.0;
                double bottom = 0.0;

                // If using LastChildFill then the last element takes up all the remainder space
                int lastDockElement = (LastChildFill ? _validCount - 1 : _validCount);

                // Calculate the target rectangle for each element
                int i = 0;
                foreach (UIElement element in elements)
                {
                    // We ignore items being removed
                    if (stateDict[element].Status != MetaElementStatus.Removing)
                    {
                        // Calculate the maximum possible rect for the element
                        Rect newTargetRect = new Rect(x, y,
                                                      Math.Max(0.0, (double)(finalSize.Width - (x + right))),
                                                      Math.Max(0.0, (double)(finalSize.Height - (y + bottom))));

                        if (i < lastDockElement)
                        {
                            // Constrain target rect by the dock edge
                            Size desiredSize = element.DesiredSize;
                            switch (GetDock(element))
                            {
                                case Dock.Left:
                                    x += desiredSize.Width;
                                    newTargetRect.Width = desiredSize.Width;
                                    break;
                                case Dock.Top:
                                    y += desiredSize.Height;
                                    newTargetRect.Height = desiredSize.Height;
                                    break;
                                case Dock.Right:
                                    right += desiredSize.Width;
                                    newTargetRect.X = Math.Max(0.0, (double)(finalSize.Width - right));
                                    newTargetRect.Width = desiredSize.Width;
                                    break;
                                case Dock.Bottom:
                                    bottom += desiredSize.Height;
                                    newTargetRect.Y = Math.Max(0.0, (double)(finalSize.Height - bottom));
                                    newTargetRect.Height = desiredSize.Height;
                                    break;
                            }
                        }

                        // Store the new target rectangle
                        if (!stateDict[element].TargetRect.Equals(newTargetRect))
                        {
                            stateDict[element].TargetChanged = true;
                            stateDict[element].TargetRect = newTargetRect;
                        }

                        i++;
                    }
                }
            }
        }
        #endregion

        #region Private
        private static void OnDockChanged(DependencyObject d,
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

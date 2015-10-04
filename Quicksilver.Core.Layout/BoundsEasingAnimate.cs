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
    /// Base class for animations that require easing and start/end position calculations.
    /// </summary>
    public abstract class BoundsEasingAnimate : EasingAnimate
    {
        #region Dependancy Properties
        /// <summary>
        /// Identifies the Location dependency property.
        /// </summary>
        public static readonly DependencyProperty LocationProperty;

        /// <summary>
        /// Identifies the Size dependency property.
        /// </summary>
        public static readonly DependencyProperty SizeProperty;
        #endregion

        #region Identity
        static BoundsEasingAnimate()
        {
            LocationProperty = DependencyProperty.Register("Location",
                                                           typeof(AnimateLocation),
                                                           typeof(BoundsEasingAnimate),
                                                           new PropertyMetadata(
                                                           AnimateLocation.Target));

            SizeProperty = DependencyProperty.Register("Size",
                                                       typeof(AnimateSize),
                                                       typeof(BoundsEasingAnimate),
                                                       new PropertyMetadata(
                                                       AnimateSize.Original));
        }

        /// <summary>
        /// Initialize a new instance of the SizePositionAnimate class.
        /// </summary>
        /// <param name="target">State of elements to animate.</param>
        protected BoundsEasingAnimate(MetaElementStatus target)
            : base(target)
        {
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets the new/removing location.
        /// </summary>
        public AnimateLocation Location
        {
            get { return (AnimateLocation)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        /// <summary>
        /// Gets and sets the new/removing size.
        /// </summary>
        public AnimateSize Size
        {
            get { return (AnimateSize)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }
        #endregion

        #region Protected
        /// <summary>
        /// Calculate appropriate rectangle from given current state and target location.
        /// </summary>
        /// <param name="location">Location enumeration.</param>
        /// <param name="metaPanel">Reference to owning panel instance.</param>
        /// <param name="elementState">Animation state of element.</param>
        /// <returns>Calculated rectangle using provided location.</returns>
        protected Rect RectFromLocation(AnimateLocation location,
                                        MetaPanelBase metaPanel,
                                        MetaElementState elementState)
        {
            // Nearest edge is converted into a particular edge
            switch (location)
            {
                case AnimateLocation.NearestEdge:
                case AnimateLocation.NearestEdgePaged:
                    bool paged = (location == AnimateLocation.NearestEdgePaged);

                    // Find distance from each edge
                    double left = Math.Abs(elementState.TargetRect.Left);
                    double top = Math.Abs(elementState.TargetRect.Top);
                    double right = Math.Abs(metaPanel.ActualWidth - elementState.TargetRect.Right);
                    double bottom = Math.Abs(metaPanel.ActualHeight - elementState.TargetRect.Bottom);

                    // Find nearest distance for vertical and horizontal
                    double horz = (left < right ? left : right);
                    double vert = (top < bottom ? top : bottom);

                    // Is horizontal nearest?
                    if (horz <= vert)
                    {
                        // Is the left the nearest?
                        if (horz == left)
                            location = (paged ? AnimateLocation.LeftPaged : AnimateLocation.Left);
                        else
                            location = (paged ? AnimateLocation.RightPaged : AnimateLocation.Right);
                    }
                    else
                    {
                        // Is the top the nearest?
                        if (vert == top)
                            location = (paged ? AnimateLocation.TopPaged : AnimateLocation.Top);
                        else
                            location = (paged ? AnimateLocation.BottomPaged : AnimateLocation.Bottom);
                    }
                    break;
            }

            switch (location)
            {
                case AnimateLocation.Target:
                    return elementState.TargetRect;
                case AnimateLocation.Center:
                    return new Rect((metaPanel.ActualWidth / 2) - (elementState.TargetRect.Width - 2),
                                    (metaPanel.ActualHeight / 2) - (elementState.TargetRect.Height - 2),
                                    elementState.TargetRect.Width,
                                    elementState.TargetRect.Height);
                case AnimateLocation.Top:
                    return new Rect(elementState.TargetRect.X,
                                    -elementState.TargetRect.Height,
                                    elementState.TargetRect.Width,
                                    elementState.TargetRect.Height);
                case AnimateLocation.TopPaged:
                    return new Rect(elementState.TargetRect.X,
                                    -metaPanel.ActualHeight + elementState.TargetRect.Y,
                                    elementState.TargetRect.Width,
                                    elementState.TargetRect.Height);
                case AnimateLocation.Bottom:
                    return new Rect(elementState.TargetRect.X,
                                    metaPanel.ActualHeight,
                                    elementState.TargetRect.Width,
                                    elementState.TargetRect.Height);
                case AnimateLocation.BottomPaged:
                    return new Rect(elementState.TargetRect.X,
                                    metaPanel.ActualHeight + elementState.TargetRect.Y,
                                    elementState.TargetRect.Width,
                                    elementState.TargetRect.Height);
                case AnimateLocation.Left:
                    return new Rect(-elementState.TargetRect.Width,
                                    elementState.TargetRect.Y,
                                    elementState.TargetRect.Width,
                                    elementState.TargetRect.Height);
                case AnimateLocation.LeftPaged:
                    return new Rect(-metaPanel.ActualWidth + elementState.TargetRect.X,
                                    elementState.TargetRect.Y,
                                    elementState.TargetRect.Width,
                                    elementState.TargetRect.Height);
                case AnimateLocation.Right:
                    return new Rect(metaPanel.ActualWidth,
                                    elementState.TargetRect.Y,
                                    elementState.TargetRect.Width,
                                    elementState.TargetRect.Height);
                case AnimateLocation.RightPaged:
                    return new Rect(metaPanel.ActualWidth + elementState.TargetRect.X,
                                    elementState.TargetRect.Y,
                                    elementState.TargetRect.Width,
                                    elementState.TargetRect.Height);
                case AnimateLocation.TopLeft:
                    return new Rect(-elementState.TargetRect.Width,
                                    -elementState.TargetRect.Height,
                                    elementState.TargetRect.Width,
                                    elementState.TargetRect.Height);
                case AnimateLocation.TopRight:
                    return new Rect(metaPanel.ActualWidth,
                                    -elementState.TargetRect.Height,
                                    elementState.TargetRect.Width,
                                    elementState.TargetRect.Height);
                case AnimateLocation.BottomLeft:
                    return new Rect(-elementState.TargetRect.Width,
                                    metaPanel.ActualHeight,
                                    elementState.TargetRect.Width,
                                    elementState.TargetRect.Height);
                case AnimateLocation.BottomRight:
                    return new Rect(metaPanel.ActualWidth,
                                    metaPanel.ActualHeight,
                                    elementState.TargetRect.Width,
                                    elementState.TargetRect.Height);
                default:
                    // Should never happen!
                    Debug.Assert(false);
                    return new Rect();
            }
        }

        /// <summary>
        /// Calculate appropriate rectangle from given current state and target size.
        /// </summary>
        /// <param name="size">Size enumeration.</param>
        /// <param name="rect">Rectangle to modify.</param>
        /// <param name="elementState">Animation state of element.</param>
        /// <returns>Calculated rectangle using provided size.</returns>
        protected Rect RectFromSize(AnimateSize size,
                                    Rect rect,
                                    MetaElementState elementState)
        {
            Size minSize = elementState.Element.DesiredSize;

            switch (size)
            {
                case AnimateSize.Original:
                    return rect;
                case AnimateSize.ZeroWidthLeft:
                    return new Rect(rect.X, rect.Y, minSize.Width, rect.Height);
                case AnimateSize.ZeroWidthCenter:
                    return new Rect(rect.X + ((rect.Width - minSize.Width) / 2), rect.Y, minSize.Width, rect.Height);
                case AnimateSize.ZeroWidthRight:
                    return new Rect(rect.Right - minSize.Width, rect.Y, minSize.Width, rect.Height);
                case AnimateSize.ZeroHeightTop:
                    return new Rect(rect.X, rect.Y, rect.Width, minSize.Height);
                case AnimateSize.ZeroHeightCenter:
                    return new Rect(rect.X, rect.Y + (rect.Height - minSize.Height) / 2, rect.Width, minSize.Height);
                case AnimateSize.ZeroHeightBottom:
                    return new Rect(rect.X, rect.Bottom - minSize.Height, rect.Width, minSize.Height);
                case AnimateSize.ZeroZeroCenter:
                    return new Rect(rect.X + (rect.Width - minSize.Width) / 2, rect.Y + (rect.Height - minSize.Height) / 2, minSize.Width, minSize.Height);
                case AnimateSize.ZeroZeroTopLeft:
                    return new Rect(rect.X, rect.Y, minSize.Width, minSize.Height);
                case AnimateSize.ZeroZeroTopRight:
                    return new Rect(rect.Right - minSize.Width, rect.Y, minSize.Width, minSize.Height);
                case AnimateSize.ZeroZeroBottomLeft:
                    return new Rect(rect.X, rect.Bottom - minSize.Height, minSize.Width, minSize.Height);
                case AnimateSize.ZeroZeroBottomRight:
                    return new Rect(rect.Right - minSize.Width, rect.Bottom - minSize.Height, minSize.Width, minSize.Height);
                default:
                    // Should never happen!
                    Debug.Assert(false);
                    return new Rect();
            }
        }
        #endregion
    }
}

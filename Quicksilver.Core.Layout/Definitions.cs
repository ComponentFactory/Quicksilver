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
    #region UIElementList
    /// <summary>
    /// Manage a list of UIElement instances.
    /// </summary>
    public class UIElementList : List<UIElement> 
    { 
    }
    #endregion

    #region CellIdLookup
    /// <summary>
    /// Lookup between cell id and UIElements that are using that cell id.
    /// </summary>
    public class CellIdToElement : Dictionary<int, UIElementList> 
    { 
    }
    #endregion

    #region AnimateLocation
    /// <summary>
    /// Specifies a postion to animate to/from.
    /// </summary>
    public enum AnimateLocation
    {
        /// <summary>
        /// Location from/to the calculated target.
        /// </summary>
        Target,

        /// <summary>
        /// Location from/to the center of the panel.
        /// </summary>
        Center,

        /// <summary>
        /// Location from/to the top edge.
        /// </summary>
        Top,

        /// <summary>
        /// Location from/to a paged distance from the top edge.
        /// </summary>
        TopPaged,

        /// <summary>
        /// Location from/to the bottom edge.
        /// </summary>
        Bottom,

        /// <summary>
        /// Location from/to a paged distance from the bottom edge.
        /// </summary>
        BottomPaged,

        /// <summary>
        /// Location from/to the left edge.
        /// </summary>
        Left,

        /// <summary>
        /// Location from/to a paged distance from the left edge.
        /// </summary>
        LeftPaged,

        /// <summary>
        /// Location from/to the right edge.
        /// </summary>
        Right,

        /// <summary>
        /// Location from/to a paged distance from the right edge.
        /// </summary>
        RightPaged,

        /// <summary>
        /// Location from/to the top left corner.
        /// </summary>
        TopLeft,

        /// <summary>
        /// Location from/to the top right corner.
        /// </summary>
        TopRight,

        /// <summary>
        /// Location from/to the bottom left corner.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// Location from/to the bottom right corner.
        /// </summary>
        BottomRight,

        /// <summary>
        /// Location from/to the nearest edge.
        /// </summary>
        NearestEdge,

        /// <summary>
        /// Location from/to a paged distance from the nearest edge.
        /// </summary>
        NearestEdgePaged,
    }
    #endregion

    #region AnimateSize
    /// <summary>
    /// Specifies a size to animate to/from.
    /// </summary>
    public enum AnimateSize
    {
        /// <summary>
        /// Size is not modified.
        /// </summary>
        Original,

        /// <summary>
        /// Zero sized in the center location.
        /// </summary>
        ZeroZeroCenter,

        /// <summary>
        /// Zero sized at the top left location.
        /// </summary>
        ZeroZeroTopLeft,

        /// <summary>
        /// Zero sized at the top right location.
        /// </summary>
        ZeroZeroTopRight,

        /// <summary>
        /// Zero sized at the bottom left location.
        /// </summary>
        ZeroZeroBottomLeft,

        /// <summary>
        /// Zero sized at the bottom right location.
        /// </summary>
        ZeroZeroBottomRight,

        /// <summary>
        /// Zero sized width at left location but with full height.
        /// </summary>
        ZeroWidthLeft,

        /// <summary>
        /// Zero sized width at center location but with full height.
        /// </summary>
        ZeroWidthCenter,

        /// <summary>
        /// Zero sized width at right location but with full height.
        /// </summary>
        ZeroWidthRight,

        /// <summary>
        /// Zero sized height at top location but with full width.
        /// </summary>
        ZeroHeightTop,

        /// <summary>
        /// Zero sized height at center location but with full width.
        /// </summary>
        ZeroHeightCenter,

        /// <summary>
        /// Zero sized height at bottom location but with full width.
        /// </summary>
        ZeroHeightBottom,
    }
    #endregion

    #region ILogicalParent
    /// <summary>
    /// Interface exposed by a logical parent for adding and removing logical children.
    /// </summary>
    public interface ILogicalParent
    {
        /// <summary>
        /// Adds the provided object to the logical tree of this element.
        /// </summary>
        /// <param name="child">Child element to be added.</param>
        void LogicalChildAdd(object child);

        /// <summary>
        /// Removes the provided object from the logical tree of this element.
        /// </summary>
        /// <param name="child">Child element to be removed.</param>
        void LogicalChildRemove(object child);
    }
    #endregion

    #region ITreeNode
    /// <summary>
    /// Interface exposed by elements compatible with the tree layouts.
    /// </summary>
    public interface ITreeNode
    {
        /// <summary>
        /// Gets an array of the parent nodes.
        /// </summary>
        /// <returns></returns>
        ITreeNode[] ParentNodes();

        /// <summary>
        /// Gets an array of child nodes.
        /// </summary>
        /// <returns></returns>
        ITreeNode[] ChildNodes();
    }
    #endregion
}

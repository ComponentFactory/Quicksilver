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
using System.Linq;
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
    /// Specifies a docking position for a child element of the DockLayout.
    /// </summary>
    public enum Dock
    {
        /// <summary>
        /// Specifies a docking position of left.
        /// </summary>
        Left = 0,

        /// <summary>
        /// Specifies a docking position of top.
        /// </summary>
        Top = 1,

        /// <summary>
        /// Specifies a docking position of right.
        /// </summary>
        Right = 2,

        /// <summary>
        /// Specifies a docking position of bottom.
        /// </summary>
        Bottom = 3,
    }

    /// <summary>
    /// Arranges child elements against the panel edges. 
    /// </summary>
    public partial class DockLayout : Layout
    {
        #region Identity
        static DockLayout()
        {
            LastChildFillProperty = DependencyProperty.Register("LastChildFill", 
                                                                typeof(bool),
                                                                typeof(DockLayout), 
                                                                new PropertyMetadata(true, 
                                                                new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            DockProperty = DependencyProperty.RegisterAttached("Dock",
                                                               typeof(Dock),
                                                               typeof(DockLayout),
                                                               new PropertyMetadata(Dock.Left,
                                                               new PropertyChangedCallback(OnDockChanged)));
        }
        #endregion
    }
}

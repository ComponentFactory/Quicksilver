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
                                                               new FrameworkPropertyMetadata(Dock.Left,
                                                               new PropertyChangedCallback(OnDockChanged)), 
                                                               new ValidateValueCallback(IsValidDock));
        }
        #endregion

        #region Private
        private static bool IsValidDock(object o)
        {
            Dock dock = (Dock)o;

            if ((dock != Dock.Left) && (dock != Dock.Top) && (dock != Dock.Right))
                return (dock == Dock.Bottom);

            return true;
        }
        #endregion
    }
}


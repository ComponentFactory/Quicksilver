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
    /// MetaPanel predefined with DockLayout appropriate settings.
    /// </summary>
    public partial class MetaDockPanel : FixedMetaPanelBase
    {
        #region Dependancy Properties
        /// <summary>
        /// Identifies the LastChildFill dependency property.
        /// </summary>
        public static readonly DependencyProperty LastChildFillProperty;
        #endregion

        #region Identity
        static MetaDockPanel()
        {
            LastChildFillProperty = DependencyProperty.Register("LastChildFill", 
                                                                typeof(bool),
                                                                typeof(MetaDockPanel), 
                                                                new PropertyMetadata(true, 
                                                                new PropertyChangedCallback(OnNeedMeasureOnChanged)));
        }

        /// <summary>
        /// Initialize a new instance of the MetaDockPanel class.
        /// </summary>
        public MetaDockPanel()
        {
            DockLayout layout = new DockLayout();
            layout.SetBinding(DockLayout.LastChildFillProperty, ThisBinding("LastChildFill"));
            Layouts.Add(layout);
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets or sets a value that indicating if the last element is used to fill the remainder space.
        /// </summary>
        public bool LastChildFill
        {
            get { return (bool)GetValue(LastChildFillProperty); }
            set { SetValue(LastChildFillProperty, value); }
        }
        #endregion
    }
}

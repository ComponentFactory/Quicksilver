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
    /// MetaPanel predefined with StackLayout appropriate settings.
    /// </summary>
    public partial class MetaStackPanel : FixedMetaPanelBase
    {
        #region Dependancy Properties
        /// <summary>
        /// Identifies the Orientation dependency property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty;
        #endregion

        #region Identity
        static MetaStackPanel()
        {
            OrientationProperty = DependencyProperty.Register("Orientation",
                                                              typeof(Orientation),
                                                              typeof(MetaStackPanel),
                                                              new PropertyMetadata(Orientation.Vertical,
                                                              new PropertyChangedCallback(OnNeedMeasureOnChanged)));
        }

        /// <summary>
        /// Initialize a new instance of the MetaStackPanel class.
        /// </summary>
        public MetaStackPanel()
        {
            StackLayout layout = new StackLayout();
            layout.SetBinding(StackLayout.OrientationProperty, ThisBinding("Orientation"));
            Layouts.Add(layout);
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
        #endregion
    }
}

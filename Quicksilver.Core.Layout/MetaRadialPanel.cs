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
    /// MetaPanel predefined with RadialLayout appropriate settings.
    /// </summary>
    public partial class MetaRadialPanel : FixedMetaPanelBase
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

        #region Identity
        static MetaRadialPanel()
        {
            StartAngleProperty = DependencyProperty.Register("StartAngle", 
                                                             typeof(double),
                                                             typeof(MetaRadialPanel), 
                                                             new PropertyMetadata(0.0, 
                                                             new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            EndAngleProperty = DependencyProperty.Register("EndAngle",
                                                            typeof(double),
                                                            typeof(MetaRadialPanel),
                                                            new PropertyMetadata(360.0,
                                                            new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            CircleProperty = DependencyProperty.Register("Circle",
                                                         typeof(bool),
                                                         typeof(MetaRadialPanel),
                                                         new PropertyMetadata(false,
                                                         new PropertyChangedCallback(OnNeedMeasureOnChanged)));
        }

        /// <summary>
        /// Initialize a new instance of the MetaRadialPanel class.
        /// </summary>
        public MetaRadialPanel()
        {
            RadialLayout layout = new RadialLayout();
            layout.SetBinding(RadialLayout.StartAngleProperty, ThisBinding("StartAngle"));
            layout.SetBinding(RadialLayout.EndAngleProperty, ThisBinding("EndAngle"));
            layout.SetBinding(RadialLayout.CircleProperty, ThisBinding("Circle"));
            Layouts.Add(layout);
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
        #endregion
    }
}

// *****************************************************************************
// 
//  © Component Factory Pty Ltd 2011. All rights reserved.
//	The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, PO Box 1504, 
//  Glen Waverley, Vic 3150, Australia and are supplied subject to licence terms.
// 
//  Version 1.0.8.0 	www.ComponentFactory.com
// *****************************************************************************

using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace ComponentFactory.Quicksilver.Layout
{
    /// <summary>
    /// Defines column-specific properties that apply to the GridLayout element.
    /// </summary>
    public partial class ColumnDefinition : BaseDefinition
    {
        #region Identity
        static ColumnDefinition()
        {
            WidthProperty = DependencyProperty.Register("Width",
                                                        typeof(GridLength),
                                                        typeof(ColumnDefinition),
                                                        new PropertyMetadata(new GridLength(1, GridUnitType.Star),
                                                        new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            MinWidthProperty = DependencyProperty.Register("MinWidth",
                                                           typeof(double),
                                                           typeof(ColumnDefinition),
                                                           new PropertyMetadata(0.0,
                                                           new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            MaxWidthProperty = DependencyProperty.Register("MaxWidth",
                                                           typeof(double),
                                                           typeof(ColumnDefinition),
                                                           new PropertyMetadata(double.PositiveInfinity,
                                                           new PropertyChangedCallback(OnNeedMeasureOnChanged)));
        }
        #endregion
    }
}

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
    /// Defines row-specific properties that apply to the GridLayout element.
    /// </summary>
    public partial class RowDefinition : BaseDefinition
    {
        #region Identity
        static RowDefinition()
        {
            HeightProperty = DependencyProperty.Register("Height",
                                                         typeof(GridLength),
                                                         typeof(RowDefinition),
                                                         new FrameworkPropertyMetadata(new GridLength(1, GridUnitType.Star),
                                                         new PropertyChangedCallback(OnNeedMeasureOnChanged)),
                                                         new ValidateValueCallback(IsSizePropertyValueValid));

            MinHeightProperty = DependencyProperty.Register("MinHeight",
                                                            typeof(double),
                                                            typeof(RowDefinition),
                                                            new FrameworkPropertyMetadata(0.0,
                                                            new PropertyChangedCallback(OnNeedMeasureOnChanged)),
                                                            new ValidateValueCallback(IsMinSizePropertyValueValid));

            MaxHeightProperty = DependencyProperty.Register("MaxHeight",
                                                            typeof(double),
                                                            typeof(RowDefinition),
                                                            new FrameworkPropertyMetadata(double.PositiveInfinity,
                                                            new PropertyChangedCallback(OnNeedMeasureOnChanged)),
                                                            new ValidateValueCallback(IsMaxSizePropertyValueValid));
        }
        #endregion    
    }
}

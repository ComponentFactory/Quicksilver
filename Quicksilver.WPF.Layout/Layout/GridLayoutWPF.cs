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
    /// Arranges child elements in a flexible grid area that consists of columns and rows.
    /// </summary>
    public partial class GridLayout : Layout
    {
        #region Identity
        static GridLayout()
        {
            ColumnProperty = DependencyProperty.RegisterAttached("Column",
                                                                 typeof(int),
                                                                 typeof(GridLayout),
                                                                 new FrameworkPropertyMetadata(0,
                                                                 new PropertyChangedCallback(OnAttachedPropertyChanged)),
                                                                 new ValidateValueCallback(OnIsIntValueNotNegative));

            ColumnSpanProperty = DependencyProperty.RegisterAttached("ColumnSpan",
                                                                     typeof(int),
                                                                     typeof(GridLayout),
                                                                     new FrameworkPropertyMetadata(1,
                                                                     new PropertyChangedCallback(OnAttachedPropertyChanged)),
                                                                     new ValidateValueCallback(OnIsIntValueGreaterThanZero));

            RowProperty = DependencyProperty.RegisterAttached("Row",
                                                              typeof(int),
                                                              typeof(GridLayout),
                                                              new FrameworkPropertyMetadata(0,
                                                              new PropertyChangedCallback(OnAttachedPropertyChanged)),
                                                              new ValidateValueCallback(OnIsIntValueNotNegative));

            RowSpanProperty = DependencyProperty.RegisterAttached("RowSpan",
                                                                  typeof(int),
                                                                  typeof(GridLayout),
                                                                  new FrameworkPropertyMetadata(1,
                                                                  new PropertyChangedCallback(OnAttachedPropertyChanged)),
                                                                  new ValidateValueCallback(OnIsIntValueGreaterThanZero));
        }
        #endregion

        #region Private
        private static bool OnIsIntValueNotNegative(object value)
        {
            return (((int)value) >= 0);
        }

        private static bool OnIsIntValueGreaterThanZero(object value)
        {
            return (((int)value) > 0);
        }
        #endregion
    }
}

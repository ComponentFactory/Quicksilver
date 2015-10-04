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
    /// Manage a collection of column definitions.
    /// </summary>
    public class ColumnDefinitionCollection : DefinitionCollection<ColumnDefinition> { };

    /// <summary>
    /// Defines column-specific properties that apply to the GridLayout element.
    /// </summary>
    public partial class ColumnDefinition : BaseDefinition
    {
        #region Dependancy Properties
        /// <summary>
        /// Identifies the Width dependency property.
        /// </summary>
        public static readonly DependencyProperty WidthProperty;

        /// <summary>
        /// Identifies the MinWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty MinWidthProperty;

        /// <summary>
        /// Identifies the MaxWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxWidthProperty;
        #endregion

        #region Instance Fields
        private double _actualWidth;
        #endregion

        #region Public
        /// <summary>
        /// Gets the rendered width of this element.
        /// </summary>
        public double ActualWidth
        {
            get { return _actualWidth; }
            internal set { _actualWidth = value; }
        }

        /// <summary>
        /// Gets and sets the suggested width of a column.
        /// </summary>
        public GridLength Width
        {
            get { return (GridLength)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that represents the minimum width of a column.
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public double MinWidth
        {
            get { return (double)GetValue(MinWidthProperty); }
            set { SetValue(MinWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that represents the maximum width of a column.
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public double MaxWidth
        {
            get { return (double)GetValue(MaxWidthProperty); }
            set { SetValue(MaxWidthProperty, value); }
        }
        #endregion
    }
}

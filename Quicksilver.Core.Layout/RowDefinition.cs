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
    /// Manage a collection of row definitions.
    /// </summary>
    public class RowDefinitionCollection : DefinitionCollection<RowDefinition> { };

    /// <summary>
    /// Defines row-specific properties that apply to the GridLayout element.
    /// </summary>
    public partial class RowDefinition : BaseDefinition
    {
        #region Dependancy Properties
        /// <summary>
        /// Identifies the Height dependency property.
        /// </summary>
        public static readonly DependencyProperty HeightProperty;

        /// <summary>
        /// Identifies the MinHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty MinHeightProperty;

        /// <summary>
        /// Identifies the MaxHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxHeightProperty;
        #endregion

        #region Instance Fields
        private double _actualHeight;
        #endregion

        #region Public
        /// <summary>
        /// Gets the rendered height of this element.
        /// </summary>
        public double ActualHeight
        {
            get { return _actualHeight; }
            internal set { _actualHeight = value; }
        }

        /// <summary>
        /// Gets and sets the suggested height of a row.
        /// </summary>
        public GridLength Height
        {
            get { return (GridLength)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that represents the minimum height of a row.
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public double MinHeight
        {
            get { return (double)GetValue(MinHeightProperty); }
            set { SetValue(MinHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that represents the maximum height of a row.
        /// </summary>
        [TypeConverter(typeof(LengthConverter))]
        public double MaxHeight
        {
            get { return (double)GetValue(MaxHeightProperty); }
            set { SetValue(MaxHeightProperty, value); }
        }
        #endregion
    }
}

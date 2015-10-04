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
    /// MetaPanel predefined with WrapLayout appropriate settings.
    /// </summary>
    public partial class MetaUniformGridPanel : FixedMetaPanelBase
    {
        #region Dependancy Properties
        /// <summary>
        /// Identifies the Rows dependency property.
        /// </summary>
        public static readonly DependencyProperty RowsProperty;

        /// <summary>
        /// Identifies the Columns dependency property.
        /// </summary>
        public static readonly DependencyProperty ColumnsProperty;

        /// <summary>
        /// Identifies the FirstColumn dependency property.
        /// </summary>
        public static readonly DependencyProperty FirstColumnProperty;
        #endregion

        #region Identity
        static MetaUniformGridPanel()
        {
            RowsProperty = DependencyProperty.Register("Rows", 
                                                       typeof(int),
                                                       typeof(MetaUniformGridPanel), 
                                                       new PropertyMetadata(0, 
                                                       new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            ColumnsProperty = DependencyProperty.Register("Columns",
                                                          typeof(int),
                                                          typeof(MetaUniformGridPanel),
                                                          new PropertyMetadata(0,
                                                          new PropertyChangedCallback(OnNeedMeasureOnChanged)));

            FirstColumnProperty = DependencyProperty.Register("FirstColumn",
                                                              typeof(int),
                                                              typeof(MetaUniformGridPanel),
                                                              new PropertyMetadata(0,
                                                              new PropertyChangedCallback(OnNeedMeasureOnChanged)));
        }

        /// <summary>
        /// Initialize a new instance of the MetaUniformGridPanel class.
        /// </summary>
        public MetaUniformGridPanel()
        {
            UniformGridLayout layout = new UniformGridLayout();
            layout.SetBinding(UniformGridLayout.RowsProperty, ThisBinding("Rows"));
            layout.SetBinding(UniformGridLayout.ColumnsProperty, ThisBinding("Columns"));
            layout.SetBinding(UniformGridLayout.FirstColumnProperty, ThisBinding("FirstColumn"));
            Layouts.Add(layout);
        }
        #endregion

        #region Public
        /// <summary>
        ///Gets or sets the number of rows that are in the grid.
        /// </summary>
        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number of columns that are in the grid.
        /// </summary>
        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number of leading blank cells in the first row of the grid.
        /// </summary>
        public int FirstColumn
        {
            get { return (int)GetValue(FirstColumnProperty); }
            set { SetValue(FirstColumnProperty, value); }
        }
        #endregion
    }
}

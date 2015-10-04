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
using System.ComponentModel;
using System.Diagnostics;

namespace ComponentFactory.Quicksilver.Layout
{
    /// <summary>
    /// MetaPanel predefined with GridLayout appropriate settings.
    /// </summary>
    public partial class MetaGridPanel : FixedMetaPanelBase
    {
        #region Instance Fields
        private GridLayout _layout;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the MetaGridPanel class.
        /// </summary>
        public MetaGridPanel()
        {
            _layout = new GridLayout();
            Layouts.Add(_layout);
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets a ColumnDefinitionCollection defined on this instance of Grid.
        /// </summary>
        #if !SILVERLIGHT
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        #endif
        public ColumnDefinitionCollection ColumnDefinitions
        {
            get { return _layout.ColumnDefinitions; }
        }

        /// <summary>
        /// Gets a RowDefinitionCollection defined on this instance of Grid. 
        /// </summary>
        #if !SILVERLIGHT
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        #endif
        public RowDefinitionCollection RowDefinitions
        {
            get { return _layout.RowDefinitions; }
        }
        #endregion
    }
}

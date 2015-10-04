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
    /// Panel that has polymorphic layout/animate strategies.
    /// </summary>
    public partial class MetaPanel : MetaPanelBase
    {
        #region Public
        /// <summary>
        /// Gets and sets the collection of layout strategies used for child elements.
        /// </summary>
        public LayoutCollection LayoutDefinitions
        {
            get { return Layouts; }
        }

        /// <summary>
        /// Gets and sets the collection of animate strategies used for child elements.
        /// </summary>
        public AnimateDefinitions AnimateDefinitions
        {
            get { return Animates; }
        }
        #endregion
    }
}

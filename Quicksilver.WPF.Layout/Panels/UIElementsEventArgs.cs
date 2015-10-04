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
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Media;
using System.Reflection;

namespace ComponentFactory.Quicksilver.Layout
{
    /// <summary>
    /// Contains an array of UIElement instances associated with an event.
    /// </summary>
    public class UIElementsEventArgs : EventArgs
    {
        #region Instance Fields
        private UIElement[] _elements;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the UIElementsEventArgs class.
        /// </summary>
        /// <param name="element">Single element.</param>
        public UIElementsEventArgs(UIElement element)
        {
            _elements = new UIElement[] { element };
        }

        /// <summary>
        /// Initialize a new instance of the UIElementsEventArgs class.
        /// </summary>
        /// <param name="elements">Array of elements.</param>
        public UIElementsEventArgs(UIElement[] elements)
        {
            _elements = elements;
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets an array of elements associated with the event.
        /// </summary>
        public UIElement[] Elements
        {
            get { return _elements; }
        }
        #endregion
    }
}

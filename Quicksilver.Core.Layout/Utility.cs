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
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

namespace ComponentFactory.Quicksilver.Layout
{
    internal static class Utility
    {
        #region Static Fields
        private static readonly Size _sizeZero = new Size(0, 0);
        private static readonly Size _sizeInfinity = new Size(double.PositiveInfinity, double.PositiveInfinity);
        private static readonly Point _pointZero = new Point(0, 0);
        #endregion

        #region Public
        /// <summary>
        /// Gets a Size defined with zero values.
        /// </summary>
        public static Size SizeZero
        {
            get { return _sizeZero; }
        }

        /// <summary>
        /// Gets a Size defined with infinity values.
        /// </summary>
        public static Size SizeInfinity
        {
            get { return _sizeInfinity; }
        }

        /// <summary>
        /// Gets a Size defined with infinity values.
        /// </summary>
        public static Point PointZero
        {
            get { return _pointZero; }
        }
        #endregion
    }
}

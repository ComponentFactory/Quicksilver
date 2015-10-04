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
    /// <summary>
    /// Animates the location/size of an element from current to target.
    /// </summary>
    public class MovePositionAnimate : EasingAnimate
    {
        #region Public
        /// <summary>
        /// Initialize a new instance of the MovePositionAnimate class.
        /// </summary>
        public MovePositionAnimate()
            : base(MetaElementStatus.Existing)
        {
        }
        #endregion
    }
}

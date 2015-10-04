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
using System.Linq;
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
    /// Base class for animations that require an easing algorithm.
    /// </summary>
    public partial class EasingAnimate : Animate
    {
        #region Identity
        static EasingAnimate()
        {
            DurationProperty = DependencyProperty.Register("Duration",
                                                           typeof(double),
                                                           typeof(EasingAnimate),
                                                           new PropertyMetadata((double)DEFAULT_DURATION));

            EasingProperty = DependencyProperty.Register("Easing",
                                                         typeof(Easing),
                                                         typeof(EasingAnimate),
                                                         new PropertyMetadata((Easing)DEFAULT_EASING));
        }
        #endregion
    }
}

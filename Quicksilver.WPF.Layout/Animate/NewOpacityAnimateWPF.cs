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
    /// Animates element opacity for new elements.
    /// </summary>
    public partial class NewOpacityAnimate : OpacityEasingAnimate
    {
        #region Identity
        static NewOpacityAnimate()
        {
            StartProperty = DependencyProperty.Register("Start",
                                                        typeof(double),
                                                        typeof(NewOpacityAnimate),
                                                        new PropertyMetadata((double)DEFAULT_START, null,
                                                        new CoerceValueCallback(OnCoerceStart)));

            EndProperty = DependencyProperty.Register("End",
                                                      typeof(double),
                                                      typeof(NewOpacityAnimate),
                                                      new PropertyMetadata((double)DEFAULT_END, null,
                                                      new CoerceValueCallback(OnCoerceEnd)));
        }
        #endregion

        #region Private
        private static object OnCoerceStart(DependencyObject d, object baseValue)
        {
            if ((baseValue == null) || !(baseValue is double))
                return (double)DEFAULT_START;
            else
                return Math.Min(Math.Max((double)baseValue, 0.0), 1.0);
        }

        private static object OnCoerceEnd(DependencyObject d, object baseValue)
        {
            if ((baseValue == null) || !(baseValue is double))
                return (double)DEFAULT_END;
            else
                return Math.Min(Math.Max((double)baseValue, 0.0), 1.0);
        }
        #endregion
    }
}

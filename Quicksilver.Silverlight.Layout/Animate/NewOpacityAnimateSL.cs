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
                                                        new PropertyMetadata((double)DEFAULT_START));

            EndProperty = DependencyProperty.Register("End",
                                                      typeof(double),
                                                      typeof(NewOpacityAnimate),
                                                      new PropertyMetadata((double)DEFAULT_END));
        }
        #endregion
    }
}

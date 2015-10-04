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
    /// Move elements directly to the target without animating between.
    /// </summary>
    public class NullAnimate : Animate
    {
        #region Public
        /// <summary>
        /// Perform animation effects on the set of children.
        /// </summary>
        /// <param name="animateId">Identifier of the animate to be used.</param>
        /// <param name="metaPanel">Reference to owning panel instance.</param>
        /// <param name="stateDict">Dictionary of per-element state.</param>
        /// <param name="elements">Collection of elements to be animated.</param>
        /// <param name="elapsedMilliseconds">Elapsed milliseconds since last animation cycle.</param>
        public override void ApplyAnimation(string animateId,
                                            MetaPanelBase metaPanel,
                                            MetaElementStateDict stateDict,
                                            ICollection elements,
                                            double elapsedMilliseconds)
        {
            // Only apply if we match the incoming animate identifier
            if (string.IsNullOrEmpty(Id) || Id.Equals(animateId))
            {
                foreach (UIElement element in elements)
                {
                    // Immediately move to the target rectangle
                    MetaElementState elementState = stateDict[element];
                    elementState.CurrentRect = elementState.TargetRect;
                }
            }
        }
        #endregion
    }
}

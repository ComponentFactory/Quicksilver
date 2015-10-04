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
    /// Base class for animations that require easing and start/end opacity calculations.
    /// </summary>
    public abstract class OpacityEasingAnimate : EasingAnimate
    {
        #region Identity
        /// <summary>
        /// Initialize a new instance of the OpacityEasingAnimate class.
        /// </summary>
        /// <param name="target">State of elements to animate.</param>
        protected OpacityEasingAnimate(MetaElementStatus target)
            : base(target)
        {
        }
        #endregion

        #region Public
        /// <summary>
        /// Perform animation effects on the set of children.
        /// </summary>
        /// <param name="animateId">Identifier of the animate to be used.</param>
        /// <param name="metaPanel">Reference to owning panel instance.</param>
        /// <param name="stateDict">Dictionary of per-element state.</param>
        /// <param name="elements">Collection of elements to be animated.</param>
        /// <param name="elapsedMilliseconds">Elapsed milliseconds since last animation cycle.</param>
        /// <param name="startOpacity">Opacity for start of animation.</param>
        /// <param name="endOpacity">Opacity for end of animation.</param>
        public void ApplyAnimation(string animateId,
                                   MetaPanelBase metaPanel,
                                   MetaElementStateDict stateDict,
                                   ICollection elements,
                                   double elapsedMilliseconds,
                                   double startOpacity,
                                   double endOpacity)
        {
            // Only apply if we match the incoming animate identifier
            if (string.IsNullOrEmpty(Id) || Id.Equals(animateId))
            {
                // Only grab the dependancy properties once, for improved perf
                double duration = Duration;

                foreach (UIElement element in elements)
                {
                    // Only apply animation to element of required target state
                    MetaElementState elementState = stateDict[element];
                    if (elementState.Status == Target)
                    {
                        // If the element is the correct target and correct status for starting animation
                        if ((!elementState.RemoveCalculated && (Target == MetaElementStatus.Removing)) ||
                            (elementState.NewCalculating && (Target == MetaElementStatus.New)))
                        {
                            // Use the starting opacity
                            element.Opacity = startOpacity;
                            elementState.ElapsedOpacityTime = 0;
                        }
                        else
                        {
                            // Only perform animation if some time has actually ellapsed and not already at target
                            if ((elapsedMilliseconds > 0) && (element.Opacity != (double)endOpacity))
                            {
                                // Add new elapsed time to the animation running time
                                elementState.ElapsedOpacityTime += elapsedMilliseconds;

                                // Does elapsed time indicate animation should have completed?
                                if (elementState.ElapsedOpacityTime >= duration)
                                    element.Opacity = endOpacity;
                                else
                                    element.Opacity = EasingCalculation.Calculate(elementState.ElapsedOpacityTime, startOpacity, endOpacity - startOpacity, duration);
                            }
                        }

                        // If not yet at target opacity then not finished with animating
                        elementState.AnimateComplete &= (element.Opacity == (double)endOpacity);
                    }
                }
            }
        }
        #endregion
    }
}

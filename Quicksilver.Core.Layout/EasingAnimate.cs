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
    /// Base class for animations that require an easing algorithm.
    /// </summary>
    public abstract partial class EasingAnimate : Animate
    {
        #region Constants
        internal static readonly double DEFAULT_DURATION = 333;
        internal static readonly Easing DEFAULT_EASING = Easing.QuadraticInOut;
        #endregion

        #region Dependancy Properties
        /// <summary>
        /// Identifies the Duration dependency property.
        /// </summary>
        public static readonly DependencyProperty DurationProperty;

        /// <summary>
        /// Identifies the Easing dependency property.
        /// </summary>
        public static readonly DependencyProperty EasingProperty;
        #endregion

        #region Instance Fields
        private MetaElementStatus _target;
        private EasingCalculation _easing;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the MovePositionAnimate class.
        /// </summary>
        /// <param name="target">State of elements to animate.</param>
        protected EasingAnimate(MetaElementStatus target)
        {
            _target = target;
            _easing = new EasingCalculation(DEFAULT_EASING);
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets or sets the how long the animation should take to reach the new target rectangle.
        /// </summary>
        public double Duration
        {
            get { return (double)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the how to ease the animation values from start to end values.
        /// </summary>
        public Easing Easing
        {
            get { return (Easing)GetValue(EasingProperty); }
            set { SetValue(EasingProperty, value); }
        }

        /// <summary>
        /// Define a callback to use for all easing calculations.
        /// </summary>
        /// <param name="callback">Delegate to use for each easing calculation.</param>
        public void EasingCallback(EasingEquation callback)
        {
            _easing.EasingEquation = callback;
        }

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
                // Update the easing equation with latest value
                EasingCalculation.Easing = Easing;

                // Cache dependancy properties for faster perf
                double duration = Math.Max((double)1, Duration);

                foreach (UIElement element in elements)
                {
                    // Only apply animation to element of required target state
                    MetaElementState elementState = stateDict[element];
                    if (elementState.Status == Target)
                    {
                        Rect targetRect = elementState.TargetRect;
                        Rect currentRect = (elementState.CurrentRect.IsEmpty ? targetRect : elementState.CurrentRect);

                        // If start of animation....
                        if (elementState.TargetChanged)
                        {
                            // Cache starting information
                            elementState.StartRect = currentRect;
                            elementState.ElapsedBoundsTime = 0;
                        }
                        else
                        {
                            // Only perform animation if some time has actually ellapsed and not already at target
                            if ((elapsedMilliseconds > 0) && !currentRect.Equals(targetRect))
                            {
                                // Add new elapsed time to the animation running time
                                elementState.ElapsedBoundsTime += elapsedMilliseconds;

                                // Does elapsed time indicate animation should have completed?
                                if (elementState.ElapsedBoundsTime >= duration)
                                    currentRect = targetRect;
                                else
                                {
                                    Rect startRect = elementState.StartRect;
                                    double elapsedTime = elementState.ElapsedBoundsTime;

                                    // Using animation easing to discover new target rectangle corners
                                    double left = EasingCalculation.Calculate(elapsedTime, startRect.X, targetRect.X - startRect.X, duration);
                                    double top = EasingCalculation.Calculate(elapsedTime, startRect.Y, targetRect.Y - startRect.Y, duration);
                                    double bottom = EasingCalculation.Calculate(elapsedTime, startRect.Bottom, targetRect.Bottom - startRect.Bottom, duration);
                                    double right = EasingCalculation.Calculate(elapsedTime, startRect.Right, targetRect.Right - startRect.Right, duration);

                                    // Normalize edges left/right edges
                                    if (left > right)
                                    {
                                        elapsedTime = left;
                                        left = right;
                                        right = elapsedTime;
                                    }

                                    // Normalize edges top/bottom edges
                                    if (top > bottom)
                                    {
                                        elapsedTime = top;
                                        top = bottom;
                                        bottom = elapsedTime;
                                    }

                                    currentRect = new Rect(left, top, right - left, bottom - top);
                                }
                            }
                        }

                        // Put back the updated rectangle and decide if more animation is needed
                        elementState.CurrentRect = currentRect;
                        elementState.AnimateComplete &= (currentRect.Equals(targetRect));
                    }
                }
            }
        }
        #endregion

        #region Protected
        /// <summary>
        /// Gets access to the easing calculation class.
        /// </summary>
        protected EasingCalculation EasingCalculation
        {
            get { return _easing; }
        }

        /// <summary>
        /// Gets access to the target element status
        /// </summary>
        protected MetaElementStatus Target
        {
            get { return _target; }
        }
        #endregion
    }
}

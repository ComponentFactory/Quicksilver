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
    /// Specifies an equation used to ease values from a start to end value over a time period.
    /// </summary>
    public enum Easing
    {
        /// <summary>Specifies a constant motion, with no acceleration.</summary>
        Linear = 0,
        /// <summary>Specifies starting motion from a zero velocity, and then accelerates motion as it executes.</summary>
        QuadraticIn,
        /// <summary>Specifies starting motion fast, and then decelerates motion to a zero velocity as it executes.</summary>
        QuadraticOut,
        /// <summary>Specifies to start the motion from a zero velocity, accelerate motion, then decelerate to a zero velocity.</summary>
        QuadraticInOut,
        /// <summary>Specifies starting motion from zero velocity, and then accelerates motion as it executes.</summary>
        CubicIn,
        /// <summary>Specifies starting motion fast, and then decelerates motion to a zero velocity as it executes.</summary>
        CubicOut,
        /// <summary>Specifies to start the motion from zero velocity, accelerates motion, then decelerates back to a zero velocity.</summary>
        CubicInOut,
        /// <summary>Specifies starting motion from a zero velocity, and then accelerates motion as it executes.</summary>
        QuarticIn,
        /// <summary>Specifies starting motion fast, and then decelerates motion to a zero velocity.</summary>
        QuarticOut,
        /// <summary>Specifies to start the motion from a zero velocity, accelerate motion, then decelerate to a zero velocity.</summary>
        QuarticInOut,
        /// <summary>Specifies starting motion from zero velocity, and then accelerates motion as it executes.</summary>
        QuinticIn,
        /// <summary>Specifies starting motion fast, and then decelerates motion to a zero velocity as it executes.</summary>
        QuinticOut,
        /// <summary>Specifies to start the motion from a zero velocity, accelerate motion, then decelerate to a zero velocity.</summary>
        QuinticInOut,
        /// <summary>Specifies starting motion from zero velocity, and then accelerates motion as it executes.</summary>
        SineIn,
        /// <summary>Specifies starting motion fast, and then decelerates motion to a zero velocity as it executes.</summary>
        SineOut,
        /// <summary>Specifies to start the motion from a zero velocity, accelerate motion, then decelerate to a zero velocity.</summary>
        SineInOut,
        /// <summary>Specifies starting motion slowly, and then accelerates motion as it executes.</summary>
        CircularIn,
        /// <summary>Specifies starting motion fast, and then decelerates motion as it executes.</summary>
        CircularOut,
        /// <summary>Specifies to start the motion slowly, accelerate motion, then decelerate.</summary>
        CircularInOut,
        /// <summary>Specifies starting slowly and then accelerating.</summary>
        ExponentialIn,
        /// <summary>Specifies starting motion slowly, and then accelerates motion as it executes.</summary>
        ExponentialOut,
        /// <summary>Specifies starting the motion slowly, accelerate motion, then decelerate.</summary>
        ExponentialInOut,
        /// <summary>Specifies starting the bounce motion slowly, and then accelerates motion as it executes.</summary>
        BounceIn,
        /// <summary>Specifies starting the bounce motion fast, and then decelerates motion as it executes.</summary>
        BounceOut,
        /// <summary>Specifies to start the bounce motion slowly, accelerate motion, then decelerate.</summary>
        BounceInOut,
        /// <summary>Specifies starting the motion by backtracking, then reversing direction and moving toward the target.</summary>
        BackIn,
        /// <summary>Specifies starting the motion by moving towards the target, overshooting it slightly, and then reversing direction back toward the target.</summary>
        BackOut,
        /// <summary>Specifies to start the motion by backtracking, then reversing direction and moving toward target, overshooting target slightly, reversing direction again, and then moving back toward the target.</summary>
        BackInOut
    }

    /// <summary>
    /// Signature of a delegate that calculates an easing values.
    /// </summary>
    /// <param name="t">Elapsed time.</param>
    /// <param name="b">Starting value.</param>
    /// <param name="c">Delta from start to ending value.</param>
    /// <param name="d">Total duration.</param>
    /// <returns>Calculated value.</returns>
    public delegate double EasingEquation(double t, double b, double c, double d);

    /// <summary>
    /// Provides calculation of easing values using predefined easing equations.
    /// </summary>
    public class EasingCalculation
    {
        #region Static Fields
        private static readonly EasingEquation[] _predefinedEquations = new EasingEquation[] 
        { 
            /* Linear           */   (t, b, c, d) => { return c * t / d + b; },
            /* QuadraticIn      */   (t, b, c, d) => { return c * (t /= d) * t + b; },
            /* QuadraticOut     */   (t, b, c, d) => { return -c * (t /= d) * (t - 2) + b; },
            /* QuadraticInOut   */   (t, b, c, d) => { if ((t /= d / 2) < 1) return c / 2 * t * t + b; return -c / 2 * ((--t) * (t - 2) - 1) + b; },
            /* CubicIn          */   (t, b, c, d) => { return c * (t /= d) * t * t + b; },
            /* CubicOut         */   (t, b, c, d) => { return c * ((t = t / d - 1) * t * t + 1) + b; },
            /* CubicInOut       */   (t, b, c, d) => { if ((t /= d / 2) < 1) return c / 2 * t * t * t + b; else return c / 2 * ((t -= 2) * t * t + 2) + b; },
            /* QuarticIn        */   (t, b, c, d) => { return c * (t /= d) * t * t * t + b; },
            /* QuarticOut       */   (t, b, c, d) => { return -c * ((t = t / d - 1) * t * t * t - 1) + b; },
            /* QuarticInOut     */   (t, b, c, d) => { if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b; return -c / 2 * ((t -= 2) * t * t * t - 2) + b; },
            /* QuinticIn        */   (t, b, c, d) => { return c * (t /= d) * t * t * t * t + b; },
            /* QuinticOut       */   (t, b, c, d) => { return c * ((t = t / d - 1) * t * t * t * t + 1) + b; },
            /* QuinticInOut     */   (t, b, c, d) => { if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b; return c / 2 * ((t -= 2) * t * t * t * t + 2) + b; },
            /* SineIn           */   (t, b, c, d) => { return -c * Math.Cos(t / d * (Math.PI / 2)) + c + b; },
            /* SineOut          */   (t, b, c, d) => { return c * Math.Sin(t / d * (Math.PI / 2)) + b; },
            /* SineInOut        */   (t, b, c, d) => { return -c / 2 * (Math.Cos(Math.PI * t / d) - 1) + b; },
            /* CircularIn       */   (t, b, c, d) => { return -c * (Math.Sqrt(1 - (t /= d) * t) - 1) + b; },
            /* CircularOut      */   (t, b, c, d) => { return c * Math.Sqrt(1 - (t = t / d - 1) * t) + b; },
            /* CircularInOut    */   (t, b, c, d) => { if ((t /= d / 2) < 1) return -c / 2 * (Math.Sqrt(1 - t * t) - 1) + b; else return c / 2 * (Math.Sqrt(1 - (t -= 2) * t) + 1) + b; },
            /* ExponentialIn    */   (t, b, c, d) => { return (t == 0) ? b : c * Math.Pow(2, 10 * (t / d - 1)) + b; },
            /* ExponentialOut   */   (t, b, c, d) => { return (t == d) ? b + c : c * (-Math.Pow(2, -10 * t / d) + 1) + b; },
            /* ExponentialInOut */   (t, b, c, d) => { if (t == 0) return b; if (t == d) return b + c; if ((t /= d / 2) < 1) return c / 2 * Math.Pow(2, 10 * (t - 1)) + b; return c / 2 * (-Math.Pow(2, -10 * --t) + 2) + b; },
            /* BounceIn         */   (t, b, c, d) => { double t1=d-t; double bout; if ((t1/=d) < (1/2.75)) bout = c*(7.5625*t1*t1); else if (t1 < (2/2.75)) bout = c*(7.5625*(t1-=(1.5/2.75))*t1 + .75); else if (t1 < (2.5/2.75)) bout = c*(7.5625*(t1-=(2.25/2.75))*t1 + .9375); else bout = c*(7.5625*(t1-=(2.625/2.75))*t1 + .984375); return c - bout + b; },
            /* BounceOut        */   (t, b, c, d) => { if ((t /= d) < (1 / 2.75)) return c * (7.5625 * t * t) + b; else if (t < (2 / 2.75)) return c * (7.5625 * (t -= (1.5 / 2.75)) * t + .75) + b; else if (t < (2.5 / 2.75)) return c * (7.5625 * (t -= (2.25 / 2.75)) * t + .9375) + b; else return c * (7.5625 * (t -= (2.625 / 2.75)) * t + .984375) + b; },
            /* BounceInOut      */   (t, b, c, d) => { double x; if (t < d/2) { double t2 = t*2; double t1=d-t2; if ((t1/=d) < (1/2.75)) x = c*(7.5625*t1*t1); else if (t1 < (2/2.75)) x = c*(7.5625*(t1-=(1.5/2.75))*t1 + .75); else if (t1 < (2.5/2.75)) x = c*(7.5625*(t1-=(2.25/2.75))*t1 + .9375); else x = c*(7.5625*(t1-=(2.625/2.75))*t1 + .984375); return (c - x) * .5 + b; } else { double t2 = t*2-d; if ((t2 /= d) < (1 / 2.75)) x = c * (7.5625 * t2 * t2); else if (t2 < (2 / 2.75)) x = c * (7.5625 * (t2 -= (1.5 / 2.75)) * t2 + .75); else if (t2 < (2.5 / 2.75)) x = c * (7.5625 * (t2 -= (2.25 / 2.75)) * t2 + .9375); else x = c * (7.5625 * (t2 -= (2.625 / 2.75)) * t2 + .984375); return (x * .5) + c*.5 + b; } },
            /* BackIn           */   (t, b, c, d) => { double s = 1.7; return c * (t /= d) * t * ((s + 1) * t - s) + b; },
            /* BackOut          */   (t, b, c, d) => { double s = 1.7; return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b; },
            /* BackInOut        */   (t, b, c, d) => { double bout; double t2 = (t < d / 2) ? d - (t * 2) : (t * 2) - d; if ((t2 /= d) < (1 / 2.75)) bout = c * (7.5625 * t2 * t2); else if (t2 < (2 / 2.75)) bout = c * (7.5625 * (t2 -= (1.5 / 2.75)) * t2 + .75); else if (t2 < (2.5 / 2.75)) bout = c * (7.5625 * (t2 -= (2.25 / 2.75)) * t2 + .9375); else bout = c * (7.5625 * (t2 -= (2.625 / 2.75)) * t2 + .984375); if (t < d / 2) return bout * .5 + c * .5 + b; else return bout * .5 + b; }
        };
        #endregion                                                                         

        #region Instance Fields
        private Easing _easing;
        private EasingEquation _equation;
        private EasingEquation _override;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the EasingCalculation class.
        /// </summary>
        ///  <param name="easing">Initial easing algorithm.</param>
        public EasingCalculation(Easing easing)
        {
            Easing = easing;
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets the easing to be applied.
        /// </summary>
        public Easing Easing
        {
            get { return _easing; }

            set
            {
                _easing = value;

                // Update the delegate we use for performing calculations
                _equation = _predefinedEquations[(int)_easing];
            }
        }

        /// <summary>
        /// Gets and sets a delegate used to perform easing calculations.
        /// </summary>
        public EasingEquation EasingEquation
        {
            get { return _override; }
            set { _override = value; }
        }

        /// <summary>
        /// Find the easing value using the current easing equation.
        /// </summary>
        /// <param name="t">Elapsed time.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Delta change from start to ending value.</param>
        /// <param name="d">Total duration.</param>
        /// <returns>Calculated value.</returns>
        public double Calculate(double t, double b, double c, double d)
        {
            // Use any defined override in preference to the predefined easing
            return (_override ?? _equation)(t, b, c, d);
        }
        #endregion
    }
}

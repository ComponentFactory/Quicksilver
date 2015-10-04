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
    /// Animates element opacity for removing elements.
    /// </summary>
    public partial class RemoveOpacityAnimate : OpacityEasingAnimate
    {
        #region Constants
        private static readonly double DEFAULT_START = 1.0;
        private static readonly double DEFAULT_END = 0.0;
        #endregion

        #region Dependancy Properties
        /// <summary>
        /// Identifies the Start dependency property.
        /// </summary>
        public static readonly DependencyProperty StartProperty;

        /// <summary>
        /// Identifies the End dependency property.
        /// </summary>
        public static readonly DependencyProperty EndProperty;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the RemoveOpacityAnimate class.
        /// </summary>
        public RemoveOpacityAnimate()
            : base(MetaElementStatus.Removing)
        {
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets the starting opacity.
        /// </summary>
        public double Start
        {
            get { return (double)GetValue(StartProperty); }
            set { SetValue(StartProperty, value); }
        }

        /// <summary>
        /// Gets and sets the ending opacity.
        /// </summary>
        public double End
        {
            get { return (double)GetValue(EndProperty); }
            set { SetValue(EndProperty, value); }
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
            ApplyAnimation(animateId, metaPanel, stateDict, elements, elapsedMilliseconds, Start, End);
        }
        #endregion
    }
}

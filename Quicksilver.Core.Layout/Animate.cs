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
    /// Base class that all animation implementations derive from.
    /// </summary>
    public abstract class Animate : MeasureElement
    {
        #region Dependancy Properties
        /// <summary>
        /// Identifies the Id dependency property.
        /// </summary>
        public static readonly DependencyProperty IdProperty;
        #endregion

        #region Identity
        static Animate()
        {
            IdProperty = DependencyProperty.Register("Id", 
                                                     typeof(string),
                                                     typeof(Animate), 
                                                     new PropertyMetadata(string.Empty, 
                                                     new PropertyChangedCallback(OnNeedMeasureOnChanged)));

        }
        #endregion

        #region Public
        /// <summary>
        /// Gets or sets a value the identifier used to name this layout.
        /// </summary>
        public string Id
        {
            get { return (string)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        /// <summary>
        /// Perform animation effects on the set of children.
        /// </summary>
        /// <param name="animateId">Identifier of the animate to be used.</param>
        /// <param name="metaPanel">Reference to owning panel instance.</param>
        /// <param name="stateDict">Dictionary of per-element state.</param>
        /// <param name="elements">Collection of elements to be animated.</param>
        /// <param name="elapsedMilliseconds">Elapsed milliseconds since last animation cycle.</param>
        public abstract void ApplyAnimation(string animateId,
                                            MetaPanelBase metaPanel,
                                            MetaElementStateDict stateDict,
                                            ICollection elements,
                                            double elapsedMilliseconds);
        #endregion
    }
}

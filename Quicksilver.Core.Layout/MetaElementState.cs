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
    /// Lifetime status of the element.
    /// </summary>
    public enum MetaElementStatus
    {
        /// <summary>
        /// Element is recently added and ready for initial animation.
        /// </summary>
        New,

        /// <summary>
        /// Element is avilable for position to position animation effects.
        /// </summary>
        Existing,

        /// <summary>
        /// Element is in the process of being removed once final animation completes.
        /// </summary>
        Removing
    }

    /// <summary>
    /// Per-element state information.
    /// </summary>
    public class MetaElementState
    {
        #region Identity
        /// <summary>
        /// Initialize a new instance of the MetaElementState class.
        /// </summary>
        /// <param name="element">Element associated with the state.</param>
        public MetaElementState(UIElement element)
        {
            Element = element;
            Status = MetaElementStatus.New;
            AnimateComplete = true;
            NewCalculating = true;
            RemoveCalculated = false;
            TargetChanged = true;
            TargetRect = Rect.Empty;
            CurrentRect = Rect.Empty;
            StartRect = Rect.Empty;
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets the element associated with this state information.
        /// </summary>
        public UIElement Element { get; set; }

        /// <summary>
        /// Gets and sets the current status of the element.
        /// </summary>
        public MetaElementStatus Status { get; set; }

        /// <summary>
        /// Gets and sets a value indicating if animation has completed for the element.
        /// </summary>
        public bool AnimateComplete { get; set; }

        /// <summary>
        /// Gets and sets a value indicating if the element needs new element calculations.
        /// </summary>
        public bool NewCalculating { get; set; }

        /// <summary>
        /// Gets and sets a value indicating if the element needs remove element calculations.
        /// </summary>
        public bool RemoveCalculated { get; set; }

        /// <summary>
        /// Gets and sets the target rectangle calculated by the layout.
        /// </summary>
        public Rect TargetRect { get; set; }

        /// <summary>
        /// Gets and sets a value indicating if the target rectangle has been changed.
        /// </summary>
        public bool TargetChanged { get; set; }

        /// <summary>
        /// Gets and sets the current rectangle calculated by the animation.
        /// </summary>
        public Rect CurrentRect { get; set; }

        /// <summary>
        /// Gets and sets the starting rectangle for animation.
        /// </summary>
        public Rect StartRect { get; set; }

        /// <summary>
        /// Gets and sets the elapsed time since start of bounds animation.
        /// </summary>
        public double ElapsedBoundsTime { get; set; }

        /// <summary>
        /// Gets and sets the elapsed time since start of opacity animation.
        /// </summary>
        public double ElapsedOpacityTime { get; set; }
        #endregion
    }

    /// <summary>
    /// List of element state instances.
    /// </summary>
    public class MetaElementStateList : List<MetaElementState> { }

    /// <summary>
    /// Collection of mappings linking element state to a element instance.
    /// </summary>
    public class MetaElementStateDict : Dictionary<UIElement, MetaElementState> { }
}

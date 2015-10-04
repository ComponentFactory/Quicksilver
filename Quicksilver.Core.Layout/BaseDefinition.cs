// *****************************************************************************
// 
//  © Component Factory Pty Ltd 2011. All rights reserved.
//	The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, PO Box 1504, 
//  Glen Waverley, Vic 3150, Australia and are supplied subject to licence terms.
// 
//  Version 1.0.8.0 	www.ComponentFactory.com
// *****************************************************************************

using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Diagnostics;

namespace ComponentFactory.Quicksilver.Layout
{
    /// <summary>
    /// Base class for definition classes to derive from.
    /// </summary>
    #if SILVERLIGHT
    public abstract partial class BaseDefinition : MeasureDependency
    #else
    public abstract partial class BaseDefinition : FrameworkContentElement
    #endif
    {
        #region Public
        /// <summary>
        /// Add this element into the logical tree.
        /// </summary>
        /// <param name="parent">Interface of logical parent.</param>
        public virtual void AddToLogicalTree(ILogicalParent parent)
        {
            if (parent != null)
                parent.LogicalChildAdd(this);
        }

        /// <summary>
        /// Add this element from the logial tree.
        /// </summary>
        /// <param name="parent">Interface of logical parent.</param>
        public virtual void RemoveFromLogicalTree(ILogicalParent parent)
        {
            if (parent != null)
                parent.LogicalChildRemove(this);
        }
        #endregion
    }
}

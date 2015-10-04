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
    /// Defines common row/column definition details.
    /// </summary>
    public abstract partial class BaseDefinition : FrameworkContentElement
    {
        #region Events
        /// <summary>
        /// Occurs when the element requires a measure.
        /// </summary>
        public event EventHandler NeedMeasure;
        #endregion

        #region Protected
        /// <summary>
        /// Raises the NeedMeasure event.
        /// </summary>
        /// <param name="sender">Source instance.</param>
        /// <param name="e">An EventArgs containing the event data.</param>
        protected virtual void OnNeedMeasure(object sender, EventArgs e)
        {
            EventHandler handler = NeedMeasure;
            if (handler != null)
                handler(this, e);
        }
        
        /// <summary>
        /// Validate if the value is appropriate for a Width property.
        /// </summary>
        /// <param name="value">Value to validate.</param>
        /// <returns>True if valid; otherwise false.</returns>
        protected static bool IsSizePropertyValueValid(object value)
        {
            GridLength length = (GridLength)value;
            return (length.Value >= 0.0);
        }

        /// <summary>
        /// Validate if the value is appropriate for a MinWidth property.
        /// </summary>
        /// <param name="value">Value to validate.</param>
        /// <returns>True if valid; otherwise false.</returns>
        protected static bool IsMinSizePropertyValueValid(object value)
        {
            double num = (double)value;
            return (!double.IsNaN(num) && (num >= 0.0) && !double.IsPositiveInfinity(num));
        }

        /// <summary>
        /// Validate if the value is appropriate for a MaxWidth property.
        /// </summary>
        /// <param name="value">Value to validate.</param>
        /// <returns>True if valid; otherwise false.</returns>
        protected static bool IsMaxSizePropertyValueValid(object value)
        {
            double num = (double)value;
            return (!double.IsNaN(num) && (num >= 0.0));
        }

        /// <summary>
        /// Invoked when a property change requires a measure to occur.
        /// </summary>
        /// <param name="d">Owning object.</param>
        /// <param name="e">Details of property that has changed.</param>
        protected static void OnNeedMeasureOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BaseDefinition sender = (BaseDefinition)d;
            sender.OnNeedMeasure(sender, EventArgs.Empty);
        }
        #endregion
    }
}

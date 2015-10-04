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
using System.Collections.Generic;

namespace ComponentFactory.Quicksilver.Binding
{
    /// <summary>
    /// Specifies the type of a lexical token.
    /// </summary>
    public class EvalResult
    {
        #region Identity
        /// <summary>
        /// Initialize a new instance of the EvalResult class.
        /// </summary>
        public EvalResult()
            : this(TypeCode.Empty, null)
        {
        }

        /// <summary>
        /// Initialize a new instance of the EvalResult class.
        /// </summary>
        /// <param name="type">Type of the result value.</param>
        /// <param name="value">Result value.</param>
        public EvalResult(TypeCode type, object value)
        {
            Type = type;
            Value = value;
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets the type of the result value.
        /// </summary>
        public TypeCode Type { get; set; }

        /// <summary>
        /// Gets and sets the result value.
        /// </summary>
        public object Value { get; set; }
        #endregion
    }
}

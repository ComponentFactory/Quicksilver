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
    /// EvalNode that represents a literal.
    /// </summary>
    public class EvalNodeLiteral : EvalNode
    {
        #region Instance Fields
        private TypeCode _type;
        private object _value;
        private Language _language;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the EvalNodeLiteral class.
        /// </summary>
        /// <param name="type">Type of the literal contained.</param>
        /// <param name="value">Actual literal value.</param>
        /// <param name="language">Language used for evaluation.</param>
        public EvalNodeLiteral(TypeCode type, 
                               object value,
                               Language language)
        {
            _type = type;
            _value = value;
            _language = language;
        }

        /// <summary>
        /// Human readable version of the expression.
        /// </summary>
        /// <returns>String representation of the stored expression.</returns>
        public override string ToString()
        {
            if (_value == null)
                return (_language == Language.CSharp ? "null" : "Nothing");
            else
                return _value.ToString();
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets the type of the literal.
        /// </summary>
        public TypeCode Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Gets and sets the literal value.
        /// </summary>
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        /// <summary>
        /// Evalaute this node and return result.
        /// </summary>
        /// <param name="thisObject">Reference to object that is exposed as 'this'.</param>
        /// <returns>Result value and type of that result.</returns>
        public override EvalResult Evaluate(object thisObject)
        {
            // Result of evaluating a literal is always the literal itself
            return new EvalResult(Type, Value);
        }
        #endregion
    }
}

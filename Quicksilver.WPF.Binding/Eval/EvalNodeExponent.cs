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
    /// EvalNode that represents a binary exponent operation.
    /// </summary>
    public class EvalNodeExponent : EvalNode
    {
        #region Instance Fields
        private Language _language;
        #endregion

        #region Identity
        /// <summary>
        /// Instantiate a new instance of the EvalNodeExponent node.
        /// </summary>
        /// <param name="child">Specifies the first child node.</param>
        /// <param name="language">Language syntax and semantics to use.</param>
        public EvalNodeExponent(EvalNode child,
                                Language language)
        {
            _language = language;
            Append(child);
        }

        /// <summary>
        /// Human readable version of the algebraic expression.
        /// </summary>
        /// <returns>String representation of the stored expression.</returns>
        public override string ToString()
        {
            return "(" + this[0].ToString() + " ^ " + this[1].ToString() + ")";
        }
        #endregion

        #region Public
        /// <summary>
        /// Evalaute this node and return result.
        /// </summary>
        /// <param name="thisObject">Reference to object that is exposed as 'this'.</param>
        /// <returns>Result value and type of that result.</returns>
        public override EvalResult Evaluate(object thisObject)
        {
            // Get the result from evaluating both children
            EvalResult left = this[0].Evaluate(thisObject);
            EvalResult right = this[1].Evaluate(thisObject);

            // Both sides must be converted to doubles
            if (!ImplicitConverter.CanConvertToDouble(left.Value, _language))
                throw new ApplicationException("Cannot convert '" + left.Type.ToString() + "' type to 'Double' for left side of exponent '^' operation.");

            if (!ImplicitConverter.CanConvertToDouble(right.Value, _language))
                throw new ApplicationException("Cannot convert '" + left.Type.ToString() + "' type to 'Double' for right side of exponent '^' operation.");

            double leftDouble = ImplicitConverter.ConvertToDouble(left.Value, _language);
            double rightDouble = ImplicitConverter.ConvertToDouble(right.Value, _language);
            return new EvalResult(TypeCode.Double, Math.Pow(leftDouble, rightDouble));
        }
        #endregion
    }
}

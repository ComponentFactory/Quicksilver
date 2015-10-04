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
    /// EvalNode that represents the ?? conditional.
    /// </summary>
    public class EvalNodeNullCoalescing : EvalNode
    {
        #region Identity
        /// <summary>
        /// Instantiate a new instance of the EvalNodeNullCoalescing node.
        /// </summary>
        /// <param name="child">Specifies the child node.</param>
        public EvalNodeNullCoalescing(EvalNode child)
        {
            Append(child);
        }

        /// <summary>
        /// Human readable version of the algebraic expression.
        /// </summary>
        /// <returns>String representation of the stored expression.</returns>
        public override string ToString()
        {
            return "(" + this[0].ToString() + "??" + this[1].ToString() + ")";
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
            // Get the result from evaluating the left side of operand
            EvalResult left = this[0].Evaluate(thisObject);

            // We can only handle an object result
            if (left.Type != TypeCode.Object)
                throw new ApplicationException("Null coalescing '??' requires a 'object' type for left operand.");

            // If conditional is not null then return it
            if (left.Value != null)
                return left;
            else
            {
                // Get the result from evaluating the right side operand
                EvalResult right = this[1].Evaluate(thisObject);

                // We can only handle an object result
                if (right.Type != TypeCode.Object)
                    throw new ApplicationException("Null coalescing '??' requires a 'object' type for right operand.");

                return right;
            }
        }
        #endregion
    }
}

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
    /// EvalNode that represents a binary concatenate operation.
    /// </summary>
    public class EvalNodeConcatenate : EvalNode
    {
        #region Identity
        /// <summary>
        /// Instantiate a new instance of the EvalNodeConcatenate node.
        /// </summary>
        /// <param name="child">Specifies the first child node.</param>
        public EvalNodeConcatenate(EvalNode child)
        {
            Append(child);
        }

        /// <summary>
        /// Human readable version of the algebraic expression.
        /// </summary>
        /// <returns>String representation of the stored expression.</returns>
        public override string ToString()
        {
            return "(" + this[0].ToString() + " & " + this[1].ToString() + ")";
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

            // Convert both sides to a string if possible
            string leftString = (left.Value != null ? left.Value.ToString() : string.Empty);
            string rightString = (right.Value != null ? right.Value.ToString() : string.Empty);
            return new EvalResult(TypeCode.String, leftString + rightString);
        }
        #endregion
    }
}

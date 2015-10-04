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
    /// EvalNode that represents the ?: conditional.
    /// </summary>
    public class EvalNodeConditional : EvalNode
    {
        #region Identity
        /// <summary>
        /// Instantiate a new instance of the EvalNodeConditional node.
        /// </summary>
        /// <param name="child1">Specifies the first child.</param>
        /// <param name="child2">Specifies the second child.</param>
        public EvalNodeConditional(EvalNode child1,
                                   EvalNode child2)
        {
            Append(child1);
            Append(child2);
        }

        /// <summary>
        /// Human readable version of the algebraic expression.
        /// </summary>
        /// <returns>String representation of the stored expression.</returns>
        public override string ToString()
        {
            return "(" + this[0].ToString() + "?" + this[1].ToString() + ":" + this[2].ToString() + ")";
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
            // Get the result from evaluating the condition
            EvalResult cond = this[0].Evaluate(thisObject);

            // We can only handle a boolean result
            if (cond.Type != TypeCode.Boolean)
                throw new ApplicationException("Conditional '?:' requires a 'bool' type for evaluating.");

            // Decide which value to return based on boolean result
            if ((bool)cond.Value)
                return this[1].Evaluate(thisObject);
            else
                return this[2].Evaluate(thisObject);
        }
        #endregion
    }
}

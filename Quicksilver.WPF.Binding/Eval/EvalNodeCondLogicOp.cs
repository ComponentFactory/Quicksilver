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
    /// EvalNode that represents a binary conditional logical operation.
    /// </summary>
    public class EvalNodeCondLogicOp : EvalNode
    {
        #region Types
        /// <summary>
        /// Specifies the type of unary operation.
        /// </summary>
        public enum CompareOp
        {
            /// <summary>Conditional logical AND between two children.</summary>
            And,

            /// <summary>Conditional logical OR between two children.</summary>
            Or
        }
        #endregion

        #region Instance Fields
        private CompareOp _operation;
        private Language _language;
        #endregion

        #region Identity
        /// <summary>
        /// Instantiate a new instance of the EvalNodeCondLogicOp node.
        /// </summary>
        /// <param name="operation">Specifies the binary compare to represent.</param>
        /// <param name="child">Specifies the first child node.</param>
        /// <param name="language">Language used for evaluation.</param>
        public EvalNodeCondLogicOp(CompareOp operation,
                                   EvalNode child,
                                   Language language)
        {
            _language = language;
            _operation = operation;
            Append(child);
        }

        /// <summary>
        /// Instantiate a new instance of the EvalNodeCondLogicOp node.
        /// </summary>
        /// <param name="operation">Specifies the binary compare to represent.</param>
        /// <param name="child1">Specifies the first child node.</param>
        /// <param name="child2">Specifies the second child node.</param>
        public EvalNodeCondLogicOp(CompareOp operation,
                                   EvalNode child1,
                                   EvalNode child2)
        {
            _operation = operation;
            Append(child1);
            Append(child2);
        }

        /// <summary>
        /// Human readable version of the algebraic expression.
        /// </summary>
        /// <returns>String representation of the stored expression.</returns>
        public override string ToString()
        {
            string gap = (_language == Language.CSharp ? "" : " ");
            return "(" + this[0].ToString() + gap + OperationString + gap + this[1].ToString() + ")";
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets the conditional logical operation.
        /// </summary>
        public CompareOp Operation
        {
            get { return _operation; }
            set { _operation = value; }
        }

        /// <summary>
        /// Evalaute this node and return result.
        /// </summary>
        /// <param name="thisObject">Reference to object that is exposed as 'this'.</param>
        /// <returns>Result value and type of that result.</returns>
        public override EvalResult Evaluate(object thisObject)
        {
            // Always evaluate the first child
            EvalResult left = this[0].Evaluate(thisObject);

            // We can only handle a boolean result
            if (left.Type != TypeCode.Boolean)
            {
                // Try and perform implicit conversion to a boolean
                if (ImplicitConverter.CanConvertToBoolean(left.Value, _language))
                {
                    left.Type = TypeCode.Boolean;
                    left.Value = ImplicitConverter.ConvertToBoolean(left.Value, _language);
                }
                else
                    throw new ApplicationException("Operator '" + OperationString + "' can only operate on 'bool' types.");
            }

            switch (Operation)
            {
                case CompareOp.Or:
                    // If 'true' then there is no need to evaluate the right side
                    if ((bool)left.Value)
                        return left;
                    break;
                case CompareOp.And:
                    // If 'false' then there is no need to evaluate the right side
                    if (!(bool)left.Value)
                        return left;
                    break;
                default:
                    throw new ApplicationException("Unimplemented conditional logical operator '" + Operation.ToString() + "'.");
            }

            // Need to evaluate the second child
            left = this[1].Evaluate(thisObject);

            // We can only handle a boolean result
            if (left.Type != TypeCode.Boolean)
            {
                // Try and perform implicit conversion to a boolean
                if (ImplicitConverter.CanConvertToBoolean(left.Value, _language))
                {
                    left.Type = TypeCode.Boolean;
                    left.Value = ImplicitConverter.ConvertToBoolean(left.Value, _language);
                }
                else
                    throw new ApplicationException("Operator '" + OperationString + "' can only operate on 'bool' types.");
            }

            // Just return whatever the result is as the result of the conditional operation
            return left;
        }
        #endregion

        #region Private
        private string OperationString
        {
            get
            {
                switch (Operation)
                {
                    case CompareOp.And:
                        return (_language == Language.CSharp ? "&&" : "AndAlso");
                    case CompareOp.Or:
                        return (_language == Language.CSharp ? "||" : "OrElse");
                    default:
                        throw new ApplicationException("Unrecognized conditional logical operator '" + Operation.ToString() + "'.");
                }
            }
        }
        #endregion
    }
}

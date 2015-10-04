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
    /// EvalNode that represents a unary operation.
    /// </summary>
    public class EvalNodeUnaryOp : EvalNode
    {
        #region Types
        /// <summary>
        /// Specifies the type of unary operation.
        /// </summary>
        public enum UnaryOp
        {
            /// <summary>Unary operation that does nothing to child.</summary>
            Plus,

            /// <summary>Unary operation that inverts child numeric value.</summary>
            Minus,

            /// <summary>Unary operation that inverts child logical value.</summary>
            Not,

            /// <summary>Unary operation that inverts child bitwise value.</summary>
            Complement
        }
        #endregion

        #region Instance Fields
        private UnaryOp _operation;
        private Language _language;
        #endregion

        #region Identity
        /// <summary>
        /// Instantiate a new instance of the EvalNodeUnaryOp node.
        /// </summary>
        /// <param name="operation">Specifies the unary operation to represent.</param>
        /// <param name="child">Specifies the child node.</param>
        /// <param name="language">Language used for evaluation.</param>
        public EvalNodeUnaryOp(UnaryOp operation,
                               EvalNode child,
                               Language language)
        {
            _operation = operation;
            _language = language;
            Append(child);
        }

        /// <summary>
        /// Human readable version of the algebraic expression.
        /// </summary>
        /// <returns>String representation of the stored expression.</returns>
        public override string ToString()
        {
            return "(" + OperationString + this[0].ToString() + ")";
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets the unary operation of the node.
        /// </summary>
        public UnaryOp Operation
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
            // Get the result from evaluating the single child
            EvalResult ret = this[0].Evaluate(thisObject);

            switch (Operation)
            {
                case UnaryOp.Plus:
                    return EvaluatePlus(ret);
                case UnaryOp.Minus:
                    return EvaluateMinus(ret);
                case UnaryOp.Not:
                    return EvaluateNot(ret);
                case UnaryOp.Complement:
                    return EvaluateComplement(ret);
            }

            return ret;
        }
        #endregion

        #region Private
        private EvalResult EvaluatePlus(EvalResult ret)
        {
            // Unary plus means we do nothing as long as the type is valid
            switch (ret.Type)
            {
                case TypeCode.Char:
                    if (_language == Language.VBNet)
                        throw new ApplicationException("Operation '+' cannot be applied to operand of type '" + ret.Type.ToString() + "'.");
                    else
                        break;
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    break;
                default:
                    throw new ApplicationException("Operation '+' cannot be applied to operand of type '" + ret.Type.ToString() + "'.");
            }

            return ret;
        }

        private EvalResult EvaluateMinus(EvalResult ret)
        {
            // Processing depends on the type we are provided with
            switch (ret.Type)
            {
                case TypeCode.Byte:
                    ret.Value = 0 - ((Byte)ret.Value);
                    break;
                case TypeCode.SByte:
                    ret.Value = 0 - ((SByte)ret.Value);
                    break;
                case TypeCode.Char:
                    if (_language != Language.VBNet)
                    {
                        ret.Value = 0 - ((Char)ret.Value);
                        break;
                    }
                    else
                        throw new ApplicationException("Operation '-' cannot be applied to operand of type '" + ret.Type.ToString() + "'.");
                case TypeCode.Int16:
                    ret.Value = 0 - ((Int16)ret.Value);
                    break;
                case TypeCode.Int32:
                    ret.Value = 0 - ((Int32)ret.Value);
                    break;
                case TypeCode.Int64:
                    ret.Value = 0 - ((Int64)ret.Value);
                    break;
                case TypeCode.UInt16:
                    // Convert to long and subtract from zero
                    ret.Value = 0 - (Int64)((UInt16)ret.Value);
                    ret.Type = TypeCode.Int64;
                    break;
                case TypeCode.UInt32:
                    // Convert to long and subtract from zero
                    ret.Value = 0 - (Int64)((UInt32)ret.Value);
                    ret.Type = TypeCode.Int64;
                    break;
                case TypeCode.Single:
                    ret.Value = 0 - ((Single)ret.Value);
                    break;
                case TypeCode.Double:
                    ret.Value = 0 - ((Double)ret.Value);
                    break;
                case TypeCode.Decimal:
                    ret.Value = 0 - ((Decimal)ret.Value);
                    break;
                default:
                    throw new ApplicationException("Operation '-' cannot be applied to operand of type '" + ret.Type.ToString() + "'.");
            }

            return ret;
        }

        private EvalResult EvaluateNot(EvalResult ret)
        {
            // Unary plus means we do nothing as long as the type is valid
            switch (ret.Type)
            {
                case TypeCode.Boolean:
                    ret.Value = !(bool)ret.Value;
                    break;
                default:
                    throw new ApplicationException("Logical invert cannot be applied to operand of type '" + ret.Type.ToString() + "'.");
            }

            return ret;
        }

        private EvalResult EvaluateComplement(EvalResult ret)
        {
            // Unary plus means we do nothing as long as the type is valid
            switch (ret.Type)
            {
                case TypeCode.Byte:
                    ret.Value = ~(Byte)ret.Value;
                    break;
                case TypeCode.SByte:
                    ret.Value = ~(SByte)ret.Value;
                    break;
                case TypeCode.Char:
                    if (_language != Language.VBNet)
                        ret.Value = ~(Char)ret.Value;
                    else
                        throw new ApplicationException("Operation '~' cannot be applied to operand of type '" + ret.Type.ToString() + "'.");
                    break;
                case TypeCode.Int16:
                    ret.Value = ~(Int16)ret.Value;
                    break;
                case TypeCode.Int32:
                    ret.Value = ~(Int32)ret.Value;
                    break;
                case TypeCode.Int64:
                    ret.Value = ~(Int64)ret.Value;
                    break;
                case TypeCode.UInt16:
                    ret.Value = ~(UInt16)ret.Value;
                    break;
                case TypeCode.UInt32:
                    ret.Value = ~(UInt32)ret.Value;
                    break;
                case TypeCode.UInt64:
                    ret.Value = ~(UInt64)ret.Value;
                    break;
                default:
                    throw new ApplicationException("Operation '~' cannot be applied to operand of type '" + ret.Type.ToString() + "'.");
            }

            return ret;
        }

        private string OperationString
        {
            get
            {
                switch (Operation)
                {
                    case UnaryOp.Plus:
                        return "+";
                    case UnaryOp.Minus:
                        return "-";
                    case UnaryOp.Not:
                        return (_language == Language.CSharp ? "!" : "Not ");
                    case UnaryOp.Complement:
                        return "~";
                    default:
                        throw new ApplicationException("Unrecognized unary operation '" + Operation.ToString() + "'.");
                }
            }
        }
        #endregion
    }
}

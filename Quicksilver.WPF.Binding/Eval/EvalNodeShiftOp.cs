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
    /// EvalNode that represents a bitwise shift operation.
    /// </summary>
    public class EvalNodeShiftOp : EvalNode
    {
        #region Types
        /// <summary>
        /// Specifies the type of shift operation.
        /// </summary>
        public enum ShiftOp
        {
            /// <summary>Left bitwise shift.</summary>
            Left,

            /// <summary>Right bitwise shift.</summary>
            Right,
        }
        #endregion

        #region Instance Fields
        private ShiftOp _operation;
        private Language _language;
        #endregion

        #region Identity
        /// <summary>
        /// Instantiate a new instance of the EvalNodeShiftOp node.
        /// </summary>
        /// <param name="operation">Specifies the shift operation to represent.</param>
        /// <param name="child">Specifies the first child node.</param>
        /// <param name="language">Language used for evaluation.</param>
        public EvalNodeShiftOp(ShiftOp operation,
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
            string gap = (_language == Language.CSharp ? "" : " ");
            return "(" + this[0].ToString() + gap + OperationString + gap + this[1].ToString() + ")";
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets the bitwise shift operation of the node.
        /// </summary>
        public ShiftOp Operation
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
            // Get the result from evaluating both children
            EvalResult left = this[0].Evaluate(thisObject);
            EvalResult right = this[1].Evaluate(thisObject);

            int count;

            if (_language == Language.CSharp)
            {
                // The shift count is always an Int32, so convert anything smaller to an Int32
                switch (right.Type)
                {
                    case TypeCode.Char:
                        count = (Int32)(Char)right.Value;
                        break;
                    case TypeCode.Byte:
                        count = (Int32)(Byte)right.Value;
                        break;
                    case TypeCode.SByte:
                        count = (Int32)(SByte)right.Value;
                        break;
                    case TypeCode.UInt16:
                        count = (Int32)(UInt16)right.Value;
                        break;
                    case TypeCode.Int16:
                        count = (Int32)(Int16)right.Value;
                        break;
                    case TypeCode.Int32:
                        count = (Int32)right.Value;
                        break;
                    default:
                        throw new ApplicationException("Shift operation '" + OperationString + "' cannot convert the shift count from type '" + right.Type.ToString() + "' to 'Int32'.");
                }
            }
            else
            {
                if (ImplicitConverter.CanConvertToInt32(right.Value, Language.VBNet))
                    count = ImplicitConverter.ConvertToInt32(right.Value, Language.VBNet);
                else
                    throw new ApplicationException("Shift operation '" + OperationString + "' cannot convert the shift count to a 'Int32' value.");
            }

            // Perform the actual shift operation
            switch (left.Type)
            {
                case TypeCode.Byte:
                    return EvalShiftOp((Int32)(Byte)left.Value, count);
                case TypeCode.SByte:
                    return EvalShiftOp((Int32)(SByte)left.Value, count);
                case TypeCode.Char:
                    if (_language == Language.CSharp)
                        return EvalShiftOp((Int32)(Char)left.Value, count);
                    break;
                case TypeCode.UInt16:
                    return EvalShiftOp((Int32)(UInt16)left.Value, count);
                case TypeCode.Int16:
                    return EvalShiftOp((Int32)(Int16)left.Value, count);
                case TypeCode.Int32:
                    return EvalShiftOp((Int32)(Int32)left.Value, count);
                case TypeCode.UInt32:
                    return EvalShiftOp((UInt32)left.Value, count);
                case TypeCode.Int64:
                    return EvalShiftOp((Int64)left.Value, count);
                case TypeCode.UInt64:
                    return EvalShiftOp((UInt64)left.Value, count);
                case TypeCode.Boolean:
                    if (_language == Language.VBNet)
                        return EvalShiftOp((Int16)ImplicitConverter.ConvertToInt16(left.Value, Language.VBNet), count);
                    break;
                case TypeCode.Single:
                    if (_language == Language.VBNet)
                        return EvalShiftOp((Int64)(Single)left.Value, count);
                    break;
                case TypeCode.Double:
                    if (_language == Language.VBNet)
                        return EvalShiftOp((Int64)(Double)left.Value, count);
                    break;
                case TypeCode.Decimal:
                    if (_language == Language.VBNet)
                        return EvalShiftOp((Int64)(Decimal)left.Value, count);
                    break;
            }

            throw new ApplicationException("Shift operation '" + OperationString + "' cannot shift type '" + left.Type.ToString() + "'.");
        }
        #endregion

        #region Private
        private EvalResult EvalShiftOp(Int16 x, int count)
        {
            EvalResult ret = new EvalResult(TypeCode.Int16, null);

            switch (Operation)
            {
                case ShiftOp.Left:
                    ret.Value = (Int16)(x << count);
                    break;
                case ShiftOp.Right:
                    ret.Value = (Int16)(x >> count);
                    break;
                default:
                    throw new ApplicationException("Unrecognized 'Int32' type shift operation '" + OperationString);
            }

            return ret;
        }

        private EvalResult EvalShiftOp(Int32 x, int count)
        {
            EvalResult ret = new EvalResult(TypeCode.Int32, null);

            switch (Operation)
            {
                case ShiftOp.Left:
                    ret.Value = x << count;
                    break;
                case ShiftOp.Right:
                    ret.Value = x >> count;
                    break;
                default:
                    throw new ApplicationException("Unrecognized 'Int32' type shift operation '" + OperationString);
            }

            return ret;
        }

        private EvalResult EvalShiftOp(UInt32 x, int count)
        {
            EvalResult ret = new EvalResult(TypeCode.UInt32, null);

            switch (Operation)
            {
                case ShiftOp.Left:
                    ret.Value = x << count;
                    break;
                case ShiftOp.Right:
                    ret.Value = x >> count;
                    break;
                default:
                    throw new ApplicationException("Unrecognized 'UInt32' type shift operation '" + OperationString);
            }

            return ret;
        }

        private EvalResult EvalShiftOp(Int64 x, int count)
        {
            EvalResult ret = new EvalResult(TypeCode.Int64, null);

            switch (Operation)
            {
                case ShiftOp.Left:
                    ret.Value = x << count;
                    break;
                case ShiftOp.Right:
                    ret.Value = x >> count;
                    break;
                default:
                    throw new ApplicationException("Unrecognized 'Int64' type shift operation '" + OperationString);
            }

            return ret;
        }

        private EvalResult EvalShiftOp(UInt64 x, int count)
        {
            EvalResult ret = new EvalResult(TypeCode.UInt64, null);

            switch (Operation)
            {
                case ShiftOp.Left:
                    ret.Value = x << count;
                    break;
                case ShiftOp.Right:
                    ret.Value = x >> count;
                    break;
                default:
                    throw new ApplicationException("Unrecognized 'UInt64' type shift operation '" + OperationString);
            }

            return ret;
        }

        private string OperationString
        {
            get
            {
                switch (Operation)
                {
                    case ShiftOp.Left:
                        return "<<";
                    case ShiftOp.Right:
                        return ">>";
                    default:
                        throw new ApplicationException("Unrecognized bitwise shift operation '" + Operation.ToString() + "'.");
                }
            }
        }
        #endregion
    }
}

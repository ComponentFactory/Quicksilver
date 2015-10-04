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
    /// EvalNode that represents a binary operation.
    /// </summary>
    public class EvalNodeBinaryOp : EvalNode
    {
        #region Types
        /// <summary>
        /// Specifies the type of unary operation.
        /// </summary>
        public enum BinaryOp
        {
            /// <summary>Binary operation to add two children.</summary>
            Add = 0x0001,

            /// <summary>Binary operation to subtract two children.</summary>
            Subtract = 0x0002,

            /// <summary>Binary operation to multiply two children.</summary>
            Multiply = 0x0003,

            /// <summary>Binary operation to divide two children.</summary>
            Divide = 0x0004,

            /// <summary>Binary operation to find remainder of dividing two children.</summary>
            Remainder = 0x0005,

            /// <summary>Binary operation to divide two children and use only integer result.</summary>
            IntegerDivide = 0x0006,

            /// <summary>Binary operation to logical OR two children.</summary>
            LogicalOr = 0x0007,

            /// <summary>Binary operation to logical XOR two children.</summary>
            LogicalXor = 0x0008,

            /// <summary>Binary operation to logical AND two children.</summary>
            LogicalAnd = 0x0009,

            /// <summary>Binary compare '==' add two children.</summary>
            Equal = 0x0100,

            /// <summary>Binary compare '!=' add two children.</summary>
            NotEqual = 0x0101,

            /// <summary>Binary compare '&lt;' add two children.</summary>
            LessThan = 0x0102,

            /// <summary>Binary compare '&lt;=' add two children.</summary>
            LessThanEqual = 0x0103,

            /// <summary>Binary compare '&gt;' add two children.</summary>
            GreaterThan = 0x0104,

            /// <summary>Binary compare '&gt;=' add two children.</summary>
            GreaterThanEqual = 0x0105,

            /// <summary>Bit pattern that all comparison operations share.</summary>
            CompareOps = 0x0100,
        }
        #endregion

        #region Instance Fields
        private BinaryOp _operation;
        private Language _language;
        #endregion

        #region Identity
        /// <summary>
        /// Instantiate a new instance of the EvalNodeBinaryOp node.
        /// </summary>
        /// <param name="operation">Specifies the binary operation to represent.</param>
        /// <param name="child">Specifies the first child node.</param>
        /// <param name="language">Language used for evaluation.</param>
        public EvalNodeBinaryOp(BinaryOp operation,
                                EvalNode child,
                                Language language)
        {
            _operation = operation;
            _language = language;
            Append(child);
        }

        /// <summary>
        /// Instantiate a new instance of the EvalNodeBinaryOp node.
        /// </summary>
        /// <param name="operation">Specifies the binary operation to represent.</param>
        /// <param name="child1">Specifies the first child node.</param>
        /// <param name="child2">Specifies the second child node.</param>
        public EvalNodeBinaryOp(BinaryOp operation,
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
        /// Gets and sets the binary operation of the node.
        /// </summary>
        public BinaryOp Operation
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
            
            // Evaluate by examing the types involved and the operation
            return Eval(left, right);
        }
        #endregion

        #region Private
        private EvalResult Eval(EvalResult left, EvalResult right)
        {
            // If either side is a string then we perform string concatenation
            if ((left.Type == TypeCode.String) || (right.Type == TypeCode.String))
                return EvalString(left, right);
            else
            {
                // If either side is a real number
                if ((left.Type == TypeCode.Single) || (left.Type == TypeCode.Double) ||
                    (right.Type == TypeCode.Single) || (right.Type == TypeCode.Double))
                {
                    return EvalReal(left, right);
                }
                else
                {
                    // If either side is a decimal number
                    if ((left.Type == TypeCode.Decimal) || (right.Type == TypeCode.Decimal))
                        return EvalDecimal(left, right);
                    else
                    {
                        // If either side is a boolean
                        if ((left.Type == TypeCode.Boolean) || (right.Type == TypeCode.Boolean))
                            return EvalBoolean(left, right);
                        else
                        {
                            // If either side is an object
                            if ((left.Type == TypeCode.Object) || (right.Type == TypeCode.Object))
                                return EvalObject(left, right);
                            else
                            {
                                // Both sides must be an integer
                                return EvalInteger(left, right);
                            }
                        }
                    }
                }
            }
        }

        private EvalResult EvalString(EvalResult left, EvalResult right)
        {
            // VB.NET handles a string with another value by converting both sides to double
            if (_language == Language.VBNet)
            {
                switch (Operation)
                {
                    case BinaryOp.Add:
                    case BinaryOp.Equal:
                    case BinaryOp.NotEqual:
                        // If one of the sides is not a string...
                        if ((left.Type != TypeCode.String) || (right.Type != TypeCode.String))
                        {
                            // If the non-string is a character then we convert the character to a string
                            if ((left.Type == TypeCode.Char) || (right.Type == TypeCode.Char))
                            {
                                if (left.Type == TypeCode.Char)
                                {
                                    left.Value = left.Value.ToString();
                                    left.Type = TypeCode.String;
                                }

                                if (right.Type == TypeCode.Char)
                                {
                                    right.Value = right.Value.ToString();
                                    right.Type = TypeCode.String;
                                }

                                // Try conversion again
                                return Eval(left, right);
                            }
                            else
                            {
                                // We need to convert both sides to a double
                                if (ImplicitConverter.CanConvertToDouble(left.Value, Language.VBNet) &&
                                    ImplicitConverter.CanConvertToDouble(right.Value, Language.VBNet))
                                {
                                    // Convert both sides to a double
                                    left.Value = ImplicitConverter.ConvertToDouble(left.Value, Language.VBNet);
                                    right.Value = ImplicitConverter.ConvertToDouble(right.Value, Language.VBNet);

                                    left.Type = TypeCode.Double;
                                    right.Type = TypeCode.Double;

                                    // Try conversion again
                                    return Eval(left, right);

                                }
                                else
                                    throw new ApplicationException("Operator '" + OperationString + "' cannot be applied to operands of type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                            }
                        }
                        break;
                }
            }
            
            switch(Operation)
            {
                case BinaryOp.Add:
                    StringBuilder result = new StringBuilder();

                    if (left.Value != null)
                        result.Append(left.Value.ToString());
                    else
                        throw new ApplicationException("Operator '" + OperationString + "' cannot be applied to operands of type 'null'.");

                    if (right.Value != null)
                        result.Append(right.Value.ToString());
                    else
                        throw new ApplicationException("Operator '" + OperationString + "' cannot be applied to operands of type 'null'.");

                    // Return the concatenated strings as the result
                    return new EvalResult(TypeCode.String, result.ToString());
                case BinaryOp.Equal:
                case BinaryOp.NotEqual:
                    // Both sides must be strings
                    if ((left.Type == TypeCode.String) && (left.Type == TypeCode.String))
                    {
                        // Return result of comparing the two strings
                        bool compare = ((string)left.Value == (string)right.Value);
                        return new EvalResult(TypeCode.Boolean, (Operation == BinaryOp.Equal ? compare : !compare));
                    }
                    else
                        throw new ApplicationException("Operator '" + OperationString + "' cannot be applied to operands of type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                default:
                    throw new ApplicationException("Operator '" + OperationString + "' cannot be applied to operands of type 'string'.");
            }
        }

        private EvalResult EvalReal(EvalResult left, EvalResult right)
        {
            // Return type is boolean for comparisons otherwise a float unless either side contains a double
            bool hasDouble = (left.Type == TypeCode.Double) || (right.Type == TypeCode.Double);
            EvalResult ret = new EvalResult(IsComparison ? TypeCode.Boolean : (hasDouble ? TypeCode.Double : TypeCode.Single), null);

            switch (left.Type)
            {
                case TypeCode.Boolean:
                    if (_language == Language.CSharp)
                        throw new ApplicationException("Operator '" + OperationString + "' cannot be applied to operands of type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");

                    if (hasDouble)
                        ret.Value = EvalBinaryOp(ImplicitConverter.ConvertToDouble(left.Value, Language.VBNet), (double)right.Value);
                    else
                        ret.Value = EvalBinaryOp(ImplicitConverter.ConvertToSingle(left.Value, Language.VBNet), (float)right.Value);
                    break;
                case TypeCode.Byte:
                    if (hasDouble)
                        ret.Value = EvalBinaryOp((double)(Byte)left.Value, (double)right.Value);
                    else
                        ret.Value = EvalBinaryOp((float)(Byte)left.Value, (float)right.Value);
                    break;
                case TypeCode.SByte:
                    if (hasDouble)
                        ret.Value = EvalBinaryOp((double)(SByte)left.Value, (double)right.Value);
                    else
                        ret.Value = EvalBinaryOp((float)(SByte)left.Value, (float)right.Value);
                    break;
                case TypeCode.Char:
                    if (_language == Language.VBNet)
                        throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                    else
                    {
                        if (hasDouble)
                            ret.Value = EvalBinaryOp((double)(Char)left.Value, (double)right.Value);
                        else
                            ret.Value = EvalBinaryOp((float)(Char)left.Value, (float)right.Value);
                    }
                    break;
                case TypeCode.UInt16:
                    if (hasDouble)
                        ret.Value = EvalBinaryOp((double)(UInt16)left.Value, (double)right.Value);
                    else
                        ret.Value = EvalBinaryOp((float)(UInt16)left.Value, (float)right.Value);
                    break;
                case TypeCode.UInt32:
                    if (hasDouble)
                        ret.Value = EvalBinaryOp((double)(UInt32)left.Value, (double)right.Value);
                    else
                        ret.Value = EvalBinaryOp((float)(UInt32)left.Value, (float)right.Value);
                    break;
                case TypeCode.UInt64:
                    if (hasDouble)
                        ret.Value = EvalBinaryOp((double)(UInt64)left.Value, (double)right.Value);
                    else
                        ret.Value = EvalBinaryOp((float)(UInt64)left.Value, (float)right.Value);
                    break;
                case TypeCode.Int16:
                    if (hasDouble)
                        ret.Value = EvalBinaryOp((double)(Int16)left.Value, (double)right.Value);
                    else
                        ret.Value = EvalBinaryOp((float)(Int16)left.Value, (float)right.Value);
                    break;
                case TypeCode.Int32:
                    if (hasDouble)
                        ret.Value = EvalBinaryOp((double)(Int32)left.Value, (double)right.Value);
                    else
                        ret.Value = EvalBinaryOp((float)(Int32)left.Value, (float)right.Value);
                    break;
                case TypeCode.Int64:
                    if (hasDouble)
                        ret.Value = EvalBinaryOp((double)(Int64)left.Value, (double)right.Value);
                    else
                        ret.Value = EvalBinaryOp((float)(Int64)left.Value, (float)right.Value);
                    break;
                case TypeCode.Single:
                case TypeCode.Double:
                    switch (right.Type)
                    {
                        case TypeCode.Boolean:
                            if (_language == Language.CSharp)
                                throw new ApplicationException("Operator '" + OperationString + "' cannot be applied to operands of type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");

                            if (hasDouble)
                                ret.Value = EvalBinaryOp((double)left.Value, ImplicitConverter.ConvertToDouble(right.Value, Language.VBNet));
                            else
                                ret.Value = EvalBinaryOp((float)left.Value, ImplicitConverter.ConvertToSingle(right.Value, Language.VBNet));
                            break;
                        case TypeCode.Byte:
                            if (hasDouble)
                                ret.Value = EvalBinaryOp((double)left.Value, (double)(Byte)right.Value);
                            else
                                ret.Value = EvalBinaryOp((float)left.Value, (float)(Byte)right.Value);
                            break;
                        case TypeCode.SByte:
                            if (hasDouble)
                                ret.Value = EvalBinaryOp((double)left.Value, (double)(SByte)right.Value);
                            else
                                ret.Value = EvalBinaryOp((float)left.Value, (float)(SByte)right.Value);
                            break;
                        case TypeCode.Char:
                            if (_language == Language.VBNet)
                                throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                            else
                            {
                                if (hasDouble)
                                    ret.Value = EvalBinaryOp((double)left.Value, (double)(Char)right.Value);
                                else
                                    ret.Value = EvalBinaryOp((float)left.Value, (float)(Char)right.Value);
                            }
                            break;
                        case TypeCode.UInt16:
                            if (hasDouble)
                                ret.Value = EvalBinaryOp((double)left.Value, (double)(UInt16)right.Value);
                            else
                                ret.Value = EvalBinaryOp((float)left.Value, (float)(UInt16)right.Value);
                            break;
                        case TypeCode.UInt32:
                            if (hasDouble)
                                ret.Value = EvalBinaryOp((double)left.Value, (double)(UInt32)right.Value);
                            else
                                ret.Value = EvalBinaryOp((float)left.Value, (float)(UInt32)right.Value);
                            break;
                        case TypeCode.UInt64:
                            if (hasDouble)
                                ret.Value = EvalBinaryOp((double)left.Value, (double)(UInt64)right.Value);
                            else
                                ret.Value = EvalBinaryOp((float)left.Value, (float)(UInt64)right.Value);
                            break;
                        case TypeCode.Int16:
                            if (hasDouble)
                                ret.Value = EvalBinaryOp((double)left.Value, (double)(Int16)right.Value);
                            else
                                ret.Value = EvalBinaryOp((float)left.Value, (float)(Int16)right.Value);
                            break;
                        case TypeCode.Int32:
                            if (hasDouble)
                                ret.Value = EvalBinaryOp((double)left.Value, (double)(Int32)right.Value);
                            else
                                ret.Value = EvalBinaryOp((float)left.Value, (float)(Int32)right.Value);
                            break;
                        case TypeCode.Int64:
                            if (hasDouble)
                                ret.Value = EvalBinaryOp((double)left.Value, (double)(Int64)right.Value);
                            else
                                ret.Value = EvalBinaryOp((float)left.Value, (float)(Int64)right.Value);
                            break;
                        case TypeCode.Single:
                            if (hasDouble)
                                ret.Value = EvalBinaryOp((double)left.Value, (double)(float)right.Value);
                            else
                                ret.Value = EvalBinaryOp((float)left.Value, (float)right.Value);
                            break;
                        case TypeCode.Double:
                            if (left.Type == TypeCode.Single)
                                ret.Value = EvalBinaryOp((double)(float)left.Value, (double)right.Value);
                            else
                                ret.Value = EvalBinaryOp((double)left.Value, (double)right.Value);
                            break;
                        default:
                            throw new ApplicationException("Operator '" + OperationString + "' cannot be applied to operands of type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                    }
                    break;
                default:
                    throw new ApplicationException("Operator '" + OperationString + "' cannot be applied to operands of type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
            }

            return ret;
        }

        private EvalResult EvalDecimal(EvalResult left, EvalResult right)
        {
            // Result is Decimal for non-comparison operations, otherwise boolean
            EvalResult ret = new EvalResult(IsComparison ? TypeCode.Boolean : TypeCode.Decimal, null);

            switch (left.Type)
            {
                case TypeCode.Boolean:
                    if (_language == Language.CSharp)
                        throw new ApplicationException("Operator '" + OperationString + "' cannot be applied to operands of type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");

                    ret.Value = EvalBinaryOp(ImplicitConverter.ConvertToDecimal(left.Value, Language.VBNet), (decimal)right.Value);
                    break;
                case TypeCode.Byte:
                    ret.Value = EvalBinaryOp((decimal)(Byte)left.Value, (decimal)right.Value);
                    break;
                case TypeCode.SByte:
                    ret.Value = EvalBinaryOp((decimal)(SByte)left.Value, (decimal)right.Value);
                    break;
                case TypeCode.Char:
                    if (_language == Language.VBNet)
                        throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                    else
                        ret.Value = EvalBinaryOp((decimal)(Char)left.Value, (decimal)right.Value);
                    break;
                case TypeCode.UInt16:
                    ret.Value = EvalBinaryOp((decimal)(UInt16)left.Value, (decimal)right.Value);
                    break;
                case TypeCode.UInt32:
                    ret.Value = EvalBinaryOp((decimal)(UInt32)left.Value, (decimal)right.Value);
                    break;
                case TypeCode.UInt64:
                    ret.Value = EvalBinaryOp((decimal)(UInt64)left.Value, (decimal)right.Value);
                    break;
                case TypeCode.Int16:
                    ret.Value = EvalBinaryOp((decimal)(Int16)left.Value, (decimal)right.Value);
                    break;
                case TypeCode.Int32:
                    ret.Value = EvalBinaryOp((decimal)(Int32)left.Value, (decimal)right.Value);
                    break;
                case TypeCode.Int64:
                    ret.Value = EvalBinaryOp((decimal)(Int64)left.Value, (decimal)right.Value);
                    break;
                case TypeCode.Decimal:
                    switch (right.Type)
                    {
                        case TypeCode.Boolean:
                            if (_language == Language.CSharp)
                                throw new ApplicationException("Operator '" + OperationString + "' cannot be applied to operands of type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");

                            ret.Value = EvalBinaryOp((decimal)left.Value, ImplicitConverter.ConvertToDecimal(right.Value, Language.VBNet));
                            break;
                        case TypeCode.Byte:
                            ret.Value = EvalBinaryOp((decimal)left.Value, (decimal)(Byte)right.Value);
                            break;
                        case TypeCode.SByte:
                            ret.Value = EvalBinaryOp((decimal)left.Value, (decimal)(SByte)right.Value);
                            break;
                        case TypeCode.Char:
                            if (_language == Language.VBNet)
                                throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                            else
                                ret.Value = EvalBinaryOp((decimal)left.Value, (decimal)(Char)right.Value);
                            break;
                        case TypeCode.UInt16:
                            ret.Value = EvalBinaryOp((decimal)left.Value, (decimal)(UInt16)right.Value);
                            break;
                        case TypeCode.UInt32:
                            ret.Value = EvalBinaryOp((decimal)left.Value, (decimal)(UInt32)right.Value);
                            break;
                        case TypeCode.UInt64:
                            ret.Value = EvalBinaryOp((decimal)left.Value, (decimal)(UInt64)right.Value);
                            break;
                        case TypeCode.Int16:
                            ret.Value = EvalBinaryOp((decimal)left.Value, (decimal)(Int16)right.Value);
                            break;
                        case TypeCode.Int32:
                            ret.Value = EvalBinaryOp((decimal)left.Value, (decimal)(Int32)right.Value);
                            break;
                        case TypeCode.Int64:
                            ret.Value = EvalBinaryOp((decimal)left.Value, (decimal)(Int64)right.Value);
                            break;
                        case TypeCode.Single:
                            ret.Value = EvalBinaryOp((decimal)left.Value, (decimal)right.Value);
                            break;
                        case TypeCode.Decimal:
                            ret.Value = EvalBinaryOp((decimal)left.Value, (decimal)right.Value);
                            break;
                        default:
                            throw new ApplicationException("Operator '" + OperationString + "' cannot be applied to operands of type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                    }
                    break;
                default:
                    throw new ApplicationException("Operator '" + OperationString + "' cannot be applied to operands of type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
            }

            return ret;
        }

        private EvalResult EvalBoolean(EvalResult left, EvalResult right)
        {
            // VB.NET converts boolean values to Int16 for processing unless both are boolean and it is a logical operation
            if ((_language == Language.VBNet) && !IsLogical)
            {
                if (left.Type == TypeCode.Boolean)
                {
                    left.Value = ImplicitConverter.ConvertToInt16(left.Value, Language.VBNet);
                    left.Type = TypeCode.Int16;
                }

                if (right.Type == TypeCode.Boolean)
                {
                    right.Value = ImplicitConverter.ConvertToInt16(right.Value, Language.VBNet);
                    right.Type = TypeCode.Int16;
                }

                // Try conversion again
                return Eval(left, right);
            }

            // Result is always a boolean type
            EvalResult ret = new EvalResult(TypeCode.Boolean, null);

            switch (left.Type)
            {
                case TypeCode.Boolean:
                    switch (right.Type)
                    {
                        case TypeCode.Boolean:
                            ret.Value = EvalBinaryOp((bool)left.Value, (bool)right.Value);
                            break;
                        default:
                            throw new ApplicationException("Operator '" + OperationString + "' cannot be applied to operands of type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                    }
                    break;
                default:
                    throw new ApplicationException("Operator '" + OperationString + "' cannot be applied to operands of type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
            }

            return ret;
        }

        private EvalResult EvalObject(EvalResult left, EvalResult right)
        {
            // We can only handle both sides being object type
            if ((left.Type == TypeCode.Object) && (right.Type == TypeCode.Object))
            {
                // We can only handle == and != operations
                switch (Operation)
                {
                    case BinaryOp.Equal:
                        return new EvalResult(TypeCode.Boolean, left.Value == right.Value);
                    case BinaryOp.NotEqual:
                        return new EvalResult(TypeCode.Boolean, left.Value != right.Value);
                }
            }

            throw new ApplicationException("Operator '" + OperationString + "' cannot be applied to operands of type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
        }

        private EvalResult EvalInteger(EvalResult left, EvalResult right)
        {
            // Find largest integer type from both sides
            TypeCode largest = (TypeCode)Math.Max((int)left.Type, (int)right.Type);

            // VB.NET returns a result that is the size of the largest
            if (_language == Language.VBNet)
            {
                if (largest <= TypeCode.Char)
                    return EvalIntegerCharVB(left, right);
                else if (largest <= TypeCode.SByte)
                    return EvalIntegerSByteVB(left, right);
                else if (largest == TypeCode.Byte)
                    return EvalIntegerByteVB(left, right);
                else if (largest == TypeCode.Int16)
                    return EvalIntegerInt16VB(left, right);
                else if (largest == TypeCode.UInt16)
                    return EvalIntegerUInt16VB(left, right);
            }

            // C# always returns Int32 as the smallest result
            if (largest <= TypeCode.Int32)
                return EvalIntegerInt32(left, right);
            else if (largest == TypeCode.UInt32)
                return EvalIntegerUInt32(left, right);
            else if (largest == TypeCode.Int64)
                return EvalIntegerInt64(left, right);
            else
                return EvalIntegerUInt64(left, right);
        }

        private EvalResult EvalIntegerCharVB(EvalResult left, EvalResult right)
        {
            // Result is a string for non-comparison operations, otherwise boolean
            EvalResult ret = new EvalResult(IsComparison ? TypeCode.Boolean : TypeCode.String, null);

            // Special case the addition of two chars to be a string
            if (Operation == BinaryOp.Add)
                ret.Value = left.Value.ToString() + right.Value.ToString();
            else
                ret.Value = EvalBinaryOp((Char)left.Value, (Char)right.Value);

            return ret;
        }

        private EvalResult EvalIntegerSByteVB(EvalResult left, EvalResult right)
        {
            // Result is SByte for non-comparison operations, otherwise boolean
            EvalResult ret = new EvalResult(IsComparison ? TypeCode.Boolean : TypeCode.SByte, null);

            // SByte is the smallest integer type, so both sides must be SByte
            ret.Value = EvalBinaryOp((SByte)left.Value, (SByte)right.Value);
            return ret;
        }

        private EvalResult EvalIntegerByteVB(EvalResult left, EvalResult right)
        {
            // Result is Byte or Int16 for non-comparison operations, otherwise boolean
            EvalResult ret = new EvalResult(IsComparison ? TypeCode.Boolean : TypeCode.Int16, null);

            // Either the left or the right must be exactly Byte with the other a SByte
            switch (left.Type)
            {
                case TypeCode.Byte:
                    ret.Value = EvalBinaryOp((Int16)(Byte)left.Value, (Int16)(SByte)right.Value);
                    break;
                case TypeCode.SByte:
                    switch (right.Type)
                    {
                        case TypeCode.Byte:
                            ret.Value = EvalBinaryOp((Int16)(SByte)left.Value, (Int16)(Byte)right.Value);
                            break;
                        case TypeCode.SByte:
                            ret.Type = TypeCode.Byte;
                            ret.Value = EvalBinaryOp((Byte)left.Value, (Byte)right.Value);
                            break;
                        default:
                            throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                    }
                    break;
                default:
                    throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
            }

            return ret;
        }

        private EvalResult EvalIntegerInt16VB(EvalResult left, EvalResult right)
        {
            // Result is Int16 for non-comparison operations, otherwise boolean
            EvalResult ret = new EvalResult(IsComparison ? TypeCode.Boolean : TypeCode.Int16, null);

            // Either the left or the right must be exactly Int16 with the other a less than Int16
            switch (left.Type)
            {
                case TypeCode.Byte:
                    ret.Value = EvalBinaryOp((Int16)(Byte)left.Value, (Int16)right.Value);
                    break;
                case TypeCode.SByte:
                    ret.Value = EvalBinaryOp((Int16)(SByte)left.Value, (Int16)right.Value);
                    break;
                case TypeCode.Int16:
                    switch (right.Type)
                    {
                        case TypeCode.Byte:
                            ret.Value = EvalBinaryOp((Int16)left.Value, (Int16)(Byte)right.Value);
                            break;
                        case TypeCode.SByte:
                            ret.Value = EvalBinaryOp((Int16)left.Value, (Int16)(SByte)right.Value);
                            break;
                        case TypeCode.Int16:
                            ret.Value = EvalBinaryOp((Int16)left.Value, (Int16)right.Value);
                            break;
                        default:
                            throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                    }
                    break;
                default:
                    throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
            }

            return ret;
        }

        private EvalResult EvalIntegerUInt16VB(EvalResult left, EvalResult right)
        {
            // Result is UInt16 or Int32 for non-comparison operations, otherwise boolean
            EvalResult ret = new EvalResult(IsComparison ? TypeCode.Boolean : TypeCode.UInt16, null);

            // Either the left or the right must be exactly UInt16 with the other a less than UInt16
            switch (left.Type)
            {
                case TypeCode.Byte:
                    ret.Value = EvalBinaryOp((UInt16)(Byte)left.Value, (UInt16)right.Value);
                    break;
                case TypeCode.SByte:
                    ret.Type = TypeCode.Int32;
                    ret.Value = EvalBinaryOp((Int32)(SByte)left.Value, (Int32)(UInt16)right.Value);
                    break;
                case TypeCode.Int16:
                    ret.Type = TypeCode.Int32;
                    ret.Value = EvalBinaryOp((Int32)(Int16)left.Value, (Int32)(UInt16)right.Value);
                    break;
                case TypeCode.UInt16:
                    switch (right.Type)
                    {
                        case TypeCode.Byte:
                            ret.Value = EvalBinaryOp((UInt16)left.Value, (UInt16)(Byte)right.Value);
                            break;
                        case TypeCode.SByte:
                            ret.Type = TypeCode.Int32;
                            ret.Value = EvalBinaryOp((Int32)(UInt16)left.Value, (Int32)(SByte)right.Value);
                            break;
                        case TypeCode.Int16:
                            ret.Value = EvalBinaryOp((Int32)(UInt16)left.Value, (Int32)(Int16)right.Value);
                            break;
                        case TypeCode.UInt16:
                            ret.Type = TypeCode.Int32;
                            ret.Value = EvalBinaryOp((UInt16)left.Value, (UInt16)right.Value);
                            break;
                        default:
                            throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                    }
                    break;
                default:
                    throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
            }

            return ret;
        }

        private EvalResult EvalIntegerInt32(EvalResult left, EvalResult right)
        {
            // Result is Int32 for non-comparison operations, otherwise boolean
            EvalResult ret = new EvalResult(IsComparison ? TypeCode.Boolean : TypeCode.Int32, null);

            // Types on either the left or right could be anything from Int16, UInt16 and Int32
            switch (left.Type)
            {
                case TypeCode.Byte:
                    switch (right.Type)
                    {
                        case TypeCode.Byte:
                            ret.Value = EvalBinaryOp((Int32)(Byte)left.Value, (Int32)(Byte)right.Value);
                            break;
                        case TypeCode.SByte:
                            ret.Value = EvalBinaryOp((Int32)(Byte)left.Value, (Int32)(SByte)right.Value);
                            break;
                        case TypeCode.Char:
                            if (_language == Language.VBNet)
                                throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                            else
                                ret.Value = EvalBinaryOp((Int32)(Byte)left.Value, (Int32)(Char)right.Value);
                            break;
                        case TypeCode.Int16:
                            ret.Value = EvalBinaryOp((Int32)(Byte)left.Value, (Int32)(Int16)right.Value);
                            break;
                        case TypeCode.Int32:
                            ret.Value = EvalBinaryOp((Int32)(Byte)left.Value, (Int32)right.Value);
                            break;
                        case TypeCode.UInt16:
                            ret.Value = EvalBinaryOp((Int32)(Byte)left.Value, (Int32)(UInt16)right.Value);
                            break;
                    }
                    break;
                case TypeCode.SByte:
                    switch (right.Type)
                    {
                        case TypeCode.Byte:
                            ret.Value = EvalBinaryOp((Int32)(SByte)left.Value, (Int32)(Byte)right.Value);
                            break;
                        case TypeCode.SByte:
                            ret.Value = EvalBinaryOp((Int32)(SByte)left.Value, (Int32)(SByte)right.Value);
                            break;
                        case TypeCode.Char:
                            if (_language == Language.VBNet)
                                throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                            else
                                ret.Value = EvalBinaryOp((Int32)(SByte)left.Value, (Int32)(Char)right.Value);
                            break;
                        case TypeCode.Int16:
                            ret.Value = EvalBinaryOp((Int32)(SByte)left.Value, (Int32)(Int16)right.Value);
                            break;
                        case TypeCode.Int32:
                            ret.Value = EvalBinaryOp((Int32)(SByte)left.Value, (Int32)right.Value);
                            break;
                        case TypeCode.UInt16:
                            ret.Value = EvalBinaryOp((Int32)(SByte)left.Value, (Int32)(UInt16)right.Value);
                            break;
                    }
                    break;
                case TypeCode.Char:
                    if (_language == Language.VBNet)
                        throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                    else
                    {
                        switch (right.Type)
                        {
                            case TypeCode.Byte:
                                ret.Value = EvalBinaryOp((Int32)(Char)left.Value, (Int32)(Byte)right.Value);
                                break;
                            case TypeCode.SByte:
                                ret.Value = EvalBinaryOp((Int32)(Char)left.Value, (Int32)(SByte)right.Value);
                                break;
                            case TypeCode.Char:
                                ret.Value = EvalBinaryOp((Int32)(Char)left.Value, (Int32)(Char)right.Value);
                                break;
                            case TypeCode.Int16:
                                ret.Value = EvalBinaryOp((Int32)(Char)left.Value, (Int32)(Int16)right.Value);
                                break;
                            case TypeCode.Int32:
                                ret.Value = EvalBinaryOp((Int32)(Char)left.Value, (Int32)right.Value);
                                break;
                            case TypeCode.UInt16:
                                ret.Value = EvalBinaryOp((Int32)(Char)left.Value, (Int32)(UInt16)right.Value);
                                break;
                        }
                    }
                    break;
                case TypeCode.Int16:
                    switch (right.Type)
                    {
                        case TypeCode.Byte:
                            ret.Value = EvalBinaryOp((Int32)(Int16)left.Value, (Int32)(Byte)right.Value);
                            break;
                        case TypeCode.SByte:
                            ret.Value = EvalBinaryOp((Int32)(Int16)left.Value, (Int32)(SByte)right.Value);
                            break;
                        case TypeCode.Char:
                            if (_language == Language.VBNet)
                                throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                            else
                                ret.Value = EvalBinaryOp((Int32)(Int16)left.Value, (Int32)(Char)right.Value);
                            break;
                        case TypeCode.Int16:
                            ret.Value = EvalBinaryOp((Int32)(Int16)left.Value, (Int32)(Int16)right.Value);
                            break;
                        case TypeCode.Int32:
                            ret.Value = EvalBinaryOp((Int32)(Int16)left.Value, (Int32)right.Value);
                            break;
                        case TypeCode.UInt16:
                            ret.Value = EvalBinaryOp((Int32)(Int16)left.Value, (Int32)(UInt16)right.Value);
                            break;
                    }
                    break;
                case TypeCode.Int32:
                    switch (right.Type)
                    {
                        case TypeCode.Byte:
                            ret.Value = EvalBinaryOp((Int32)left.Value, (Int32)(Byte)right.Value);
                            break;
                        case TypeCode.SByte:
                            ret.Value = EvalBinaryOp((Int32)left.Value, (Int32)(SByte)right.Value);
                            break;
                        case TypeCode.Char:
                            if (_language == Language.VBNet)
                                throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                            else
                                ret.Value = EvalBinaryOp((Int32)left.Value, (Int32)(Char)right.Value);
                            break;
                        case TypeCode.Int16:
                            ret.Value = EvalBinaryOp((Int32)left.Value, (Int32)(Int16)right.Value);
                            break;
                        case TypeCode.Int32:
                            ret.Value = EvalBinaryOp((Int32)left.Value, (Int32)right.Value);
                            break;
                        case TypeCode.UInt16:
                            ret.Value = EvalBinaryOp((Int32)left.Value, (Int32)(UInt16)right.Value);
                            break;
                    }
                    break;
                case TypeCode.UInt16:
                    switch (right.Type)
                    {
                        case TypeCode.Byte:
                            ret.Value = EvalBinaryOp((Int32)(UInt16)left.Value, (Int32)(Byte)right.Value);
                            break;
                        case TypeCode.SByte:
                            ret.Value = EvalBinaryOp((Int32)(UInt16)left.Value, (Int32)(SByte)right.Value);
                            break;
                        case TypeCode.Char:
                            if (_language == Language.VBNet)
                                throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                            else
                                ret.Value = EvalBinaryOp((Int32)(UInt16)left.Value, (Int32)(Char)right.Value);
                            break;
                        case TypeCode.Int16:
                            ret.Value = EvalBinaryOp((Int32)(UInt16)left.Value, (Int32)(Int16)right.Value);
                            break;
                        case TypeCode.Int32:
                            ret.Value = EvalBinaryOp((Int32)(UInt16)left.Value, (Int32)right.Value);
                            break;
                        case TypeCode.UInt16:
                            ret.Value = EvalBinaryOp((Int32)(UInt16)left.Value, (Int32)(UInt16)right.Value);
                            break;
                    }
                    break;
                default:
                    throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
            }

            return ret;
        }

        private EvalResult EvalIntegerUInt32(EvalResult left, EvalResult right)
        {
            // Result is UInt32 or Int64 for non-comparison operations, otherwise boolean
            EvalResult ret = new EvalResult(IsComparison ? TypeCode.Boolean : TypeCode.UInt32, null);

            // Either the left or the right must be exactly UInt32 and the other side less than that
            switch (left.Type)
            {
                case TypeCode.Byte:
                    ret.Value = EvalBinaryOp((UInt32)(Byte)left.Value, (UInt32)right.Value);
                    break;
                case TypeCode.SByte:
                    if (_language == Language.VBNet)
                    {
                        ret.Type = TypeCode.Int64;
                        ret.Value = EvalBinaryOp((Int64)(SByte)left.Value, (Int64)(UInt32)right.Value);
                    }
                    else
                        ret.Value = EvalBinaryOp((UInt32)(SByte)left.Value, (UInt32)right.Value);
                    break;
                case TypeCode.Char:
                    if (_language == Language.VBNet)
                        throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                    else
                        ret.Value = EvalBinaryOp((UInt32)(Char)left.Value, (UInt32)right.Value);
                    break;
                case TypeCode.Int16:
                    if (_language == Language.VBNet)
                    {
                        ret.Type = TypeCode.Int64;
                        ret.Value = EvalBinaryOp((Int64)(Int16)left.Value, (Int64)(UInt32)right.Value);
                    }
                    else
                        ret.Value = EvalBinaryOp((UInt32)(Int16)left.Value, (UInt32)right.Value);
                    break;
                case TypeCode.Int32:
                    if (_language == Language.VBNet)
                    {
                        ret.Type = TypeCode.Int64;
                        ret.Value = EvalBinaryOp((Int64)(Int32)left.Value, (Int64)(UInt32)right.Value);
                    }
                    else
                        ret.Value = EvalBinaryOp((UInt32)(Int32)left.Value, (UInt32)right.Value);
                    break;
                case TypeCode.UInt16:
                    ret.Value = EvalBinaryOp((UInt32)(UInt16)left.Value, (UInt32)right.Value);
                    break;
                case TypeCode.UInt32:
                    switch (right.Type)
                    {
                        case TypeCode.Byte:
                            ret.Value = EvalBinaryOp((UInt32)left.Value, (UInt32)(Byte)right.Value);
                            break;
                        case TypeCode.SByte:
                            if (_language == Language.VBNet)
                            {
                                ret.Type = TypeCode.Int64;
                                ret.Value = EvalBinaryOp((Int64)(UInt32)left.Value, (Int64)(SByte)right.Value);
                            }
                            else
                                ret.Value = EvalBinaryOp((UInt32)left.Value, (UInt32)(SByte)right.Value);
                            break;
                        case TypeCode.Char:
                            if (_language == Language.VBNet)
                                throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                            else
                                ret.Value = EvalBinaryOp((UInt32)left.Value, (UInt32)(Char)right.Value);
                            break;
                        case TypeCode.Int16:
                            if (_language == Language.VBNet)
                            {
                                ret.Type = TypeCode.Int64;
                                ret.Value = EvalBinaryOp((Int64)(UInt32)left.Value, (Int64)(Int16)right.Value);
                            }
                            else
                                ret.Value = EvalBinaryOp((UInt32)left.Value, (UInt32)(Int16)right.Value);
                            break;
                        case TypeCode.Int32:
                            if (_language == Language.VBNet)
                            {
                                ret.Type = TypeCode.Int64;
                                ret.Value = EvalBinaryOp((Int64)(UInt32)left.Value, (Int64)(UInt32)right.Value);
                            }
                            else
                                ret.Value = EvalBinaryOp((UInt32)left.Value, (UInt32)(UInt32)right.Value);
                            break;
                        case TypeCode.UInt16:
                            ret.Value = EvalBinaryOp((UInt32)left.Value, (UInt32)(UInt16)right.Value);
                            break;
                        case TypeCode.UInt32:
                            ret.Value = EvalBinaryOp((UInt32)left.Value, (UInt32)right.Value);
                            break;
                        default:
                            throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                    }
                    break;
                default:
                    throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
            }

            return ret;
        }

        private EvalResult EvalIntegerInt64(EvalResult left, EvalResult right)
        {
            // Result is Int64 for non-comparison operations, otherwise boolean
            EvalResult ret = new EvalResult(IsComparison ? TypeCode.Boolean : TypeCode.Int64, null);

            // Either the left or the right must be exactly Int64 and the other side less than that
            switch (left.Type)
            {
                case TypeCode.Byte:
                    ret.Value = EvalBinaryOp((Int64)(Byte)left.Value, (Int64)right.Value);
                    break;
                case TypeCode.SByte:
                    ret.Value = EvalBinaryOp((Int64)(SByte)left.Value, (Int64)right.Value);
                    break;
                case TypeCode.Char:
                    if (_language == Language.VBNet)
                        throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                    else
                        ret.Value = EvalBinaryOp((Int64)(Char)left.Value, (Int64)right.Value);
                    break;
                case TypeCode.Int16:
                    ret.Value = EvalBinaryOp((Int64)(Int16)left.Value, (Int64)right.Value);
                    break;
                case TypeCode.Int32:
                    ret.Value = EvalBinaryOp((Int64)(Int32)left.Value, (Int64)right.Value);
                    break;
                case TypeCode.UInt16:
                    ret.Value = EvalBinaryOp((Int64)(UInt16)left.Value, (Int64)right.Value);
                    break;
                case TypeCode.UInt32:
                    ret.Value = EvalBinaryOp((Int64)(UInt32)left.Value, (Int64)right.Value);
                    break;
                case TypeCode.Int64:
                    switch (right.Type)
                    {
                        case TypeCode.Byte:
                            ret.Value = EvalBinaryOp((Int64)left.Value, (Int64)(Byte)right.Value);
                            break;
                        case TypeCode.SByte:
                            ret.Value = EvalBinaryOp((Int64)left.Value, (Int64)(SByte)right.Value);
                            break;
                        case TypeCode.Char:
                            if (_language == Language.VBNet)
                                throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                            else
                                ret.Value = EvalBinaryOp((Int64)left.Value, (Int64)(Char)right.Value);
                            break;
                        case TypeCode.Int16:
                            ret.Value = EvalBinaryOp((Int64)left.Value, (Int64)(Int16)right.Value);
                            break;
                        case TypeCode.Int32:
                            ret.Value = EvalBinaryOp((Int64)left.Value, (Int64)(Int32)right.Value);
                            break;
                        case TypeCode.UInt16:
                            ret.Value = EvalBinaryOp((Int64)left.Value, (Int64)(UInt16)right.Value);
                            break;
                        case TypeCode.UInt32:
                            ret.Value = EvalBinaryOp((Int64)left.Value, (Int64)(UInt32)right.Value);
                            break;
                        case TypeCode.Int64:
                            ret.Value = EvalBinaryOp((Int64)left.Value, (Int64)right.Value);
                            break;
                        default:
                            throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                    }
                    break;
                default:
                    throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
            }

            return ret;
        }

        private EvalResult EvalIntegerUInt64(EvalResult left, EvalResult right)
        {
            // Result is UInt64 or Decimal for non-comparison operations, otherwise boolean
            EvalResult ret = new EvalResult(IsComparison ? TypeCode.Boolean : TypeCode.UInt64, null);

            // Either the left or the right must be exactly UInt64 and the other side less than that
            switch (left.Type)
            {
                case TypeCode.Byte:
                    ret.Value = EvalBinaryOp((UInt64)(Byte)left.Value, (UInt64)right.Value);
                    break;
                case TypeCode.SByte:
                    if (_language == Language.VBNet)
                    {
                        ret.Type = TypeCode.Decimal;
                        ret.Value = EvalBinaryOp((Decimal)(SByte)left.Value, (Decimal)(UInt64)right.Value);
                    }
                    else
                        ret.Value = EvalBinaryOp((UInt64)(SByte)left.Value, (UInt64)right.Value);
                    break;
                case TypeCode.Char:
                    if (_language == Language.VBNet)
                        throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                    else
                        ret.Value = EvalBinaryOp((UInt64)(Char)left.Value, (UInt64)right.Value);
                    break;
                case TypeCode.Int16:
                    if (_language == Language.VBNet)
                    {
                        ret.Type = TypeCode.Decimal;
                        ret.Value = EvalBinaryOp((Decimal)(Int16)left.Value, (Decimal)(UInt64)right.Value);
                    }
                    else
                        ret.Value = EvalBinaryOp((UInt64)(Int16)left.Value, (UInt64)right.Value);
                    break;
                case TypeCode.Int32:
                    if (_language == Language.VBNet)
                    {
                        ret.Type = TypeCode.Decimal;
                        ret.Value = EvalBinaryOp((Decimal)(Int32)left.Value, (Decimal)(UInt64)right.Value);
                    }
                    else
                        ret.Value = EvalBinaryOp((UInt64)(Int32)left.Value, (UInt64)right.Value);
                    break;
                case TypeCode.Int64:
                    if (_language == Language.VBNet)
                    {
                        ret.Type = TypeCode.Decimal;
                        ret.Value = EvalBinaryOp((Decimal)(Int64)left.Value, (Decimal)(UInt64)right.Value);
                    }
                    else
                        ret.Value = EvalBinaryOp((UInt64)(Int64)left.Value, (UInt64)right.Value);
                    break;
                case TypeCode.UInt16:
                    ret.Value = EvalBinaryOp((UInt64)(UInt16)left.Value, (UInt64)right.Value);
                    break;
                case TypeCode.UInt32:
                    ret.Value = EvalBinaryOp((UInt64)(UInt32)left.Value, (UInt64)right.Value);
                    break;
                case TypeCode.UInt64:
                    switch (right.Type)
                    {
                        case TypeCode.Byte:
                            ret.Value = EvalBinaryOp((UInt64)left.Value, (UInt64)(Byte)right.Value);
                            break;
                        case TypeCode.SByte:
                            if (_language == Language.VBNet)
                            {
                                ret.Type = TypeCode.Decimal;
                                ret.Value = EvalBinaryOp((Decimal)(UInt64)left.Value, (Decimal)(SByte)right.Value);
                            }
                            else
                                ret.Value = EvalBinaryOp((UInt64)left.Value, (UInt64)(SByte)right.Value);
                            break;
                        case TypeCode.Char:
                            if (_language == Language.VBNet)
                                throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                            else
                                ret.Value = EvalBinaryOp((UInt64)left.Value, (UInt64)(Char)right.Value);
                            break;
                        case TypeCode.Int16:
                            if (_language == Language.VBNet)
                            {
                                ret.Type = TypeCode.Decimal;
                                ret.Value = EvalBinaryOp((Decimal)(UInt64)left.Value, (Decimal)(Int16)right.Value);
                            }
                            else
                                ret.Value = EvalBinaryOp((UInt64)left.Value, (UInt64)(Int16)right.Value);
                            break;
                        case TypeCode.Int32:
                            if (_language == Language.VBNet)
                            {
                                ret.Type = TypeCode.Decimal;
                                ret.Value = EvalBinaryOp((Decimal)(UInt64)left.Value, (Decimal)(Int32)right.Value);
                            }
                            else
                                ret.Value = EvalBinaryOp((UInt64)left.Value, (UInt64)(Int32)right.Value);
                            break;
                        case TypeCode.Int64:
                            if (_language == Language.VBNet)
                            {
                                ret.Type = TypeCode.Decimal;
                                ret.Value = EvalBinaryOp((Decimal)(UInt64)left.Value, (Decimal)(Int64)right.Value);
                            }
                            else
                                ret.Value = EvalBinaryOp((UInt64)left.Value, (UInt64)(Int64)right.Value);
                            break;
                        case TypeCode.UInt16:
                            ret.Value = EvalBinaryOp((UInt64)left.Value, (UInt64)(UInt16)right.Value);
                            break;
                        case TypeCode.UInt32:
                            ret.Value = EvalBinaryOp((UInt64)left.Value, (UInt64)(UInt32)right.Value);
                            break;
                        case TypeCode.UInt64:
                            ret.Value = EvalBinaryOp((UInt64)left.Value, (UInt64)right.Value);
                            break;
                        default:
                            throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
                    }
                    break;
                default:
                    throw new ApplicationException("Operator '" + OperationString + "' impossible between type '" + left.Type.ToString() + "' and '" + right.Type.ToString() + "'.");
            }

            return ret;
        }

        private object EvalBinaryOp(bool x, bool y)
        {
            switch (Operation)
            {
                case BinaryOp.LogicalOr:
                    return x | y;
                case BinaryOp.LogicalXor:
                    return x ^ y;
                case BinaryOp.LogicalAnd:
                    return x & y;
                case BinaryOp.Equal:
                    return (x == y);
                case BinaryOp.NotEqual:
                    return (x != y);
                case BinaryOp.Add:
                case BinaryOp.Subtract:
                case BinaryOp.Multiply:
                case BinaryOp.Divide:
                case BinaryOp.Remainder:
                case BinaryOp.LessThan:
                case BinaryOp.LessThanEqual:
                case BinaryOp.GreaterThan:
                case BinaryOp.GreaterThanEqual:
                    throw new ApplicationException("Operator '" + OperationString + "' impossible between type 'bool' and 'bool'.");
                default:
                    throw new ApplicationException("Unrecognized 'Int32' type binary operation '" + OperationString);
            }
        }

        private object EvalBinaryOp(Char x, Char y)
        {
            switch (Operation)
            {
                case BinaryOp.Equal:
                    return (x == y);
                case BinaryOp.NotEqual:
                    return (x != y);
                case BinaryOp.LessThan:
                    return (x < y);
                case BinaryOp.LessThanEqual:
                    return (x <= y);
                case BinaryOp.GreaterThan:
                    return (x > y);
                case BinaryOp.GreaterThanEqual:
                    return (x >= y);
                case BinaryOp.Subtract:
                case BinaryOp.Multiply:
                case BinaryOp.Divide:
                case BinaryOp.IntegerDivide:
                case BinaryOp.Remainder:
                case BinaryOp.LogicalOr:
                case BinaryOp.LogicalXor:
                case BinaryOp.LogicalAnd:
                default:
                    throw new ApplicationException("Unrecognized 'Char' type binary operation '" + OperationString);
            }
        }

        private object EvalBinaryOp(SByte x, SByte y)
        {
            switch (Operation)
            {
                case BinaryOp.Add:
                    return (SByte)(x + y);
                case BinaryOp.Subtract:
                    return (SByte)(x - y);
                case BinaryOp.Multiply:
                    return (SByte)(x * y);
                case BinaryOp.Divide:
                case BinaryOp.IntegerDivide:
                    return (SByte)(x / y);
                case BinaryOp.Remainder:
                    return (SByte)(x % y);
                case BinaryOp.LogicalOr:
                    return (SByte)(x | y);
                case BinaryOp.LogicalXor:
                    return (SByte)(x ^ y);
                case BinaryOp.LogicalAnd:
                    return (SByte)(x & y);
                case BinaryOp.Equal:
                    return (x == y);
                case BinaryOp.NotEqual:
                    return (x != y);
                case BinaryOp.LessThan:
                    return (x < y);
                case BinaryOp.LessThanEqual:
                    return (x <= y);
                case BinaryOp.GreaterThan:
                    return (x > y);
                case BinaryOp.GreaterThanEqual:
                    return (x >= y);
                default:
                    throw new ApplicationException("Unrecognized 'SByte' type binary operation '" + OperationString);
            }
        }

        private object EvalBinaryOp(Byte x, Byte y)
        {
            switch (Operation)
            {
                case BinaryOp.Add:
                    return (Byte)(x + y);
                case BinaryOp.Subtract:
                    return (Byte)(x - y);
                case BinaryOp.Multiply:
                    return (Byte)(x * y);
                case BinaryOp.Divide:
                case BinaryOp.IntegerDivide:
                    return (Byte)(x / y);
                case BinaryOp.Remainder:
                    return (Byte)(x % y);
                case BinaryOp.LogicalOr:
                    return (Byte)(x | y);
                case BinaryOp.LogicalXor:
                    return (Byte)(x ^ y);
                case BinaryOp.LogicalAnd:
                    return (Byte)(x & y);
                case BinaryOp.Equal:
                    return (x == y);
                case BinaryOp.NotEqual:
                    return (x != y);
                case BinaryOp.LessThan:
                    return (x < y);
                case BinaryOp.LessThanEqual:
                    return (x <= y);
                case BinaryOp.GreaterThan:
                    return (x > y);
                case BinaryOp.GreaterThanEqual:
                    return (x >= y);
                default:
                    throw new ApplicationException("Unrecognized 'Byte' type binary operation '" + OperationString);
            }
        }

        private object EvalBinaryOp(UInt16 x, UInt16 y)
        {
            switch (Operation)
            {
                case BinaryOp.Add:
                    return (UInt16)(x + y);
                case BinaryOp.Subtract:
                    return (UInt16)(x - y);
                case BinaryOp.Multiply:
                    return (UInt16)(x * y);
                case BinaryOp.Divide:
                case BinaryOp.IntegerDivide:
                    return (UInt16)(x / y);
                case BinaryOp.Remainder:
                    return (UInt16)(x % y);
                case BinaryOp.LogicalOr:
                    return (UInt16)(x | y);
                case BinaryOp.LogicalXor:
                    return (UInt16)(x ^ y);
                case BinaryOp.LogicalAnd:
                    return (UInt16)(x & y);
                case BinaryOp.Equal:
                    return (x == y);
                case BinaryOp.NotEqual:
                    return (x != y);
                case BinaryOp.LessThan:
                    return (x < y);
                case BinaryOp.LessThanEqual:
                    return (x <= y);
                case BinaryOp.GreaterThan:
                    return (x > y);
                case BinaryOp.GreaterThanEqual:
                    return (x >= y);
                default:
                    throw new ApplicationException("Unrecognized 'UInt16' type binary operation '" + OperationString);
            }
        }

        private object EvalBinaryOp(Int16 x, Int16 y)
        {
            switch (Operation)
            {
                case BinaryOp.Add:
                    return (Int16)(x + y);
                case BinaryOp.Subtract:
                    return (Int16)(x - y);
                case BinaryOp.Multiply:
                    return (Int16)(x * y);
                case BinaryOp.Divide:
                case BinaryOp.IntegerDivide:
                    return (Int16)(x / y);
                case BinaryOp.Remainder:
                    return (Int16)(x % y);
                case BinaryOp.LogicalOr:
                    return (Int16)(x | y);
                case BinaryOp.LogicalXor:
                    return (Int16)(x ^ y);
                case BinaryOp.LogicalAnd:
                    return (Int16)(x & y);
                case BinaryOp.Equal:
                    return (x == y);
                case BinaryOp.NotEqual:
                    return (x != y);
                case BinaryOp.LessThan:
                    return (x < y);
                case BinaryOp.LessThanEqual:
                    return (x <= y);
                case BinaryOp.GreaterThan:
                    return (x > y);
                case BinaryOp.GreaterThanEqual:
                    return (x >= y);
                default:
                    throw new ApplicationException("Unrecognized 'Int16' type binary operation '" + OperationString);
            }
        }

        private object EvalBinaryOp(Int32 x, Int32 y)
        {
            switch (Operation)
            {
                case BinaryOp.Add:
                    return x + y;
                case BinaryOp.Subtract:
                    return x - y;
                case BinaryOp.Multiply:
                    return x * y;
                case BinaryOp.Divide:
                case BinaryOp.IntegerDivide:
                    return x / y;
                case BinaryOp.Remainder:
                    return x % y;
                case BinaryOp.LogicalOr:
                    return x | y;
                case BinaryOp.LogicalXor:
                    return x ^ y;
                case BinaryOp.LogicalAnd:
                    return x & y;
                case BinaryOp.Equal:
                    return (x == y);
                case BinaryOp.NotEqual:
                    return (x != y);
                case BinaryOp.LessThan:
                    return (x < y);
                case BinaryOp.LessThanEqual:
                    return (x <= y);
                case BinaryOp.GreaterThan:
                    return (x > y);
                case BinaryOp.GreaterThanEqual:
                    return (x >= y);
                default:
                    throw new ApplicationException("Unrecognized 'Int32' type binary operation '" + OperationString);
            }
        }

        private object EvalBinaryOp(UInt32 x, UInt32 y)
        {
            switch (Operation)
            {
                case BinaryOp.Add:
                    return x + y;
                case BinaryOp.Subtract:
                    return x - y;
                case BinaryOp.Multiply:
                    return x * y;
                case BinaryOp.Divide:
                case BinaryOp.IntegerDivide:
                    return x / y;
                case BinaryOp.Remainder:
                    return x % y;
                case BinaryOp.LogicalOr:
                    return x | y;
                case BinaryOp.LogicalXor:
                    return x ^ y;
                case BinaryOp.LogicalAnd:
                    return x & y;
                case BinaryOp.Equal:
                    return (x == y);
                case BinaryOp.NotEqual:
                    return (x != y);
                case BinaryOp.LessThan:
                    return (x < y);
                case BinaryOp.LessThanEqual:
                    return (x <= y);
                case BinaryOp.GreaterThan:
                    return (x > y);
                case BinaryOp.GreaterThanEqual:
                    return (x >= y);
                default:
                    throw new ApplicationException("Unrecognized 'UInt32' type binary operation '" + OperationString);
            }
        }

        private object EvalBinaryOp(Int64 x, Int64 y)
        {
            switch (Operation)
            {
                case BinaryOp.Add:
                    return x + y;
                case BinaryOp.Subtract:
                    return x - y;
                case BinaryOp.Multiply:
                    return x * y;
                case BinaryOp.Divide:
                case BinaryOp.IntegerDivide:
                    return x / y;
                case BinaryOp.Remainder:
                    return x % y;
                case BinaryOp.LogicalOr:
                    return x | y;
                case BinaryOp.LogicalXor:
                    return x ^ y;
                case BinaryOp.LogicalAnd:
                    return x & y;
                case BinaryOp.Equal:
                    return (x == y);
                case BinaryOp.NotEqual:
                    return (x != y);
                case BinaryOp.LessThan:
                    return (x < y);
                case BinaryOp.LessThanEqual:
                    return (x <= y);
                case BinaryOp.GreaterThan:
                    return (x > y);
                case BinaryOp.GreaterThanEqual:
                    return (x >= y);
                default:
                    throw new ApplicationException("Unrecognized 'Int64' type binary operation '" + OperationString);
            }
        }

        private object EvalBinaryOp(UInt64 x, UInt64 y)
        {
            switch (Operation)
            {
                case BinaryOp.Add:
                    return x + y;
                case BinaryOp.Subtract:
                    return x - y;
                case BinaryOp.Multiply:
                    return x * y;
                case BinaryOp.Divide:
                case BinaryOp.IntegerDivide:
                    return x / y;
                case BinaryOp.Remainder:
                    return x % y;
                case BinaryOp.LogicalOr:
                    return x | y;
                case BinaryOp.LogicalXor:
                    return x ^ y;
                case BinaryOp.LogicalAnd:
                    return x & y;
                case BinaryOp.Equal:
                    return (x == y);
                case BinaryOp.NotEqual:
                    return (x != y);
                case BinaryOp.LessThan:
                    return (x < y);
                case BinaryOp.LessThanEqual:
                    return (x <= y);
                case BinaryOp.GreaterThan:
                    return (x > y);
                case BinaryOp.GreaterThanEqual:
                    return (x >= y);
                default:
                    throw new ApplicationException("Unrecognized 'UInt64' type binary operation '" + OperationString);
            }
        }

        private object EvalBinaryOp(float x, float y)
        {
            switch(Operation)
            {
                case BinaryOp.Add:
                    return x + y;
                case BinaryOp.Subtract:
                    return x - y;
                case BinaryOp.Multiply:
                    return x * y;
                case BinaryOp.Divide:
                    return x / y;
                case BinaryOp.Remainder:
                    return x % y;
                case BinaryOp.IntegerDivide:
                    return (float)Math.Floor(x / y);
                case BinaryOp.Equal:
                    return (x == y);
                case BinaryOp.NotEqual:
                    return (x != y);
                case BinaryOp.LessThan:
                    return (x < y);
                case BinaryOp.LessThanEqual:
                    return (x <= y);
                case BinaryOp.GreaterThan:
                    return (x > y);
                case BinaryOp.GreaterThanEqual:
                    return (x >= y);
                case BinaryOp.LogicalOr:
                case BinaryOp.LogicalXor:
                case BinaryOp.LogicalAnd:
                    throw new ApplicationException("Operator '" + OperationString + "' impossible between type 'float' and 'float'.");
                default:
                    throw new ApplicationException("Unrecognized 'float' type binary operation '" + OperationString);
            }
        }

        private object EvalBinaryOp(double x, double y)
        {
            switch (Operation)
            {
                case BinaryOp.Add:
                    return x + y;
                case BinaryOp.Subtract:
                    return x - y;
                case BinaryOp.Multiply:
                    return x * y;
                case BinaryOp.Divide:
                    return x / y;
                case BinaryOp.Remainder:
                    return x % y;
                case BinaryOp.IntegerDivide:
                    return Math.Floor(x / y);
                case BinaryOp.Equal:
                    return (x == y);
                case BinaryOp.NotEqual:
                    return (x != y);
                case BinaryOp.LessThan:
                    return (x < y);
                case BinaryOp.LessThanEqual:
                    return (x <= y);
                case BinaryOp.GreaterThan:
                    return (x > y);
                case BinaryOp.GreaterThanEqual:
                    return (x >= y);
                case BinaryOp.LogicalOr:
                case BinaryOp.LogicalXor:
                case BinaryOp.LogicalAnd:
                    throw new ApplicationException("Operator '" + OperationString + "' impossible between type 'double' and 'double'.");
                default:
                    throw new ApplicationException("Unrecognized 'double' type binary operation '" + OperationString);
            }
        }

        private object EvalBinaryOp(decimal x, decimal y)
        {
            switch (Operation)
            {
                case BinaryOp.Add:
                    return x + y;
                case BinaryOp.Subtract:
                    return x - y;
                case BinaryOp.Multiply:
                    return x * y;
                case BinaryOp.Divide:
                    return x / y;
                case BinaryOp.Remainder:
                    return x % y;
                case BinaryOp.IntegerDivide:
                    return Math.Floor(x / y);
                case BinaryOp.Equal:
                    return (x == y);
                case BinaryOp.NotEqual:
                    return (x != y);
                case BinaryOp.LessThan:
                    return (x < y);
                case BinaryOp.LessThanEqual:
                    return (x <= y);
                case BinaryOp.GreaterThan:
                    return (x > y);
                case BinaryOp.GreaterThanEqual:
                    return (x >= y);
                case BinaryOp.LogicalOr:
                case BinaryOp.LogicalXor:
                case BinaryOp.LogicalAnd:
                    throw new ApplicationException("Operator '" + OperationString + "' impossible between type 'decimal' and 'decimal'.");
                default:
                    throw new ApplicationException("Unrecognized 'decimal' type binary operation '" + OperationString);
            }
        }

        private string OperationString
        {
            get
            {
                switch (Operation)
                {
                    case BinaryOp.Add:
                        return "+";
                    case BinaryOp.Subtract:
                        return "-";
                    case BinaryOp.Multiply:
                        return "*";
                    case BinaryOp.Divide:
                        return "/";
                    case BinaryOp.Remainder:
                        return (_language == Language.CSharp ? "%" : "Mod");
                    case BinaryOp.IntegerDivide:
                        return "\\";
                    case BinaryOp.LogicalOr:
                        return (_language == Language.CSharp ? "|" : "Or");
                    case BinaryOp.LogicalXor:
                        return (_language == Language.CSharp ? "^" : "Xor");
                    case BinaryOp.LogicalAnd:
                        return (_language == Language.CSharp ? "&" : "And");
                    case BinaryOp.Equal:
                        return (_language == Language.CSharp ? "==" : "=");
                    case BinaryOp.NotEqual:
                        return (_language == Language.CSharp ? "!=" : "<>");
                    case BinaryOp.LessThan:
                        return "<";
                    case BinaryOp.LessThanEqual:
                        return "<=";
                    case BinaryOp.GreaterThan:
                        return ">";
                    case BinaryOp.GreaterThanEqual:
                        return ">=";
                    default:
                        throw new ApplicationException("Unrecognized binary operation '" + Operation.ToString() + "'.");
                }
            }
        }

        private bool IsComparison
        {
            get { return ((Operation & BinaryOp.CompareOps) == BinaryOp.CompareOps); }
        }

        private bool IsLogical
        {
            get 
            {
                switch (Operation)
                {
                    case BinaryOp.LogicalAnd:
                    case BinaryOp.LogicalOr:
                    case BinaryOp.LogicalXor:
                        return true;
                    default:
                        return false;
                }
            }
        }
        #endregion
    }
}

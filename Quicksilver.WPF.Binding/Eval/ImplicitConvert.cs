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
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Globalization;

namespace ComponentFactory.Quicksilver.Binding
{
    /// <summary>
    /// Provides helper methods for performing implicit type conversion.
    /// </summary>
    public static class ImplicitConverter
    {
        /// <summary>
        /// Determines if the incoming value can be implicitly converted to the target type.
        /// </summary>
        /// <param name="target">Target type for value to be converted into.</param>
        /// <param name="value">Value to be tested.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>True if implicit conversion is possible; otherwise false;</returns>
        public static bool CanConvertTo(TypeCode target, object value, Language language)
        {
            if (value == null)
            {
                // A null can always be a null of a reference type
                return (target == TypeCode.Object);
            }
            else
            {
                // If the value is already the same target type then we are done
                if (Type.GetTypeCode(value.GetType()) == target)
                    return true;
                else
                {
                    // Check if the value can be converted to a different type
                    switch (target)
                    {
                        case TypeCode.String:
                            // Can always convert a non-null to a string
                            return true;
                        case TypeCode.Boolean:
                            return CanConvertToBoolean(value, language);
                        case TypeCode.Char:
                            return CanConvertToChar(value, language);
                        case TypeCode.SByte:
                            return CanConvertToSByte(value, language);
                        case TypeCode.Byte:
                            return CanConvertToByte(value, language);
                        case TypeCode.Int16:
                            return CanConvertToInt16(value, language);
                        case TypeCode.UInt16:
                            return CanConvertToUInt16(value, language);
                        case TypeCode.Int32:
                            return CanConvertToInt32(value, language);
                        case TypeCode.UInt32:
                            return CanConvertToUInt32(value, language);
                        case TypeCode.Int64:
                            return CanConvertToInt64(value, language);
                        case TypeCode.UInt64:
                            return CanConvertToUInt64(value, language);
                        case TypeCode.Single:
                            return CanConvertToSingle(value, language);
                        case TypeCode.Double:
                            return CanConvertToDouble(value, language);
                        case TypeCode.Decimal:
                            return CanConvertToDecimal(value, language);
                        case TypeCode.Object:
                            // Anything non-null can always be passed back boxed or as reference to object
                            return true;
                        default:
                            // All other types cannot be implicitly converted into
                            return false;
                    }
                }
            }
        }

        /// <summary>
        /// Determines if a value can be implicitly converted to Boolean.
        /// </summary>
        /// <param name="value">Value to test.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>True if implicit conversion possible; otherwise false.</returns>
        public static bool CanConvertToBoolean(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Boolean:
                            return true;
                    }
                }
                else
                {
                    try
                    {
                        switch (tc)
                        {
                            case TypeCode.Boolean:
                            case TypeCode.SByte:
                            case TypeCode.Byte:
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                            case TypeCode.Decimal:
                                return true;
                            case TypeCode.String:
                                Convert.ToBoolean(value.ToString());
                                return true;
                        }
                    }
                    catch {}
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if a value can be implicitly converted to Char.
        /// </summary>
        /// <param name="value">Value to test.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>True if implicit conversion possible; otherwise false.</returns>
        public static bool CanConvertToChar(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Char:
                            return true;
                    }
                }
                else
                {
                    switch (tc)
                    {
                        case TypeCode.Char:
                        case TypeCode.String:
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if a value can be implicitly converted to SByte.
        /// </summary>
        /// <param name="value">Value to test.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>True if implicit conversion possible; otherwise false.</returns>
        public static bool CanConvertToSByte(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.SByte:
                            return true;
                    }
                }
                else
                {
                    try
                    {
                        switch (tc)
                        {
                            case TypeCode.Boolean:
                            case TypeCode.SByte:
                                return true;
                            case TypeCode.Byte:
                                Convert.ToSByte((Byte)value);
                                return true;
                            case TypeCode.Int16:
                                Convert.ToSByte((Int16)value);
                                return true;
                            case TypeCode.UInt16:
                                Convert.ToSByte((UInt16)value);
                                return true;
                            case TypeCode.Int32:
                                Convert.ToSByte((Int32)value);
                                return true;
                            case TypeCode.UInt32:
                                Convert.ToSByte((UInt32)value);
                                return true;
                            case TypeCode.Int64:
                                Convert.ToSByte((Int64)value);
                                return true;
                            case TypeCode.UInt64:
                                Convert.ToSByte((UInt64)value);
                                return true;
                            case TypeCode.Single:
                                Convert.ToSByte((Single)value);
                                return true;
                            case TypeCode.Double:
                                Convert.ToSByte((Double)value);
                                return true;
                            case TypeCode.Decimal:
                                Convert.ToSByte((Decimal)value);
                                return true;
                            case TypeCode.String:
                                Convert.ToSByte((String)value);
                                return true;
                        }
                    }
                    catch {}
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if a value can be implicitly converted to Byte.
        /// </summary>
        /// <param name="value">Value to test.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>True if implicit conversion possible; otherwise false.</returns>
        public static bool CanConvertToByte(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Byte:
                            return true;
                    }
                }
                else
                {
                    try
                    {
                        switch (tc)
                        {
                            case TypeCode.Boolean:
                            case TypeCode.Byte:
                                return true;
                            case TypeCode.SByte:
                                Convert.ToByte((SByte)value);
                                return true;
                            case TypeCode.Int16:
                                Convert.ToByte((Int16)value);
                                return true;
                            case TypeCode.UInt16:
                                Convert.ToByte((UInt16)value);
                                return true;
                            case TypeCode.Int32:
                                Convert.ToByte((Int32)value);
                                return true;
                            case TypeCode.UInt32:
                                Convert.ToByte((UInt32)value);
                                return true;
                            case TypeCode.Int64:
                                Convert.ToByte((Int64)value);
                                return true;
                            case TypeCode.UInt64:
                                Convert.ToByte((UInt64)value);
                                return true;
                            case TypeCode.Single:
                                Convert.ToByte((Single)value);
                                return true;
                            case TypeCode.Double:
                                Convert.ToByte((Double)value);
                                return true;
                            case TypeCode.Decimal:
                                Convert.ToByte((Decimal)value);
                                return true;
                            case TypeCode.String:
                                Convert.ToByte((String)value);
                                return true;
                        }
                    }
                    catch { }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if a value can be implicitly converted to Int16.
        /// </summary>
        /// <param name="value">Value to test.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>True if implicit conversion possible; otherwise false.</returns>
        public static bool CanConvertToInt16(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.SByte:
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                            return true;
                    }
                }
                else
                {
                    try
                    {
                        switch (tc)
                        {
                            case TypeCode.Boolean:
                            case TypeCode.SByte:
                            case TypeCode.Byte:
                            case TypeCode.Int16:
                                return true;
                            case TypeCode.UInt16:
                                Convert.ToInt16((UInt16)value);
                                return true;
                            case TypeCode.Int32:
                                Convert.ToInt16((Int32)value);
                                return true;
                            case TypeCode.UInt32:
                                Convert.ToInt16((UInt32)value);
                                return true;
                            case TypeCode.Int64:
                                Convert.ToInt16((Int64)value);
                                return true;
                            case TypeCode.UInt64:
                                Convert.ToInt16((UInt64)value);
                                return true;
                            case TypeCode.Single:
                                Convert.ToInt16((Single)value);
                                return true;
                            case TypeCode.Double:
                                Convert.ToInt16((Double)value);
                                return true;
                            case TypeCode.Decimal:
                                Convert.ToInt16((Decimal)value);
                                return true;
                            case TypeCode.String:
                                Convert.ToInt16((String)value);
                                return true;
                        }
                    }
                    catch { }
                }
            }
            
            return false;
        }

        /// <summary>
        /// Determines if a value can be implicitly converted to UInt16.
        /// </summary>
        /// <param name="value">Value to test.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>True if implicit conversion possible; otherwise false.</returns>
        public static bool CanConvertToUInt16(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Byte:
                        case TypeCode.Char:
                        case TypeCode.UInt16:
                            return true;
                    }
                }
                else
                {
                    try
                    {
                        switch (tc)
                        {
                            case TypeCode.Boolean:
                            case TypeCode.Byte:
                            case TypeCode.UInt16:
                                return true;
                            case TypeCode.SByte:
                                Convert.ToUInt16((SByte)value);
                                return true;
                            case TypeCode.Int16:
                                Convert.ToUInt16((Int16)value);
                                return true;
                            case TypeCode.Int32:
                                Convert.ToUInt16((Int32)value);
                                return true;
                            case TypeCode.UInt32:
                                Convert.ToUInt16((UInt32)value);
                                return true;
                            case TypeCode.Int64:
                                Convert.ToUInt16((Int64)value);
                                return true;
                            case TypeCode.UInt64:
                                Convert.ToUInt16((UInt64)value);
                                return true;
                            case TypeCode.Single:
                                Convert.ToUInt16((Single)value);
                                return true;
                            case TypeCode.Double:
                                Convert.ToUInt16((Double)value);
                                return true;
                            case TypeCode.Decimal:
                                Convert.ToUInt16((Decimal)value);
                                return true;
                            case TypeCode.String:
                                Convert.ToUInt16((String)value);
                                return true;
                        }
                    }
                    catch { }
                }
            }
            
            return false;
        }

        /// <summary>
        /// Determines if a value can be implicitly converted to Int32.
        /// </summary>
        /// <param name="value">Value to test.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>True if implicit conversion possible; otherwise false.</returns>
        public static bool CanConvertToInt32(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Char:
                        case TypeCode.SByte:
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                            return true;
                    }
                }
                else
                {
                    try
                    {
                        switch (tc)
                        {
                            case TypeCode.Boolean:
                            case TypeCode.SByte:
                            case TypeCode.Byte:
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                            case TypeCode.Int32:
                                return true;
                            case TypeCode.UInt32:
                                Convert.ToInt32((UInt32)value);
                                return true;
                            case TypeCode.Int64:
                                Convert.ToInt32((Int64)value);
                                return true;
                            case TypeCode.UInt64:
                                Convert.ToInt32((UInt64)value);
                                return true;
                            case TypeCode.Single:
                                Convert.ToInt32((Single)value);
                                return true;
                            case TypeCode.Double:
                                Convert.ToInt32((Double)value);
                                return true;
                            case TypeCode.Decimal:
                                Convert.ToInt32((Decimal)value);
                                return true;
                            case TypeCode.String:
                                Convert.ToInt32((String)value);
                                return true;
                        }
                    }
                    catch { }
                }
            }
            
            return false;
        }

        /// <summary>
        /// Determines if a value can be implicitly converted to UInt32.
        /// </summary>
        /// <param name="value">Value to test.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>True if implicit conversion possible; otherwise false.</returns>
        public static bool CanConvertToUInt32(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Char:
                        case TypeCode.Byte:
                        case TypeCode.UInt16:
                        case TypeCode.UInt32:
                            return true;
                    }
                }
                else
                {
                    try
                    {
                        switch (tc)
                        {
                            case TypeCode.Boolean:
                            case TypeCode.Byte:
                            case TypeCode.UInt16:
                            case TypeCode.UInt32:
                                return true;
                            case TypeCode.SByte:
                                Convert.ToUInt32((SByte)value);
                                return true;
                            case TypeCode.Int16:
                                Convert.ToUInt32((Int16)value);
                                return true;
                            case TypeCode.Int32:
                                Convert.ToUInt32((Int32)value);
                                return true;
                            case TypeCode.Int64:
                                Convert.ToUInt32((Int64)value);
                                return true;
                            case TypeCode.UInt64:
                                Convert.ToUInt32((UInt64)value);
                                return true;
                            case TypeCode.Single:
                                Convert.ToUInt32((Single)value);
                                return true;
                            case TypeCode.Double:
                                Convert.ToUInt32((Double)value);
                                return true;
                            case TypeCode.Decimal:
                                Convert.ToUInt32((Decimal)value);
                                return true;
                            case TypeCode.String:
                                Convert.ToUInt32((String)value);
                                return true;
                        }
                    }
                    catch { }
                }
            }
            
            return false;
        }

        /// <summary>
        /// Determines if a value can be implicitly converted to Int64.
        /// </summary>
        /// <param name="value">Value to test.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>True if implicit conversion possible; otherwise false.</returns>
        public static bool CanConvertToInt64(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Char:
                        case TypeCode.SByte:
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                            return true;
                    }
                }
                else
                {
                    try
                    {
                        switch (tc)
                        {
                            case TypeCode.Boolean:
                            case TypeCode.SByte:
                            case TypeCode.Byte:
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                                return true;
                            case TypeCode.UInt64:
                                Convert.ToInt64((UInt64)value);
                                return true;
                            case TypeCode.Single:
                                Convert.ToInt64((Single)value);
                                return true;
                            case TypeCode.Double:
                                Convert.ToInt64((Double)value);
                                return true;
                            case TypeCode.Decimal:
                                Convert.ToInt64((Decimal)value);
                                return true;
                            case TypeCode.String:
                                Convert.ToInt64((String)value);
                                return true;
                        }
                    }
                    catch { }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if a value can be implicitly converted to UInt64.
        /// </summary>
        /// <param name="value">Value to test.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>True if implicit conversion possible; otherwise false.</returns>
        public static bool CanConvertToUInt64(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Char:
                        case TypeCode.Byte:
                        case TypeCode.UInt16:
                        case TypeCode.UInt32:
                        case TypeCode.UInt64:
                            return true;
                    }
                }
                else
                {
                    try
                    {
                        switch (tc)
                        {
                            case TypeCode.Boolean:
                            case TypeCode.Byte:
                            case TypeCode.UInt16:
                            case TypeCode.UInt32:
                            case TypeCode.UInt64:
                                return true;
                            case TypeCode.SByte:
                                Convert.ToUInt64((SByte)value);
                                return true;
                            case TypeCode.Int16:
                                Convert.ToUInt64((Int16)value);
                                return true;
                            case TypeCode.Int32:
                                Convert.ToUInt64((Int32)value);
                                return true;
                            case TypeCode.Int64:
                                Convert.ToUInt64((Int64)value);
                                return true;
                            case TypeCode.Single:
                                Convert.ToUInt64((Single)value);
                                return true;
                            case TypeCode.Double:
                                Convert.ToUInt64((Double)value);
                                return true;
                            case TypeCode.Decimal:
                                Convert.ToUInt64((Decimal)value);
                                return true;
                            case TypeCode.String:
                                Convert.ToUInt64((String)value);
                                return true;
                        }
                    }
                    catch { }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if a value can be implicitly converted to Single.
        /// </summary>
        /// <param name="value">Value to test.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>True if implicit conversion possible; otherwise false.</returns>
        public static bool CanConvertToSingle(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Char:
                        case TypeCode.SByte:
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                            return true;
                    }
                }
                else
                {
                    try
                    {
                        switch (tc)
                        {
                            case TypeCode.Boolean:
                            case TypeCode.SByte:
                            case TypeCode.Byte:
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                                return true;
                            case TypeCode.Double:
                                Convert.ToSingle((Double)value);
                                return true;
                            case TypeCode.Decimal:
                                Convert.ToSingle((Decimal)value);
                                return true;
                            case TypeCode.String:
                                Convert.ToSingle((String)value);
                                return true;
                        }
                    }
                    catch { }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if a value can be implicitly converted to Double.
        /// </summary>
        /// <param name="value">Value to test.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>True if implicit conversion possible; otherwise false.</returns>
        public static bool CanConvertToDouble(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Char:
                        case TypeCode.SByte:
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                            return true;
                    }
                }
                else
                {
                    try
                    {
                        switch (tc)
                        {
                            case TypeCode.Boolean:
                            case TypeCode.SByte:
                            case TypeCode.Byte:
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Double:
                                return true;
                            case TypeCode.Decimal:
                                Convert.ToDouble((Decimal)value);
                                return true;
                            case TypeCode.String:
                                Convert.ToDouble((String)value);
                                return true;
                        }
                    }
                    catch { }
                }
            }

            return false;
        }

        /// <summary>
        /// Determines if a value can be implicitly converted to Decimal.
        /// </summary>
        /// <param name="value">Value to test.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>True if implicit conversion possible; otherwise false.</returns>
        public static bool CanConvertToDecimal(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Char:
                        case TypeCode.SByte:
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Decimal:
                            return true;
                    }
                }
                else
                {
                    try
                    {
                        switch (tc)
                        {
                            case TypeCode.Boolean:
                            case TypeCode.SByte:
                            case TypeCode.Byte:
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                            case TypeCode.Int64:
                            case TypeCode.UInt64:
                            case TypeCode.Single:
                            case TypeCode.Decimal:
                                return true;
                            case TypeCode.Double:
                                Convert.ToDecimal((Double)value);
                                return true;
                            case TypeCode.String:
                                Convert.ToDecimal((String)value);
                                return true;
                        }
                    }
                    catch { }
                }
            }

            return false;
        }

        /// <summary>
        /// Implicit conversion from incoming value to target type.
        /// </summary>
        /// <param name="target">Target type for value to be converted into.</param>
        /// <param name="value">Value to be converted.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>Boxed version of target type.</returns>
        public static object ConvertTo(TypeCode target, object value, Language language)
        {
            if (value == null)
            {
                if (target == TypeCode.Object)
                    return value;
            }
            else
            {
                // If the value is already the same target type then we are done
                if (Type.GetTypeCode(value.GetType()) == target)
                    return value;
                else
                {
                    // Perform implicit conversion for the types where it is possible
                    switch (target)
                    {
                        case TypeCode.String:
                            return value.ToString();
                        case TypeCode.Boolean:
                            return ConvertToBoolean(value, language);
                        case TypeCode.Char:
                            return ConvertToChar(value, language);
                        case TypeCode.SByte:
                            return ConvertToSByte(value, language);
                        case TypeCode.Byte:
                            return ConvertToByte(value, language);
                        case TypeCode.Int16:
                            return ConvertToInt16(value, language);
                        case TypeCode.UInt16:
                            return ConvertToUInt16(value, language);
                        case TypeCode.Int32:
                            return ConvertToInt32(value, language);
                        case TypeCode.UInt32:
                            return ConvertToUInt32(value, language);
                        case TypeCode.Int64:
                            return ConvertToInt64(value, language);
                        case TypeCode.UInt64:
                            return ConvertToUInt64(value, language);
                        case TypeCode.Single:
                            return ConvertToSingle(value, language);
                        case TypeCode.Double:
                            return ConvertToDouble(value, language);
                        case TypeCode.Decimal:
                            return ConvertToDecimal(value, language);
                        case TypeCode.Object:
                            return value;
                    }
                }
            }

            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to '" + target.ToString() + "'.");
        }

        /// <summary>
        /// Implicit conversion from incoming value to Boolean.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>Value converted to Boolean.</returns>
        public static bool ConvertToBoolean(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType()); 
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Boolean:
                            return (bool)value;
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Boolean'.");
                    }
                }
                else
                {
                    switch (tc)
                    {
                        case TypeCode.Boolean:
                            return (bool)value;
                        case TypeCode.SByte:
                            return ((SByte)value != 0);
                        case TypeCode.Byte:
                            return ((Byte)value != 0);
                        case TypeCode.Int16:
                            return ((Int16)value != 0);
                        case TypeCode.UInt16:
                            return ((UInt16)value != 0);
                        case TypeCode.Int32:
                            return ((Int32)value != 0);
                        case TypeCode.UInt32:
                            return ((UInt32)value != 0);
                        case TypeCode.Int64:
                            return ((Int64)value != 0);
                        case TypeCode.UInt64:
                            return ((UInt64)value != 0);
                        case TypeCode.Single:
                            return ((Single)value != 0);
                        case TypeCode.Double:
                            return ((Double)value != 0);
                        case TypeCode.Decimal:
                            return ((Decimal)value != 0);
                        case TypeCode.String:
                            switch (((string)value).ToLower())
                            {
                                case "true":
                                    return true;
                                case "false":
                                    return false;
                                default:
                                    throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Boolean' for value '" + value.ToString() + "'.");
                            }
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Boolean'.");
                    }
                }
            }
            else
                throw new NullReferenceException("Cannot convert incoming 'null' to an 'Boolean' value.");
        }

        /// <summary>
        /// Implicit conversion from incoming value to Char.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>Value converted to Char.</returns>
        public static char ConvertToChar(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Char:
                            return (char)value;
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Char'.");
                    }
                }
                else
                {
                    switch (tc)
                    {
                        case TypeCode.Char:
                            return (char)value;
                        case TypeCode.String:
                            if (string.IsNullOrEmpty((string)value))
                                return (char)0;
                            else
                                return ((string)value)[0];
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Char'.");
                    }
                }
            }
            else
                throw new NullReferenceException("Cannot convert incoming 'null' to an 'Char' value.");
        }

        /// <summary>
        /// Implicit conversion from incoming value to SByte.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>Value converted to SByte.</returns>
        public static SByte ConvertToSByte(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType()); 
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.SByte:
                            return (SByte)value;
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'SByte'.");
                    }
                }
                else
                {
                    switch (tc)
                    {
                        case TypeCode.Boolean:
                            return (bool)value ? (SByte)(-1) : (SByte)0;
                        case TypeCode.SByte:
                            return (SByte)value;
                        case TypeCode.Byte:
                            return Convert.ToSByte((Byte)value);
                        case TypeCode.Int16:
                            return Convert.ToSByte((Int16)value);
                        case TypeCode.UInt16:
                            return Convert.ToSByte((UInt16)value);
                        case TypeCode.Int32:
                            return Convert.ToSByte((Int32)value);
                        case TypeCode.UInt32:
                            return Convert.ToSByte((UInt32)value);
                        case TypeCode.Int64:
                            return Convert.ToSByte((Int64)value);
                        case TypeCode.UInt64:
                            return Convert.ToSByte((UInt64)value);
                        case TypeCode.Single:
                            return Convert.ToSByte((Single)value);
                        case TypeCode.Double:
                            return Convert.ToSByte((Double)value);
                        case TypeCode.Decimal:
                            return Convert.ToSByte((Decimal)value);
                        case TypeCode.String:
                            return Convert.ToSByte((String)value);
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'SByte'.");
                    }
                }
            }
            else
                throw new NullReferenceException("Cannot convert incoming 'null' to an 'SByte' value.");
        }

        /// <summary>
        /// Implicit conversion from incoming value to Byte.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>Value converted to Byte.</returns>
        public static Byte ConvertToByte(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Byte:
                            return (Byte)value;
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Byte'.");
                    }
                }
                else
                {
                    switch (tc)
                    {
                        case TypeCode.Boolean:
                            return (bool)value ? (Byte)255 : (Byte)0;
                        case TypeCode.SByte:
                            return Convert.ToByte((SByte)value);
                        case TypeCode.Byte:
                            return (Byte)value;
                        case TypeCode.Int16:
                            return Convert.ToByte((Int16)value);
                        case TypeCode.UInt16:
                            return Convert.ToByte((UInt16)value);
                        case TypeCode.Int32:
                            return Convert.ToByte((Int32)value);
                        case TypeCode.UInt32:
                            return Convert.ToByte((UInt32)value);
                        case TypeCode.Int64:
                            return Convert.ToByte((Int64)value);
                        case TypeCode.UInt64:
                            return Convert.ToByte((UInt64)value);
                        case TypeCode.Single:
                            return Convert.ToByte((Single)value);
                        case TypeCode.Double:
                            return Convert.ToByte((Double)value);
                        case TypeCode.Decimal:
                            return Convert.ToByte((Decimal)value);
                        case TypeCode.String:
                            return Convert.ToByte((String)value);
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Byte'.");
                    }
                }
            }
            else
                throw new NullReferenceException("Cannot convert incoming 'null' to an 'Byte' value.");
        }

        /// <summary>
        /// Implicit conversion from incoming value to Int16.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>Value converted to Int16.</returns>
        public static Int16 ConvertToInt16(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.SByte:
                            return (Int16)(SByte)value;
                        case TypeCode.Byte:
                            return (Int16)(Byte)value;
                        case TypeCode.Int16:
                            return (Int16)value;
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Int16'.");
                    }
                }
                else
                {
                    switch (tc)
                    {
                        case TypeCode.Boolean:
                            return (bool)value ? (Int16)(-1) : (Int16)0;
                        case TypeCode.SByte:
                            return (Int16)(SByte)value;
                        case TypeCode.Byte:
                            return (Int16)(Byte)value;
                        case TypeCode.Int16:
                            return (Int16)value;
                        case TypeCode.UInt16:
                            return Convert.ToInt16((UInt16)value);
                        case TypeCode.Int32:
                            return Convert.ToInt16((Int32)value);
                        case TypeCode.UInt32:
                            return Convert.ToInt16((UInt32)value);
                        case TypeCode.Int64:
                            return Convert.ToInt16((Int64)value);
                        case TypeCode.UInt64:
                            return Convert.ToInt16((UInt64)value);
                        case TypeCode.Single:
                            return Convert.ToInt16((Single)value);
                        case TypeCode.Double:
                            return Convert.ToInt16((Double)value);
                        case TypeCode.Decimal:
                            return Convert.ToInt16((Decimal)value);
                        case TypeCode.String:
                            return Convert.ToInt16((String)value);
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Int16'.");
                    }
                }
            }
            else
                throw new NullReferenceException("Cannot convert incoming 'null' to an 'Int16' value.");
        }

        /// <summary>
        /// Implicit conversion from incoming value to UInt16.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>Value converted to UInt16.</returns>
        public static UInt16 ConvertToUInt16(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Byte:
                            return (UInt16)(Byte)value;
                        case TypeCode.Char:
                            return (UInt16)(Char)value;
                        case TypeCode.UInt16:
                            return (UInt16)value;
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'UInt16'.");
                    }
                }
                else
                {
                    switch (tc)
                    {
                        case TypeCode.Boolean:
                            return (bool)value ? UInt16.MaxValue : (UInt16)0;
                        case TypeCode.Byte:
                            return (UInt16)(Byte)value;
                        case TypeCode.UInt16:
                            return (UInt16)value;
                        case TypeCode.SByte:
                            return Convert.ToUInt16((SByte)value);
                        case TypeCode.Int16:
                            return Convert.ToUInt16((Int16)value);
                        case TypeCode.Int32:
                            return Convert.ToUInt16((Int32)value);
                        case TypeCode.UInt32:
                            return Convert.ToUInt16((UInt32)value);
                        case TypeCode.Int64:
                            return Convert.ToUInt16((Int64)value);
                        case TypeCode.UInt64:
                            return Convert.ToUInt16((UInt64)value);
                        case TypeCode.Single:
                            return Convert.ToUInt16((Single)value);
                        case TypeCode.Double:
                            return Convert.ToUInt16((Double)value);
                        case TypeCode.Decimal:
                            return Convert.ToUInt16((Decimal)value);
                        case TypeCode.String:
                            return Convert.ToUInt16((String)value);
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'UInt16'.");
                    }
                }
            }
            else
                throw new NullReferenceException("Cannot convert incoming 'null' to an 'UInt16' value.");
        }

        /// <summary>
        /// Implicit conversion from incoming value to Int32.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>Value converted to Int32.</returns>
        public static Int32 ConvertToInt32(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Char:
                            return (Int32)(Char)value;
                        case TypeCode.SByte:
                            return (Int32)(SByte)value;
                        case TypeCode.Byte:
                            return (Int32)(Byte)value;
                        case TypeCode.Int16:
                            return (Int32)(Int16)value;
                        case TypeCode.UInt16:
                            return (Int32)(UInt16)value;
                        case TypeCode.Int32:
                            return (Int32)value;
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Int32'.");
                    }
                }
                else
                {
                    switch (tc)
                    {
                        case TypeCode.Boolean:
                            return (bool)value ? (Int32)(-1) : (Int32)0;
                        case TypeCode.SByte:
                            return (Int32)(SByte)value;
                        case TypeCode.Byte:
                            return (Int32)(Byte)value;
                        case TypeCode.Int16:
                            return (Int32)(Int16)value;
                        case TypeCode.UInt16:
                            return (Int32)(UInt16)value;
                        case TypeCode.Int32:
                            return (Int32)value;
                        case TypeCode.UInt32:
                            return Convert.ToInt32((UInt32)value);
                        case TypeCode.Int64:
                            return Convert.ToInt32((Int64)value);
                        case TypeCode.UInt64:
                            return Convert.ToInt32((UInt64)value);
                        case TypeCode.Single:
                            return Convert.ToInt32((Single)value);
                        case TypeCode.Double:
                            return Convert.ToInt32((Double)value);
                        case TypeCode.Decimal:
                            return Convert.ToInt32((Decimal)value);
                        case TypeCode.String:
                            return Convert.ToInt32((String)value);
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Int32'.");
                    }
                }
            }
            else
                throw new NullReferenceException("Cannot convert incoming 'null' to an 'Int32' value.");
        }

        /// <summary>
        /// Implicit conversion from incoming value to UInt32.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>Value converted to UInt32.</returns>
        public static UInt32 ConvertToUInt32(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Char:
                            return (UInt32)(Char)value;
                        case TypeCode.Byte:
                            return (UInt32)(Byte)value;
                        case TypeCode.UInt16:
                            return (UInt32)(UInt16)value;
                        case TypeCode.UInt32:
                            return (UInt32)value;
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'UInt32'.");
                    }
                }
                else
                {
                    switch (tc)
                    {
                        case TypeCode.Boolean:
                            return (bool)value ? UInt32.MaxValue : (UInt32)0;
                        case TypeCode.Byte:
                            return (UInt32)(Byte)value;
                        case TypeCode.UInt16:
                            return (UInt32)(UInt16)value;
                        case TypeCode.UInt32:
                            return (UInt32)value;
                        case TypeCode.SByte:
                            return Convert.ToUInt32((SByte)value);
                        case TypeCode.Int16:
                            return Convert.ToUInt32((Int16)value);
                        case TypeCode.Int32:
                            return Convert.ToUInt32((Int32)value);
                        case TypeCode.Int64:
                            return Convert.ToUInt32((Int64)value);
                        case TypeCode.UInt64:
                            return Convert.ToUInt32((UInt64)value);
                        case TypeCode.Single:
                            return Convert.ToUInt32((Single)value);
                        case TypeCode.Double:
                            return Convert.ToUInt32((Double)value);
                        case TypeCode.Decimal:
                            return Convert.ToUInt32((Decimal)value);
                        case TypeCode.String:
                            return Convert.ToUInt32((String)value);
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'UInt32'.");
                    }
                }
            }
            else
                throw new NullReferenceException("Cannot convert incoming 'null' to an 'UInt32' value.");
        }

        /// <summary>
        /// Implicit conversion from incoming value to Int64.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>Value converted to Int64.</returns>
        public static Int64 ConvertToInt64(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Char:
                            return (Int64)(Char)value;
                        case TypeCode.SByte:
                            return (Int64)(SByte)value;
                        case TypeCode.Byte:
                            return (Int64)(Byte)value;
                        case TypeCode.Int16:
                            return (Int64)(Int16)value;
                        case TypeCode.UInt16:
                            return (Int64)(UInt16)value;
                        case TypeCode.Int32:
                            return (Int64)(Int32)value;
                        case TypeCode.UInt32:
                            return (Int64)(UInt32)value;
                        case TypeCode.Int64:
                            return (Int64)value;
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Int64'.");
                    }
                }
                else
                {
                    switch (tc)
                    {
                        case TypeCode.Boolean:
                            return (bool)value ? (Int64)(-1) : (Int64)0;
                        case TypeCode.SByte:
                            return (Int64)(SByte)value;
                        case TypeCode.Byte:
                            return (Int64)(Byte)value;
                        case TypeCode.Int16:
                            return (Int64)(Int16)value;
                        case TypeCode.UInt16:
                            return (Int64)(UInt16)value;
                        case TypeCode.Int32:
                            return (Int64)(Int32)value;
                        case TypeCode.UInt32:
                            return (Int64)(UInt32)value;
                        case TypeCode.Int64:
                            return (Int64)value;
                        case TypeCode.UInt64:
                            return Convert.ToInt64((UInt64)value);
                        case TypeCode.Single:
                            return Convert.ToInt64((Single)value);
                        case TypeCode.Double:
                            return Convert.ToInt64((Double)value);
                        case TypeCode.Decimal:
                            return Convert.ToInt64((Decimal)value);
                        case TypeCode.String:
                            return Convert.ToInt64((String)value);
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Int64'.");
                    }
                }
            }
            else
                throw new NullReferenceException("Cannot convert incoming 'null' to an 'Int64' value.");
        }

        /// <summary>
        /// Implicit conversion from incoming value to UInt64.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>Value converted to UInt64.</returns>
        public static UInt64 ConvertToUInt64(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Char:
                            return (UInt64)(Char)value;
                        case TypeCode.Byte:
                            return (UInt64)(Byte)value;
                        case TypeCode.UInt16:
                            return (UInt64)(UInt16)value;
                        case TypeCode.UInt32:
                            return (UInt64)(UInt32)value;
                        case TypeCode.UInt64:
                            return (UInt64)value;
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'UInt64'.");
                    }
                }
                else
                {
                    switch (tc)
                    {
                        case TypeCode.Boolean:
                            return (bool)value ? UInt64.MaxValue : (UInt64)0;
                        case TypeCode.Byte:
                            return (UInt64)(Byte)value;
                        case TypeCode.UInt16:
                            return (UInt64)(UInt16)value;
                        case TypeCode.UInt32:
                            return (UInt64)(UInt32)value;
                        case TypeCode.UInt64:
                            return (UInt64)value;
                        case TypeCode.SByte:
                            return Convert.ToUInt64((SByte)value);
                        case TypeCode.Int16:
                            return Convert.ToUInt64((Int16)value);
                        case TypeCode.Int32:
                            return Convert.ToUInt64((Int32)value);
                        case TypeCode.Int64:
                            return Convert.ToUInt64((Int64)value);
                        case TypeCode.Single:
                            return Convert.ToUInt64((Single)value);
                        case TypeCode.Double:
                            return Convert.ToUInt64((Double)value);
                        case TypeCode.Decimal:
                            return Convert.ToUInt64((Decimal)value);
                        case TypeCode.String:
                            return Convert.ToUInt64((String)value);
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'UInt64'.");
                    }
                }
            }
            else
                throw new NullReferenceException("Cannot convert incoming 'null' to an 'UInt64' value.");
        }

        /// <summary>
        /// Implicit conversion from incoming value to Single.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>Value converted to Single.</returns>
        public static Single ConvertToSingle(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Char:
                            return (Single)(Char)value;
                        case TypeCode.SByte:
                            return (Single)(SByte)value;
                        case TypeCode.Byte:
                            return (Single)(Byte)value;
                        case TypeCode.Int16:
                            return (Single)(Int16)value;
                        case TypeCode.UInt16:
                            return (Single)(UInt16)value;
                        case TypeCode.Int32:
                            return (Single)(Int32)value;
                        case TypeCode.UInt32:
                            return (Single)(UInt32)value;
                        case TypeCode.Int64:
                            return (Single)(Int64)value;
                        case TypeCode.UInt64:
                            return (Single)(UInt64)value;
                        case TypeCode.Single:
                            return (Single)value;
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Single'.");
                    }
                }
                else
                {
                    switch (tc)
                    {
                        case TypeCode.Boolean:
                            return (bool)value ? (Single)(-1) : (Single)0;
                        case TypeCode.SByte:
                            return (Single)(SByte)value;
                        case TypeCode.Byte:
                            return (Single)(Byte)value;
                        case TypeCode.Int16:
                            return (Single)(Int16)value;
                        case TypeCode.UInt16:
                            return (Single)(UInt16)value;
                        case TypeCode.Int32:
                            return (Single)(Int32)value;
                        case TypeCode.UInt32:
                            return (Single)(UInt32)value;
                        case TypeCode.Int64:
                            return (Single)(Int64)value;
                        case TypeCode.UInt64:
                            return (Single)(UInt64)value;
                        case TypeCode.Single:
                            return (Single)value;
                        case TypeCode.Double:
                            return Convert.ToSingle((Double)value);
                        case TypeCode.Decimal:
                            return Convert.ToSingle((Decimal)value);
                        case TypeCode.String:
                            return Convert.ToSingle((String)value);
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Single'.");
                    }
                }
            }
            else
                throw new NullReferenceException("Cannot convert incoming 'null' to an 'Single' value.");
        }

        /// <summary>
        /// Implicit conversion from incoming value to Double.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>Value converted to Double.</returns>
        public static Double ConvertToDouble(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Char:
                            return (Double)(Char)value;
                        case TypeCode.SByte:
                            return (Double)(SByte)value;
                        case TypeCode.Byte:
                            return (Double)(Byte)value;
                        case TypeCode.Int16:
                            return (Double)(Int16)value;
                        case TypeCode.UInt16:
                            return (Double)(UInt16)value;
                        case TypeCode.Int32:
                            return (Double)(Int32)value;
                        case TypeCode.UInt32:
                            return (Double)(UInt32)value;
                        case TypeCode.Int64:
                            return (Double)(Int64)value;
                        case TypeCode.UInt64:
                            return (Double)(UInt64)value;
                        case TypeCode.Single:
                            return (Double)(Single)value;
                        case TypeCode.Double:
                            return (Double)value;
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Double'.");
                    }
                }
                else
                {
                    switch (tc)
                    {
                        case TypeCode.Boolean:
                            return (bool)value ? (Double)(-1) : (Double)0;
                        case TypeCode.SByte:
                            return (Double)(SByte)value;
                        case TypeCode.Byte:
                            return (Double)(Byte)value;
                        case TypeCode.Int16:
                            return (Double)(Int16)value;
                        case TypeCode.UInt16:
                            return (Double)(UInt16)value;
                        case TypeCode.Int32:
                            return (Double)(Int32)value;
                        case TypeCode.UInt32:
                            return (Double)(UInt32)value;
                        case TypeCode.Int64:
                            return (Double)(Int64)value;
                        case TypeCode.UInt64:
                            return (Double)(UInt64)value;
                        case TypeCode.Single:
                            return (Double)(Single)value;
                        case TypeCode.Double:
                            return (Double)value;
                        case TypeCode.Decimal:
                            return Convert.ToSingle((Decimal)value);
                        case TypeCode.String:
                            return Convert.ToSingle((String)value);
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Double'.");
                    }
                }
            }
            else
                throw new NullReferenceException("Cannot convert incoming 'null' to an 'Double' value.");
        }


        /// <summary>
        /// Implicit conversion from incoming value to Decimal.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <param name="language">Language rules to be used.</param>
        /// <returns>Value converted to Decimal.</returns>
        public static Decimal ConvertToDecimal(object value, Language language)
        {
            // Cannot convert null
            if (value != null)
            {
                TypeCode tc = Type.GetTypeCode(value.GetType());
                if (language == Language.CSharp)
                {
                    switch (tc)
                    {
                        case TypeCode.Char:
                            return (Decimal)(Char)value;
                        case TypeCode.SByte:
                            return (Decimal)(SByte)value;
                        case TypeCode.Byte:
                            return (Decimal)(Byte)value;
                        case TypeCode.Int16:
                            return (Decimal)(Int16)value;
                        case TypeCode.UInt16:
                            return (Decimal)(UInt16)value;
                        case TypeCode.Int32:
                            return (Decimal)(Int32)value;
                        case TypeCode.UInt32:
                            return (Decimal)(UInt32)value;
                        case TypeCode.Int64:
                            return (Decimal)(Int64)value;
                        case TypeCode.UInt64:
                            return (Decimal)(UInt64)value;
                        case TypeCode.Single:
                            return (Decimal)(Single)value;
                        case TypeCode.Decimal:
                            return (Decimal)value;
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Decimal'.");
                    }
                }
                else
                {
                    switch (tc)
                    {
                        case TypeCode.Boolean:
                            return (bool)value ? (Decimal)(-1) : (Decimal)0;
                        case TypeCode.SByte:
                            return (Decimal)(SByte)value;
                        case TypeCode.Byte:
                            return (Decimal)(Byte)value;
                        case TypeCode.Int16:
                            return (Decimal)(Int16)value;
                        case TypeCode.UInt16:
                            return (Decimal)(UInt16)value;
                        case TypeCode.Int32:
                            return (Decimal)(Int32)value;
                        case TypeCode.UInt32:
                            return (Decimal)(UInt32)value;
                        case TypeCode.Int64:
                            return (Decimal)(Int64)value;
                        case TypeCode.UInt64:
                            return (Decimal)(UInt64)value;
                        case TypeCode.Single:
                            return (Decimal)(Single)value;
                        case TypeCode.Double:
                            return Convert.ToDecimal((Double)value);
                        case TypeCode.Decimal:
                            return (Decimal)value;
                        case TypeCode.String:
                            return Convert.ToDecimal((String)value);
                        default:
                            throw new ArgumentException("Cannot perform implicit conversion from incoming '" + Type.GetTypeCode(value.GetType()).ToString() + "' to 'Decimal'.");
                    }
                }
            }
            else
                throw new NullReferenceException("Cannot convert incoming 'null' to an 'Decimal' value.");
        }
    }
}

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
    /// Evalutes the expression using the provided set of source binding values.
    /// </summary>
    public class EvalBindingConverter : IMultiValueConverter
    {
        #region Instance Fields
        private Eval _eval;
        private string _expression;
        private Language _language;
        private FrameworkElement _target;
        #endregion

        #region Identity
        /// <summary>
        /// Initialise a new instance of the EvalBindingConverter class.
        /// </summary>
        /// <param name="expression">Immutable expression to evaluate.</param>
        /// <param name="language">Language used to parse and the expression.</param>
        /// <param name="target">Framework element that is target of conversion result.</param>
        public EvalBindingConverter(string expression, 
                                    Language language,
                                    FrameworkElement target)
        {
            _expression = expression;
            _language = language;
            _target = target;
        }
        #endregion

        #region Public
        /// <summary>
        /// Converts source values to a value for the binding target.
        /// </summary>
        /// <param name="values">The array of values that the source bindings produces.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                // First time around we parse the evaluation input
                if (_eval == null)
                {
                    _eval = new Eval(_language);
                    _eval.ResolveIdentifier += new EventHandler<ResolveIdentifierEventArgs>(OnResolveIdentifier);
                    _eval.Parse(_expression);
                }

                // Every time we evaluate we could throw an exception. This is because each time around the
                // bindings can provide values of different types. So first time round the binding has an
                // integer and so works correctly but next time it could provide a string and cause a type
                // error in the evaluation.
                EvalResult ret = _eval.Evaluate(values);

                // Null means we have no result to provide
                if (ret == null)
                    return DependencyProperty.UnsetValue;
                else
                {
                    // If the return type is different to the target type
                    if (ret.Value.GetType() != targetType)
                    {
                        // If possible we perform an implicit conversion to the target
                        TypeCode targetTypeCode = Type.GetTypeCode(targetType);
                        if (ImplicitConverter.CanConvertTo(targetTypeCode, ret.Value, _language))
                            ret.Value = ImplicitConverter.ConvertTo(targetTypeCode, ret.Value, _language);
                        else
                        {
                            // Use type converters to attempt an explicit conversion
                            ret.Value = ConvertUsingConverter(ret, targetType);
                        }
                    }

                    return ret.Value;
                }
            }
            catch (ParseException pe)
            {
                Console.WriteLine("EvalBinding Parsing Exception : {0} : Index '{1}'", pe.Message, pe.Index);
                return DependencyProperty.UnsetValue;
            }
            catch (Exception e)
            {
                Console.WriteLine("EvalBinding Evaluation Exception : {0} : Eval '{1}'", e.Message, _eval.ToString());
                return DependencyProperty.UnsetValue;
            }
        }
        
        /// <summary>
        /// Converts a binding target value to the source binding values.
        /// </summary>
        /// <param name="value">The value that the binding target produces.</param>
        /// <param name="targetTypes">The array of types to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>An array of values that have been converted from the target value back to the source values.</returns>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            // Can never go backwards
            return null;
        }
        #endregion

        #region Protected
        /// <summary>
        /// Occurs when the contained eval needs to resolve an unknown identifier.
        /// </summary>
        /// <param name="sender">Source of event.</param>
        /// <param name="e">An ResolveIdentifierEventArgs containing the event data.</param>
        protected virtual void OnResolveIdentifier(object sender, ResolveIdentifierEventArgs e)
        {
            // Search for an element with the same name as the requested identifier
            if ((e.Value == null) && (_target != null))
                e.Value = _target.FindName(e.Identifier);
        }
        #endregion

        #region Private
        private object ConvertUsingConverter(EvalResult ret, Type targetType)
        {
            // Try and get a converter from the target type
            TypeConverter converter = TypeDescriptor.GetConverter(targetType);
            if (converter != null)
            {
                // Ask the converter to convert using the source type when possible, if not possible
                // fallback to requesting a string version of the source be converted to the target
                switch (ret.Type)
                {
                    case TypeCode.Boolean:
                        if (converter.CanConvertFrom(typeof(Boolean)))
                            return converter.ConvertFrom((Boolean)ret.Value);
                        else if (converter.CanConvertFrom(typeof(String)))
                            return converter.ConvertFromString(ret.Value.ToString());
                        break;
                    case TypeCode.SByte:
                        if (converter.CanConvertFrom(typeof(SByte)))
                            return converter.ConvertFrom((SByte)ret.Value);
                        else if (converter.CanConvertFrom(typeof(String)))
                            return converter.ConvertFromString(ret.Value.ToString());
                        break;
                    case TypeCode.Byte:
                        if (converter.CanConvertFrom(typeof(Byte)))
                            return converter.ConvertFrom((Byte)ret.Value);
                        else if (converter.CanConvertFrom(typeof(String)))
                            return converter.ConvertFromString(ret.Value.ToString());
                        break;
                    case TypeCode.Char:
                        if (converter.CanConvertFrom(typeof(Char)))
                            return converter.ConvertFrom((Char)ret.Value);
                        else if (converter.CanConvertFrom(typeof(String)))
                            return converter.ConvertFromString(ret.Value.ToString());
                        break;
                    case TypeCode.Int16:
                        if (converter.CanConvertFrom(typeof(Int16)))
                            return converter.ConvertFrom((Int16)ret.Value);
                        else if (converter.CanConvertFrom(typeof(String)))
                            return converter.ConvertFromString(ret.Value.ToString());
                        break;
                    case TypeCode.Int32:
                        if (converter.CanConvertFrom(typeof(Int32)))
                            return converter.ConvertFrom((Int32)ret.Value);
                        else if (converter.CanConvertFrom(typeof(String)))
                            return converter.ConvertFromString(ret.Value.ToString());
                        break;
                    case TypeCode.Int64:
                        if (converter.CanConvertFrom(typeof(Int64)))
                            return converter.ConvertFrom((Int64)ret.Value);
                        else if (converter.CanConvertFrom(typeof(String)))
                            return converter.ConvertFromString(ret.Value.ToString());
                        break;
                    case TypeCode.UInt16:
                        if (converter.CanConvertFrom(typeof(UInt16)))
                            return converter.ConvertFrom((UInt16)ret.Value);
                        else if (converter.CanConvertFrom(typeof(String)))
                            return converter.ConvertFromString(ret.Value.ToString());
                        break;
                    case TypeCode.UInt32:
                        if (converter.CanConvertFrom(typeof(UInt32)))
                            return converter.ConvertFrom((UInt32)ret.Value);
                        else if (converter.CanConvertFrom(typeof(String)))
                            return converter.ConvertFromString(ret.Value.ToString());
                        break;
                    case TypeCode.UInt64:
                        if (converter.CanConvertFrom(typeof(UInt64)))
                            return converter.ConvertFrom((UInt64)ret.Value);
                        else if (converter.CanConvertFrom(typeof(String)))
                            return converter.ConvertFromString(ret.Value.ToString());
                        break;
                    case TypeCode.Single:
                        if (converter.CanConvertFrom(typeof(Single)))
                            return converter.ConvertFrom((Single)ret.Value);
                        else if (converter.CanConvertFrom(typeof(String)))
                            return converter.ConvertFromString(ret.Value.ToString());
                        break;
                    case TypeCode.Double:
                        if (converter.CanConvertFrom(typeof(Double)))
                            return converter.ConvertFrom((Double)ret.Value);
                        else if (converter.CanConvertFrom(typeof(String)))
                            return converter.ConvertFromString(ret.Value.ToString());
                        break;
                    case TypeCode.Decimal:
                        if (converter.CanConvertFrom(typeof(Decimal)))
                            return converter.ConvertFrom((Decimal)ret.Value);
                        else if (converter.CanConvertFrom(typeof(String)))
                            return converter.ConvertFromString(ret.Value.ToString());
                        break;
                    case TypeCode.String:
                        if (converter.CanConvertFrom(typeof(String)))
                            return converter.ConvertFrom((String)ret.Value);
                        else if (converter.CanConvertFrom(typeof(String)))
                            return converter.ConvertFromString(ret.Value.ToString());
                        break;
                    case TypeCode.DateTime:
                        if (converter.CanConvertFrom(typeof(DateTime)))
                            return converter.ConvertFrom((DateTime)ret.Value);
                        else if (converter.CanConvertFrom(typeof(String)))
                            return converter.ConvertFromString(ret.Value.ToString());
                        break;
                    case TypeCode.DBNull:
                        if (converter.CanConvertFrom(typeof(DBNull)))
                            return converter.ConvertFrom((DBNull)ret.Value);
                        else if (converter.CanConvertFrom(typeof(String)))
                            return converter.ConvertFromString(ret.Value.ToString());
                        break;
                    case TypeCode.Object:
                        if (converter.CanConvertFrom(typeof(Object)))
                            return converter.ConvertFrom((Object)ret.Value);
                        else if (converter.CanConvertFrom(typeof(String)))
                            return converter.ConvertFromString(ret.Value.ToString());
                        break;
                }
            }

            // Unable to convert the value
            return ret.Value;
        }
        #endregion
    }
}

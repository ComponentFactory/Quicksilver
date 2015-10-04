// *****************************************************************************
// 
//  © Component Factory Pty Ltd 2011. All rights reserved.
//	The software and associated documentation supplied hereunder are the 
//  proprietary information of Component Factory Pty Ltd, 17/267 Nepean Hwy, 
//  Seaford, Vic 3198, Australia and are supplied subject to licence terms.
// 
//  Version 1.0.8.0 	www.ComponentFactory.com
// *****************************************************************************

using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using System.Globalization;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

namespace ComponentFactory.Quicksilver.Layout
{
    /// <summary>
    /// Converts instances of other types to and from instances of a Double that represent an object's length.
    /// </summary>
    public class LengthConverter : TypeConverter
    {
        #region Static Fields
        private static Dictionary<string, double> _unitToPixelConversions = new Dictionary<string, double>
        {
            { "px", 1.0 },
            { "in", 96.0 },
            { "cm", 37.795275590551178 },
            { "pt", 1.3333333333333333 }
        };
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the LengthConverter class.
        /// </summary>
        public LengthConverter()
        {
        }
        #endregion

        #region Public
        /// <summary>
        /// Determines whether conversion is possible from a specified type to a Double that represents an object's length.
        /// </summary>
        /// <param name="typeDescriptorContext">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="sourceType">An ITypeDescriptorContext that provides a format context.</param>
        /// <returns>true if conversion is possible; otherwise, false.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext typeDescriptorContext, 
                                            Type sourceType)
        {
            switch (Type.GetTypeCode(sourceType))
            {
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.String:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Converts instances of other data types into instances of Double that represent an object's length.
        /// </summary>
        /// <param name="typeDescriptorContext">An ITypeDescriptorContext that provides a format context.</param>
        /// <param name="cultureInfo">The CultureInfo to use as the current culture.</param>
        /// <param name="source">Identifies the object that is being converted to Double.</param>
        /// <returns>An instance of Double that is the value of the conversion.</returns>
        public override object ConvertFrom(ITypeDescriptorContext typeDescriptorContext, 
                                           CultureInfo cultureInfo, 
                                           object source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (source is string)
            {
                string text = (string)source;

                // Convert Auto to NaN
                if (string.Compare(text, "Auto", StringComparison.OrdinalIgnoreCase) == 0)
                    return double.NaN;

                // Get the unit conversion factor
                string number = text;
                double conversionFactor = 1.0;
                foreach (KeyValuePair<string, double> conversion in _unitToPixelConversions)
                {
                    if (number.EndsWith(conversion.Key, StringComparison.Ordinal))
                    {
                        conversionFactor = conversion.Value;
                        number = text.Substring(0, number.Length - conversion.Key.Length);
                        break;
                    }
                }

                // Convert the value
                return conversionFactor * Convert.ToDouble(number, cultureInfo);
            }

            return Convert.ToDouble(source, cultureInfo);
        }

        /// <summary>
        /// Determines whether conversion is possible to a specified type from a Double that represents an object's length.
        /// </summary>
        /// <param name="typeDescriptorContext">Provides contextual information about a component.</param>
        /// <param name="destinationType">Identifies the data type to evaluate for conversion.</param>
        /// <returns>true if conversion to the destinationType is possible; otherwise, false.</returns>
        public override bool CanConvertTo(ITypeDescriptorContext typeDescriptorContext, Type destinationType)
        {
            return (destinationType == typeof(string));
        }

        /// <summary>
        /// Converts other types into instances of Double that represent an object's length.
        /// </summary>
        /// <param name="typeDescriptorContext">Describes context information of a component, such as its container and PropertyDescriptor.</param>
        /// <param name="cultureInfo">Identifies culture-specific information, including the writing system and the calendar that is used.</param>
        /// <param name="value">Identifies the Object that is being converted.</param>
        /// <param name="destinationType">The data type that this instance of Double is being converted to.</param>
        /// <returns>A new Object that is the value of the conversion.</returns>
        public override object ConvertTo(ITypeDescriptorContext typeDescriptorContext, 
                                         CultureInfo cultureInfo, 
                                         object value, 
                                         Type destinationType)
        {
            if (destinationType == null)
                throw new ArgumentNullException("destinationType");

            // Can only convert a double type a string
            if ((value != null) && (value is double))
            {
                double num = (double)value;
                if (destinationType == typeof(string))
                {
                    if (double.IsNaN(num))
                        return "Auto";
                    else
                        return Convert.ToString(num, cultureInfo);
                }
            }

            return base.ConvertTo(typeDescriptorContext, cultureInfo, value, destinationType);
        }
        #endregion
    }
}

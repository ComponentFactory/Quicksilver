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
using System.Reflection;

namespace ComponentFactory.Quicksilver.Binding
{
    /// <summary>
    /// EvalNode that represents a field or property call on an object instance or type.
    /// </summary>
    public class EvalNodeFieldOrProperty : EvalNode
    {
        #region Instance Fields
        private string _identifier;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the EvalNodeFieldOrProperty class.
        /// </summary>
        /// <param name="identifier">Specifies identifier of the member to call.</param>
        public EvalNodeFieldOrProperty(string identifier)
        {
            _identifier = identifier;
        }

        /// <summary>
        /// Human readable version of the expression.
        /// </summary>
        /// <returns>String representation of the stored expression.</returns>
        public override string ToString()
        {
            return this[0].ToString() + "." + _identifier;
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets the field/property to call.
        /// </summary>
        public string Identifier
        {
            get { return _identifier; }
            set { _identifier = value; }
        }

        /// <summary>
        /// Evalaute this node and return result.
        /// </summary>
        /// <param name="thisObject">Reference to object that is exposed as 'this'.</param>
        /// <returns>Result value and type of that result.</returns>
        public override EvalResult Evaluate(object thisObject)
        {
            EvalResult target = this[0].Evaluate(thisObject);

            if (target.Value == null)
                throw new ApplicationException("Cannot access '" + Identifier + "' on a value of 'null'.");
            else if (target.Value is Type)
                return EvaluateTypeMember(target);
            else
                return EvaluateObjectMember(target);
        }
        #endregion

        #region Private
        private EvalResult EvaluateTypeMember(EvalResult target)
        {
            // Extract the type we are working against
            Type t = target.Value as Type;

            // Try and find the static field for our identifier
            FieldInfo fi = t.GetField(Identifier, BindingFlags.Static | BindingFlags.Public | BindingFlags.GetField);

            if (fi != null)
            {
                // Grab the value of the static field
                EvalResult ret = new EvalResult();
                ret.Value = fi.GetValue(null);
                return DiscoverTypeCode(ret);
            }
            else
            {
                // Try and find the static property for our identifier
                PropertyInfo pi = t.GetProperty(Identifier, BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty);

                if (pi == null)
                    throw new ApplicationException("Cannot find static public field/property called '" + Identifier + "' for type '" + t.ToString() + "'.");
                else
                {
                    // Grab the value of the static property
                    EvalResult ret = new EvalResult();
                    ret.Value = pi.GetValue(null, null);
                    return DiscoverTypeCode(ret);
                }
            }
        }

        private EvalResult EvaluateObjectMember(EvalResult target)
        {
            // Extract the type we are working against
            Type t = target.Value.GetType();

            // Try and find the instance field for our identifier
            FieldInfo fi = t.GetField(Identifier, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);

            if (fi != null)
            {
                // Grab the value of the static field
                EvalResult ret = new EvalResult();
                ret.Value = fi.GetValue(target.Value);
                return DiscoverTypeCode(ret);
            }
            else
            {
                // Try and find the instance property for our identifier
                PropertyInfo pi = t.GetProperty(Identifier, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

                if (pi == null)
                    throw new ApplicationException("Cannot find instance public field/property called '" + Identifier + "' for type '" + t.ToString() + "'.");
                else
                {
                    // Grab the value of the instance property
                    EvalResult ret = new EvalResult();
                    ret.Value = pi.GetValue(target.Value, null);
                    return DiscoverTypeCode(ret);
                }
            }
        }
        #endregion
    }
}

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
    /// EvalNode that represents a method call on an object instance or type.
    /// </summary>
    public class EvalNodeMethod : EvalNode
    {
        #region Instance Fields
        private string _identifier;
        private Language _language;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the EvalNodeMethod class.
        /// </summary>
        /// <param name="identifier">Specifies identifier of the member to call.</param>
        /// <param name="language">Language used for evaluation.</param>
        public EvalNodeMethod(string identifier,
                              Language language)
        {
            _identifier = identifier;
            _language = language;
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
        /// Gets and sets the method to call.
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
            EvalNodeArgList args = (EvalNodeArgList)this[1];

            if (target.Value == null)
                throw new ApplicationException("Cannot access '" + Identifier + "' on a value of 'null'.");

            // Evalute all the arguments
            EvalResult[] results = new EvalResult[args.Count];
            for (int i = 0; i < results.Length; i++)
                results[i] = args[i].Evaluate(thisObject);

            if (target.Value is Type)
                return EvaluateTypeMethod(target, results);
            else
                return EvaluateObjectMethod(target, results);
        }
        #endregion

        #region Private
        private EvalResult EvaluateTypeMethod(EvalResult target, EvalResult[] args)
        {
            // Get all the static methods that might of interest to us
            MethodInfo[] mis = ((Type)target.Value).GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.InvokeMethod);
            return EvaluateMethod(mis, null, args);
        }

        private EvalResult EvaluateObjectMethod(EvalResult target, EvalResult[] args)
        {
            // Get all the instance methods that might of interest to us
            MethodInfo[] mis = target.Value.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);
            return EvaluateMethod(mis, target.Value, args);
        }

        private EvalResult EvaluateMethod(MethodInfo[] mis, object target, EvalResult[] args)
        {
            if (mis == null)
            {
                if (target == null)
                    throw new ApplicationException("Cannot find static public method called '" + Identifier + "'.");
                else
                    throw new ApplicationException("Cannot find instance public method called '" + Identifier + "'.");
            }

            MethodInfo callMi = null;
            object[] callParams = null;

            // Find methods that we could call
            foreach (MethodInfo mi in mis)
            {
                // Matching name?
                if (mi.Name == Identifier)
                {
                    ParameterInfo[] pis = mi.GetParameters();

                    // Is the last parameter an array of params?
                    bool paramArray = (pis.Length > 0 ? pis[pis.Length - 1].IsDefined(typeof(ParamArrayAttribute), false) : false);

                    // It must have the same number of parameters as we have arguments
                    if (!paramArray && (pis.Length == args.Length))
                    {
                        // Match parameter types?
                        int i = 0;
                        int exact = 0;
                        object[] pisParams = new object[args.Length];
                        for (i = 0; i < pis.Length; i++)
                        {
                            TypeCode methodTc = Type.GetTypeCode(pis[i].ParameterType);

                            // Note how many parameters are an exact match
                            if (methodTc == args[i].Type)
                            {
                                pisParams[i] = args[i].Value;
                                exact++;
                            }
                            else
                            {
                                // If cannot be implicitly converted, then fail to match at all
                                if (ImplicitConverter.CanConvertTo(methodTc, args[i].Value, _language))
                                    pisParams[i] = ImplicitConverter.ConvertTo(methodTc, args[i].Value, _language);
                                else
                                    break;
                            }
                        }

                        // If all parameters exactly match
                        if (exact == pis.Length)
                        {
                            // Use this method
                            callMi = mi;
                            callParams = pisParams;
                            break;
                        }

                        // Remember the first compatible match we find
                        if ((i == pis.Length) && (callMi == null))
                        {
                            callMi = mi;
                            callParams = pisParams;
                        }
                    }
                    else if (paramArray)
                    {
                        // We can only handle packaging up a param array as an object array
                        int lastIndex = pis.Length - 1;
                        if (pis[lastIndex].ParameterType == typeof(object[]))
                        {
                            // Package up the extra parameters into an object array
                            object[] pisParams = new object[pis.Length];
                            object[] lastParams = new object[args.Length - lastIndex];
                            pisParams[lastIndex] = lastParams;
                            for (int j = 0; j < lastParams.Length; j++)
                                lastParams[j] = args[j + lastIndex].Value;

                            // Match parameter types for all but the last 'object[]' entry?
                            int i = 0;
                            int exact = 0;
                            for (i = 0; i < lastIndex; i++)
                            {
                                TypeCode methodTc = Type.GetTypeCode(pis[i].ParameterType);

                                // Note how many parameters are an exact match
                                if (methodTc == args[i].Type)
                                {
                                    pisParams[i] = args[i].Value;
                                    exact++;
                                }
                                else
                                {
                                    // If cannot be implicitly converted, then fail to match at all
                                    if (ImplicitConverter.CanConvertTo(methodTc, args[i].Value, _language))
                                        pisParams[i] = ImplicitConverter.ConvertTo(methodTc, args[i].Value, _language);
                                    else
                                        break;
                                }
                            }

                            // If all parameters exactly match
                            if (exact == lastIndex)
                            {
                                // Use this method
                                callMi = mi;
                                callParams = pisParams;
                                break;
                            }

                            // Remember the first compatible match we find
                            if ((i == lastIndex) && (callMi == null))
                            {
                                callMi = mi;
                                callParams = pisParams;
                            }
                        }
                    }
                }
            }

            if (callMi == null)
            {
                if (target == null)
                    throw new ApplicationException("Cannot find static public method called '" + Identifier + "' with matching parameters to those provided.");
                else
                    throw new ApplicationException("Cannot find instance public method called '" + Identifier + "' with matching parameters to those provided.");
            }

            // Call the static method and return result
            EvalResult ret = new EvalResult();
            ret.Value = callMi.Invoke(target, callParams);
            return DiscoverTypeCode(ret);
        }
        #endregion
    }
}

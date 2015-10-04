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
    /// EvalNode that represents an array index.
    /// </summary>
    public class EvalNodeArrayIndex : EvalNode
    {
        #region Instance Fielsd
        private Language _language;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the EvalNodeArrayIndex class.
        /// </summary>
        /// <param name="child">Specifies the child node.</param>
        /// <param name="language">Language used for evaluation.</param>
        public EvalNodeArrayIndex(EvalNode child,
                                  Language language)
        {
            _language = language;
            Append(child);
        }

        /// <summary>
        /// Human readable version of the expression.
        /// </summary>
        /// <returns>String representation of the stored expression.</returns>
        public override string ToString()
        {
            if (_language == Language.CSharp)
                return this[0].ToString() + "[" + this[1].ToString() + "]";
            else
                return this[0].ToString() + "(" + this[1].ToString() + ")";
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
            EvalResult target = this[0].Evaluate(thisObject);
            EvalNodeArgList args = (EvalNodeArgList)this[1];

            // We can only handle a target that is an object
            if (target.Type != TypeCode.Object)
                throw new ApplicationException("Array index cannot index a value of type '" + target.Type.ToString() + "'.");
            else if (target.Value == null)
                throw new ApplicationException("Array index cannot index a value of 'null'.");
            else if (target.Value is Type)
                throw new ApplicationException("Array index cannot index a 'type' object.");

            // Evalute all the arguments
            EvalResult[] results = new EvalResult[args.Count];
            for (int i = 0; i < results.Length; i++)
                results[i] = args[i].Evaluate(thisObject);

            // Is this an actual array of a type?
            if (target.Value.GetType().IsArray)
                return EvaluateObjectArray(target, results, thisObject);
            else
                return EvaluateObjectInstance(target, results, thisObject);
        }
        #endregion

        #region Private
        private EvalResult EvaluateObjectArray(EvalResult target, 
                                               EvalResult[] args, 
                                               object thisObject)
        {
            // We know the value is an actual array instance
            Array array = (Array)target.Value;

            // Must have same number of arguments as the array rank
            if (array.Rank != args.Length)
                throw new ApplicationException("Array expects '" + array.Rank.ToString() + "' indices but '" + args.Length.ToString() + "' have been specified.");

            // Construct the set of indices for accessing array
            long[] indices = new long[args.Length];
            for (int i = 0; i < args.Length; i++)
                indices[i] = ImplicitConverter.ConvertToInt64(args[i].Value, _language);

            EvalResult ret = new EvalResult();
            ret.Value = array.GetValue(indices);
            return DiscoverTypeCode(ret);
        }

        private EvalResult EvaluateObjectInstance(EvalResult target,
                                                  EvalResult[] args, 
                                                  object thisObject)
        {
            // Get the type definition of the target instance
            Type t = target.Value.GetType();

            // Get the list of all properties that might be of interest to us
            PropertyInfo[] pis = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

            PropertyInfo callPi = null;
            object[] callParams = null;

            // Find properties that we could call
            foreach (PropertyInfo pi in pis)
            {
                // Object indexers are compiled from 'this[...]' in C# to the fixed name 'Item'
                if (pi.Name == "Item")
                {
                    // Grab the list of parameters for this property accessor
                    ParameterInfo[] infos = pi.GetIndexParameters();

                    // Is the last parameter an array of params?
                    bool paramArray = (infos.Length > 0 ? infos[infos.Length - 1].IsDefined(typeof(ParamArrayAttribute), false) : false);

                    // It must have the same number of parameters as we have arguments
                    if (!paramArray && (infos.Length == args.Length))
                    {
                        // Match parameter types?
                        int i = 0;
                        int exact = 0;
                        object[] infoParams = new object[args.Length];
                        for (i = 0; i < infos.Length; i++)
                        {
                            TypeCode paramTc = Type.GetTypeCode(infos[i].ParameterType);

                            // Note how many parameters are an exact match
                            if (paramTc == args[i].Type)
                            {
                                infoParams[i] = args[i].Value;
                                exact++;
                            }
                            else
                            {
                                // If cannot be implicitly converted, then fail to match at all
                                if (ImplicitConverter.CanConvertTo(paramTc, args[i].Value, _language))
                                    infoParams[i] = ImplicitConverter.ConvertTo(paramTc, args[i].Value, _language);
                                else
                                    break;
                            }
                        }

                        // If all parameters exactly match
                        if (exact == infos.Length)
                        {
                            // Use this method
                            callPi = pi;
                            callParams = infoParams;
                            break;
                        }

                        // Remember the first compatible match we find
                        if ((i == infos.Length) && (callPi == null))
                        {
                            callPi = pi;
                            callParams = infoParams;
                        }
                    }
                    else if (paramArray)
                    {
                        // We can only handle packaging up a param array as an object array
                        int lastIndex = infos.Length - 1;
                        if (infos[lastIndex].ParameterType == typeof(object[]))
                        {
                            // Package up the extra parameters into an object array
                            object[] infoParams = new object[infos.Length];
                            object[] lastParams = new object[args.Length - lastIndex];
                            infoParams[lastIndex] = lastParams;
                            for (int j = 0; j < lastParams.Length; j++)
                                lastParams[j] = args[j + lastIndex].Value;

                            // Match parameter types for all but the last 'object[]' entry?
                            int i = 0;
                            int exact = 0;
                            for (i = 0; i < lastIndex; i++)
                            {
                                TypeCode paramTc = Type.GetTypeCode(infos[i].ParameterType);

                                // Note how many parameters are an exact match
                                if (paramTc == args[i].Type)
                                {
                                    infoParams[i] = args[i].Value;
                                    exact++;
                                }
                                else
                                {
                                    // If cannot be implicitly converted, then fail to match at all
                                    if (ImplicitConverter.CanConvertTo(paramTc, args[i].Value, _language))
                                        infoParams[i] = ImplicitConverter.ConvertTo(paramTc, args[i].Value, _language);
                                    else
                                        break;
                                }
                            }

                            // If all parameters exactly match
                            if (exact == lastIndex)
                            {
                                // Use this method
                                callPi = pi;
                                callParams = infoParams;
                                break;
                            }

                            // Remember the first compatible match we find
                            if ((i == lastIndex) && (callPi == null))
                            {
                                callPi = pi;
                                callParams = infoParams;
                            }
                        }
                    }
                }
            }

            if (callPi == null)
                throw new ApplicationException("Cannot find array acessor for type '" + target.Type.ToString() + "' with matching number and type of arguments.");

            // Call the static method and return result
            EvalResult ret = new EvalResult();
            ret.Value = callPi.GetValue(target.Value, callParams);
            return DiscoverTypeCode(ret);
        }
        #endregion
    }
}

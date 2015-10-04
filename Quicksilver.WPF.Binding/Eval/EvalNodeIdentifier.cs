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
    /// EvalNode that represents an identifier.
    /// </summary>
    public class EvalNodeIdentifier : EvalNode
    {
        #region Static Fields
        private static Dictionary<string, Type> _stringToTypesCS;
        private static Dictionary<string, Type> _stringToTypesVB;
        #endregion

        #region Instance Fields
        private Eval _eval;
        private Language _language;
        private string _identifier;
        #endregion

        #region Identity
        static EvalNodeIdentifier()
        {
            // Language specific keywords that map to types, without these you would 
            // need to specify 'Int32.MaxValue' instead of just 'int.MaxValue'
            _stringToTypesCS = new Dictionary<string, Type>();
            _stringToTypesVB = new Dictionary<string, Type>();

            _stringToTypesCS.Add("bool", typeof(Boolean));
            _stringToTypesCS.Add("byte", typeof(Byte));
            _stringToTypesCS.Add("sbyte", typeof(SByte));
            _stringToTypesCS.Add("char", typeof(Char));
            _stringToTypesCS.Add("int", typeof(Int32));
            _stringToTypesCS.Add("uint", typeof(UInt32));
            _stringToTypesCS.Add("long", typeof(Int64));
            _stringToTypesCS.Add("ulong", typeof(UInt64));
            _stringToTypesCS.Add("float", typeof(Single));
            _stringToTypesCS.Add("double", typeof(Double));
            _stringToTypesCS.Add("decimal", typeof(Decimal));
            _stringToTypesCS.Add("string", typeof(String));
            _stringToTypesCS.Add("object", typeof(Object));

            _stringToTypesVB.Add("Date", typeof(DateTime));
            _stringToTypesVB.Add("Short", typeof(Int16));
            _stringToTypesVB.Add("Integer", typeof(Int32));
            _stringToTypesVB.Add("Long", typeof(Int64));
            _stringToTypesVB.Add("UShort", typeof(UInt16));
            _stringToTypesVB.Add("UInteger", typeof(UInt32));
            _stringToTypesVB.Add("ULong", typeof(UInt64));
        }

        /// <summary>
        /// Initialize a new instance of the EvalNodeIdentifier class.
        /// </summary>
        /// <param name="eval">Owning eval instance..</param>
        /// <param name="identifier">Specifies identifier of the member to call.</param>
        /// <param name="language">Language used for evaluation.</param>
        public EvalNodeIdentifier(Eval eval, string identifier, Language language)
        {
            _eval = eval;
            _identifier = identifier;
            _language = language;
        }

        /// <summary>
        /// Human readable version of the expression.
        /// </summary>
        /// <returns>String representation of the stored expression.</returns>
        public override string ToString()
        {
            return _identifier;
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets the member to call.
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
            switch (Identifier)
            {
                case "null":
                    if (_language == Language.CSharp)
                        return new EvalResult(TypeCode.Object, null);
                    else
                        throw new ApplicationException("Identifier 'null' not valid in language choice, did you mean 'Nothing'?");
                case "Nothing":
                    if (_language == Language.VBNet)
                        return new EvalResult(TypeCode.Object, null);
                    else
                        throw new ApplicationException("Identifier 'Nothing' not valid in language choice, did you mean 'null'?");
                case "this":
                    if (_language == Language.CSharp)
                        return new EvalResult(TypeCode.Object, thisObject);
                    else
                        throw new ApplicationException("Identifier 'this' not valid in language choice, did you mean 'Me'?");
                case "Me":
                    if (_language == Language.VBNet)
                        return new EvalResult(TypeCode.Object, thisObject);
                    else
                        throw new ApplicationException("Identifier 'Me' not valid in language choice, did you mean 'this'?");
                default:
                    // Does the string represent a name of a type?
                    Type retType = null;
                    if (_language == Language.CSharp)
                    {
                        if (_stringToTypesCS.TryGetValue(Identifier, out retType))
                            return new EvalResult(TypeCode.Object, retType);
                    }
                    else
                    {
                        if (_stringToTypesVB.TryGetValue(Identifier, out retType))
                            return new EvalResult(TypeCode.Object, retType);
                    }

                    // See if the string is a type found in the default namespace
                    retType = Type.GetType("System." + Identifier);
                    if (retType != null)
                        return new EvalResult(TypeCode.Object, retType);

                    // Last resort is to ask for help from the eval instance
                    if (_eval != null)
                    {
                        // Ask the eval to resolve the identifier
                        object ret = _eval.PerformResolveIdentifier(Identifier);

                        // If we have a value then use it
                        if (ret != null)
                            return DiscoverTypeCode(new EvalResult(TypeCode.Object, ret));
                    }

                    throw new ApplicationException("Unrecognized identifier '" + Identifier + "' is not a known type in System namespace for a predefined name.");
            }
        }
        #endregion
    }
}

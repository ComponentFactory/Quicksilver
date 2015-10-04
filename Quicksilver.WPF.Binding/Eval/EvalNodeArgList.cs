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
    /// Base class used for implementing specific types of evaluation node.
    /// </summary>
    public class EvalNodeArgList : EvalNode
    {
        #region Instance Fields
        private Language _language;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the EvalNodeArgList class.
        /// </summary>
        /// <param name="language">Language used for evaluation.</param>
        public EvalNodeArgList(Language language)
        {
            _language = language;
        }

        /// <summary>
        /// Initialize a new instance of the EvalNodeArgList class.
        /// </summary>
        /// <param name="argument">Initial argument to store.</param>
        /// <param name="language">Language used for evaluation.</param>
        public EvalNodeArgList(EvalNode argument,
                               Language language)
        {
            _language = language;
            Append(argument);
        }

        /// <summary>
        /// Human readable version of the expression.
        /// </summary>
        /// <returns>String representation of the stored expression.</returns>
        public override string ToString()
        {
            if (Count == 0)
                return string.Empty;
            else
            {
                StringBuilder ret = new StringBuilder();

                for (int i = 0; i < Count; i++)
                {
                    if (i > 0)
                        ret.Append(_language == Language.CSharp ? "," : ", ");

                    ret.Append(this[i].ToString());
                }

                return ret.ToString();
            }
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
            throw new ApplicationException("EvalNodeArgList should not be called to evaluate itself.");
        }
        #endregion
    }
}

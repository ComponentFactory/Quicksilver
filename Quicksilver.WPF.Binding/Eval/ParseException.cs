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
    /// Exception indicating error parsing an input string.
    /// </summary>
    public class ParseException : ApplicationException
    {
        #region Instance Fields
        private int _index;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the ParseException instance.
        /// </summary>
        /// <param name="index">Index of character position associated with error.</param>
        /// <param name="message">Error message.</param>
        public ParseException(int index, string message)
            : base(message)
        {
            _index = index;
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets access to the character index in the input string associated with the error.
        /// </summary>
        public int Index
        {
            get { return _index; }
        }
        #endregion
    }
}

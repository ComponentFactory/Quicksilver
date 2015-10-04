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
    /// Event arguments associated with an event to resolve an identifier to an object.
    /// </summary>
    public class ResolveIdentifierEventArgs : EventArgs
    {
        #region Identity
        /// <summary>
        /// Initialize a new instance of the ResolveIdentifierEventArgs class.
        /// </summary>
        /// <param name="identifier">Identifier to resolve.</param>
        public ResolveIdentifierEventArgs(string identifier)
        {
            Identifier = identifier;
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets the type of the result value.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// Gets and sets the result value.
        /// </summary>
        public object Value { get; set; }
        #endregion
    }
}

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
    /// Specifies the type of a lexical token.
    /// </summary>
    internal enum TokenType
    {
        /// <summary>Specifies a '+' symbol.</summary>
        Plus,

        /// <summary>Specifies a '-' symbol.</summary>
        Minus,

        /// <summary>Specifies a '*' symbol.</summary>
        Multiply,

        /// <summary>Specifies a '/' symbol.</summary>
        Divide,

        /// <summary>Specifies a '!' symbol.</summary>
        Not,

        /// <summary>Specifies a '~' symbol.</summary>
        Complement,

        /// <summary>Specifies a '%' symbol.</summary>
        Remainder,

        /// <summary>Specifies a '\' symbol.</summary>
        BackSlash,

        /// <summary>Specifies a '?' symbol.</summary>
        QuestionMark,

        /// <summary>Specifies a '??' symbol.</summary>
        DoubleQuestionMark,

        /// <summary>Specifies a ':' symbol.</summary>
        Colon,

        /// <summary>Specifies a '|' symbol.</summary>
        Or,

        /// <summary>Specifies a '||' symbol.</summary>
        DoubleOr,

        /// <summary>Specifies a '&amp;' symbol.</summary>
        And,

        /// <summary>Specifies a '&amp;&amp;' symbol.</summary>
        DoubleAnd,

        /// <summary>Specifies a '^' symbol.</summary>
        Hat,

        /// <summary>Specifies a '&lt;' symbol.</summary>
        LessThan,

        /// <summary>Specifies a '&lt;&lt;' symbol.</summary>
        ShiftLeft,

        /// <summary>Specifies a '>' symbol.</summary>
        ShiftRight,

        /// <summary>Specifies a '&lt;=' symbol.</summary>
        LessThanEqual,

        /// <summary>Specifies a '>' symbol.</summary>
        GreaterThan,

        /// <summary>Specifies a '>=' symbol.</summary>
        GreaterThanEqual,

        /// <summary>Specifies a '=' symbol.</summary>
        Equal,

        /// <summary>Specifies a '==' symbol.</summary>
        DoubleEqual,

        /// <summary>Specifies a '!=' symbol.</summary>
        NotEqual,

        /// <summary>Specifies a '&lt;&gt;' symbol.</summary>
        LessGreater,

        /// <summary>Specifies a '.' symbol.</summary>
        Dot,

        /// <summary>Specifies a ',' symbol.</summary>
        Comma,

        /// <summary>Specifies a '(' symbol.</summary>
        OpenRoundBracket,

        /// <summary>Specifies a ')' symbol.</summary>
        CloseRoundBracket,

        /// <summary>Specifies a '[' symbol.</summary>
        OpenSquareBracket,

        /// <summary>Specifies a ']' symbol.</summary>
        CloseSquareBracket,

        /// <summary>Specifies a 'Or' symbol.</summary>
        KeywordOr,

        /// <summary>Specifies a 'OrElse' symbol.</summary>
        KeywordOrElse,

        /// <summary>Specifies a 'And' symbol.</summary>
        KeywordAnd,

        /// <summary>Specifies a 'AndAlso' symbol.</summary>
        KeywordAndAlso,

        /// <summary>Specifies a 'Xor' symbol.</summary>
        KeywordXor,

        /// <summary>Specifies a 'Not' symbol.</summary>
        KeywordNot,

        /// <summary>Specifies a 'Mod' symbol.</summary>
        KeywordMod,

        /// <summary>Specifies a integer literal value.</summary>
        IntegerLiteral,

        /// <summary>Specifies a real literal value.</summary>
        RealLiteral,

        /// <summary>Specifies a character literal value.</summary>
        CharLiteral,
        
        /// <summary>Specifies a string literal value.</summary>
        StringLiteral,

        /// <summary>Specifies a boolean literal value.</summary>
        BooleanLiteral,

        /// <summary>Specifies an identifier value.</summary>
        Identifier,

        /// <summary>Specifies an array index value.</summary>
        ArrayIndex,

        /// <summary>Specifies end of input.</summary>
        EndOfInput,
    }

    /// <summary>
    /// Token represents a single lexical item.
    /// </summary>
    internal class Token
    {
        #region Instance Fields
        private TokenType _type;
        private int _index;
        private TypeCode _typeCode;
        private object _value;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the Token class.
        /// </summary>
        /// <param name="type">Type of value contained by the token.</param>
        /// <param name="index">Character position for start of token.</param>
        public Token(TokenType type,
                     int index)
        {
            _type = type;
            _index = index;
            _typeCode = TypeCode.Empty;
        }

        /// <summary>
        /// Initialize a new instance of the Token class.
        /// </summary>
        /// <param name="type">Type of value contained by the token.</param>
        /// <param name="index">Character position for start of token.</param>
        /// <param name="typeCode">Type of the value provided.</param>
        /// <param name="value">Token value.</param>
        public Token(TokenType type,
                     int index,
                     TypeCode typeCode,
                     object value)
        {
            _type = type;
            _index = index;
            _typeCode = typeCode;
            _value = value;
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets the type of the token.
        /// </summary>
        public TokenType Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Gets the character index for start of the token in input.
        /// </summary>
        public int Index
        {
            get { return _index; }
        }

        /// <summary>
        /// Gets the type associated with the token value.
        /// </summary>
        public TypeCode TypeCode
        {
            get { return _typeCode; }
        }

        /// <summary>
        /// Gets the token value.
        /// </summary>
        public object Value
        {
            get { return _value; }
        }
        #endregion
    }
}

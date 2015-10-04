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
    /// Accept and evaluate simple arithmetic expressions.  
    /// </summary>
    /// <remarks>
    ///
    /// Top down predictive parser using an LL(1) BNF
    ///
    ///  expression ::= term expression2
    ///
    /// expression2 ::= '+' term expression2 |
    ///                 '-' term expression2 |
    ///                 {empty}
    ///
    ///        term ::= factor term2
    ///
    ///       term2 ::= '*' factor term2 |
    ///                 '/' factor term2 |
    ///                 {empty}
    ///
    ///      factor ::= '+' factor |
    ///                 '-' factor |
    ///                 primary
    ///
    ///     primary ::= integer-literal |
    ///                 real-literal
    ///                 '(' expression ')'
    ///
    /// </remarks>
    public class Arithmetic
    {
        #region Instance Fields
        private EvalNode _root;
        private Language _language;
        private Tokeniser _tokeniser;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the Arithmetic class.
        /// </summary>
        public Arithmetic()
        {
            _language = Language.CSharp;
        }

        /// <summary>
        /// Initialize a new instance of the Arithmetic class.
        /// </summary>
        /// <param name="language">Language used for parsing.</param>
        public Arithmetic(Language language)
        {
            _language = language;
        }

        /// <summary>
        /// Initialize a new instance of the Arithmetic class.
        /// </summary>
        /// <param name="input">Input string to parse.</param>
        public Arithmetic(string input)
            : this(input, Language.CSharp)
        {
        }

        /// <summary>
        /// Initialize a new instance of the Arithmetic class.
        /// </summary>
        /// <param name="input">Input string to parse.</param>
        /// <param name="language">Language used for parsing.</param>
        public Arithmetic(string input, Language language)
        {
            _language = language;
            Parse(input);
        }

        /// <summary>
        /// Human readable version of the parsed input.
        /// </summary>
        /// <returns>String representation of the stored expression.</returns>
        public override string ToString()
        {
            if (Root == null)
                return string.Empty;
            else
                return Root.ToString();
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets and sets the language used for parsing.
        /// </summary>
        public Language Language
        {
            get { return _language; }
            set { _language = value; }
        }

        /// <summary>
        /// Gets access to the root of the abstract syntax tree generated from parse call.
        /// </summary>
        public EvalNode Root
        {
            get { return _root; }
        }

        /// <summary>
        /// Parse the provided input string.
        /// </summary>
        /// <param name="input">Input text.</param>
        public virtual void Parse(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                // Generate entire set of tokens in one go
                _tokeniser = new Tokeniser(input, Language);

                // Parse into abstract syntax tree
                EvalNode eval = ParseExpression();

                // Should be no more input available
                if (Tokeniser.Peek.Type != TokenType.EndOfInput)
                    throw new ParseException(Tokeniser.Peek.Index, "Found '" + Tokeniser.Peek.Type.ToString() + "' instead of end of input");
                else
                    _root = eval;
            }
            else
                _root = null;
        }

        /// <summary>
        /// Evaluate and return result.
        /// </summary>
        /// <returns>Result of evaluation.</returns>
        public virtual EvalResult Evaluate()
        {
            return Evaluate(null);
        }

        /// <summary>
        /// Evaluate and return result.
        /// </summary>
        /// <param name="thisObject">Reference to object that is exposed as 'this'.</param>
        /// <returns>Result of evaluation.</returns>
        public virtual EvalResult Evaluate(object thisObject)
        {
            if (Root == null)
                return null;
            else
                return Root.Evaluate(thisObject);
        }
        #endregion

        #region Protected
        /// <summary>
        /// Gets access to the tokeniser instance.
        /// </summary>
        internal Tokeniser Tokeniser
        {
            get { return _tokeniser; }
        }

        /// <summary>
        /// Parse the 'expression' rule.
        /// </summary>
        /// <remarks>
        ///
        /// expression ::= term expression2
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseExpression()
        {
            EvalNode term = ParseTerm();
            EvalNode expr2 = ParseExpression2();

            if (expr2 != null)
            {
                expr2.Prepend(term);
                return expr2.Root;
            }
            else
                return term.Root;
        }

        /// <summary>
        /// Parse the 'expression2' rule.
        /// </summary>
        /// <remarks>
        /// 
        /// expression2 ::= '+' term expression2 |
        ///                 '-' term expression2 |
        ///                 {empty}
        ///                 
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseExpression2()
        {
            if (Tokeniser.Next(TokenType.Plus))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.Add, ParseTerm(), Language);
                EvalNode expr2 = ParseExpression2();
                if (expr2 != null)
                    expr2.Prepend(op);
                return op;
            }

            if (Tokeniser.Next(TokenType.Minus))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.Subtract, ParseTerm(), Language);
                EvalNode expr2 = ParseExpression2();
                if (expr2 != null)
                    expr2.Prepend(op);
                return op;
            }
            
            return null;
        }

        /// <summary>
        /// Parse the 'term' rule.
        /// </summary>
        /// <remarks>
        ///
        /// term ::= factor term2
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseTerm()
        {
            EvalNode factor = ParseFactor();
            EvalNode term2 = ParseTerm2();

            if (term2 != null)
            {
                term2.Prepend(factor);
                return term2;
            }
            else
                return factor;
        }

        /// <summary>
        /// Process the term2 rule.
        /// </summary>
        /// <remarks>
        ///
        /// term2 ::= '*' factor term2 |
        ///           '/' factor term2 |
        ///           {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseTerm2()
        {
            if (Tokeniser.Next(TokenType.Multiply))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.Multiply, ParseFactor(), Language);
                EvalNode term2 = ParseTerm2();
                if (term2 != null)
                    term2.Prepend(op);
                return op;
            }

            if (Tokeniser.Next(TokenType.Divide))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.Divide, ParseFactor(), Language);
                EvalNode term2 = ParseTerm2();
                if (term2 != null)
                    term2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'factor' rule.
        /// </summary>
        /// <remarks>
        ///
        /// factor ::= '+' factor |
        ///            '-' factor |
        ///            primary
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseFactor()
        {
            if (Tokeniser.Next(TokenType.Plus))
                return new EvalNodeUnaryOp(EvalNodeUnaryOp.UnaryOp.Plus, ParseFactor(), Language);

            if (Tokeniser.Next(TokenType.Minus))
                return new EvalNodeUnaryOp(EvalNodeUnaryOp.UnaryOp.Minus, ParseFactor(), Language);
            
            return ParsePrimary();
        }

        /// <summary>
        /// Parse the 'primary' rule.
        /// </summary>
        /// <remarks>
        ///
        /// primary ::= integer-literal |
        ///             real-literal
        ///             '(' expression ')'
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParsePrimary()
        {
            if ((Tokeniser.Peek.Type == TokenType.IntegerLiteral) ||
                (Tokeniser.Peek.Type == TokenType.RealLiteral))
                return new EvalNodeLiteral(Tokeniser.Peek.TypeCode, Tokeniser.Next().Value, Language);

            if (Tokeniser.Next(TokenType.OpenRoundBracket))
            {
                EvalNode expr = ParseExpression();

                if (!Tokeniser.Next(TokenType.CloseRoundBracket))
                {
                    if (Tokeniser.Peek.Type == TokenType.EndOfInput)
                        throw new ParseException(Tokeniser.Peek.Index, "Found '" + Tokeniser.Peek.Type.ToString() + "' instead of expected ')'");
                    else
                        throw new ParseException(Tokeniser.Peek.Index, "Found '" + Tokeniser.Peek.Type.ToString() + "' at index '" + Tokeniser.Peek.Index + "' instead of expected ')'");
                }

                return expr;
            }

            if (Tokeniser.Peek.Type == TokenType.EndOfInput)
                throw new ParseException(Tokeniser.Peek.Index, "Found '" + Tokeniser.Peek.Type.ToString() + "' instead of some more input");
            else
                throw new ParseException(Tokeniser.Peek.Index, "Found '" + Tokeniser.Peek.Type.ToString() + "' at index '" + Tokeniser.Peek.Index + "' instead of either a primary or '('");
        }
        #endregion
    }
}

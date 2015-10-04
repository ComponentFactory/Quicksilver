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
    /// Accept and evaluate simple expressions using a specific .NET language syntax/semantics.
    /// </summary>
    /// <remarks>
    ///
    /// C# is implemented using the following LL(1) BNF.
    /// ------------------------------------------------
    ///
    ///   expression ::= condOr cond2
    ///
    ///        cond2 ::= '?' expression ':' expression |
    ///                  '??' expression |
    ///                  {empty}
    ///
    ///       condOr ::= condAnd condOr2
    ///
    ///      condOr2 ::= '||' condAnd condOr2 |
    ///                  {empty}
    ///
    ///      condAnd ::= logicalOr condAnd2
    /// 
    ///     condAnd2 ::= '&amp;&amp;' logicalOr condAnd2 |
    ///                  {empty}
    ///
    ///    logicalOr ::= logicalXor logicalOr2
    ///
    ///   logicalOr2 ::= '|' logicalXor logicalOr2 |
    ///                  {empty}
    ///
    ///   logicalXor ::= logicalAnd logicalXor2
    ///
    ///  logicalXor2 ::= '^' logicalAnd logicalXor2 |
    ///                  {empty}
    ///                 
    ///   logicalAnd ::= equality logicalAnd2
    ///
    ///  logicalAnd2 ::= '&amp;' equality logicalAnd2 |
    ///                  {empty}
    ///                 
    ///     equality ::= relational equality2
    ///
    ///    equality2 ::= '==' relational equality2 |
    ///                  '!=' relational equality2 |
    ///                  {empty}
    ///
    ///   relational ::= shift relational2
    ///
    ///  relational2 ::= '&lt;' shift relational2  |
    ///                  '>' shift relational2     |
    ///                  '&lt;=' shift relational2 |
    ///                  '>=' shift relational2    |
    ///                  {empty}
    ///
    ///        shift ::= addsub shift2
    ///
    ///       shift2 ::= '&lt;&lt;' addsub shift2 |
    ///                  '>>' addsub shift2 |
    ///                  {empty}
    ///
    ///       addsub ::= term expression2
    ///
    ///  expression2 ::= '+' term expression2 |
    ///                  '-' term expression2 |
    ///                  {empty}
    ///
    ///         term ::= factor term2
    ///
    ///        term2 ::= '*' factor term2 |
    ///                  '/' factor term2 |
    ///                  '%' factor term2 |
    ///                  {empty}
    ///
    ///       factor ::= '+' factor |
    ///                  '-' factor |
    ///                  '!' factor |
    ///                  '~' factor |
    ///                  access
    ///                 
    ///       access ::= primary access2
    ///        
    ///      access2 ::= '.' identifier methodcall access2 |
    ///                  '[' arglist1 ']' access2        |
    ///                  {empty}
    ///
    ///   methodcall ::= '(' arglist ')' |
    ///                   {empty}
    ///
    ///      primary ::= integer-literal    |
    ///                  real-literal       |
    ///                  boolean-literal    |
    ///                  char-literal       |
    ///                  string-literal     |
    ///                  identifier         |
    ///                  '(' expression ')'
    ///
    ///      arglist ::= expression arglist2 |
    ///                  {empty}
    ///
    ///     arglist1 ::= expression arglist2 |
    ///
    ///     arglist2 ::= ',' expression arglist2 |
    ///                  {empty}
    ///
    ///
    /// VB.NET is implemented using the following LL(1) BNF.
    /// ----------------------------------------------------
    ///
    ///    expression ::= logicalOrVB logicalXor2VB
    ///
    /// logicalXor2VB ::= 'Xor' logicalOrVB logicalXor2VB |
    ///                   {empty}
    ///
    ///   logicalOrVB ::= logicalAndVB logicalOr2VB
    ///
    ///  logicalOr2VB ::= 'Or' logicalAndVB logicalOr2VB |
    ///                   'OrElse' logicalAndVB logicalOr2VB |
    ///                   {empty}
    ///                 
    ///  logicalAndVB ::= logicalNotVB logicalAnd2VB
    ///
    /// logicalAnd2VB ::= 'And' logicalNotVB logicalAnd2VB |
    ///                   'AndAlso' logicalNotVB logicalAnd2VB |
    ///                   {empty}
    ///                 
    ///  logicalNotVB ::= "Not' logicalNotVB |
    ///                   relationalVB
    ///
    ///  relationalVB ::= shiftVB relational2VB
    ///
    /// relational2VB ::= '&lt;' shiftVB relational2VB  |
    ///                   '>' shiftVB relational2VB     |
    ///                   '&lt;=' shiftVB relational2VB |
    ///                   '>=' shiftVB relational2VB    |
    ///                   '=' shiftVB relational2VB    |
    ///                   '&lt;&gt;' shiftVB relational2VB    |
    ///                   {empty}
    ///
    ///       shiftVB ::= concatVB shift2VB
    ///
    ///      shift2VB ::= '&lt;&lt;' concatVB shift2VB |
    ///                   '>>' concatVB shift2VB |
    ///                   {empty}
    /// 
    ///      concatVB ::= addsubVB concat2VB
    ///
    ///     concat2VB ::= '&amp;' addsubVB concat2VB |
    ///                   {empty}
    ///
    ///      addsubVB ::= modVB addsub2VB
    ///
    ///     addsub2VB ::= '+' modVB addsub2VB |
    ///                   '-' modVB addsub2VB |
    ///                   {empty}
    ///
    ///         modVB ::= intdivVB mod2VB
    ///
    ///        mod2VB ::= 'Mod' intdivVB mod2VB |
    ///                   {empty}
    ///
    ///      intdivVB ::= termVB intdiv2VB
    ///
    ///     intdiv2VB ::= '\' termVB intdiv2VB |
    ///                   {empty}
    ///
    ///        termVB ::= factorVB term2VB
    ///
    ///       term2VB ::= '*' factorVB term2VB |
    ///                   '/' factorVB term2VB |
    ///                   {empty}
    /// 
    ///      factorVB ::= '+' factorVB |
    ///                   '-' factorVB |
    ///                   expoVB
    ///                 
    ///        expoVB ::= accessVB expo2VB
    ///
    ///       expo2VB ::= '^' accessVB expo2VB |
    ///                   {empty}
    ///
    ///      accessVB ::= primary access2VB
    ///        
    ///     access2VB ::= '.' identifier methodcall access2VB |
    ///                   '(' arglist1 ')' access2VB          |
    ///                   {empty}
    /// 
    ///    methodcall ::= '(' arglist ')' |
    ///                   {empty}
    ///
    ///       primary ::= integer-literal    |
    ///                   real-literal       |
    ///                   boolean-literal    |
    ///                   char-literal       |
    ///                   string-literal     |
    ///                   identifier         |
    ///                   '(' expression ')'
    /// 
    ///       arglist ::= expression arglist2 |
    ///                   {empty}
    ///
    ///      arglist1 ::= expression arglist2 |
    /// 
    ///      arglist2 ::= ',' expression arglist2 |
    ///                   {empty}
    ///
    /// </remarks>
    public class Eval : Arithmetic
    {
        #region Events
        /// <summary>
        /// Occurs when an identifier needs resolving during evaluation.
        /// </summary>
        public event EventHandler<ResolveIdentifierEventArgs> ResolveIdentifier;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the Eval class.
        /// </summary>
        public Eval()
            : base()
        {
        }

        /// <summary>
        /// Initialize a new instance of the Eval class.
        /// </summary>
        /// <param name="language">Language used for parsing.</param>
        public Eval(Language language)
            : base(language)
        {
        }

        /// <summary>
        /// Initialize a new instance of the Eval class.
        /// </summary>
        /// <param name="input">Input string to parse.</param>
        public Eval(string input)
            : base(input)
        {
        }

        /// <summary>
        /// Initialize a new instance of the Eval class.
        /// </summary>
        /// <param name="input">Input string to parse.</param>
        /// <param name="language">Language used for parsing.</param>
        public Eval(string input, Language language)
            : base(input, language)
        {
        }
        #endregion

        #region Public
        /// <summary>
        /// Attempt to resolve the provided identifier into a value.
        /// </summary>
        /// <param name="identifier">Identifier to resolve.</param>
        /// <returns>Resolved value.</returns>
        public object PerformResolveIdentifier(string identifier)
        {
            ResolveIdentifierEventArgs e = new ResolveIdentifierEventArgs(identifier);
            OnResolveIdentifier(e);
            return e.Value;
        }
        #endregion

        #region Protected
        /// <summary>
        /// Raises the ResolveIdentifier event.
        /// </summary>
        /// <param name="e">An ResolveIdentifierEventArgs containing the event data.</param>
        protected virtual void OnResolveIdentifier(ResolveIdentifierEventArgs e)
        {
            if (ResolveIdentifier != null)
                ResolveIdentifier(this, e);
        }
        #endregion

        #region Protected
        /// <summary>
        /// Parse the 'expression' rule.
        /// </summary>
        /// <remarks>
        ///
        /// expression ::= condOr cond2
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected override EvalNode ParseExpression()
        {
            if (Language == Language.VBNet)
                return ParseExpressionVB();
            else
                return ParseExpressionCS();
        }

        #region C# Grammer
        /// <summary>
        /// Parse the 'expression' rule.
        /// </summary>
        /// <remarks>
        ///
        /// expression ::= condOr cond2
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseExpressionCS()
        {
            EvalNode or = ParseCondOr();
            EvalNode cond2 = ParseCond2();

            if (cond2 != null)
            {
                cond2.Prepend(or);
                return cond2.Root;
            }
            else
                return or.Root;
        }

        /// <summary>
        /// Parse the 'cond2' rule.
        /// </summary>
        /// <remarks>
        ///
        /// cond2 ::= '?' expression ':' expression |
        ///           '??' expression |
        ///           {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseCond2()
        {
            if (Tokeniser.Next(TokenType.QuestionMark))
            {
                EvalNode op1 = ParseExpression();

                if (Tokeniser.Next(TokenType.Colon))
                    return new EvalNodeConditional(op1, ParseExpression());
                else
                {
                    if (Tokeniser.Peek.Type == TokenType.EndOfInput)
                        throw new ParseException(Tokeniser.Peek.Index, "Found '" + Tokeniser.Peek.Type.ToString() + "' instead of some more input");
                    else
                        throw new ParseException(Tokeniser.Peek.Index, "Found '" + Tokeniser.Peek.Type.ToString() + "' at index '" + Tokeniser.Peek.Index + "' instead of ':'");
                }
            }

            if (Tokeniser.Next(TokenType.DoubleQuestionMark))
            {
                EvalNode op1 = ParseExpression();
                return new EvalNodeNullCoalescing(op1);
            }

            return null;
        }

        /// <summary>
        /// Parse the 'condOr' rule.
        /// </summary>
        /// <remarks>
        ///
        /// condOr ::= condAnd condOr2
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseCondOr()
        {
            EvalNode and = ParseCondAnd();
            EvalNode or2 = ParseCondOr2();

            if (or2 != null)
            {
                or2.Prepend(and);
                return or2.Root;
            }
            else
                return and;
        }

        /// <summary>
        /// Parse the 'condOr2' rule.
        /// </summary>
        /// <remarks>
        ///
        /// condOr2 ::= '||' condAnd condOr2 |
        ///             {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseCondOr2()
        {
            if (Tokeniser.Next(TokenType.DoubleOr))
            {
                EvalNode op = new EvalNodeCondLogicOp(EvalNodeCondLogicOp.CompareOp.Or, ParseCondAnd(), Language);
                EvalNode or2 = ParseCondOr2();
                if (or2 != null)
                    or2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'condAnd' rule.
        /// </summary>
        /// <remarks>
        ///
        /// condAnd ::= logicalOr condAnd2
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseCondAnd()
        {
            EvalNode and = ParseLogicalOr();
            EvalNode and2 = ParseCondAnd2();

            if (and2 != null)
            {
                and2.Prepend(and);
                return and2.Root;
            }
            else
                return and;
        }

        /// <summary>
        /// Parse the 'condAnd2' rule.
        /// </summary>
        /// <remarks>
        ///
        /// condAnd2 ::= '&amp;&amp;' logicalOr condAnd2 |
        ///              {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseCondAnd2()
        {
            if (Tokeniser.Next(TokenType.DoubleAnd))
            {
                EvalNode op = new EvalNodeCondLogicOp(EvalNodeCondLogicOp.CompareOp.And, ParseLogicalOr(), Language);
                EvalNode and2 = ParseCondAnd2();
                if (and2 != null)
                    and2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'logicalOr' rule.
        /// </summary>
        /// <remarks>
        ///
        /// logicalOr ::= logicalXor logicalOr2
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseLogicalOr()
        {
            EvalNode xor = ParseLogicalXor();
            EvalNode or2 = ParseLogicalOr2();

            if (or2 != null)
            {
                or2.Prepend(xor);
                return or2.Root;
            }
            else
                return xor;
        }

        /// <summary>
        /// Parse the 'logicalOr2' rule.
        /// </summary>
        /// <remarks>
        ///
        /// logicalOr2 ::= '|' logicalXor logicalOr2 |
        ///                {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseLogicalOr2()
        {
            if (Tokeniser.Next(TokenType.Or))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.LogicalOr, ParseLogicalXor(), Language);
                EvalNode or2 = ParseLogicalOr2();
                if (or2 != null)
                    or2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'logicalXor' rule.
        /// </summary>
        /// <remarks>
        ///
        /// logicalXor ::= logicalAnd logicalXor2
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseLogicalXor()
        {
            EvalNode and = ParseLogicalAnd();
            EvalNode xor2 = ParseLogicalXor2();

            if (xor2 != null)
            {
                xor2.Prepend(and);
                return xor2.Root;
            }
            else
                return and;
        }

        /// <summary>
        /// Parse the 'logicalXor2' rule.
        /// </summary>
        /// <remarks>
        ///
        /// logicalXor2 ::= '^' logicalAnd logicalXor2 |
        ///                 {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseLogicalXor2()
        {
            if (Tokeniser.Next(TokenType.Hat))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.LogicalXor, ParseLogicalAnd(), Language);
                EvalNode xor2 = ParseLogicalXor2();
                if (xor2 != null)
                    xor2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'logicalAnd' rule.
        /// </summary>
        /// <remarks>
        ///
        /// logicalAnd ::= equality logicalAnd2
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseLogicalAnd()
        {
            EvalNode equality = ParseEquality();
            EvalNode and2 = ParseLogicalAnd2();

            if (and2 != null)
            {
                and2.Prepend(equality);
                return and2.Root;
            }
            else
                return equality;
        }

        /// <summary>
        /// Parse the 'logicalAnd2' rule.
        /// </summary>
        /// <remarks>
        ///
        /// logicalAnd2 ::= '&amp;' equality logicalAnd2 |
        ///                 {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseLogicalAnd2()
        {
            if (Tokeniser.Next(TokenType.And))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.LogicalAnd, ParseEquality(), Language);
                EvalNode and2 = ParseLogicalAnd2();
                if (and2 != null)
                    and2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'equality' rule.
        /// </summary>
        /// <remarks>
        ///
        /// equality ::= relational equality2
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseEquality()
        {
            EvalNode relate = ParseRelational();
            EvalNode equality2 = ParseEquality2();

            if (equality2 != null)
            {
                equality2.Prepend(relate);
                return equality2.Root;
            }
            else
                return relate;
        }

        /// <summary>
        /// Parse the 'equality2' rule.
        /// </summary>
        /// <remarks>
        ///
        /// equality2 ::= '==' relational equality2 |
        ///               '!=' relational equality2 |
        ///               {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseEquality2()
        {
            if (Tokeniser.Next(TokenType.DoubleEqual))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.Equal, ParseRelational(), Language);
                EvalNode equality2 = ParseEquality2();
                if (equality2 != null)
                    equality2.Prepend(op);
                return op;
            }

            if (Tokeniser.Next(TokenType.NotEqual))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.NotEqual, ParseRelational(), Language);
                EvalNode equality2 = ParseEquality2();
                if (equality2 != null)
                    equality2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'relational' rule.
        /// </summary>
        /// <remarks>
        ///
        /// relational ::= shift relational2
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseRelational()
        {
            EvalNode shift = ParseShift();
            EvalNode relate2 = ParseRelational2();

            if (relate2 != null)
            {
                relate2.Prepend(shift);
                return relate2.Root;
            }
            else
                return shift;
        }

        /// <summary>
        /// Parse the 'relational2' rule.
        /// </summary>
        /// <remarks>
        ///
        /// relational2 ::= '&lt;' shift relational2 |
        ///                 '>' shift relational2 |
        ///                 '&lt;=' shift relational2 |
        ///                 '>=' shift relational2 |
        ///                 {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseRelational2()
        {
            if (Tokeniser.Next(TokenType.LessThan))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.LessThan, ParseShift(), Language);
                EvalNode relate2 = ParseRelational2();
                if (relate2 != null)
                    relate2.Prepend(op);
                return op;
            }

            if (Tokeniser.Next(TokenType.GreaterThan))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.GreaterThan, ParseShift(), Language);
                EvalNode relate2 = ParseRelational2();
                if (relate2 != null)
                    relate2.Prepend(op);
                return op;
            }

            if (Tokeniser.Next(TokenType.LessThanEqual))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.LessThanEqual, ParseShift(), Language);
                EvalNode relate2 = ParseRelational2();
                if (relate2 != null)
                    relate2.Prepend(op);
                return op;
            }

            if (Tokeniser.Next(TokenType.GreaterThanEqual))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.GreaterThanEqual, ParseShift(), Language);
                EvalNode relate2 = ParseRelational2();
                if (relate2 != null)
                    relate2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'shift' rule.
        /// </summary>
        /// <remarks>
        ///
        /// shift ::= addsub shift2
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseShift()
        {
            EvalNode addsub = ParseAddSub();
            EvalNode shift2 = ParseShift2();

            if (shift2 != null)
            {
                shift2.Prepend(addsub);
                return shift2.Root;
            }
            else
                return addsub;
        }

        /// <summary>
        /// Parse the 'shift2' rule.
        /// </summary>
        /// <remarks>
        ///
        /// shift2 ::= '&lt;&lt;' addsub shift2 |
        ///            '>>' addsub shift2 |
        ///            {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseShift2()
        {
            if (Tokeniser.Next(TokenType.ShiftLeft))
            {
                EvalNode op = new EvalNodeShiftOp(EvalNodeShiftOp.ShiftOp.Left, ParseAddSub(), Language);
                EvalNode shift2 = ParseShift2();
                if (shift2 != null)
                    shift2.Prepend(op);
                return op;
            }

            if (Tokeniser.Next(TokenType.ShiftRight))
            {
                EvalNode op = new EvalNodeShiftOp(EvalNodeShiftOp.ShiftOp.Right, ParseAddSub(), Language);
                EvalNode shift2 = ParseShift2();
                if (shift2 != null)
                    shift2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'addsub' rule.
        /// </summary>
        /// <remarks>
        ///
        /// addsub ::= term expression2
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseAddSub()
        {
            EvalNode term = ParseTerm();
            EvalNode expr2 = ParseExpression2();

            if (expr2 != null)
            {
                expr2.Prepend(term);
                return expr2.Root;
            }
            else
                return term;
        }
        
        /// <summary>
        /// Process the 'term2' rule.
        /// </summary>
        /// <remarks>
        ///
        /// term2 ::= '*' factor term2 |
        ///           '/' factor term2 |
        ///           '%' factor term2 |
        ///           {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected override EvalNode ParseTerm2()
        {
            if (Tokeniser.Next(TokenType.Remainder))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.Remainder, ParseFactor(), Language);
                EvalNode term2 = ParseTerm2();
                if (term2 != null)
                    term2.Prepend(op);
                return op;
            }

            // Base handles '*' and '/'
            return base.ParseTerm2();
        }

        /// <summary>
        /// Parse the 'factor' rule.
        /// </summary>
        /// <remarks>
        ///
        /// factor ::= '+' factor |
        ///            '-' factor |
        ///            '!' factor |
        ///            '~' factor |
        ///            access
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected override EvalNode ParseFactor()
        {
            if (Tokeniser.Next(TokenType.Plus))
                return new EvalNodeUnaryOp(EvalNodeUnaryOp.UnaryOp.Plus, ParseFactor(), Language);

            if (Tokeniser.Next(TokenType.Minus))
                return new EvalNodeUnaryOp(EvalNodeUnaryOp.UnaryOp.Minus, ParseFactor(), Language);

            if (Tokeniser.Next(TokenType.Not))
                return new EvalNodeUnaryOp(EvalNodeUnaryOp.UnaryOp.Not, ParseFactor(), Language);

            if (Tokeniser.Next(TokenType.Complement))
                return new EvalNodeUnaryOp(EvalNodeUnaryOp.UnaryOp.Complement, ParseFactor(), Language);

            return ParseAccess();
        }

        /// <summary>
        /// Parse the 'access' rule.
        /// </summary>
        /// <remarks>
        ///
        /// access ::= primary access2
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseAccess()
        {
            EvalNode prim = ParsePrimary();
            EvalNode acc2 = ParseAccess2();

            if (acc2 != null)
            {
                acc2.Prepend(prim);
                return acc2.Root;
            }
            else
                return prim;
        }

        /// <summary>
        /// Process the 'term2' rule.
        /// </summary>
        /// <remarks>
        ///
        /// access2 ::= '.' identifier methodcall access2 |
        ///             '[' arglist1 ']' access2          |
        ///             {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseAccess2()
        {
            if (Tokeniser.Next(TokenType.Dot))
            {
                if (Tokeniser.Peek.Type == TokenType.Identifier)
                {
                    string identifier = (string)Tokeniser.Next().Value;

                    EvalNode op;
                    EvalNode funccall = ParseMethodCall();
                    if (funccall != null)
                    {
                        op = new EvalNodeMethod(identifier, Language);
                        op.Prepend(funccall);
                    }
                    else
                        op = new EvalNodeFieldOrProperty(identifier);

                    EvalNode acc2 = ParseAccess2();
                    if (acc2 != null)
                        acc2.Prepend(op);

                    return op;
                }
                else
                    throw new ParseException(Tokeniser.Peek.Index, "Found '" + Tokeniser.Peek.Type.ToString() + "' at index '" + Tokeniser.Peek.Index + "' instead of expected identifier");
            }

            if (Tokeniser.Next(TokenType.OpenSquareBracket))
            {
                EvalNode expr = ParseArgList1();

                if (!Tokeniser.Next(TokenType.CloseSquareBracket))
                {
                    if (Tokeniser.Peek.Type == TokenType.EndOfInput)
                        throw new ParseException(Tokeniser.Peek.Index, "Found '" + Tokeniser.Peek.Type.ToString() + "' instead of close array access");
                    else
                        throw new ParseException(Tokeniser.Peek.Index, "Found '" + Tokeniser.Peek.Type.ToString() + "' at index '" + Tokeniser.Peek.Index + "' instead of close array access'");
                }

                EvalNode op = new EvalNodeArrayIndex(expr, Language);
                EvalNode acc2 = ParseAccess2();
                if (acc2 != null)
                    acc2.Prepend(op);

                return op;
            }

            return null;
        }

        /// <summary>
        /// Process the 'methodcall' rule.
        /// </summary>
        /// <remarks>
        ///
        /// methodcall ::= '(' arglist ')' |
        ///                {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseMethodCall()
        {
            if (Tokeniser.Next(TokenType.OpenRoundBracket))
            {
                EvalNode arglist = ParseArgList();

                if (!Tokeniser.Next(TokenType.CloseRoundBracket))
                {
                    if (Tokeniser.Peek.Type == TokenType.EndOfInput)
                        throw new ParseException(Tokeniser.Peek.Index, "Found '" + Tokeniser.Peek.Type.ToString() + "' instead of expected ')'");
                    else
                        throw new ParseException(Tokeniser.Peek.Index, "Found '" + Tokeniser.Peek.Type.ToString() + "' at index '" + Tokeniser.Peek.Index + "' instead of expected ')'");
                }

                return arglist ?? new EvalNodeArgList(Language);
            }

            return null;
        }

        /// <summary>
        /// Parse the 'primary' rule.
        /// </summary>
        /// <remarks>
        ///
        /// primary ::= integer-literal    |
        ///             real-literal       |
        ///             boolean-literal    |
        ///             char-literal       |
        ///             string-literal     |
        ///             identifier         |
        ///             '(' expression ')'
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected override EvalNode ParsePrimary()
        {
            if ((Tokeniser.Peek.Type == TokenType.BooleanLiteral) ||
                (Tokeniser.Peek.Type == TokenType.CharLiteral) ||
                (Tokeniser.Peek.Type == TokenType.StringLiteral))
                return new EvalNodeLiteral(Tokeniser.Peek.TypeCode, Tokeniser.Next().Value, Language);

            if (Tokeniser.Peek.Type == TokenType.Identifier)
                return new EvalNodeIdentifier(this, (string)Tokeniser.Next().Value, Language);

            // Base handles integer-literal, real-literal and (expression) 
            return base.ParsePrimary();
        }

        /// <summary>
        /// Process the 'arglist' rule.
        /// </summary>
        /// <remarks>
        ///
        /// arglist ::= expression arglist2 |
        ///             {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseArgList()
        {
            if (Tokeniser.Peek.Type != TokenType.CloseRoundBracket)
            {
                EvalNode args = new EvalNodeArgList(ParseExpression(), Language);
                EvalNodeArgList arg2 = ParseArgList2();
                if (arg2 != null)
                    args.Append(arg2.RemoveAll());

                return args;
            }

            return null;
        }

        /// <summary>
        /// Process the 'arglist1' rule.
        /// </summary>
        /// <remarks>
        ///
        /// arglist1 ::= expression arglist2 |
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseArgList1()
        {
            EvalNode args = new EvalNodeArgList(ParseExpression(), Language);
            EvalNodeArgList arg2 = ParseArgList2();
            if (arg2 != null)
                args.Append(arg2.RemoveAll());

            return args;
        }

        /// <summary>
        /// Process the 'arglist2' rule.
        /// </summary>
        /// <remarks>
        ///
        /// arglist2 ::= ',' expression arglist2 |
        ///              {empty}
        ///
        ///</remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNodeArgList ParseArgList2()
        {
            if (Tokeniser.Next(TokenType.Comma))
            {
                EvalNodeArgList args = new EvalNodeArgList(ParseExpression(), Language);
                EvalNodeArgList arg2 = ParseArgList2();
                if (arg2 != null)
                    args.Append(arg2.RemoveAll());

                return args;
            }

            return null;
        }
        #endregion

        #region VB.NET Grammer
        /// <summary>
        /// Parse the 'expression' rule.
        /// </summary>
        /// <remarks>
        ///
        /// expression ::= logicalOrVB logicalXor2VB
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseExpressionVB()
        {
            EvalNode or = ParseLogicalOrVB();
            EvalNode xor2 = ParseLogicalXor2VB();

            if (xor2 != null)
            {
                xor2.Prepend(or);
                return xor2.Root;
            }
            else
                return or.Root;
        }

        /// <summary>
        /// Parse the 'logicalXor2VB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// logicalXor2VB ::= 'Xor' logicalOrVB logicalXor2VB |
        ///                   {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseLogicalXor2VB()
        {
            if (Tokeniser.Next(TokenType.KeywordXor))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.LogicalXor, ParseLogicalOrVB(), Language);
                EvalNode xor2 = ParseLogicalXor2VB();
                if (xor2 != null)
                    xor2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'logicalOrVB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// logicalOrVB ::= logicalAndVB logicalOr2VB
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseLogicalOrVB()
        {
            EvalNode and = ParseLogicalAndVB();
            EvalNode or2 = ParseLogicalOr2VB();

            if (or2 != null)
            {
                or2.Prepend(and);
                return or2.Root;
            }
            else
                return and;
        }
        
        /// <summary>
        /// Parse the 'logicalOr2VB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// logicalOr2VB ::= 'Or' logicalAndVB logicalOr2VB |
        ///                  'OrElse' logicalAndVB logicalOr2VB |
        ///                  {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseLogicalOr2VB()
        {
            if (Tokeniser.Next(TokenType.KeywordOr))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.LogicalOr, ParseLogicalAndVB(), Language);
                EvalNode or2 = ParseLogicalOr2VB();
                if (or2 != null)
                    or2.Prepend(op);
                return op;
            }

            if (Tokeniser.Next(TokenType.KeywordOrElse))
            {
                EvalNode op = new EvalNodeCondLogicOp(EvalNodeCondLogicOp.CompareOp.Or, ParseLogicalAndVB(), Language);
                EvalNode or2 = ParseLogicalOr2VB();
                if (or2 != null)
                    or2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'logicalAndVB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// logicalAndVB ::= logicalNotVB logicalAnd2VB
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseLogicalAndVB()
        {
            EvalNode not = ParseLogicalNotVB();
            EvalNode and2 = ParseLogicalAnd2VB();

            if (and2 != null)
            {
                and2.Prepend(not);
                return and2.Root;
            }
            else
                return not;
        }
        
        /// <summary>
        /// Parse the 'logicalAnd2VB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// logicalAnd2VB ::= 'And' logicalNotVB logicalAnd2VB |
        ///                   'AndAlso' logicalNotVB logicalAnd2VB |
        ///                   {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseLogicalAnd2VB()
        {
            if (Tokeniser.Next(TokenType.KeywordAnd))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.LogicalAnd, ParseLogicalNotVB(), Language);
                EvalNode and2 = ParseLogicalAnd2VB();
                if (and2 != null)
                    and2.Prepend(op);
                return op;
            }

            if (Tokeniser.Next(TokenType.KeywordAndAlso))
            {
                EvalNode op = new EvalNodeCondLogicOp(EvalNodeCondLogicOp.CompareOp.And, ParseLogicalNotVB(), Language);
                EvalNode and2 = ParseLogicalAnd2VB();
                if (and2 != null)
                    and2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'logicalNotVB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// logicalNotVB ::= 'Not' logicalNotVB |
        ///                  relationalVB
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseLogicalNotVB()
        {
            if (Tokeniser.Next(TokenType.KeywordNot))
                return new EvalNodeUnaryOp(EvalNodeUnaryOp.UnaryOp.Not, ParseLogicalNotVB(), Language);

            return ParseRelationalVB();
        }

        /// <summary>
        /// Parse the 'relationalVB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// relationalVB ::= shiftVB relational2VB
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseRelationalVB()
        {
            EvalNode shift = ParseShiftVB();
            EvalNode relate2 = ParseRelational2VB();

            if (relate2 != null)
            {
                relate2.Prepend(shift);
                return relate2.Root;
            }
            else
                return shift;
        }

        /// <summary>
        /// Parse the 'relational2VB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// relational2VB ::= '&lt;'     shiftVB relational2VB |
        ///                   '>'        shiftVB relational2VB |
        ///                   '&lt;='    shiftVB relational2VB |
        ///                   '>='       shiftVB relational2VB |
        ///                   '='        shiftVB relational2VB |
        ///                   '&lt;&gt;' shiftVB relational2VB |
        ///                   {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseRelational2VB()
        {
            if (Tokeniser.Next(TokenType.LessThan))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.LessThan, ParseShiftVB(), Language);
                EvalNode relate2 = ParseRelational2VB();
                if (relate2 != null)
                    relate2.Prepend(op);
                return op;
            }

            if (Tokeniser.Next(TokenType.GreaterThan))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.GreaterThan, ParseShiftVB(), Language);
                EvalNode relate2 = ParseRelational2VB();
                if (relate2 != null)
                    relate2.Prepend(op);
                return op;
            }

            if (Tokeniser.Next(TokenType.LessThanEqual))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.LessThanEqual, ParseShiftVB(), Language);
                EvalNode relate2 = ParseRelational2VB();
                if (relate2 != null)
                    relate2.Prepend(op);
                return op;
            }

            if (Tokeniser.Next(TokenType.GreaterThanEqual))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.GreaterThanEqual, ParseShiftVB(), Language);
                EvalNode relate2 = ParseRelational2VB();
                if (relate2 != null)
                    relate2.Prepend(op);
                return op;
            }

            if (Tokeniser.Next(TokenType.Equal))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.Equal, ParseShiftVB(), Language);
                EvalNode relate2 = ParseRelational2VB();
                if (relate2 != null)
                    relate2.Prepend(op);
                return op;
            }

            if (Tokeniser.Next(TokenType.LessGreater))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.NotEqual, ParseShiftVB(), Language);
                EvalNode relate2 = ParseRelational2VB();
                if (relate2 != null)
                    relate2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'shiftVB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// shiftVB ::= concatVB shift2VB
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseShiftVB()
        {
            EvalNode concat = ParseConcatVB();
            EvalNode shift2 = ParseShift2VB();

            if (shift2 != null)
            {
                shift2.Prepend(concat);
                return shift2.Root;
            }
            else
                return concat;
        }

        /// <summary>
        /// Process the 'shift2VB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// shift2VB ::= '&lt;&lt;' concatVB shift2VB |
        ///              '>>' concatVB shift2VB       |
        ///              {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseShift2VB()
        {
            if (Tokeniser.Next(TokenType.ShiftLeft))
            {
                EvalNode op = new EvalNodeShiftOp(EvalNodeShiftOp.ShiftOp.Left, ParseConcatVB(), Language);
                EvalNode shift2 = ParseShift2VB();
                if (shift2 != null)
                    shift2.Prepend(op);
                return op;
            }

            if (Tokeniser.Next(TokenType.ShiftRight))
            {
                EvalNode op = new EvalNodeShiftOp(EvalNodeShiftOp.ShiftOp.Right, ParseConcatVB(), Language);
                EvalNode shift2 = ParseShift2VB();
                if (shift2 != null)
                    shift2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'concatVB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// concatVB ::= addsubVB concat2VB
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseConcatVB()
        {
            EvalNode addsub = ParseAddSubVB();
            EvalNode concat2 = ParseConcat2VB();

            if (concat2 != null)
            {
                concat2.Prepend(addsub);
                return concat2.Root;
            }
            else
                return addsub;
        }

        /// <summary>
        /// Process the 'concat2VB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// concat2VB ::= '&amp;' addsubVB concat2VB |
        ///               {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseConcat2VB()
        {
            if (Tokeniser.Next(TokenType.And))
            {
                EvalNode op = new EvalNodeConcatenate(ParseAddSubVB());
                EvalNode concat2 = ParseConcat2VB();
                if (concat2 != null)
                    concat2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'addsubVB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// addsubVB ::= modVB addsub2VB
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseAddSubVB()
        {
            EvalNode mod = ParseModVB();
            EvalNode addsub2 = ParseAddSub2VB();

            if (addsub2 != null)
            {
                addsub2.Prepend(mod);
                return addsub2.Root;
            }
            else
                return mod;
        }

        /// <summary>
        /// Process the 'addsub2VB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// addsub2VB ::= '+' modVB addsub2VB |
        ///               '-' modVB addsub2VB |
        ///               {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseAddSub2VB()
        {
            if (Tokeniser.Next(TokenType.Plus))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.Add, ParseModVB(), Language);
                EvalNode addsub2 = ParseAddSub2VB();
                if (addsub2 != null)
                    addsub2.Prepend(op);
                return op;
            }

            if (Tokeniser.Next(TokenType.Minus))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.Subtract, ParseModVB(), Language);
                EvalNode addsub2 = ParseAddSub2VB();
                if (addsub2 != null)
                    addsub2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'modVB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// modVB ::= intdivVB mod2VB
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseModVB()
        {
            EvalNode intdiv = ParseIntDivVB();
            EvalNode mod2 = ParseMod2VB();

            if (mod2 != null)
            {
                mod2.Prepend(intdiv);
                return mod2.Root;
            }
            else
                return intdiv;
        }

        /// <summary>
        /// Process the 'mod2VB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// mod2VB ::= 'Mod' intdivVB mod2VB |
        ///            {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseMod2VB()
        {
            if (Tokeniser.Next(TokenType.KeywordMod))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.Remainder, ParseIntDivVB(), Language);
                EvalNode mod2 = ParseMod2VB();
                if (mod2 != null)
                    mod2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'intdivVB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// intdivVB ::= termVB intdiv2VB
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseIntDivVB()
        {
            EvalNode term = ParseTermVB();
            EvalNode intdiv2 = ParseIntDiv2VB();

            if (intdiv2 != null)
            {
                intdiv2.Prepend(term);
                return intdiv2.Root;
            }
            else
                return term;
        }

        /// <summary>
        /// Process the 'intdiv2VB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// intdiv2VB ::= '\' termVB intdiv2VB |
        ///               {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseIntDiv2VB()
        {
            if (Tokeniser.Next(TokenType.BackSlash))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.IntegerDivide, ParseTermVB(), Language);
                EvalNode intdiv2 = ParseIntDiv2VB();
                if (intdiv2 != null)
                    intdiv2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'termVB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// termVB ::= factorVB term2VB
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseTermVB()
        {
            EvalNode factor = ParseFactorVB();
            EvalNode term2 = ParseTerm2VB();

            if (term2 != null)
            {
                term2.Prepend(factor);
                return term2.Root;
            }
            else
                return factor;
        }

        /// <summary>
        /// Process the 'term2VB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// term2VB ::= '*' factorVB term2VB |
        ///             '/' factorVB term2VB |
        ///             {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseTerm2VB()
        {
            if (Tokeniser.Next(TokenType.Multiply))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.Multiply, ParseFactorVB(), Language);
                EvalNode term2 = ParseTerm2VB();
                if (term2 != null)
                    term2.Prepend(op);
                return op;
            }

            if (Tokeniser.Next(TokenType.Divide))
            {
                EvalNode op = new EvalNodeBinaryOp(EvalNodeBinaryOp.BinaryOp.Divide, ParseFactorVB(), Language);
                EvalNode term2 = ParseTerm2VB();
                if (term2 != null)
                    term2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'factorVB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// factorVB ::= '+' factorVB |
        ///              '-' factorVB |
        ///              expoVB
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseFactorVB()
        {
            if (Tokeniser.Next(TokenType.Plus))
                return new EvalNodeUnaryOp(EvalNodeUnaryOp.UnaryOp.Plus, ParseFactorVB(), Language);

            if (Tokeniser.Next(TokenType.Minus))
                return new EvalNodeUnaryOp(EvalNodeUnaryOp.UnaryOp.Minus, ParseFactorVB(), Language);

            return ParseExpoVB();
        }

        /// <summary>
        /// Parse the 'expoVB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// expoVB ::= accessVB expo2VB
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseExpoVB()
        {
            EvalNode access = ParseAccessVB();
            EvalNode expo2 = ParseExpo2VB();

            if (expo2 != null)
            {
                expo2.Prepend(access);
                return expo2.Root;
            }
            else
                return access;
        }

        /// <summary>
        /// Parse the 'expo2VB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// expo2VB ::= '^' accessVB expo2VB |
        ///             {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseExpo2VB()
        {
            if (Tokeniser.Next(TokenType.Hat))
            {
                EvalNode op = new EvalNodeExponent(ParseAccessVB(), Language);
                EvalNode expo2 = ParseExpo2VB();
                if (expo2 != null)
                    expo2.Prepend(op);
                return op;
            }

            return null;
        }

        /// <summary>
        /// Parse the 'accessVB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// accessVB ::= primary access2VB
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseAccessVB()
        {
            EvalNode prim = ParsePrimary();
            EvalNode access2 = ParseAccess2VB();

            if (access2 != null)
            {
                access2.Prepend(prim);
                return access2.Root;
            }
            else
                return prim;
        }

        /// <summary>
        /// Process the 'access2VB' rule.
        /// </summary>
        /// <remarks>
        ///
        /// access2VB ::= '.' identifier methodcall access2VB |
        ///              '(' arglist1 ')' access2VB           |
        ///              {empty}
        ///
        /// </remarks>
        /// <returns>Parsed node.</returns>
        protected virtual EvalNode ParseAccess2VB()
        {
            if (Tokeniser.Next(TokenType.Dot))
            {
                if (Tokeniser.Peek.Type == TokenType.Identifier)
                {
                    string identifier = (string)Tokeniser.Next().Value;

                    EvalNode op;
                    EvalNode funccall = ParseMethodCall();
                    if (funccall != null)
                    {
                        op = new EvalNodeMethod(identifier, Language);
                        op.Prepend(funccall);
                    }
                    else
                        op = new EvalNodeFieldOrProperty(identifier);

                    EvalNode acc2 = ParseAccess2VB();
                    if (acc2 != null)
                        acc2.Prepend(op);

                    return op;
                }
                else
                    throw new ParseException(Tokeniser.Peek.Index, "Found '" + Tokeniser.Peek.Type.ToString() + "' at index '" + Tokeniser.Peek.Index + "' instead of expected identifier");
            }

            if (Tokeniser.Next(TokenType.OpenRoundBracket))
            {
                EvalNode expr = ParseArgList1();

                if (!Tokeniser.Next(TokenType.CloseRoundBracket))
                {
                    if (Tokeniser.Peek.Type == TokenType.EndOfInput)
                        throw new ParseException(Tokeniser.Peek.Index, "Found '" + Tokeniser.Peek.Type.ToString() + "' instead of close array ')'");
                    else
                        throw new ParseException(Tokeniser.Peek.Index, "Found '" + Tokeniser.Peek.Type.ToString() + "' at index '" + Tokeniser.Peek.Index + "' instead of close array access  ')'");
                }

                EvalNode op = new EvalNodeArrayIndex(expr, Language);
                EvalNode acc2 = ParseAccess2VB();
                if (acc2 != null)
                    acc2.Prepend(op);

                return op;
            }

            return null;
        }
        #endregion
        #endregion
    }
}

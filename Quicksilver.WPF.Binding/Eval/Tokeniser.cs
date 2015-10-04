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
using System.Globalization;

namespace ComponentFactory.Quicksilver.Binding
{
    /// <summary>
    /// Converts a string into a list of lexical tokens.
    /// </summary>
    internal class Tokeniser
    {
        #region Static Fields
        private static Dictionary<char, TokenType> _singleCharLookup;
        private static string _hexCharacters = "0123456789ABCDEFabcdef";
        private static string _octalCharacters = "01234567";
        private static string _realCharactersCS = "eEfFdDMm";
        private static string _realCharactersVB = "EFRD";
        private static string _realSuffixCharactersCS = "fFdDMm";
        private static string _realSuffixCharactersVB = "FRD";
        #endregion

        #region Instance Fields
        private Language _language;
        private List<Token> _tokens;
        private int _length;
        #endregion

        #region Identity
        static Tokeniser()
        {
            // Use hash table for quick lookup of single characters
            _singleCharLookup = new Dictionary<char, TokenType>();
            _singleCharLookup.Add('+', TokenType.Plus);
            _singleCharLookup.Add('-', TokenType.Minus);
            _singleCharLookup.Add('*', TokenType.Multiply);
            _singleCharLookup.Add('/', TokenType.Divide);
            _singleCharLookup.Add('~', TokenType.Complement);
            _singleCharLookup.Add('%', TokenType.Remainder);
            _singleCharLookup.Add('&', TokenType.And);
            _singleCharLookup.Add('\\', TokenType.BackSlash);
            _singleCharLookup.Add(':', TokenType.Colon);
            _singleCharLookup.Add('^', TokenType.Hat);
            _singleCharLookup.Add('.', TokenType.Dot);
            _singleCharLookup.Add(',', TokenType.Comma);
            _singleCharLookup.Add('=', TokenType.Equal);
            _singleCharLookup.Add('(', TokenType.OpenRoundBracket);
            _singleCharLookup.Add(')', TokenType.CloseRoundBracket);
            _singleCharLookup.Add('[', TokenType.OpenSquareBracket);
            _singleCharLookup.Add(']', TokenType.CloseSquareBracket);
        }

        /// <summary>
        /// Initialize a new instance of the Tokeniser class.
        /// </summary>
        /// <param name="input">String to convert into a series of tokens.</param>
        /// <param name="language">Language used for parsing.</param>
        public Tokeniser(string input, Language language)
        {
            // Language determines characters are converted into tokens
            _language = language;

            // Ensure we always have a string instance to work against
            if (input == null)
                input = string.Empty;

            // Create lists for holding all the tokens we find
            _tokens = new List<Token>();

            // Convert the string to an array of characters
            char[] chars = input.ToCharArray();

            // Cache length of the input
            _length = chars.Length;

            // Keep getting the next token until we reach the end-of-input
            int index = 0;
            while (index < _length)
            {
                // Get the next token
                Token token = NextToken(chars, ref index);

                // If reached the end of input, then we are done
                if (token.Type == TokenType.EndOfInput)
                    break;
                else
                    _tokens.Add(token);
            }
        }
        #endregion

        #region Public
        /// <summary>
        /// Gets a peek at the next token that is available, without removing it.
        /// </summary>
        public Token Peek
        {
            get
            {
                // If no tokens left then return the end-of-input token
                if (_tokens.Count == 0)
                    return new Token(TokenType.EndOfInput, _length);
                else
                    return _tokens[0];
            }
        }

        /// <summary>
        /// Gets the next token and removes it.
        /// </summary>
        public Token Next()
        {
            // If no tokens left then return the end-of-input token
            if (_tokens.Count == 0)
                return new Token(TokenType.EndOfInput, _length);
            else
            {
                // Remove token from list and then return it
                Token t = _tokens[0];
                _tokens.RemoveAt(0);
                return t;
            }
        }

        /// <summary>
        /// If the next token matchs the requested type then just eat it.
        /// </summary>
        /// <param name="type">Type to eat if found.</param>
        /// <returns>True if type was eaten; otherwise false.</returns>
        public bool Next(TokenType type)
        {
            if (Peek.Type == type)
            {
                if (type != TokenType.EndOfInput)
                    Next();

                return true;
            }
            else
                return false;
        }
        #endregion

        #region Private
        private Token NextToken(char[] chars, ref int index)
        {
            // Remove any leading whitespace
            while((index < _length) && (char.IsWhiteSpace(chars[index])))
                index++;

            // Check for end-of-input
            if (index == _length)
                return new Token(TokenType.EndOfInput, _length);
            else
            {
                // Number literals can start with a digit
                if (char.IsDigit(chars[index]))
                    return NextNumberLiteral(chars, ref index);

                // Real number literal can start with a decimal point but only if followed by a digit
                if ((chars[index] == '.') && ((index+1) < _length) && char.IsDigit(chars[index+1]))
                    return NextNumberLiteral(chars, ref index);

                // Perform language specific tokenising
                if (_language == Language.VBNet)
                {
                    // Check for '&H' as start of a hex integer literal
                    if ((chars[index] == '&') && ((index + 1) < _length) && (chars[index + 1] == 'H'))
                    {
                        // Move past '&H'
                        index += 2;
                        return NextNumberCodedLiteralVB(chars, ref index, 16);
                    }

                    // Check for '&O' as start of an octal integer literal
                    if ((chars[index] == '&') && ((index + 1) < _length) && (chars[index + 1] == 'O'))
                    {
                        // Move past '&O'
                        index += 2;
                        return NextNumberCodedLiteralVB(chars, ref index, 8);
                    }
                }
                else
                {
                    // Or and DoubleOr tokens both start with the '|' character
                    if (chars[index] == '|')
                    {
                        index++;
                        if ((index < _length) && (chars[index] == '|'))
                            return new Token(TokenType.DoubleOr, index++);
                        else
                            return new Token(TokenType.Or, index);
                    }

                    // And and DoubleAnd tokens both start with the '&' character
                    if (chars[index] == '&')
                    {
                        index++;
                        if ((index < _length) && (chars[index] == '&'))
                            return new Token(TokenType.DoubleAnd, index++);
                        else
                            return new Token(TokenType.And, index);
                    }

                    // Not and NotEqual tokens both start with the '!' character
                    if (chars[index] == '!')
                    {
                        index++;
                        if ((index < _length) && (chars[index] == '='))
                            return new Token(TokenType.NotEqual, index++);
                        else
                            return new Token(TokenType.Not, index);
                    }

                    // QuestionMark and DoubleQuestionMark tokens both start with the '?' character
                    if (chars[index] == '?')
                    {
                        index++;
                        if ((index < _length) && (chars[index] == '?'))
                            return new Token(TokenType.DoubleQuestionMark, index++);
                        else
                            return new Token(TokenType.QuestionMark, index);
                    }

                    // Check for a '//' which indicates the rest of the input on the line is a comment
                    if (((index + 2) <= _length) && (chars[index] == '/') &&  (chars[index+1] == '/'))
                        return new Token(TokenType.EndOfInput, _length);

                    // Check for a '/*' which indicates the start of a comment block
                    if (((index + 1) < _length) && (chars[index] == '/') && (chars[index + 1] == '*'))
                    {
                        // Move past '/*'
                        index += 2;

                        // Keep going till we find the matching '*/'
                        while (index < (_length - 1))
                        {
                            if ((chars[index] == '*') && (chars[index + 1] == '/'))
                            {
                                // Skip over '*/'
                                index += 2;

                                // Return the token after the comment as the next token
                                return NextToken(chars, ref index);
                            }

                            index++;
                        }

                        throw new ParseException(index, "End of comment '*/' not found");
                    }
                }

                // The '@' symbol can be used in front of a char/string literal
                if (chars[index] == '@')
                {
                    // If start of a char/string then process without escaping
                    if ((chars[index + 1] == '\'') || (chars[index + 1] == '\"'))
                    {
                        index++;
                        return NextCharOrStringLiteral(chars, ref index, false);
                    }
                }

                // Without '@' symbol in front the char/string literal is escaped
                if ((chars[index] == '\'') || (chars[index] == '\"'))
                    return NextCharOrStringLiteral(chars, ref index, true);

                // Check for double '==' as a match for DoubleEqual token
                if ((chars[index] == '=') && ((index+1) < _length) && (chars[index+1] == '='))
                {
                    Token ret = new Token(TokenType.DoubleEqual, index);
                    index += 2;
                    return ret;
                }

                // LessThan, LessThanEqual, ShiftLeft and LessGreater tokens all start with the '<' character
                if (chars[index] == '<')
                {
                    index++;
                    if ((index < _length) && (chars[index] == '='))
                        return new Token(TokenType.LessThanEqual, index++);
                    else if ((index < _length) && (chars[index] == '<'))
                        return new Token(TokenType.ShiftLeft, index++);
                    else if ((index < _length) && (chars[index] == '>'))
                        return new Token(TokenType.LessGreater, index++);
                    else
                        return new Token(TokenType.LessThan, index);
                }

                // GreaterThan, GreaterThanEqual and ShiftRight tokens all start with the '>' character
                if (chars[index] == '>')
                {
                    index++;
                    if ((index < _length) && (chars[index] == '='))
                        return new Token(TokenType.GreaterThanEqual, index++);
                    else if ((index < _length) && (chars[index] == '>'))
                        return new Token(TokenType.ShiftRight, index++);
                    else
                        return new Token(TokenType.GreaterThan, index);
                }

                // Identifiers always start with an alphabetic character
                if (char.IsLetter(chars[index]))
                    return NextIdentifier(chars, ref index);

                // Finally see if the character is a single character token we have in lookup hash
                TokenType type;
                if (_singleCharLookup.TryGetValue(chars[index], out type))
                    return new Token(type, index++);
                else
                {
                    // Unrecognized character, so throw an exception
                    throw new ParseException(index, "Unrecognized character '" + chars[index] + "' at index '" + index.ToString() + "'");
                }
            }
        }

        private Token NextNumberLiteral(char[] chars, ref int index)
        {
            // Move past all the decimal digits
            int start = index;
            while ((index < _length) && (char.IsDigit(chars[index])))
                index++;

            // Only C# uses '0x' as the method of specifying hex values
            if (_language == Language.CSharp)
            {
                // If we only have a single digit that is '0'
                if (((index - start) == 1) && (chars[start] == '0'))
                {
                    // And the next character is an 'x' then we have '0x' which is the prefix for hex format
                    if ((index < _length) && ((chars[index] == 'x') || (chars[index] == 'X')))
                        return NextHexLiteral(chars, start, ref index);
                }
            }

            // If the next character is a decimal point
            if ((index < _length) && (chars[index] == '.'))
            {
                // Move past the decimal point
                index++;

                // There must be a digit after the decimal point
                if ((index >= _length) || !char.IsDigit(chars[index]))
                    throw new ParseException(index - 1, "Must have at least 1 digit after the decimal point at index '" + (index - 1).ToString() + "'");

                // Find the end of the fraction part
                while ((index < _length) && char.IsDigit(chars[index]))
                    index++;

                // Process real literal from exponent onwards
                return NextRealLiteral(chars, start, ref index);
            }

            if (_language == Language.CSharp)
            {
                // If the next character is an exponent or type suffix then it is a real number
                if ((index < _length) && _realCharactersCS.Contains(chars[index].ToString()))
                    return NextRealLiteral(chars, start, ref index);
            }
            else
            {
                // If the next character is an exponent or type suffix then it is a real number
                if ((index < _length) && _realCharactersVB.Contains(chars[index].ToString()))
                    return NextRealLiteral(chars, start, ref index);
            }

            // Must be an integer and so process any type suffix for integers
            return NextIntegerLiteral(chars, start, ref index, 10);
        }

        private Token NextHexLiteral(char[] chars, int start, ref int index)
        {
            // Move past the 'x' in the '0x' prefix
            index++;

            // Move past all the hex characters
            while ((index < _length) && _hexCharacters.Contains(chars[index].ToString()))
                index++;

            // If there were no characters after the hex prefix
            if ((index - start) == 2)
                throw new ParseException(index, "Hex literal prefix '0x' found but with no hex characters following at index '" + index.ToString() + "'");
            else
            {
                // Process any type suffix
                return NextIntegerLiteral(chars, start, ref index, 16);
            }
        }

        private Token NextNumberCodedLiteralVB(char[] chars, ref int index, int numberBase)
        {
            int start = index;

            // Move past all the hex/octal characters
            string codeCharacters = (numberBase == 16 ? _hexCharacters : _octalCharacters);
            while ((index < _length) && codeCharacters.Contains(chars[index].ToString()))
                index++;

            // If there were no characters after the hex/octal prefix
            if ((index - start) == 0)
                throw new ParseException(index, "Hex/Octal literal prefix found but with no hex/octal characters following at index '" + index.ToString() + "'");
            else
            {
                // Process any type suffix
                return NextIntegerLiteral(chars, start, ref index, numberBase);
            }

        }

        private Token NextIntegerLiteral(char[] chars, int start, ref int index, int numberBase)
        {
            if (_language == Language.CSharp)
                return NextIntegerLiteralCS(chars, start, ref index, numberBase);
            else
                return NextIntegerLiteralVB(chars, start, ref index, numberBase);
        }

        private Token NextIntegerLiteralCS(char[] chars, int start, ref int index, int numberBase)
        {
            bool typeU = false;
            bool typeL = false;

            // Extract the integer literal string (excluding any type suffix)
            string value = new string(chars, start, index - start);

            // If another character is available then check for type suffix characters
            if (index < _length)
            {
                // Check for the unsigned type suffix character
                if ((chars[index] == 'u') || (chars[index] == 'U'))
                {
                    typeU = true;
                    index++;

                    // Check for the long type suffix character
                    if ((index < _length) && ((chars[index] == 'l') || (chars[index] == 'L')))
                    {
                        index++;
                        typeL = true;
                    }
                }
                else
                {
                    // Check for the long type suffix character
                    if ((chars[index] == 'l') || (chars[index] == 'L'))
                    {
                        index++;
                        typeL = true;

                        // Check for the unsigned type suffix character
                        if ((index < _length) && ((chars[index] == 'u') || (chars[index] == 'U')))
                        {
                            index++;
                            typeU = true;
                        }
                    }
                }
            }

            // Do we try and represent value as an Int32?
            bool failed = false;
            if (!typeU && !typeL)
            {
                try
                {
                    return new Token(TokenType.IntegerLiteral, start, TypeCode.Int32, Convert.ToInt32(value, numberBase));
                }
                catch(OverflowException) 
                { 
                    failed = true; 
                }
            }

            // Do we try and represent value as an UInt32?
            if (failed || (typeU && !typeL))
            {
                try
                {
                    return new Token(TokenType.IntegerLiteral, start, TypeCode.UInt32, Convert.ToUInt32(value, numberBase));
                }
                catch(OverflowException) { }
            }

            // Do we try and represent value as an Int64?
            if (failed || (!typeU && typeL))
            {
                try
                {
                    return new Token(TokenType.IntegerLiteral, start, TypeCode.Int64, Convert.ToInt64(value, numberBase));
                }
                catch (OverflowException) { }
            }

            // Final attempt is as a unsigned long
            return new Token(TokenType.IntegerLiteral, start, TypeCode.UInt64, Convert.ToUInt64(value, numberBase));
        }

        private Token NextIntegerLiteralVB(char[] chars, int start, ref int index, int numberBase)
        {
            // Extract the integer literal string (excluding any type suffix)
            string value = new string(chars, start, index - start);

            // Does the number have the 'S' Int16 suffix?
            if ((index < _length) && (chars[index] == 'S'))
            {
                // Move past 'S'
                index++;
                return new Token(TokenType.IntegerLiteral, start, TypeCode.Int16, Convert.ToInt16(value, numberBase));
            }

            // Does the number have the 'I' Int32 suffix?
            if ((index < _length) && (chars[index] == 'I'))
            {
                // Move past 'I'
                index++;
                return new Token(TokenType.IntegerLiteral, start, TypeCode.Int32, Convert.ToInt32(value, numberBase));
            }

            // Does the number have the 'L' Int64 suffix?
            if ((index < _length) && (chars[index] == 'L'))
            {
                // Move past 'I'
                index++;
                return new Token(TokenType.IntegerLiteral, start, TypeCode.Int64, Convert.ToInt64(value, numberBase));
            }

            try
            {
                // No suffix menas we first try using an Int32
                return new Token(TokenType.IntegerLiteral, start, TypeCode.Int32, Convert.ToInt32(value, numberBase));
            }
            catch (OverflowException)
            {
                // Try again using Int64 as the last resort
                return new Token(TokenType.IntegerLiteral, start, TypeCode.Int64, Convert.ToInt64(value, numberBase));
            }
        }

        private Token NextRealLiteral(char[] chars, int start, ref int index)
        {
            // If the next character is an exponent part
            if ((index < _length) && ((chars[index] == 'e') || (chars[index] == 'E')))
            {
                // Move past the exponent character
                index++;

                // Move past the optional sign character
                if ((index < _length) && ((chars[index] == '+') || (chars[index] == '-')))
                    index++;

                // There must be at least one digit in the exponent
                if ((index >= _length) || !char.IsDigit(chars[index]))
                    throw new ParseException(index - 1, "Must have at least 1 digit in the exponent at index '" + (index - 1).ToString() + "'");

                // Find the end of the exponent part
                while ((index < _length) && char.IsDigit(chars[index]))
                    index++;
            }

            // Extract the real literal string (excluding any type suffix)
            string value = new string(chars, start, index - start);

            if (_language == Language.CSharp)
            {
                // If the next character is a type suffix
                if ((index < _length) && _realSuffixCharactersCS.Contains(chars[index].ToString()))
                {
                    switch (chars[index])
                    {
                        case 'f':
                        case 'F':
                            index++;
                            return new Token(TokenType.RealLiteral, start, TypeCode.Single, Convert.ToSingle(value));
                        case 'm':
                        case 'M':
                            index++;
                            return new Token(TokenType.RealLiteral, start, TypeCode.Decimal, Decimal.Parse(value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint));
                        default:
                            // Move past the double prefix
                            index++;
                            break;
                    }
                }
            }
            else
            {
                // If the next character is a type suffix
                if ((index < _length) && _realSuffixCharactersVB.Contains(chars[index].ToString()))
                {
                    switch (chars[index])
                    {
                        case 'F':
                            index++;
                            return new Token(TokenType.RealLiteral, start, TypeCode.Single, Convert.ToSingle(value));
                        case 'D':
                            index++;
                            return new Token(TokenType.RealLiteral, start, TypeCode.Decimal, Decimal.Parse(value, NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint));
                        default:
                            // Move past the double prefix
                            index++;
                            break;
                    }
                }
            }

            // Default to conversion into a double type
            return new Token(TokenType.RealLiteral, start, TypeCode.Double, Convert.ToDouble(value));
        }

        private Token NextIdentifier(char[] chars, ref int index)
        {
            // Move past all the alphanumeric characters
            int start = index;
            while ((index < _length) && (char.IsLetterOrDigit(chars[index])))
                index++;

            // Extact the identifier string
            string identifier = new string(chars, start, index - start);

            // Look for any language specific keywords
            if (_language == Language.VBNet)
            {
                switch (identifier)
                {
                    case "Or":
                        return new Token(TokenType.KeywordOr, start);
                    case "OrElse":
                        return new Token(TokenType.KeywordOrElse, start);
                    case "Not":
                        return new Token(TokenType.KeywordNot, start);
                    case "And":
                        return new Token(TokenType.KeywordAnd, start);
                    case "AndAlso":
                        return new Token(TokenType.KeywordAndAlso, start);
                    case "Xor":
                        return new Token(TokenType.KeywordXor, start);
                    case "Mod":
                        return new Token(TokenType.KeywordMod, start);
                    case "True":
                        return new Token(TokenType.BooleanLiteral, start, TypeCode.Boolean, true);
                    case "False":
                        return new Token(TokenType.BooleanLiteral, start, TypeCode.Boolean, false);
                }
            }
            else
            {
                switch (identifier)
                {
                    case "true":
                        return new Token(TokenType.BooleanLiteral, start, TypeCode.Boolean, true);
                    case "false":
                        return new Token(TokenType.BooleanLiteral, start, TypeCode.Boolean, false);
                }
            }

            return new Token(TokenType.Identifier, start, TypeCode.String, identifier);
        }

        private Token NextCharOrStringLiteral(char[] chars, ref int index, bool escaped)
        {
            // Are we scanning single for double quotes?
            bool singleQuote = (chars[index++] == '\'');
            int start = index;

            StringBuilder contents = new StringBuilder();

            // Store everything until we find the matching end quote/double quotes or end of input
            while ((index < _length) && ((singleQuote && chars[index] != '\'') ||
                                         (!singleQuote && chars[index] != '\"')))
            {
                // Is this the start of an escaped character?
                if (escaped && (chars[index] == '\\'))
                {
                    // Move past the back slash character
                    index++;

                    // We need another character to be present
                    if (index >= _length)
                        throw new ParseException(index - 1, "End of input found when processing a character/string literal");
                    else
                    {
                        switch (chars[index++])
                        {
                            case '\'':
                                contents.Append('\'');
                                break;
                            case '\"':
                                contents.Append('\"');
                                break;
                            case '\\':
                                contents.Append('\\');
                                break;
                            case 'a':
                                contents.Append('\a');
                                break;
                            case 'b':
                                contents.Append('\b');
                                break;
                            case 'f':
                                contents.Append('\f');
                                break;
                            case 'n':
                                contents.Append('\n');
                                break;
                            case 'r':
                                contents.Append('\r');
                                break;
                            case 't':
                                contents.Append('\t');
                                break;
                            case 'v':
                                contents.Append('\v');
                                break;
                            default:
                                throw new ParseException(index - 1, "Unrecognized character '" + chars[index-1] + "' after '\\' escape chararater");
                        }
                    }
                }
                else
                    contents.Append(chars[index++]);
            }

            // If not escaping the quote and double quote then look to replace double entries now
            if (!escaped)
            {
                contents = contents.Replace(@"''", "\'");
                contents = contents.Replace(@"""", "\"");
            }

            // We should now find the ending quote/double quote
            if ((index < _length) && ((singleQuote && chars[index] == '\'') || (!singleQuote && chars[index] == '\"')))
            {
                // Move past the end quote/double quotes
                index++;

                // Under VB.NET a single character string can have an extra character after it...
                if ((_language == Language.VBNet) && (contents.Length == 1) && (index < _length))
                {
                    // A 'c' means the string is actually treated as a character instead
                    if (chars[index] == 'c')
                    {
                        // Move past the 'c'
                        index++;
    
                        // Return the single char
                        return new Token(TokenType.CharLiteral, start, TypeCode.Char, (char)contents[0]);
                    }
                }

                if (singleQuote && (contents.Length == 1) && (_language == Language.CSharp))
                    return new Token(TokenType.CharLiteral, start, TypeCode.Char, (char)contents[0]);
                else
                    return new Token(TokenType.StringLiteral, start, TypeCode.String, contents.ToString());
            }
            else
            {
                if (singleQuote)
                    throw new ParseException(index, "No matching end quote found");
                else
                    throw new ParseException(index, "No matching end double quotes found");
            }
        }
        #endregion
    }
}

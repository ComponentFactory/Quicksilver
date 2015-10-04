using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComponentFactory.Quicksilver.Binding;

namespace ComponentFactory.Quicksilver.UnitTests.WPF
{
    public class UnitTestArithmetic
    {
        public static bool PerformTests()
        {
            try
            {
                TestLiterals();
                TestUnary();
                TestBinaryTimesDivide();
                TestBinaryPlusMinus();
                TestBinaryShift();
                TestRelational();
                TestEquality();
                TestLogical();
                TestConditionalLogical();
                TestConditional();
                TestPrecedence();

                return true;
            }
            catch (Exception e)
            {
                ErrorWindow ew = new ErrorWindow();
                ew.DataContext = e;
                ew.ShowDialog();
                return false;
            }
        }

        public static void TestLiterals()
        {
            // Literals (positive tests)
            TestArithmeticSuccess("1", TypeCode.Int32, 1);
            TestArithmeticSuccess("-1", TypeCode.Int32, -1);
            TestArithmeticSuccess("1u", TypeCode.UInt32, 1u);
            TestArithmeticSuccess("1U", TypeCode.UInt32, 1U);
            TestArithmeticSuccess("1l", TypeCode.Int64, 1L);
            TestArithmeticSuccess("-1l", TypeCode.Int64, -1L);
            TestArithmeticSuccess("1L", TypeCode.Int64, 1L);
            TestArithmeticSuccess("1ul", TypeCode.UInt64, 1ul);
            TestArithmeticSuccess("1uL", TypeCode.UInt64, 1uL);
            TestArithmeticSuccess("1Ul", TypeCode.UInt64, 1Ul);
            TestArithmeticSuccess("1UL", TypeCode.UInt64, 1UL);
            TestArithmeticSuccess("1lu", TypeCode.UInt64, 1Lu);
            TestArithmeticSuccess("1Lu", TypeCode.UInt64, 1Lu);
            TestArithmeticSuccess("1lU", TypeCode.UInt64, 1LU);
            TestArithmeticSuccess("1LU", TypeCode.UInt64, 1LU);
            TestArithmeticSuccess("1f", TypeCode.Single, 1f);
            TestArithmeticSuccess("-1f", TypeCode.Single, -1f);
            TestArithmeticSuccess("1F", TypeCode.Single, 1F);
            TestArithmeticSuccess("1e1f", TypeCode.Single, 1e1f);
            TestArithmeticSuccess("1E+1f", TypeCode.Single, 1e+1f);
            TestArithmeticSuccess("1e-1f", TypeCode.Single, 1e-1f);
            TestArithmeticSuccess("1e10f", TypeCode.Single, 1e10f);
            TestArithmeticSuccess("1E+10f", TypeCode.Single, 1e+10f);
            TestArithmeticSuccess("1e-10f", TypeCode.Single, 1e-10f);
            TestArithmeticSuccess(".1e1f", TypeCode.Single, .1e1f);
            TestArithmeticSuccess(".1E+1f", TypeCode.Single, .1e+1f);
            TestArithmeticSuccess(".1e-1f", TypeCode.Single, .1e-1f);
            TestArithmeticSuccess(".1e10f", TypeCode.Single, .1e10f);
            TestArithmeticSuccess(".1e+10f", TypeCode.Single, .1e+10f);
            TestArithmeticSuccess(".1E-10f", TypeCode.Single, .1e-10f);
            TestArithmeticSuccess("1d", TypeCode.Double, 1d);
            TestArithmeticSuccess("-1d", TypeCode.Double, -1d);
            TestArithmeticSuccess("1D", TypeCode.Double, 1D);
            TestArithmeticSuccess("1e1d", TypeCode.Double, 1e1d);
            TestArithmeticSuccess("1E+1d", TypeCode.Double, 1e+1d);
            TestArithmeticSuccess("1e-1d", TypeCode.Double, 1e-1d);
            TestArithmeticSuccess("1E10d", TypeCode.Double, 1e10d);
            TestArithmeticSuccess("1e+10d", TypeCode.Double, 1e+10d);
            TestArithmeticSuccess("1e-10d", TypeCode.Double, 1e-10d);
            TestArithmeticSuccess(".1e1d", TypeCode.Double, .1e1d);
            TestArithmeticSuccess(".1E+1d", TypeCode.Double, .1e+1d);
            TestArithmeticSuccess(".1e-1d", TypeCode.Double, .1e-1d);
            TestArithmeticSuccess(".1E10d", TypeCode.Double, .1e10d);
            TestArithmeticSuccess(".1e+10d", TypeCode.Double, .1e+10d);
            TestArithmeticSuccess(".1e-10d", TypeCode.Double, .1e-10d);
            TestArithmeticSuccess(".1", TypeCode.Double, .1d);
            TestArithmeticSuccess("1.0", TypeCode.Double, 1d);
            TestArithmeticSuccess("-1.0", TypeCode.Double, -1d);
            TestArithmeticSuccess(".1f", TypeCode.Single, .1f);
            TestArithmeticSuccess("1.0f", TypeCode.Single, 1f);
            TestArithmeticSuccess("-1.0f", TypeCode.Single, -1f);
            TestArithmeticSuccess("1m", TypeCode.Decimal, 1m);
            TestArithmeticSuccess("-1m", TypeCode.Decimal, -1m);
            TestArithmeticSuccess("1.0m", TypeCode.Decimal, 1.0m);
            TestArithmeticSuccess("-1.0m", TypeCode.Decimal, -1.0m);
            TestArithmeticSuccess("1M", TypeCode.Decimal, 1M);
            TestArithmeticSuccess("1e1m", TypeCode.Decimal, 1e1m);
            TestArithmeticSuccess("1E+1m", TypeCode.Decimal, 1e+1m);
            TestArithmeticSuccess("1e-1m", TypeCode.Decimal, 1e-1m);
            TestArithmeticSuccess("1e10m", TypeCode.Decimal, 1e10m);
            TestArithmeticSuccess("1e+10m", TypeCode.Decimal, 1e+10m);
            TestArithmeticSuccess("1e-10m", TypeCode.Decimal, 1e-10m);
            TestArithmeticSuccess(".1e1m", TypeCode.Decimal, .1e1m);
            TestArithmeticSuccess(".1E+1m", TypeCode.Decimal, .1e+1m);
            TestArithmeticSuccess(".1e-1m", TypeCode.Decimal, .1e-1m);
            TestArithmeticSuccess(".1e10m", TypeCode.Decimal, .1e10m);
            TestArithmeticSuccess(".1e+10m", TypeCode.Decimal, .1e+10m);
            TestArithmeticSuccess(".1E-10m", TypeCode.Decimal, .1e-10m);
            TestArithmeticSuccess("0x1", TypeCode.Int32, 0x1);
            TestArithmeticSuccess("(.1e+10m)", TypeCode.Decimal, .1e+10m);
            TestArithmeticSuccess("((.1e+10m))", TypeCode.Decimal, .1e+10m);
            TestArithmeticSuccess("(((.1e+10m)))", TypeCode.Decimal, .1e+10m);

            // Literals (negative tests)
            TestArithmeticFailedParseError("1g", 1);
            TestArithmeticFailedParseError("1ll", 2);
            TestArithmeticFailedParseError("1ld", 2);
            TestArithmeticFailedParseError("1lf", 2);
            TestArithmeticFailedParseError("1lm", 2);
            TestArithmeticFailedParseError("1uu", 2);
            TestArithmeticFailedParseError("1ud", 2);
            TestArithmeticFailedParseError("1uf", 2);
            TestArithmeticFailedParseError("1um", 2);
            TestArithmeticFailedParseError("1fl", 2);
            TestArithmeticFailedParseError("1fu", 2);
            TestArithmeticFailedParseError("1fd", 2);
            TestArithmeticFailedParseError("1fm", 2);
            TestArithmeticFailedParseError("1dl", 2);
            TestArithmeticFailedParseError("1du", 2);
            TestArithmeticFailedParseError("1df", 2);
            TestArithmeticFailedParseError("1dm", 2);
            TestArithmeticFailedParseError("0x", 2);
            TestArithmeticFailedParseError(".0x1", 2);
            TestArithmeticFailedParseError("0xg", 2);
            TestArithmeticFailedParseError("true", 0);
            TestArithmeticFailedParseError("false", 0);
            TestArithmeticFailedParseError("'a'", 1);
            TestArithmeticFailedParseError("'ab'", 1);
            TestArithmeticFailedParseError("\"ab\"", 1);
        }

        public static void TestUnary()
        {
            // Unary operations (positive tests)
            TestArithmeticSuccess("+1", TypeCode.Int32, +1);
            TestArithmeticSuccess("++1", TypeCode.Int32, +(+1));
            TestArithmeticSuccess("+++1", TypeCode.Int32, +(+(+1)));
            TestArithmeticSuccess("+1u", TypeCode.UInt32, +1u);
            TestArithmeticSuccess("++1u", TypeCode.UInt32, +(+1u));
            TestArithmeticSuccess("+++1u", TypeCode.UInt32, +(+(+1u)));
            TestArithmeticSuccess("+1l", TypeCode.Int64, +1L);
            TestArithmeticSuccess("++1l", TypeCode.Int64, +(+1L));
            TestArithmeticSuccess("+++1l", TypeCode.Int64, +(+(+1L)));
            TestArithmeticSuccess("+1ul", TypeCode.UInt64, +1ul);
            TestArithmeticSuccess("++1ul", TypeCode.UInt64, +(+1ul));
            TestArithmeticSuccess("+++1ul", TypeCode.UInt64, +(+(+1ul)));
            TestArithmeticSuccess("+1f", TypeCode.Single, +1f);
            TestArithmeticSuccess("++1f", TypeCode.Single, +(+1f));
            TestArithmeticSuccess("+++1f", TypeCode.Single, +(+(+1f)));
            TestArithmeticSuccess("+1d", TypeCode.Double, +1d);
            TestArithmeticSuccess("++1d", TypeCode.Double, +(+1d));
            TestArithmeticSuccess("+++1d", TypeCode.Double, +(+(+1d)));
            TestArithmeticSuccess("+1m", TypeCode.Decimal, +1m);
            TestArithmeticSuccess("++1m", TypeCode.Decimal, +(+1m));
            TestArithmeticSuccess("+++1m", TypeCode.Decimal, +(+(+1m)));
            TestArithmeticSuccess("-1", TypeCode.Int32, -1);
            TestArithmeticSuccess("--1", TypeCode.Int32, -(-1));
            TestArithmeticSuccess("---1", TypeCode.Int32, -(-(-1)));
            TestArithmeticSuccess("-1u", TypeCode.Int64, -1u);
            TestArithmeticSuccess("--1u", TypeCode.Int64, -(-1u));
            TestArithmeticSuccess("---1u", TypeCode.Int64, -(-(-1u)));
            TestArithmeticSuccess("-1l", TypeCode.Int64, -1L);
            TestArithmeticSuccess("--1l", TypeCode.Int64, -(-1L));
            TestArithmeticSuccess("---1l", TypeCode.Int64, -(-(-1L)));
            TestArithmeticSuccess("-1f", TypeCode.Single, -1f);
            TestArithmeticSuccess("--1f", TypeCode.Single, -(-1f));
            TestArithmeticSuccess("---1f", TypeCode.Single, -(-(-1f)));
            TestArithmeticSuccess("-1d", TypeCode.Double, -1d);
            TestArithmeticSuccess("--1d", TypeCode.Double, -(-1d));
            TestArithmeticSuccess("---1d", TypeCode.Double, -(-(-1d)));
            TestArithmeticSuccess("-1m", TypeCode.Decimal, -1m);
            TestArithmeticSuccess("--1m", TypeCode.Decimal, -(-1m));
            TestArithmeticSuccess("---1m", TypeCode.Decimal, -(-(-1m)));

            // Unary operations (negative tests)
            TestArithmeticFailedParseError("+true", 1);
            TestArithmeticFailedParseError("-true", 1);
            TestArithmeticFailedParseError("~true", 0);
            TestArithmeticFailedParseError("+false", 1);
            TestArithmeticFailedParseError("-false", 1);
            TestArithmeticFailedParseError("!true", 1);
            TestArithmeticFailedParseError("!1", 1);
            TestArithmeticFailedParseError("!1u", 1);
            TestArithmeticFailedParseError("!1l", 1);
            TestArithmeticFailedParseError("!1ul", 1);
            TestArithmeticFailedParseError("!1f", 1);
            TestArithmeticFailedParseError("!1d", 1);
            TestArithmeticFailedParseError("!1m", 1);
            TestArithmeticFailedParseError("~false", 0);
            TestArithmeticFailedParseError("~1", 0);
            TestArithmeticFailedParseError("~1u", 0);
            TestArithmeticFailedParseError("~1l", 0);
            TestArithmeticFailedParseError("~1ul", 0);
            TestArithmeticFailedParseError("~1f", 0);
            TestArithmeticFailedParseError("~1d", 0);
            TestArithmeticFailedParseError("~1m", 0);
        }

        public static void TestBinaryTimesDivide()
        {
            // Binary *,/ operations (positive tests)
            TestArithmeticSuccess("1*2", TypeCode.Int32, 1 * 2);
            TestArithmeticSuccess("1*2u", TypeCode.UInt32, 1 * 2u);
            TestArithmeticSuccess("1u*2u", TypeCode.UInt32, 1u * 2u);
            TestArithmeticSuccess("1*2l", TypeCode.Int64, 1 * 2L);
            TestArithmeticSuccess("1u*2l", TypeCode.Int64, 1u * 2L);
            TestArithmeticSuccess("1ul*2l", TypeCode.UInt64, 1ul * 2L);
            TestArithmeticSuccess("1l*2l", TypeCode.Int64, 1L * 2L);
            TestArithmeticSuccess("1*2ul", TypeCode.UInt64, 1 * 2ul);
            TestArithmeticSuccess("1u*2ul", TypeCode.UInt64, 1u * 2ul);
            TestArithmeticSuccess("1l*2ul", TypeCode.UInt64, 1L * 2ul);
            TestArithmeticSuccess("1ul*2ul", TypeCode.UInt64, 1ul * 2ul);
            TestArithmeticSuccess("1f*2f", TypeCode.Single, 1f * 2f);
            TestArithmeticSuccess("1f*2d", TypeCode.Double, 1f * 2d);
            TestArithmeticSuccess("1d*2d", TypeCode.Double, 1d * 2d);
            TestArithmeticSuccess("1m*2m", TypeCode.Decimal, 1m * 2m);
            TestArithmeticSuccess("1*2m", TypeCode.Decimal, 1 * 2m);
            TestArithmeticSuccess("1u*2m", TypeCode.Decimal, 1u * 2m);
            TestArithmeticSuccess("1l*2m", TypeCode.Decimal, 1L * 2m);
            TestArithmeticSuccess("1ul*2m", TypeCode.Decimal, 1ul*2m);
            TestArithmeticSuccess("1/2", TypeCode.Int32, 1 / 2);
            TestArithmeticSuccess("1/2u", TypeCode.UInt32, 1 / 2u);
            TestArithmeticSuccess("1u/2u", TypeCode.UInt32, 1u / 2u);
            TestArithmeticSuccess("1/2l", TypeCode.Int64, 1 / 2L);
            TestArithmeticSuccess("1u/2l", TypeCode.Int64, 1u / 2L);
            TestArithmeticSuccess("1ul/2l", TypeCode.UInt64, 1ul / 2L);
            TestArithmeticSuccess("1l/2l", TypeCode.Int64, 1L / 2L);
            TestArithmeticSuccess("1/2ul", TypeCode.UInt64, 1 / 2ul);
            TestArithmeticSuccess("1u/2ul", TypeCode.UInt64, 1u / 2ul);
            TestArithmeticSuccess("1l/2ul", TypeCode.UInt64, 1L / 2ul);
            TestArithmeticSuccess("1ul/2ul", TypeCode.UInt64, 1ul / 2ul);
            TestArithmeticSuccess("1f/2f", TypeCode.Single, 1f / 2f);
            TestArithmeticSuccess("1f/2d", TypeCode.Double, 1f / 2d);
            TestArithmeticSuccess("1d/2d", TypeCode.Double, 1d / 2d);
            TestArithmeticSuccess("1m/2m", TypeCode.Decimal, 1m / 2m);
            TestArithmeticSuccess("1/2m", TypeCode.Decimal, 1 / 2m);
            TestArithmeticSuccess("1u/2m", TypeCode.Decimal, 1u / 2m);
            TestArithmeticSuccess("1l/2m", TypeCode.Decimal, 1L / 2m);
            TestArithmeticSuccess("1ul/2m", TypeCode.Decimal, 1ul / 2m);

            // Binary *,/ operations (negative tests)
            TestArithmeticFailedParseError("1%2", 1);
            TestArithmeticFailedParseError("1*true", 2);
            TestArithmeticFailedParseError("1/true", 2);
            TestArithmeticFailedError("1f*2m");
            TestArithmeticFailedError("1d*2m");
            TestArithmeticFailedError("1f/2m");
            TestArithmeticFailedError("1d/2m");
        }

        public static void TestBinaryPlusMinus()
        {
            // Binary +,- operations (positive tests)
            TestArithmeticSuccess("1+2", TypeCode.Int32, 1 + 2);
            TestArithmeticSuccess("1+2u", TypeCode.UInt32, 1 + 2u);
            TestArithmeticSuccess("1u+2u", TypeCode.UInt32, 1u + 2u);
            TestArithmeticSuccess("1+2l", TypeCode.Int64, 1 + 2L);
            TestArithmeticSuccess("1u+2l", TypeCode.Int64, 1u + 2L);
            TestArithmeticSuccess("1ul+2l", TypeCode.UInt64, 1ul + 2L);
            TestArithmeticSuccess("1l+2l", TypeCode.Int64, 1L + 2L);
            TestArithmeticSuccess("1+2ul", TypeCode.UInt64, 1 + 2ul);
            TestArithmeticSuccess("1u+2ul", TypeCode.UInt64, 1u + 2ul);
            TestArithmeticSuccess("1l+2ul", TypeCode.UInt64, 1L + 2ul);
            TestArithmeticSuccess("1ul+2ul", TypeCode.UInt64, 1ul + 2ul);
            TestArithmeticSuccess("1f+2f", TypeCode.Single, 1f + 2f);
            TestArithmeticSuccess("1f+2d", TypeCode.Double, 1f + 2d);
            TestArithmeticSuccess("1d+2d", TypeCode.Double, 1d + 2d);
            TestArithmeticSuccess("1m+2m", TypeCode.Decimal, 1m + 2m);
            TestArithmeticSuccess("1+2m", TypeCode.Decimal, 1 + 2m);
            TestArithmeticSuccess("1u+2m", TypeCode.Decimal, 1u + 2m);
            TestArithmeticSuccess("1l+2m", TypeCode.Decimal, 1L + 2m);
            TestArithmeticSuccess("1ul+2m", TypeCode.Decimal, 1ul + 2m);
            TestArithmeticSuccess("1-2", TypeCode.Int32, 1 - 2);
            TestArithmeticSuccess("2-1u", TypeCode.UInt32, 2 - 1u);
            TestArithmeticSuccess("2u-1u", TypeCode.UInt32, 2u - 1u);
            TestArithmeticSuccess("1-2l", TypeCode.Int64, 1 - 2L);
            TestArithmeticSuccess("1u-2l", TypeCode.Int64, 1u - 2L);
            TestArithmeticSuccess("2ul-1l", TypeCode.UInt64, 2ul - 1L);
            TestArithmeticSuccess("1l-2l", TypeCode.Int64, 1L - 2L);
            TestArithmeticSuccess("2-1ul", TypeCode.UInt64, 2 - 1ul);
            TestArithmeticSuccess("2u-1ul", TypeCode.UInt64, 2u - 1ul);
            TestArithmeticSuccess("2l-1ul", TypeCode.UInt64, 2L - 1ul);
            TestArithmeticSuccess("2ul-1ul", TypeCode.UInt64, 2ul - 1ul);
            TestArithmeticSuccess("1f-2f", TypeCode.Single, 1f - 2f);
            TestArithmeticSuccess("1f-2d", TypeCode.Double, 1f - 2d);
            TestArithmeticSuccess("1d-2d", TypeCode.Double, 1d - 2d);
            TestArithmeticSuccess("1m-2m", TypeCode.Decimal, 1m - 2m);
            TestArithmeticSuccess("1-2m", TypeCode.Decimal, 1 - 2m);
            TestArithmeticSuccess("1u-2m", TypeCode.Decimal, 1u - 2m);
            TestArithmeticSuccess("1l-2m", TypeCode.Decimal, 1L - 2m);
            TestArithmeticSuccess("1ul-2m", TypeCode.Decimal, 1ul - 2m);

            // Binary +,- operations operations (negative tests)
            TestArithmeticFailedParseError("1+true", 2);
            TestArithmeticFailedParseError("1-true", 2);
            TestArithmeticFailedError("1f+2m");
            TestArithmeticFailedError("1d+2m");
            TestArithmeticFailedError("1f-2m");
            TestArithmeticFailedError("1d-2m");
        }

        public static void TestBinaryShift()
        {
            // Binary <<,>> operations (negative tests)
            TestArithmeticFailedParseError("1<<1", 2);
            TestArithmeticFailedParseError("1>>1", 2);
        }

        public static void TestRelational()
        {
            // Binary <=,<,>,>= operations (negative tests)
            TestArithmeticFailedParseError("1<2", 2);
            TestArithmeticFailedParseError("1<=2", 2);
            TestArithmeticFailedParseError("1>2", 2);
            TestArithmeticFailedParseError("1>=2", 2);
        }

        public static void TestEquality()
        {
            // Binary =,!= operations (negative tests)
            TestArithmeticFailedParseError("1==2", 1);
            TestArithmeticFailedParseError("1!=2", 2);
        }

        public static void TestLogical()
        {
            // Binary &,|,^ operations (negative tests)
            TestArithmeticFailedParseError("1&2", 2);
            TestArithmeticFailedParseError("1|2", 2);
            TestArithmeticFailedParseError("1^2", 1);
        }

        public static void TestConditionalLogical()
        {
            // Binary &&,|| operations (negative tests)
            TestArithmeticFailedParseError("1&&2", 2);
            TestArithmeticFailedParseError("1||2", 2);
        }

        public static void TestConditional()
        {
            // Binary ?:,?? operation (negative tests)
            TestArithmeticFailedParseError("1?1:2", 2);
            TestArithmeticFailedParseError("1??1", 2);
        }

        public static void TestPrecedence()
        {
            // Operator precedence (positive tests)
            TestArithmeticSuccess("1d+2d*3d", TypeCode.Double, 1d + 2d * 3d);
            TestArithmeticSuccess("1d+2d/3d", TypeCode.Double, 1d + 2d / 3d);
            TestArithmeticSuccess("1d-2d*3d", TypeCode.Double, 1d - 2d * 3d);
            TestArithmeticSuccess("1d-2d/3d", TypeCode.Double, 1d - 2d / 3d);
            TestArithmeticSuccess("1d*2d+3d", TypeCode.Double, 1d * 2d + 3d);
            TestArithmeticSuccess("1d/2d+3d", TypeCode.Double, 1d / 2d + 3d);
            TestArithmeticSuccess("1d*2d-3d", TypeCode.Double, 1d * 2d - 3d);
            TestArithmeticSuccess("1d/2d-3d", TypeCode.Double, 1d / 2d - 3d);
            TestArithmeticSuccess("(1+2)*(3*4)", TypeCode.Int32, (1 + 2) * (3 * 4));
            TestArithmeticSuccess("(1*2)+(3*4)", TypeCode.Int32, (1 * 2) + (3 * 4));
            TestArithmeticSuccess("(-1+-2)*(-3*-4)", TypeCode.Int32, (-1 + -2) * (-3 * -4));
            TestArithmeticSuccess("-(1*2)+-(3*4)", TypeCode.Int32, -(1 * 2) + -(3 * 4));
        }

        public static void TestArithmeticSuccess(string input, TypeCode resultCode, object resultValue)
        {
            Arithmetic arith = new Arithmetic(input);
            EvalResult result = arith.Evaluate();

            if ((resultCode != result.Type) || (Type.GetTypeCode(resultValue.GetType()) != resultCode))
                throw new ApplicationException("Arithmetic result type incorrect. Expected '" + resultCode.ToString() + "' but received '" + result.Type.ToString() + "' for input string '" + input + "'.");
            else
            {
                // Check result specific value
                switch (resultCode)
                {
                    case TypeCode.Boolean:
                        if ((Boolean)resultValue == (Boolean)result.Value)
                            return;
                        else
                            break;
                    case TypeCode.Byte:
                        if ((Byte)resultValue == (Byte)result.Value)
                            return;
                        else
                            break;
                    case TypeCode.Char:
                        if ((Char)resultValue == (Char)result.Value)
                            return;
                        else
                            break;
                    case TypeCode.DateTime:
                        if ((DateTime)resultValue == (DateTime)result.Value)
                            return;
                        else
                            break;
                    case TypeCode.DBNull:
                        if ((DBNull)resultValue == (DBNull)result.Value)
                            return;
                        else
                            break;
                    case TypeCode.Decimal:
                        if ((Decimal)resultValue == (Decimal)result.Value)
                            return;
                        else
                            break;
                    case TypeCode.Double:
                        if ((Double)resultValue == (Double)result.Value)
                            return;
                        else
                            break;
                    case TypeCode.Empty:
                        if (resultValue == null)
                            return;
                        else
                            break;
                    case TypeCode.Int16:
                        if ((Int16)resultValue == (Int16)result.Value)
                            return;
                        else
                            break;
                    case TypeCode.Int32:
                        if ((Int32)resultValue == (Int32)result.Value)
                            return;
                        else
                            break;
                    case TypeCode.Int64:
                        if ((Int64)resultValue == (Int64)result.Value)
                            return;
                        else
                            break;
                    case TypeCode.Object:
                        if (resultValue == result.Value)
                            return;
                        else
                            break;
                    case TypeCode.SByte:
                        if ((SByte)resultValue == (SByte)result.Value)
                            return;
                        else
                            break;
                    case TypeCode.Single:
                        if ((Single)resultValue == (Single)result.Value)
                            return;
                        else
                            break;
                    case TypeCode.String:
                        if ((String)resultValue == (String)result.Value)
                            return;
                        else
                            break;
                    case TypeCode.UInt16:
                        if ((UInt16)resultValue == (UInt16)result.Value)
                            return;
                        else
                            break;
                    case TypeCode.UInt32:
                        if ((UInt32)resultValue == (UInt32)result.Value)
                            return;
                        else
                            break;
                    case TypeCode.UInt64:
                        if ((UInt64)resultValue == (UInt64)result.Value)
                            return;
                        else
                            break;
                }

                throw new ApplicationException("Arithmetic result value incorrect. Expected '" + resultValue.ToString() + "' but received '" + result.Value.ToString() + "' for input string '" + input + "'.");
            }
        }

        public static void TestArithmeticFailedParseError(string input, int errorIndex)
        {
            try
            {
                Arithmetic arith = new Arithmetic(input);
                throw new ApplicationException("Arithmetic parse error expected but not found. Input string '" + input + "' and expected error at index '" + errorIndex.ToString() + "'.");
            }
            catch (ParseException pe)
            {
                if (pe.Index == errorIndex)
                    return;
                else
                    throw new ApplicationException("Arithmetic parse error at wrong index location. Input string '" + input + "' and expected error at index '" + errorIndex.ToString() + "' but found at '" + pe.Index.ToString() + "'.");
            }
        }

        public static void TestArithmeticFailedError(string input)
        {
            try
            {
                Arithmetic arith = new Arithmetic(input);
                arith.Evaluate();
            }
            catch (ParseException pe)
            {
                throw new ApplicationException("Arithmetic parse error not expected. Input string '" + input + "' and parse error at index '" + pe.Index.ToString() + "'.");
            }
            catch (Exception)
            {
                return;
            }

            throw new ApplicationException("Arithmetic error expected but not found. Input string '" + input + "'.");
        }
    }
}

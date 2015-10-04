using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComponentFactory.Quicksilver.Binding;

namespace ComponentFactory.Quicksilver.UnitTests.WPF
{
    public class UnitTestEval
    {
        public class SimpleObject
        {
            public SimpleObject()
                : this(null)
            {
            }

            public SimpleObject(SimpleObject child)
            {
                FieldChild = child;
            }

            public Byte FieldByte = 10;
            public Char FieldChar = 'a';
            public Int32 FieldInt32 = 100;
            public Int64 FieldInt64 = 100;
            public Single FieldSingle = 100f;
            public Double FieldDouble = 100d;
            public Decimal FieldDecimal = 100m;
            public String FieldString = "abc";
            public SimpleObject FieldChild;

            public Byte PropertyByte            { get { return FieldByte; } }
            public Char PropertyChar            { get { return FieldChar; } }
            public Int32 PropertyInt32          { get { return FieldInt32; } }
            public Int64 PropertyInt64          { get { return FieldInt64; } }
            public Single PropertySingle        { get { return FieldSingle; } }
            public Double PropertyDouble        { get { return FieldDouble; } }
            public Decimal PropertyDecimal      { get { return FieldDecimal; } }
            public String PropertyString        { get { return FieldString; } }
            public SimpleObject PropertyChild   { get { return FieldChild; } }

            public Byte MethodByte()                                { return FieldByte; }
            public Char MethodChar()                                { return FieldChar; }
            public Int32 MethodInt32()                              { return FieldInt32; }
            public Int64 MethodInt64()                              { return FieldInt64; }
            public Single MethodSingle()                            { return FieldSingle; }
            public Double MethodDouble()                            { return FieldDouble; }
            public Decimal MethodDecimal()                          { return FieldDecimal; }
            public String MethodString()                            { return FieldString; }
            public String MethodString(int first)                   { return first.ToString(); }
            public String MethodString(int first, int second)       { return first.ToString() + ":" + second.ToString(); }
            public String MethodString(int first, float second)     { return first.ToString() + ":" + second.ToString(); }
            public SimpleObject MethodChild()                       { return FieldChild; }

            public string this[Int32 index] { get { return index.ToString(); } }
            public string this[Int32 index, params object[] args] { get { return index.ToString() + args.Length.ToString(); } }
        }

        public static SimpleObject _simpleObject = new SimpleObject(new SimpleObject());
        public static object[] _emptyArray = new object[] { };
        public static object[] _singleArray = new object[] { 100 };
        public static object[] _manyArray = new object[] { true, 100, 100u, 100ul, 100L, 100f, 100d, 100m, 'a', "abc", null, DateTime.Now, _simpleObject };
        public static object[][] _arrayInsideArray = new object[][] { _emptyArray, _singleArray, _manyArray };
        public static int[,] _rank2Array = new int[,] { { 0, 1 }, { 2, 3 }, { 4, 5 } };
        public static float[,,] _rank3Array = new float[,,] { { { 0f, 1f }, { 2f, 3f }, { 4f, 5f } }, { { 6f, 7f }, { 8f, 9f }, { 10f, 11f } } };
        public static object[] _rankArray = new object[] { _rank2Array, _rank3Array };

        /// <summary>
        /// Perform unit tests on each of the Eval functional areas
        /// </summary>
        /// <returns></returns>
        public static bool PerformTests()
        {
            try
            {
                TestLiteral();
                TestUnary();
                TestBinaryTimesDivideRemainder();
                TestBinaryExponentIntDivConcat();
                TestBinaryPlusMinus();
                TestBinaryLogical();
                TestBinaryShift();
                TestRelational();
                TestEquality();
                TestConditionalLogical();
                TestConditional();
                TestIdentifier();
                TestArrayIndex();
                TestFieldOrProperty();
                TestMethod();
                TestPrecedence();
                TestComment();

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

        public static void TestLiteral()
        {
            // Literals (positive tests)
            TestEvalSuccessVB("Nothing", TypeCode.Object, null);
            TestEvalSuccessVB("True", TypeCode.Boolean, true);
            TestEvalSuccessVB("False", TypeCode.Boolean, false);
            TestEvalSuccessVB("1S", TypeCode.Int16, (Int16)1);
            TestEvalSuccessVB("1I", TypeCode.Int32, 1);
            TestEvalSuccessVB("1L", TypeCode.Int64, 1L);
            TestEvalSuccessVB("1", TypeCode.Int32, 1);
            TestEvalSuccessVB("500000000", TypeCode.Int32, 500000000);
            TestEvalSuccessVB("5000000000", TypeCode.Int64, 5000000000L);
            TestEvalSuccessVB("1F", TypeCode.Single, 1f);
            TestEvalSuccessVB("-1F", TypeCode.Single, -1f);
            TestEvalSuccessVB("1R", TypeCode.Double, 1d);
            TestEvalSuccessVB("-1R", TypeCode.Double, -1d);
            TestEvalSuccessVB("1E1R", TypeCode.Double, 1e1d);
            TestEvalSuccessVB("1E+1R", TypeCode.Double, 1e+1d);
            TestEvalSuccessVB("1D", TypeCode.Decimal, 1m);
            TestEvalSuccessVB("-1D", TypeCode.Decimal, -1m);
            TestEvalSuccessVB("1.0D", TypeCode.Decimal, 1.0m);
            TestEvalSuccessVB("-1.0D", TypeCode.Decimal, -1.0m);
            TestEvalSuccessVB("1E1D", TypeCode.Decimal, 1e1m);
            TestEvalSuccessVB("&H7", TypeCode.Int32, 7);
            TestEvalSuccessVB("&H17", TypeCode.Int32, 0x17);
            TestEvalSuccessVB("&H17S", TypeCode.Int16, (Int16)0x17);
            TestEvalSuccessVB("&H17I", TypeCode.Int32, 0x17);
            TestEvalSuccessVB("&H17L", TypeCode.Int64, 0x17L);
            TestEvalSuccessVB("&HABC17", TypeCode.Int32, 0xABC17);
            TestEvalSuccessVB("&HDEF17", TypeCode.Int32, 0xDEF17);
            TestEvalSuccessVB("&O7", TypeCode.Int32, 7);
            TestEvalSuccessVB("&O17", TypeCode.Int32, 15);
            TestEvalSuccessVB("&O17S", TypeCode.Int16, (Int16)15);
            TestEvalSuccessVB("&O17I", TypeCode.Int32, 15);
            TestEvalSuccessVB("&O17L", TypeCode.Int64, 15L);
            TestEvalSuccessVB("''", TypeCode.String, string.Empty);
            TestEvalSuccessVB("\"\"", TypeCode.String, string.Empty);
            TestEvalSuccessVB("'a'", TypeCode.String, "a");
            TestEvalSuccessVB("'a'c", TypeCode.Char, 'a');
            TestEvalSuccessVB("\"a\"c", TypeCode.Char, 'a');
            TestEvalSuccessVB("\"a\"", TypeCode.String, "a");
            TestEvalSuccessVB("'ab'", TypeCode.String, "ab");
            TestEvalSuccessVB("\"ab\"", TypeCode.String, "ab");
            TestEvalSuccessVB("\"a'b\"", TypeCode.String, "a'b");
            TestEvalSuccessVB("(True)", TypeCode.Boolean, true);
            TestEvalSuccessVB("((True))", TypeCode.Boolean, true);
            TestEvalSuccessVB("(((True)))", TypeCode.Boolean, true);

            TestEvalSuccessCS("null", TypeCode.Object, null);
            TestEvalSuccessCS("true", TypeCode.Boolean, true);
            TestEvalSuccessCS("false", TypeCode.Boolean, false);
            TestEvalSuccessCS("1", TypeCode.Int32, 1);
            TestEvalSuccessCS("-1", TypeCode.Int32, -1);
            TestEvalSuccessCS("1u", TypeCode.UInt32, 1u);
            TestEvalSuccessCS("1U", TypeCode.UInt32, 1U);
            TestEvalSuccessCS("1l", TypeCode.Int64, 1L);
            TestEvalSuccessCS("-1l", TypeCode.Int64, -1L);
            TestEvalSuccessCS("1L", TypeCode.Int64, 1L);
            TestEvalSuccessCS("1ul", TypeCode.UInt64, 1ul);
            TestEvalSuccessCS("1uL", TypeCode.UInt64, 1uL);
            TestEvalSuccessCS("1Ul", TypeCode.UInt64, 1Ul);
            TestEvalSuccessCS("1UL", TypeCode.UInt64, 1UL);
            TestEvalSuccessCS("1lu", TypeCode.UInt64, 1Lu);
            TestEvalSuccessCS("1Lu", TypeCode.UInt64, 1Lu);
            TestEvalSuccessCS("1lU", TypeCode.UInt64, 1LU);
            TestEvalSuccessCS("1LU", TypeCode.UInt64, 1LU);
            TestEvalSuccessCS("500000000", TypeCode.Int32, 500000000);
            TestEvalSuccessCS("5000000000", TypeCode.Int64, 5000000000L);
            TestEvalSuccessCS("1f", TypeCode.Single, 1f);
            TestEvalSuccessCS("-1f", TypeCode.Single, -1f);
            TestEvalSuccessCS("1F", TypeCode.Single, 1F);
            TestEvalSuccessCS("1e1f", TypeCode.Single, 1e1f);
            TestEvalSuccessCS("1E+1f", TypeCode.Single, 1e+1f);
            TestEvalSuccessCS("1e-1f", TypeCode.Single, 1e-1f);
            TestEvalSuccessCS("1e10f", TypeCode.Single, 1e10f);
            TestEvalSuccessCS("1E+10f", TypeCode.Single, 1e+10f);
            TestEvalSuccessCS("1e-10f", TypeCode.Single, 1e-10f);
            TestEvalSuccessCS(".1e1f", TypeCode.Single, .1e1f);
            TestEvalSuccessCS(".1E+1f", TypeCode.Single, .1e+1f);
            TestEvalSuccessCS(".1e-1f", TypeCode.Single, .1e-1f);
            TestEvalSuccessCS(".1e10f", TypeCode.Single, .1e10f);
            TestEvalSuccessCS(".1e+10f", TypeCode.Single, .1e+10f);
            TestEvalSuccessCS(".1E-10f", TypeCode.Single, .1e-10f);
            TestEvalSuccessCS("1d", TypeCode.Double, 1d);
            TestEvalSuccessCS("-1d", TypeCode.Double, -1d);
            TestEvalSuccessCS("1D", TypeCode.Double, 1D);
            TestEvalSuccessCS("1e1d", TypeCode.Double, 1e1d);
            TestEvalSuccessCS("1E+1d", TypeCode.Double, 1e+1d);
            TestEvalSuccessCS("1e-1d", TypeCode.Double, 1e-1d);
            TestEvalSuccessCS("1E10d", TypeCode.Double, 1e10d);
            TestEvalSuccessCS("1e+10d", TypeCode.Double, 1e+10d);
            TestEvalSuccessCS("1e-10d", TypeCode.Double, 1e-10d);
            TestEvalSuccessCS(".1e1d", TypeCode.Double, .1e1d);
            TestEvalSuccessCS(".1E+1d", TypeCode.Double, .1e+1d);
            TestEvalSuccessCS(".1e-1d", TypeCode.Double, .1e-1d);
            TestEvalSuccessCS(".1E10d", TypeCode.Double, .1e10d);
            TestEvalSuccessCS(".1e+10d", TypeCode.Double, .1e+10d);
            TestEvalSuccessCS(".1e-10d", TypeCode.Double, .1e-10d);
            TestEvalSuccessCS(".1", TypeCode.Double, .1d);
            TestEvalSuccessCS("1.0", TypeCode.Double, 1d);
            TestEvalSuccessCS("-1.0", TypeCode.Double, -1d);
            TestEvalSuccessCS(".1f", TypeCode.Single, .1f);
            TestEvalSuccessCS("1.0f", TypeCode.Single, 1f);
            TestEvalSuccessCS("-1.0f", TypeCode.Single, -1f);
            TestEvalSuccessCS("1m", TypeCode.Decimal, 1m);
            TestEvalSuccessCS("-1m", TypeCode.Decimal, -1m);
            TestEvalSuccessCS("1.0m", TypeCode.Decimal, 1.0m);
            TestEvalSuccessCS("-1.0m", TypeCode.Decimal, -1.0m);
            TestEvalSuccessCS("1M", TypeCode.Decimal, 1M);
            TestEvalSuccessCS("1e1m", TypeCode.Decimal, 1e1m);
            TestEvalSuccessCS("1E+1m", TypeCode.Decimal, 1e+1m);
            TestEvalSuccessCS("1e-1m", TypeCode.Decimal, 1e-1m);
            TestEvalSuccessCS("1e10m", TypeCode.Decimal, 1e10m);
            TestEvalSuccessCS("1e+10m", TypeCode.Decimal, 1e+10m);
            TestEvalSuccessCS("1e-10m", TypeCode.Decimal, 1e-10m);
            TestEvalSuccessCS(".1e1m", TypeCode.Decimal, .1e1m);
            TestEvalSuccessCS(".1E+1m", TypeCode.Decimal, .1e+1m);
            TestEvalSuccessCS(".1e-1m", TypeCode.Decimal, .1e-1m);
            TestEvalSuccessCS(".1e10m", TypeCode.Decimal, .1e10m);
            TestEvalSuccessCS(".1e+10m", TypeCode.Decimal, .1e+10m);
            TestEvalSuccessCS(".1E-10m", TypeCode.Decimal, .1e-10m);
            TestEvalSuccessCS("0x1", TypeCode.Int32, 0x1);
            TestEvalSuccessCS("''", TypeCode.String, string.Empty);
            TestEvalSuccessCS("\"\"", TypeCode.String, string.Empty);
            TestEvalSuccessCS("'\"\"'", TypeCode.String, "\"\"");
            TestEvalSuccessCS("'a'", TypeCode.Char, 'a');
            TestEvalSuccessCS("'ab'", TypeCode.String, "ab");
            TestEvalSuccessCS("\"ab\"", TypeCode.String, "ab");
            TestEvalSuccessCS("(true)", TypeCode.Boolean, true);
            TestEvalSuccessCS("((true))", TypeCode.Boolean, true);
            TestEvalSuccessCS("(((true)))", TypeCode.Boolean, true);
            TestEvalSuccessCS("(.1e+10m)", TypeCode.Decimal, .1e+10m);
            TestEvalSuccessCS("((.1e+10m))", TypeCode.Decimal, .1e+10m);
            TestEvalSuccessCS("(((.1e+10m)))", TypeCode.Decimal, .1e+10m);

            // Literals (negative tests)
            TestEvalFailedErrorVB("true");
            TestEvalFailedErrorVB("false");
            TestEvalFailedParseErrorVB("&HG", 2);
            TestEvalFailedParseErrorVB("&O8", 2);
            TestEvalFailedParseErrorVB("0x1", 1);
            TestEvalFailedParseErrorVB("1s", 1);
            TestEvalFailedParseErrorVB("1i", 1);
            TestEvalFailedParseErrorVB("1l", 1);
            TestEvalFailedParseErrorVB("1r", 1);
            TestEvalFailedParseErrorVB("1d", 1);
            TestEvalFailedParseErrorVB("1m", 1);
            TestEvalFailedParseErrorVB("1M", 1);

            TestEvalFailedErrorCS("True");
            TestEvalFailedErrorCS("False");
            TestEvalFailedParseErrorCS("&HG", 1);
            TestEvalFailedParseErrorCS("&O8", 1);
            TestEvalFailedParseErrorCS("1r", 1);
            TestEvalFailedParseErrorCS("1R", 1);
            TestEvalFailedParseErrorCS("1g", 1);
            TestEvalFailedParseErrorCS("1ll", 2);
            TestEvalFailedParseErrorCS("1ld", 2);
            TestEvalFailedParseErrorCS("1lf", 2);
            TestEvalFailedParseErrorCS("1lm", 2);
            TestEvalFailedParseErrorCS("1uu", 2);
            TestEvalFailedParseErrorCS("1ud", 2);
            TestEvalFailedParseErrorCS("1uf", 2);
            TestEvalFailedParseErrorCS("1um", 2);
            TestEvalFailedParseErrorCS("1fl", 2);
            TestEvalFailedParseErrorCS("1fu", 2);
            TestEvalFailedParseErrorCS("1fd", 2);
            TestEvalFailedParseErrorCS("1fm", 2);
            TestEvalFailedParseErrorCS("1dl", 2);
            TestEvalFailedParseErrorCS("1du", 2);
            TestEvalFailedParseErrorCS("1df", 2);
            TestEvalFailedParseErrorCS("1dm", 2);
            TestEvalFailedParseErrorCS("0x", 2);
            TestEvalFailedParseErrorCS(".0x1", 2);
            TestEvalFailedParseErrorCS("0xg", 2);
        }

        public static void TestUnary()
        {
            // Unary (C# +,-) (VB.NET +,-,Not) operations (positive tests)
            TestEvalSuccessVB("Not True", TypeCode.Boolean, false);
            TestEvalSuccessVB("Not False", TypeCode.Boolean, true);
            TestEvalSuccessVB("Not Not True", TypeCode.Boolean, true);
            TestEvalSuccessVB("Not Not False", TypeCode.Boolean, false);
            TestEvalSuccessVB("Not Not Not True", TypeCode.Boolean, false);
            TestEvalSuccessVB("Not Not Not False", TypeCode.Boolean, true);
            TestEvalSuccessVB("Not Not Not True", TypeCode.Boolean, false);
            TestEvalSuccessVB("Not(Not(Not False))", TypeCode.Boolean, true);
            TestEvalSuccessVB("+1", TypeCode.Int32, +1);
            TestEvalSuccessVB("++1", TypeCode.Int32, +(+1));
            TestEvalSuccessVB("+++1", TypeCode.Int32, +(+(+1)));
            TestEvalSuccessVB("+1L", TypeCode.Int64, +1L);
            TestEvalSuccessVB("++1L", TypeCode.Int64, +(+1L));
            TestEvalSuccessVB("+++1L", TypeCode.Int64, +(+(+1L)));
            TestEvalSuccessVB("+1F", TypeCode.Single, +1f);
            TestEvalSuccessVB("++1F", TypeCode.Single, +(+1f));
            TestEvalSuccessVB("+++1F", TypeCode.Single, +(+(+1f)));
            TestEvalSuccessVB("+1R", TypeCode.Double, +1d);
            TestEvalSuccessVB("++1R", TypeCode.Double, +(+1d));
            TestEvalSuccessVB("+++1R", TypeCode.Double, +(+(+1d)));
            TestEvalSuccessVB("+1D", TypeCode.Decimal, +1m);
            TestEvalSuccessVB("++1D", TypeCode.Decimal, +(+1m));
            TestEvalSuccessVB("+++1D", TypeCode.Decimal, +(+(+1m)));
            TestEvalSuccessVB("-1", TypeCode.Int32, -1);
            TestEvalSuccessVB("--1", TypeCode.Int32, -(-1));
            TestEvalSuccessVB("---1", TypeCode.Int32, -(-(-1)));
            TestEvalSuccessVB("-1L", TypeCode.Int64, -1L);
            TestEvalSuccessVB("--1L", TypeCode.Int64, -(-1L));
            TestEvalSuccessVB("---1L", TypeCode.Int64, -(-(-1L)));
            TestEvalSuccessVB("-1F", TypeCode.Single, -1f);
            TestEvalSuccessVB("--1F", TypeCode.Single, -(-1f));
            TestEvalSuccessVB("---1F", TypeCode.Single, -(-(-1f)));
            TestEvalSuccessVB("-1R", TypeCode.Double, -1d);
            TestEvalSuccessVB("--1R", TypeCode.Double, -(-1d));
            TestEvalSuccessVB("---1R", TypeCode.Double, -(-(-1d)));
            TestEvalSuccessVB("-1D", TypeCode.Decimal, -1m);
            TestEvalSuccessVB("--1D", TypeCode.Decimal, -(-1m));
            TestEvalSuccessVB("---1D", TypeCode.Decimal, -(-(-1m)));
            TestEvalSuccessVB("-+-1D", TypeCode.Decimal, -(+(-1m)));
            TestEvalSuccessVB("+-+1D", TypeCode.Decimal, +(-(+1m)));

            TestEvalSuccessCS("!true", TypeCode.Boolean, false);
            TestEvalSuccessCS("!false", TypeCode.Boolean, true);
            TestEvalSuccessCS("!!true", TypeCode.Boolean, true);
            TestEvalSuccessCS("!!false", TypeCode.Boolean, false);
            TestEvalSuccessCS("!!!true", TypeCode.Boolean, false);
            TestEvalSuccessCS("!!!false", TypeCode.Boolean, true);
            TestEvalSuccessCS("+1", TypeCode.Int32, +1);
            TestEvalSuccessCS("++1", TypeCode.Int32, +(+1));
            TestEvalSuccessCS("+++1", TypeCode.Int32, +(+(+1)));
            TestEvalSuccessCS("+1u", TypeCode.UInt32, +1u);
            TestEvalSuccessCS("++1u", TypeCode.UInt32, +(+1u));
            TestEvalSuccessCS("+++1u", TypeCode.UInt32, +(+(+1u)));
            TestEvalSuccessCS("+1l", TypeCode.Int64, +1L);
            TestEvalSuccessCS("++1l", TypeCode.Int64, +(+1L));
            TestEvalSuccessCS("+++1l", TypeCode.Int64, +(+(+1L)));
            TestEvalSuccessCS("+1ul", TypeCode.UInt64, +1ul);
            TestEvalSuccessCS("++1ul", TypeCode.UInt64, +(+1ul));
            TestEvalSuccessCS("+++1ul", TypeCode.UInt64, +(+(+1ul)));
            TestEvalSuccessCS("+1f", TypeCode.Single, +1f);
            TestEvalSuccessCS("++1f", TypeCode.Single, +(+1f));
            TestEvalSuccessCS("+++1f", TypeCode.Single, +(+(+1f)));
            TestEvalSuccessCS("+1d", TypeCode.Double, +1d);
            TestEvalSuccessCS("++1d", TypeCode.Double, +(+1d));
            TestEvalSuccessCS("+++1d", TypeCode.Double, +(+(+1d)));
            TestEvalSuccessCS("+1m", TypeCode.Decimal, +1m);
            TestEvalSuccessCS("++1m", TypeCode.Decimal, +(+1m));
            TestEvalSuccessCS("+++1m", TypeCode.Decimal, +(+(+1m)));
            TestEvalSuccessCS("-1", TypeCode.Int32, -1);
            TestEvalSuccessCS("--1", TypeCode.Int32, -(-1));
            TestEvalSuccessCS("---1", TypeCode.Int32, -(-(-1)));
            TestEvalSuccessCS("-1u", TypeCode.Int64, -1u);
            TestEvalSuccessCS("--1u", TypeCode.Int64, -(-1u));
            TestEvalSuccessCS("---1u", TypeCode.Int64, -(-(-1u)));
            TestEvalSuccessCS("-1l", TypeCode.Int64, -1L);
            TestEvalSuccessCS("--1l", TypeCode.Int64, -(-1L));
            TestEvalSuccessCS("---1l", TypeCode.Int64, -(-(-1L)));
            TestEvalSuccessCS("-1f", TypeCode.Single, -1f);
            TestEvalSuccessCS("--1f", TypeCode.Single, -(-1f));
            TestEvalSuccessCS("---1f", TypeCode.Single, -(-(-1f)));
            TestEvalSuccessCS("-1d", TypeCode.Double, -1d);
            TestEvalSuccessCS("--1d", TypeCode.Double, -(-1d));
            TestEvalSuccessCS("---1d", TypeCode.Double, -(-(-1d)));
            TestEvalSuccessCS("-1m", TypeCode.Decimal, -1m);
            TestEvalSuccessCS("--1m", TypeCode.Decimal, -(-1m));
            TestEvalSuccessCS("---1m", TypeCode.Decimal, -(-(-1m)));
            TestEvalSuccessCS("-+-1m", TypeCode.Decimal, -(+(-1m)));
            TestEvalSuccessCS("+-+1m", TypeCode.Decimal, +(-(+1m)));
            TestEvalSuccessCS("~1", TypeCode.Int32, ~1);
            TestEvalSuccessCS("~~1", TypeCode.Int32, ~~1);
            TestEvalSuccessCS("~~~1", TypeCode.Int32, ~~~1);
            TestEvalSuccessCS("~1u", TypeCode.UInt32, ~1u);
            TestEvalSuccessCS("~~1u", TypeCode.UInt32, ~~1u);
            TestEvalSuccessCS("~~~1u", TypeCode.UInt32, ~~~1u);
            TestEvalSuccessCS("~1l", TypeCode.Int64, ~1L);
            TestEvalSuccessCS("~~1l", TypeCode.Int64, ~~1L);
            TestEvalSuccessCS("~~~1l", TypeCode.Int64, ~~~1L);
            TestEvalSuccessCS("~1ul", TypeCode.UInt64, ~1ul);
            TestEvalSuccessCS("~~1ul", TypeCode.UInt64, ~~1ul);
            TestEvalSuccessCS("~~~1ul", TypeCode.UInt64, ~~~1ul);

            // Unary (C# +,-) (VB.NET +,-,Not) operations (negative tests)
            TestEvalFailedParseErrorVB("!1", 0);
            TestEvalFailedParseErrorVB("~1", 0);
            TestEvalFailedErrorVB("-'a'c");
            TestEvalFailedErrorVB("+'a'c");
            TestEvalFailedErrorVB("Not 1");
            TestEvalFailedErrorVB("Not 1S");
            TestEvalFailedErrorVB("Not 1L");
            TestEvalFailedErrorVB("Not 1I");
            TestEvalFailedErrorVB("Not 1F");
            TestEvalFailedErrorVB("Not 1D");
            TestEvalFailedErrorVB("Not 1R");
            TestEvalFailedErrorVB("Not 'a'c");
            TestEvalFailedErrorVB("Not 'abc'");

            TestEvalFailedErrorCS("+true");
            TestEvalFailedErrorCS("-true");
            TestEvalFailedErrorCS("~true");
            TestEvalFailedErrorCS("+false");
            TestEvalFailedErrorCS("-false");
            TestEvalFailedErrorCS("~false");
            TestEvalFailedErrorCS("!1");
            TestEvalFailedErrorCS("!1u");
            TestEvalFailedErrorCS("!1l");
            TestEvalFailedErrorCS("!1ul");
            TestEvalFailedErrorCS("!1f");
            TestEvalFailedErrorCS("!1d");
            TestEvalFailedErrorCS("!1m");
            TestEvalFailedErrorCS("~1f");
            TestEvalFailedErrorCS("~1d");
            TestEvalFailedErrorCS("~1m");
        }

        public static void TestBinaryTimesDivideRemainder()
        {
            // Binary (C# = *,/,%) (VB.NET *,/,Mod) operations (positive tests)
            TestEvalSuccessVB("1*2", TypeCode.Int32, 1 * 2);
            TestEvalSuccessVB("1*2L", TypeCode.Int64, 1 * 2L);
            TestEvalSuccessVB("1L*2L", TypeCode.Int64, 1L * 2L);
            TestEvalSuccessVB("1F*2F", TypeCode.Single, 1f * 2f);
            TestEvalSuccessVB("1F*2R", TypeCode.Double, 1f * 2d);
            TestEvalSuccessVB("1R*2R", TypeCode.Double, 1d * 2d);
            TestEvalSuccessVB("1D*2D", TypeCode.Decimal, 1m * 2m);
            TestEvalSuccessVB("1*2D", TypeCode.Decimal, 1 * 2m);
            TestEvalSuccessVB("1L*2D", TypeCode.Decimal, 1L * 2m);
            TestEvalSuccessVB("1/2", TypeCode.Int32, 1 / 2);
            TestEvalSuccessVB("1/2L", TypeCode.Int64, 1 / 2L);
            TestEvalSuccessVB("1L/2L", TypeCode.Int64, 1L / 2L);
            TestEvalSuccessVB("1F/2F", TypeCode.Single, 1f / 2f);
            TestEvalSuccessVB("1F/2R", TypeCode.Double, 1f / 2d);
            TestEvalSuccessVB("1R/2R", TypeCode.Double, 1d / 2d);
            TestEvalSuccessVB("1D/2D", TypeCode.Decimal, 1m / 2m);
            TestEvalSuccessVB("1/2D", TypeCode.Decimal, 1 / 2m);
            TestEvalSuccessVB("1L/2D", TypeCode.Decimal, 1L / 2m);
            TestEvalSuccessVB("1 Mod 2", TypeCode.Int32, 1 % 2);
            TestEvalSuccessVB("1 Mod 2L", TypeCode.Int64, 1 % 2L);
            TestEvalSuccessVB("1L Mod 2L", TypeCode.Int64, 1L % 2L);
            TestEvalSuccessVB("1F Mod 2F", TypeCode.Single, 1f % 2f);
            TestEvalSuccessVB("1F Mod 2R", TypeCode.Double, 1f % 2d);
            TestEvalSuccessVB("1R Mod 2R", TypeCode.Double, 1d % 2d);
            TestEvalSuccessVB("1D Mod 2D", TypeCode.Decimal, 1m % 2m);
            TestEvalSuccessVB("1 Mod 2D", TypeCode.Decimal, 1 % 2m);
            TestEvalSuccessVB("1L Mod 2D", TypeCode.Decimal, 1L % 2m);

            TestEvalSuccessCS("1*2", TypeCode.Int32, 1 * 2);
            TestEvalSuccessCS("1*2u", TypeCode.UInt32, 1 * 2u);
            TestEvalSuccessCS("1u*2u", TypeCode.UInt32, 1u * 2u);
            TestEvalSuccessCS("1*2l", TypeCode.Int64, 1 * 2L);
            TestEvalSuccessCS("1u*2l", TypeCode.Int64, 1u * 2L);
            TestEvalSuccessCS("1ul*2l", TypeCode.UInt64, 1ul * 2L);
            TestEvalSuccessCS("1l*2l", TypeCode.Int64, 1L * 2L);
            TestEvalSuccessCS("1*2ul", TypeCode.UInt64, 1 * 2ul);
            TestEvalSuccessCS("1u*2ul", TypeCode.UInt64, 1u * 2ul);
            TestEvalSuccessCS("1l*2ul", TypeCode.UInt64, 1L * 2ul);
            TestEvalSuccessCS("1ul*2ul", TypeCode.UInt64, 1ul * 2ul);
            TestEvalSuccessCS("1f*2f", TypeCode.Single, 1f * 2f);
            TestEvalSuccessCS("1f*2d", TypeCode.Double, 1f * 2d);
            TestEvalSuccessCS("1d*2d", TypeCode.Double, 1d * 2d);
            TestEvalSuccessCS("1m*2m", TypeCode.Decimal, 1m * 2m);
            TestEvalSuccessCS("1*2m", TypeCode.Decimal, 1 * 2m);
            TestEvalSuccessCS("1u*2m", TypeCode.Decimal, 1u * 2m);
            TestEvalSuccessCS("1l*2m", TypeCode.Decimal, 1L * 2m);
            TestEvalSuccessCS("1ul*2m", TypeCode.Decimal, 1ul*2m);
            TestEvalSuccessCS("1/2", TypeCode.Int32, 1 / 2);
            TestEvalSuccessCS("1/2u", TypeCode.UInt32, 1 / 2u);
            TestEvalSuccessCS("1u/2u", TypeCode.UInt32, 1u / 2u);
            TestEvalSuccessCS("1/2l", TypeCode.Int64, 1 / 2L);
            TestEvalSuccessCS("1u/2l", TypeCode.Int64, 1u / 2L);
            TestEvalSuccessCS("1ul/2l", TypeCode.UInt64, 1ul / 2L);
            TestEvalSuccessCS("1l/2l", TypeCode.Int64, 1L / 2L);
            TestEvalSuccessCS("1/2ul", TypeCode.UInt64, 1 / 2ul);
            TestEvalSuccessCS("1u/2ul", TypeCode.UInt64, 1u / 2ul);
            TestEvalSuccessCS("1l/2ul", TypeCode.UInt64, 1L / 2ul);
            TestEvalSuccessCS("1ul/2ul", TypeCode.UInt64, 1ul / 2ul);
            TestEvalSuccessCS("1f/2f", TypeCode.Single, 1f / 2f);
            TestEvalSuccessCS("1f/2d", TypeCode.Double, 1f / 2d);
            TestEvalSuccessCS("1d/2d", TypeCode.Double, 1d / 2d);
            TestEvalSuccessCS("1m/2m", TypeCode.Decimal, 1m / 2m);
            TestEvalSuccessCS("1/2m", TypeCode.Decimal, 1 / 2m);
            TestEvalSuccessCS("1u/2m", TypeCode.Decimal, 1u / 2m);
            TestEvalSuccessCS("1l/2m", TypeCode.Decimal, 1L / 2m);
            TestEvalSuccessCS("1ul/2m", TypeCode.Decimal, 1ul / 2m);
            TestEvalSuccessCS("1%2", TypeCode.Int32, 1 % 2);
            TestEvalSuccessCS("1%2u", TypeCode.UInt32, 1 % 2u);
            TestEvalSuccessCS("1u%2u", TypeCode.UInt32, 1u % 2u);
            TestEvalSuccessCS("1%2l", TypeCode.Int64, 1 % 2L);
            TestEvalSuccessCS("1u%2l", TypeCode.Int64, 1u % 2L);
            TestEvalSuccessCS("1ul%2l", TypeCode.UInt64, 1ul % 2L);
            TestEvalSuccessCS("1l%2l", TypeCode.Int64, 1L % 2L);
            TestEvalSuccessCS("1%2ul", TypeCode.UInt64, 1 % 2ul);
            TestEvalSuccessCS("1u%2ul", TypeCode.UInt64, 1u % 2ul);
            TestEvalSuccessCS("1l%2ul", TypeCode.UInt64, 1L % 2ul);
            TestEvalSuccessCS("1ul%2ul", TypeCode.UInt64, 1ul % 2ul);
            TestEvalSuccessCS("1f%2f", TypeCode.Single, 1f % 2f);
            TestEvalSuccessCS("1f%2d", TypeCode.Double, 1f % 2d);
            TestEvalSuccessCS("1d%2d", TypeCode.Double, 1d % 2d);
            TestEvalSuccessCS("1m%2m", TypeCode.Decimal, 1m % 2m);
            TestEvalSuccessCS("1%2m", TypeCode.Decimal, 1 % 2m);
            TestEvalSuccessCS("1u%2m", TypeCode.Decimal, 1u % 2m);
            TestEvalSuccessCS("1l%2m", TypeCode.Decimal, 1L % 2m);
            TestEvalSuccessCS("1ul%2m", TypeCode.Decimal, 1ul % 2m);

            // Binary (C# = *,/,%) (VB.NET *,/) operations (negative tests)
            TestEvalFailedErrorVB("1F*2D");
            TestEvalFailedErrorVB("1R*2D");
            TestEvalFailedErrorVB("1F/2D");
            TestEvalFailedErrorVB("1R/2D");
            TestEvalFailedErrorVB("'a'c*1");
            TestEvalFailedErrorVB("'a'c/1");
            TestEvalFailedErrorVB("'abc'*1");
            TestEvalFailedErrorVB("'abc'/1");
            TestEvalFailedParseErrorVB("1%True", 1);
            TestEvalFailedParseErrorVB("True%False", 4);
            TestEvalFailedParseErrorVB("True%1", 4);
            TestEvalFailedParseErrorVB("1%1", 1);
            TestEvalFailedParseErrorVB("1F%2D", 2);
            TestEvalFailedParseErrorVB("1R%2D", 2);

            TestEvalFailedErrorCS("true*false");
            TestEvalFailedErrorCS("true*1");
            TestEvalFailedErrorCS("1*true");
            TestEvalFailedErrorCS("true/false");
            TestEvalFailedErrorCS("true/1");
            TestEvalFailedErrorCS("1/true");
            TestEvalFailedErrorCS("true%false");
            TestEvalFailedErrorCS("true%1");
            TestEvalFailedErrorCS("1%true");
            TestEvalFailedErrorCS("1f*2m");
            TestEvalFailedErrorCS("1d*2m");
            TestEvalFailedErrorCS("1f/2m");
            TestEvalFailedErrorCS("1d/2m");
            TestEvalFailedErrorCS("1f%2m");
            TestEvalFailedErrorCS("1d%2m");
        }

        public static void TestBinaryExponentIntDivConcat()
        {
            // Binary (VB.NET ^,\,&) operations (positive tests)
            TestEvalSuccessVB("2^2", TypeCode.Double, Math.Pow(2, 2));
            TestEvalSuccessVB("2L^2L", TypeCode.Double, Math.Pow(2L, 2L));
            TestEvalSuccessVB("2F^2F", TypeCode.Double, Math.Pow(2f, 2f));
            TestEvalSuccessVB("2R^2R", TypeCode.Double, Math.Pow(2d, 2d));
            TestEvalSuccessVB("2^3^4", TypeCode.Double, Math.Pow(Math.Pow(2, 3), 4));
            TestEvalSuccessVB(@"5\2", TypeCode.Int32, 5 / 2);
            TestEvalSuccessVB(@"5L\2L", TypeCode.Int64, 5L / 2L);
            TestEvalSuccessVB(@"5F\2F", TypeCode.Single, (float)Math.Floor(5f / 2f));
            TestEvalSuccessVB(@"5R\2R", TypeCode.Double, Math.Floor(5d / 2d));
            TestEvalSuccessVB(@"5D\2D", TypeCode.Decimal, (decimal)Math.Floor(5d / 2d));
            TestEvalSuccessVB("'abc'&'def'", TypeCode.String, "abcdef");
            TestEvalSuccessVB("1&2", TypeCode.String, "12");
            TestEvalSuccessVB("1L&2L", TypeCode.String, "12");
            TestEvalSuccessVB("1F&2F", TypeCode.String, "12");
            TestEvalSuccessVB("1R&2R", TypeCode.String, "12");
            TestEvalSuccessVB("'a'c&2", TypeCode.String, "a2");
            TestEvalSuccessVB("1&'a'c", TypeCode.String, "1a");
            TestEvalSuccessVB("Nothing&1", TypeCode.String, "1");
            TestEvalSuccessVB("Nothing&Nothing", TypeCode.String, "");

            // Binary (VB.NET ^,\,&) operations (negative tests)
            TestEvalFailedErrorVB("'a'c^2");
            TestEvalFailedErrorVB("2^'a'c");
            TestEvalFailedErrorVB("'abc'^2");
            TestEvalFailedErrorVB("2^'abc'");
            TestEvalFailedErrorVB(@"'a'c\2");
            TestEvalFailedErrorVB(@"'abc'\2");
            TestEvalFailedErrorVB(@"2\'abc'");
            TestEvalFailedErrorVB(@"2\'c'c");
        }

        public static void TestBinaryPlusMinus()
        {
            // Binary (C# +,-) (VB.NET +,-) operations (positive tests)
            TestEvalSuccessVB("1+2", TypeCode.Int32, 1 + 2);
            TestEvalSuccessVB("1+2L", TypeCode.Int64, 1 + 2L);
            TestEvalSuccessVB("1L+2L", TypeCode.Int64, 1L + 2L);
            TestEvalSuccessVB("1F+2F", TypeCode.Single, 1f + 2f);
            TestEvalSuccessVB("1F+2R", TypeCode.Double, 1f + 2d);
            TestEvalSuccessVB("1R+2R", TypeCode.Double, 1d + 2d);
            TestEvalSuccessVB("1D+2D", TypeCode.Decimal, 1m + 2m);
            TestEvalSuccessVB("1+2D", TypeCode.Decimal, 1 + 2m);
            TestEvalSuccessVB("1L+2D", TypeCode.Decimal, 1L + 2m);
            TestEvalSuccessVB("'a'c+'b'c", TypeCode.String, "ab");
            TestEvalSuccessVB("'a'c+'bc'", TypeCode.String, "abc");
            TestEvalSuccessVB("'ab'+'c'c", TypeCode.String, "abc");
            TestEvalSuccessVB("'aa'+'bb'", TypeCode.String, "aa" + "bb");
            TestEvalSuccessVB("\"aa\"+\"bb\"", TypeCode.String, "aa" + "bb");
            TestEvalSuccessVB("1-2", TypeCode.Int32, 1 - 2);
            TestEvalSuccessVB("1-2L", TypeCode.Int64, 1 - 2L);
            TestEvalSuccessVB("1L-2L", TypeCode.Int64, 1L - 2L);
            TestEvalSuccessVB("1F-2F", TypeCode.Single, 1f - 2f);
            TestEvalSuccessVB("1F-2R", TypeCode.Double, 1f - 2d);
            TestEvalSuccessVB("1R-2R", TypeCode.Double, 1d - 2d);
            TestEvalSuccessVB("1D-2D", TypeCode.Decimal, 1m - 2m);
            TestEvalSuccessVB("1-2D", TypeCode.Decimal, 1 - 2m);
            TestEvalSuccessVB("1L-2D", TypeCode.Decimal, 1L - 2m);
            TestEvalSuccessVB("1+'2'", TypeCode.Double, (Double)3);
            TestEvalSuccessVB("1D+'2'", TypeCode.Double, (Double)3);
            TestEvalSuccessVB("1F+'2'", TypeCode.Double, (Double)3);
            TestEvalSuccessVB("1R+'2'", TypeCode.Double, (Double)3);
            TestEvalSuccessVB("'2'+1", TypeCode.Double, (Double)3);
            TestEvalSuccessVB("'2'+1D", TypeCode.Double, (Double)3);
            TestEvalSuccessVB("'2'+1F", TypeCode.Double, (Double)3);
            TestEvalSuccessVB("'2'+1R", TypeCode.Double, (Double)3);
            TestEvalSuccessVB("1F+True", TypeCode.Single, (Single)0);
            TestEvalSuccessVB("True+1F", TypeCode.Single, (Single)0);
            TestEvalSuccessVB("1R+True", TypeCode.Double, (Double)0);
            TestEvalSuccessVB("True+1R", TypeCode.Double, (Double)0);
            TestEvalSuccessVB("1D+True", TypeCode.Decimal, (Decimal)0);
            TestEvalSuccessVB("True+1D", TypeCode.Decimal, (Decimal)0);
            TestEvalSuccessVB("True+True", TypeCode.Int16, (Int16)(-2));

            TestEvalSuccessCS("1+2", TypeCode.Int32, 1 + 2);
            TestEvalSuccessCS("1+2u", TypeCode.UInt32, 1 + 2u);
            TestEvalSuccessCS("1u+2u", TypeCode.UInt32, 1u + 2u);
            TestEvalSuccessCS("1+2l", TypeCode.Int64, 1 + 2L);
            TestEvalSuccessCS("1u+2l", TypeCode.Int64, 1u + 2L);
            TestEvalSuccessCS("1ul+2l", TypeCode.UInt64, 1ul + 2L);
            TestEvalSuccessCS("1l+2l", TypeCode.Int64, 1L + 2L);
            TestEvalSuccessCS("1+2ul", TypeCode.UInt64, 1 + 2ul);
            TestEvalSuccessCS("1u+2ul", TypeCode.UInt64, 1u + 2ul);
            TestEvalSuccessCS("1l+2ul", TypeCode.UInt64, 1L + 2ul);
            TestEvalSuccessCS("1ul+2ul", TypeCode.UInt64, 1ul + 2ul);
            TestEvalSuccessCS("1f+2f", TypeCode.Single, 1f + 2f);
            TestEvalSuccessCS("1f+2d", TypeCode.Double, 1f + 2d);
            TestEvalSuccessCS("1d+2d", TypeCode.Double, 1d + 2d);
            TestEvalSuccessCS("1m+2m", TypeCode.Decimal, 1m + 2m);
            TestEvalSuccessCS("1+2m", TypeCode.Decimal, 1 + 2m);
            TestEvalSuccessCS("1u+2m", TypeCode.Decimal, 1u + 2m);
            TestEvalSuccessCS("1l+2m", TypeCode.Decimal, 1L + 2m);
            TestEvalSuccessCS("1ul+2m", TypeCode.Decimal, 1ul + 2m);
            TestEvalSuccessCS("'aa'+'bb'", TypeCode.String, "aa" + "bb");
            TestEvalSuccessCS("\"aa\"+\"bb\"", TypeCode.String, "aa" + "bb");
            TestEvalSuccessCS("\"aa\"+1", TypeCode.String, "aa" + 1);
            TestEvalSuccessCS("\"aa\"+1f", TypeCode.String, "aa" + 1f);
            TestEvalSuccessCS("\"aa\"+1m", TypeCode.String, "aa" + 1m);
            TestEvalSuccessCS("\"aa\"+true", TypeCode.String, "aa" + true);
            TestEvalSuccessCS("1+\"aa\"", TypeCode.String, 1 + "aa");
            TestEvalSuccessCS("1.5f+\"aa\"", TypeCode.String, 1.5f + "aa");
            TestEvalSuccessCS("1.7m+\"aa\"", TypeCode.String, 1.7m + "aa");
            TestEvalSuccessCS("false+\"aa\"", TypeCode.String, false + "aa");
            TestEvalSuccessCS("1-2", TypeCode.Int32, 1 - 2);
            TestEvalSuccessCS("2-1u", TypeCode.UInt32, 2 - 1u);
            TestEvalSuccessCS("2u-1u", TypeCode.UInt32, 2u - 1u);
            TestEvalSuccessCS("1-2l", TypeCode.Int64, 1 - 2L);
            TestEvalSuccessCS("1u-2l", TypeCode.Int64, 1u - 2L);
            TestEvalSuccessCS("2ul-1l", TypeCode.UInt64, 2ul - 1L);
            TestEvalSuccessCS("1l-2l", TypeCode.Int64, 1L - 2L);
            TestEvalSuccessCS("2-1ul", TypeCode.UInt64, 2 - 1ul);
            TestEvalSuccessCS("2u-1ul", TypeCode.UInt64, 2u - 1ul);
            TestEvalSuccessCS("2l-1ul", TypeCode.UInt64, 2L - 1ul);
            TestEvalSuccessCS("2ul-1ul", TypeCode.UInt64, 2ul - 1ul);
            TestEvalSuccessCS("1f-2f", TypeCode.Single, 1f - 2f);
            TestEvalSuccessCS("1f-2d", TypeCode.Double, 1f - 2d);
            TestEvalSuccessCS("1d-2d", TypeCode.Double, 1d - 2d);
            TestEvalSuccessCS("1m-2m", TypeCode.Decimal, 1m - 2m);
            TestEvalSuccessCS("1-2m", TypeCode.Decimal, 1 - 2m);
            TestEvalSuccessCS("1u-2m", TypeCode.Decimal, 1u - 2m);
            TestEvalSuccessCS("1l-2m", TypeCode.Decimal, 1L - 2m);
            TestEvalSuccessCS("1ul-2m", TypeCode.Decimal, 1ul - 2m);

            // Binary (C# +,-) (VB.NET +,-) operations (negative tests)
            TestEvalFailedErrorVB("1F+2D");
            TestEvalFailedErrorVB("1R+2D");
            TestEvalFailedErrorVB("1F-2D");
            TestEvalFailedErrorVB("1R-2D");
            TestEvalFailedErrorVB("\"aa\"+1");
            TestEvalFailedErrorVB("\"aa\"+1F");
            TestEvalFailedErrorVB("\"aa\"+1D");
            TestEvalFailedErrorVB("\"aa\"+True");
            TestEvalFailedErrorVB("1+\"aa\"");
            TestEvalFailedErrorVB("1.5F+\"aa\"");
            TestEvalFailedErrorVB("1.7D+\"aa\"");
            TestEvalFailedErrorVB("False+\"aa\"");
            TestEvalFailedErrorVB("'a'c-'b'c");

            TestEvalFailedErrorCS("true+false");
            TestEvalFailedErrorCS("true+1");
            TestEvalFailedErrorCS("1+true");
            TestEvalFailedErrorCS("true-false");
            TestEvalFailedErrorCS("true-1");
            TestEvalFailedErrorCS("1-true");
            TestEvalFailedErrorCS("1f+2m");
            TestEvalFailedErrorCS("1d+2m");
            TestEvalFailedErrorCS("1f-2m");
            TestEvalFailedErrorCS("1d-2m");
        }

        public static void TestBinaryLogical()
        {
            // Binary (C# &,|,^) (VB.NET And,Or,Xor) operations (positive tests)
            TestEvalSuccessVB("True And True", TypeCode.Boolean, true & true);
            TestEvalSuccessVB("True And False", TypeCode.Boolean, true & false);
            TestEvalSuccessVB("True Or True", TypeCode.Boolean, true | true);
            TestEvalSuccessVB("False Xor False", TypeCode.Boolean, false ^ false);
            TestEvalSuccessVB("True Or True", TypeCode.Boolean, true | true);
            TestEvalSuccessVB("&H1811 And &H1200", TypeCode.Int32, 0x1811 & 0x1200);
            TestEvalSuccessVB("&H1811 Or &H1200", TypeCode.Int32, 0x1811 | 0x1200);
            TestEvalSuccessVB("&H1811 Xor &H1200", TypeCode.Int32, 0x1811 ^ 0x1200);

            TestEvalSuccessCS("true&true", TypeCode.Boolean, true & true);
            TestEvalSuccessCS("true&false", TypeCode.Boolean, true & false);
            TestEvalSuccessCS("true|true", TypeCode.Boolean, true | true);
            TestEvalSuccessCS("false|false", TypeCode.Boolean, false | false);
            TestEvalSuccessCS("true^true", TypeCode.Boolean, true ^ true);
            TestEvalSuccessCS("true^false", TypeCode.Boolean, true ^ false);
            TestEvalSuccessCS("false^true", TypeCode.Boolean, false ^ true);
            TestEvalSuccessCS("false^false", TypeCode.Boolean, false ^ false);
            TestEvalSuccessCS("0x1811&0x1200", TypeCode.Int32, 0x1811 & 0x1200);
            TestEvalSuccessCS("0x1811|0x1200", TypeCode.Int32, 0x1811 | 0x1200);
            TestEvalSuccessCS("0x1811^0x1200", TypeCode.Int32, 0x1811 ^ 0x1200);
            TestEvalSuccessCS("6161u&4608u", TypeCode.UInt32, 6161u & 4608u);
            TestEvalSuccessCS("6161u|4608u", TypeCode.UInt32, 6161u | 4608u);
            TestEvalSuccessCS("6161u^4608u", TypeCode.UInt32, 6161u ^ 4608u);
            TestEvalSuccessCS("6161l&4608l", TypeCode.Int64, 6161L & 4608L);
            TestEvalSuccessCS("6161l|4608l", TypeCode.Int64, 6161L | 4608L);
            TestEvalSuccessCS("6161l^4608l", TypeCode.Int64, 6161L ^ 4608L);
            TestEvalSuccessCS("6161ul&4608ul", TypeCode.UInt64, 6161ul & 4608ul);
            TestEvalSuccessCS("6161ul|4608ul", TypeCode.UInt64, 6161ul | 4608ul);
            TestEvalSuccessCS("6161ul^4608ul", TypeCode.UInt64, 6161ul ^ 4608ul);

            // Binary (C# &,|,^) (VB.NET And,Or,Xor) operations (negative tests)
            TestEvalFailedErrorVB("'a'c And 'b'c");
            TestEvalFailedErrorVB("'a'c Or 'b'c");
            TestEvalFailedErrorVB("'a'c Xor 'b'c");
            TestEvalFailedParseErrorVB("True|True", 4);

            TestEvalFailedErrorCS("true&1");
            TestEvalFailedErrorCS("true&1u");
            TestEvalFailedErrorCS("true&1ul");
            TestEvalFailedErrorCS("true&1l");
            TestEvalFailedErrorCS("true&1f");
            TestEvalFailedErrorCS("true&1d");
            TestEvalFailedErrorCS("true&1m");
            TestEvalFailedErrorCS("true&'a'");
            TestEvalFailedErrorCS("true&'abc'");
            TestEvalFailedErrorCS("1f&true");
            TestEvalFailedErrorCS("1d&true");
            TestEvalFailedErrorCS("1m&true");
            TestEvalFailedErrorCS("true|1");
            TestEvalFailedErrorCS("true|1u");
            TestEvalFailedErrorCS("true|1ul");
            TestEvalFailedErrorCS("true|1l");
            TestEvalFailedErrorCS("true|1f");
            TestEvalFailedErrorCS("true|1d");
            TestEvalFailedErrorCS("true|1m");
            TestEvalFailedErrorCS("true|'a'");
            TestEvalFailedErrorCS("true|'abc'");
            TestEvalFailedErrorCS("1f|true");
            TestEvalFailedErrorCS("1d|true");
            TestEvalFailedErrorCS("1m|true");
            TestEvalFailedErrorCS("true^1");
            TestEvalFailedErrorCS("true^1u");
            TestEvalFailedErrorCS("true^1ul");
            TestEvalFailedErrorCS("true^1l");
            TestEvalFailedErrorCS("true^1f");
            TestEvalFailedErrorCS("true^1d");
            TestEvalFailedErrorCS("true^1m");
            TestEvalFailedErrorCS("true^'a'");
            TestEvalFailedErrorCS("true^'abc'");
            TestEvalFailedErrorCS("1f^true");
            TestEvalFailedErrorCS("1d^true");
            TestEvalFailedErrorCS("1m^true");
        }

        public static void TestBinaryShift()
        {
            // Binary (C# <<,>>) (VB.NET <<,>>) operations (positive tests)
            TestEvalSuccessVB("1<<1", TypeCode.Int32, 1 << 1);
            TestEvalSuccessVB("1L<<1", TypeCode.Int64, 1L << 1);
            TestEvalSuccessVB("1>>1", TypeCode.Int32, 1 >> 1);
            TestEvalSuccessVB("1L>>1", TypeCode.Int64, 1L >> 1);
            TestEvalSuccessVB("True<<1", TypeCode.Int16, (Int16)(-2));
            TestEvalSuccessVB("True>>1", TypeCode.Int16, (Int16)(-1));
            TestEvalSuccessVB("1F<<1", TypeCode.Int64, (Int64)(1 << 1));
            TestEvalSuccessVB("1F>>1", TypeCode.Int64, (Int64)(1 >> 1));
            TestEvalSuccessVB("1R<<1", TypeCode.Int64, (Int64)(1 << 1));
            TestEvalSuccessVB("1R>>1", TypeCode.Int64, (Int64)(1 >> 1));
            TestEvalSuccessVB("1D<<1", TypeCode.Int64, (Int64)(1 << 1));
            TestEvalSuccessVB("1D>>1", TypeCode.Int64, (Int64)(1 >> 1));

            TestEvalSuccessCS("1<<1", TypeCode.Int32, 1<<1);
            TestEvalSuccessCS("1u<<1", TypeCode.UInt32, 1u << 1);
            TestEvalSuccessCS("1l<<1", TypeCode.Int64, 1L << 1);
            TestEvalSuccessCS("1ul<<1", TypeCode.UInt64, 1ul << 1);
            TestEvalSuccessCS("1>>1", TypeCode.Int32, 1 >> 1);
            TestEvalSuccessCS("1u>>1", TypeCode.UInt32, 1u >> 1);
            TestEvalSuccessCS("1l>>1", TypeCode.Int64, 1L >> 1);
            TestEvalSuccessCS("1ul>>1", TypeCode.UInt64, 1ul >> 1);
            TestEvalSuccessCS("'a'<<1", TypeCode.Int32, 'a' << 1);
            TestEvalSuccessCS("'a'>>1", TypeCode.Int32, 'a' >> 1);

            // Binary (C# <<,>>) (VB.NET <<,>>) operations (negative tests)
            TestEvalFailedErrorVB("'ab'<<1");
            TestEvalFailedErrorVB("'a'>>1");
            TestEvalFailedErrorVB("'a'c>>1");

            TestEvalFailedErrorCS("true<<1");
            TestEvalFailedErrorCS("true>>1");
            TestEvalFailedErrorCS("1f<<1");
            TestEvalFailedErrorCS("1f>>1");
            TestEvalFailedErrorCS("1d<<1");
            TestEvalFailedErrorCS("1d>>1");
            TestEvalFailedErrorCS("1m<<1");
            TestEvalFailedErrorCS("1m>>1");
            TestEvalFailedErrorCS("'ab'<<1");
            TestEvalFailedErrorCS("'ab'>>1");
        }

        public static void TestRelational()
        {
            // Binary (C# <=,<,>,>=) (VB.NET <=,<,>,>=) operations (positive tests)
            TestEvalSuccessVB("1<2", TypeCode.Boolean, 1 < 2);
            TestEvalSuccessVB("1<=2", TypeCode.Boolean, 1 <= 2);
            TestEvalSuccessVB("1>2", TypeCode.Boolean, 1 > 2);
            TestEvalSuccessVB("1>=2", TypeCode.Boolean, 1 >= 2);
            TestEvalSuccessVB("2<1", TypeCode.Boolean, 2 < 1);
            TestEvalSuccessVB("2<=1", TypeCode.Boolean, 2 <= 1);
            TestEvalSuccessVB("2>1", TypeCode.Boolean, 2 > 1);
            TestEvalSuccessVB("2>=1", TypeCode.Boolean, 2 >= 1);
            TestEvalSuccessVB("1L<2L", TypeCode.Boolean, 1L < 2L);
            TestEvalSuccessVB("1L<=2L", TypeCode.Boolean, 1L <= 2L);
            TestEvalSuccessVB("1L>2L", TypeCode.Boolean, 1L > 2L);
            TestEvalSuccessVB("1L>=2L", TypeCode.Boolean, 1L >= 2L);
            TestEvalSuccessVB("2L<1L", TypeCode.Boolean, 2L < 1L);
            TestEvalSuccessVB("2L<=1L", TypeCode.Boolean, 2L <= 1L);
            TestEvalSuccessVB("2L>1L", TypeCode.Boolean, 2L > 1L);
            TestEvalSuccessVB("2L>=1L", TypeCode.Boolean, 2L >= 1L);
            TestEvalSuccessVB("1F<2F", TypeCode.Boolean, 1f < 2f);
            TestEvalSuccessVB("1F<=2F", TypeCode.Boolean, 1f <= 2f);
            TestEvalSuccessVB("1F>2F", TypeCode.Boolean, 1f > 2f);
            TestEvalSuccessVB("1F>=2F", TypeCode.Boolean, 1f >= 2f);
            TestEvalSuccessVB("2F<1F", TypeCode.Boolean, 2f < 1f);
            TestEvalSuccessVB("2F<=1F", TypeCode.Boolean, 2f <= 1f);
            TestEvalSuccessVB("2F>1F", TypeCode.Boolean, 2f > 1f);
            TestEvalSuccessVB("2F>=1F", TypeCode.Boolean, 2f >= 1f);
            TestEvalSuccessVB("1R<2R", TypeCode.Boolean, 1d < 2d);
            TestEvalSuccessVB("1R<=2R", TypeCode.Boolean, 1d <= 2d);
            TestEvalSuccessVB("1R>2R", TypeCode.Boolean, 1d > 2d);
            TestEvalSuccessVB("1R>=2R", TypeCode.Boolean, 1d >= 2d);
            TestEvalSuccessVB("2R<1R", TypeCode.Boolean, 2d < 1d);
            TestEvalSuccessVB("2R<=1R", TypeCode.Boolean, 2d <= 1d);
            TestEvalSuccessVB("2R>1R", TypeCode.Boolean, 2d > 1d);
            TestEvalSuccessVB("2R>=1R", TypeCode.Boolean, 2d >= 1d);
            TestEvalSuccessVB("1D<2D", TypeCode.Boolean, 1m < 2m);
            TestEvalSuccessVB("1D<=2D", TypeCode.Boolean, 1m <= 2m);
            TestEvalSuccessVB("1D>2D", TypeCode.Boolean, 1m > 2m);
            TestEvalSuccessVB("1D>=2D", TypeCode.Boolean, 1m >= 2m);
            TestEvalSuccessVB("2D<1D", TypeCode.Boolean, 2m < 1m);
            TestEvalSuccessVB("2D<=1D", TypeCode.Boolean, 2m <= 1m);
            TestEvalSuccessVB("2D>1D", TypeCode.Boolean, 2m > 1m);
            TestEvalSuccessVB("2D>=1D", TypeCode.Boolean, 2m >= 1m);
            TestEvalSuccessVB("1<=2D", TypeCode.Boolean, 1 <= 2m);
            TestEvalSuccessVB("1L<=2D", TypeCode.Boolean, 1L <= 2m);
            TestEvalSuccessVB("1<2D", TypeCode.Boolean, 1 < 2m);
            TestEvalSuccessVB("1L<2D", TypeCode.Boolean, 1L < 2m);
            TestEvalSuccessVB("1L>=2D", TypeCode.Boolean, 1L >= 2m);
            TestEvalSuccessVB("1>2D", TypeCode.Boolean, 1 > 2m);
            TestEvalSuccessVB("1L>2D", TypeCode.Boolean, 1L > 2m);
            TestEvalSuccessVB("True<=True", TypeCode.Boolean, true);
            TestEvalSuccessVB("True<True", TypeCode.Boolean, false);
            TestEvalSuccessVB("True>True", TypeCode.Boolean, false);
            TestEvalSuccessVB("True>=True", TypeCode.Boolean, true);
            TestEvalSuccessVB("'a'c<'a'c", TypeCode.Boolean, false);
            TestEvalSuccessVB("'a'c<='a'c", TypeCode.Boolean, true);
            TestEvalSuccessVB("'a'c>'a'c", TypeCode.Boolean, false);

            TestEvalSuccessCS("1<2", TypeCode.Boolean, 1 < 2);
            TestEvalSuccessCS("1<=2", TypeCode.Boolean, 1 <= 2);
            TestEvalSuccessCS("1>2", TypeCode.Boolean, 1 > 2);
            TestEvalSuccessCS("1>=2", TypeCode.Boolean, 1 >= 2);
            TestEvalSuccessCS("2<1", TypeCode.Boolean, 2 < 1);
            TestEvalSuccessCS("2<=1", TypeCode.Boolean, 2 <= 1);
            TestEvalSuccessCS("2>1", TypeCode.Boolean, 2 > 1);
            TestEvalSuccessCS("2>=1", TypeCode.Boolean, 2 >= 1);
            TestEvalSuccessCS("1u<2u", TypeCode.Boolean, 1u < 2u);
            TestEvalSuccessCS("1u<=2u", TypeCode.Boolean, 1u <= 2u);
            TestEvalSuccessCS("1u>2u", TypeCode.Boolean, 1u > 2u);
            TestEvalSuccessCS("1u>=2u", TypeCode.Boolean, 1u >= 2u);
            TestEvalSuccessCS("2u<1u", TypeCode.Boolean, 2u < 1u);
            TestEvalSuccessCS("2u<=1u", TypeCode.Boolean, 2u <= 1u);
            TestEvalSuccessCS("2u>1u", TypeCode.Boolean, 2u > 1u);
            TestEvalSuccessCS("2u>=1u", TypeCode.Boolean, 2u >= 1u);
            TestEvalSuccessCS("1l<2l", TypeCode.Boolean, 1L < 2L);
            TestEvalSuccessCS("1l<=2l", TypeCode.Boolean, 1L <= 2L);
            TestEvalSuccessCS("1l>2l", TypeCode.Boolean, 1L > 2L);
            TestEvalSuccessCS("1l>=2l", TypeCode.Boolean, 1L >= 2L);
            TestEvalSuccessCS("2l<1l", TypeCode.Boolean, 2L < 1L);
            TestEvalSuccessCS("2l<=1l", TypeCode.Boolean, 2L <= 1L);
            TestEvalSuccessCS("2l>1l", TypeCode.Boolean, 2L > 1L);
            TestEvalSuccessCS("2l>=1l", TypeCode.Boolean, 2L >= 1L);
            TestEvalSuccessCS("1ul<2ul", TypeCode.Boolean, 1ul < 2ul);
            TestEvalSuccessCS("1ul<=2ul", TypeCode.Boolean, 1ul <= 2ul);
            TestEvalSuccessCS("1ul>2ul", TypeCode.Boolean, 1ul > 2ul);
            TestEvalSuccessCS("1ul>=2ul", TypeCode.Boolean, 1ul >= 2ul);
            TestEvalSuccessCS("2ul<1ul", TypeCode.Boolean, 2ul < 1ul);
            TestEvalSuccessCS("2ul<=1ul", TypeCode.Boolean, 2ul <= 1ul);
            TestEvalSuccessCS("2ul>1ul", TypeCode.Boolean, 2ul > 1ul);
            TestEvalSuccessCS("2ul>=1ul", TypeCode.Boolean, 2ul >= 1ul);
            TestEvalSuccessCS("1f<2f", TypeCode.Boolean, 1f < 2f);
            TestEvalSuccessCS("1f<=2f", TypeCode.Boolean, 1f <= 2f);
            TestEvalSuccessCS("1f>2f", TypeCode.Boolean, 1f > 2f);
            TestEvalSuccessCS("1f>=2f", TypeCode.Boolean, 1f >= 2f);
            TestEvalSuccessCS("2f<1f", TypeCode.Boolean, 2f < 1f);
            TestEvalSuccessCS("2f<=1f", TypeCode.Boolean, 2f <= 1f);
            TestEvalSuccessCS("2f>1f", TypeCode.Boolean, 2f > 1f);
            TestEvalSuccessCS("2f>=1f", TypeCode.Boolean, 2f >= 1f);
            TestEvalSuccessCS("1d<2d", TypeCode.Boolean, 1d < 2d);
            TestEvalSuccessCS("1d<=2d", TypeCode.Boolean, 1d <= 2d);
            TestEvalSuccessCS("1d>2d", TypeCode.Boolean, 1d > 2d);
            TestEvalSuccessCS("1d>=2d", TypeCode.Boolean, 1d >= 2d);
            TestEvalSuccessCS("2d<1d", TypeCode.Boolean, 2d < 1d);
            TestEvalSuccessCS("2d<=1d", TypeCode.Boolean, 2d <= 1d);
            TestEvalSuccessCS("2d>1d", TypeCode.Boolean, 2d > 1d);
            TestEvalSuccessCS("2d>=1d", TypeCode.Boolean, 2d >= 1d);
            TestEvalSuccessCS("1m<2m", TypeCode.Boolean, 1m < 2m);
            TestEvalSuccessCS("1m<=2m", TypeCode.Boolean, 1m <= 2m);
            TestEvalSuccessCS("1m>2m", TypeCode.Boolean, 1m > 2m);
            TestEvalSuccessCS("1m>=2m", TypeCode.Boolean, 1m >= 2m);
            TestEvalSuccessCS("2m<1m", TypeCode.Boolean, 2m < 1m);
            TestEvalSuccessCS("2m<=1m", TypeCode.Boolean, 2m <= 1m);
            TestEvalSuccessCS("2m>1m", TypeCode.Boolean, 2m > 1m);
            TestEvalSuccessCS("2m>=1m", TypeCode.Boolean, 2m >= 1m);
            TestEvalSuccessCS("1<=2m", TypeCode.Boolean, 1 <= 2m);
            TestEvalSuccessCS("1u<=2m", TypeCode.Boolean, 1u <= 2m);
            TestEvalSuccessCS("1l<=2m", TypeCode.Boolean, 1L <= 2m);
            TestEvalSuccessCS("1ul<=2m", TypeCode.Boolean, 1ul <= 2m);
            TestEvalSuccessCS("1<2m", TypeCode.Boolean, 1 < 2m);
            TestEvalSuccessCS("1u<2m", TypeCode.Boolean, 1u < 2m);
            TestEvalSuccessCS("1l<2m", TypeCode.Boolean, 1L < 2m);
            TestEvalSuccessCS("1ul<2m", TypeCode.Boolean, 1ul < 2m);
            TestEvalSuccessCS("1u>=2m", TypeCode.Boolean, 1u >= 2m);
            TestEvalSuccessCS("1l>=2m", TypeCode.Boolean, 1L >= 2m);
            TestEvalSuccessCS("1ul>=2m", TypeCode.Boolean, 1ul >= 2m);
            TestEvalSuccessCS("1>2m", TypeCode.Boolean, 1 > 2m);
            TestEvalSuccessCS("1u>2m", TypeCode.Boolean, 1u > 2m);
            TestEvalSuccessCS("1l>2m", TypeCode.Boolean, 1L > 2m);
            TestEvalSuccessCS("1ul>2m", TypeCode.Boolean, 1ul > 2m);

            // Binary (C# <=,<,>,>=) (VB.NET <=,<,>,>=) operations (negative tests)
            TestEvalFailedErrorVB("1F<=2D");
            TestEvalFailedErrorVB("1R<=2D");
            TestEvalFailedErrorVB("1F<2D");
            TestEvalFailedErrorVB("1R<2D");
            TestEvalFailedErrorVB("1F>2D");
            TestEvalFailedErrorVB("1R>2D");
            TestEvalFailedErrorVB("1F>=2D");
            TestEvalFailedErrorVB("1R>=2D");
            TestEvalFailedErrorVB("'abc'<='abc'");
            TestEvalFailedErrorVB("'abc'<'abc'");
            TestEvalFailedErrorVB("'abc'>'abc'");
            TestEvalFailedErrorVB("'abc'>='abc'");

            TestEvalFailedErrorCS("true<=true");
            TestEvalFailedErrorCS("true<true");
            TestEvalFailedErrorCS("true>true");
            TestEvalFailedErrorCS("true>=true");
            TestEvalFailedErrorCS("1f<=2m");
            TestEvalFailedErrorCS("1d<=2m");
            TestEvalFailedErrorCS("1f<2m");
            TestEvalFailedErrorCS("1d<2m");
            TestEvalFailedErrorCS("1f>2m");
            TestEvalFailedErrorCS("1d>2m");
            TestEvalFailedErrorCS("1f>=2m");
            TestEvalFailedErrorCS("1d>=2m");
            TestEvalFailedErrorCS("'abc'<='abc'");
            TestEvalFailedErrorCS("'abc'<'abc'");
            TestEvalFailedErrorCS("'abc'>'abc'");
            TestEvalFailedErrorCS("'abc'>='abc'");
        }

        public static void TestEquality()
        {
            // Binary (C# ==,!=) (VB.NET =,<>) operations (positive tests)
            TestEvalSuccessVB("Nothing=Nothing", TypeCode.Boolean, null == null);
            TestEvalSuccessVB("Nothing<>Nothing", TypeCode.Boolean, null != null);
            TestEvalSuccessVB("Me=Nothing", TypeCode.Boolean, _singleArray == null, _singleArray);
            TestEvalSuccessVB("Me<>Nothing", TypeCode.Boolean, _singleArray != null, _singleArray);
            TestEvalSuccessVB("Nothing=Me", TypeCode.Boolean, null == _singleArray, _singleArray);
            TestEvalSuccessVB("Nothing<>Me", TypeCode.Boolean, null != _singleArray, _singleArray);
            TestEvalSuccessVB("Me=Me", TypeCode.Boolean, true, _singleArray);
            TestEvalSuccessVB("Me<>Me", TypeCode.Boolean, false, _singleArray);
            TestEvalSuccessVB("Me=Me(12)", TypeCode.Boolean, _singleArray == _manyArray[12], _manyArray);
            TestEvalSuccessVB("Me<>Me(12)", TypeCode.Boolean, _singleArray != _manyArray[12], _manyArray);
            TestEvalSuccessVB("True=True", TypeCode.Boolean, true == true);
            TestEvalSuccessVB("True=False", TypeCode.Boolean, true == false);
            TestEvalSuccessVB("True<>True", TypeCode.Boolean, true != true);
            TestEvalSuccessVB("True<>False", TypeCode.Boolean, true != false);
            TestEvalSuccessVB("'abc'='abc'", TypeCode.Boolean, "abc" == "abc");
            TestEvalSuccessVB("'abc'='def'", TypeCode.Boolean, "abc" == "def");
            TestEvalSuccessVB("2='2'", TypeCode.Boolean, true);
            TestEvalSuccessVB("2D='2'", TypeCode.Boolean, true);
            TestEvalSuccessVB("2F='2'", TypeCode.Boolean, true);
            TestEvalSuccessVB("2R='2'", TypeCode.Boolean, true);
            TestEvalSuccessVB("'abc'<>'abc'", TypeCode.Boolean, "abc" != "abc");
            TestEvalSuccessVB("'abc'<>'def'", TypeCode.Boolean, "abc" != "def");
            TestEvalSuccessVB("1=1", TypeCode.Boolean, 1 == 1);
            TestEvalSuccessVB("1=2", TypeCode.Boolean, 1 == 2);
            TestEvalSuccessVB("1<>1", TypeCode.Boolean, 1 != 1);
            TestEvalSuccessVB("1<>2", TypeCode.Boolean, 1 != 2);
            TestEvalSuccessVB("1L=1L", TypeCode.Boolean, 1L == 1L);
            TestEvalSuccessVB("1L=2L", TypeCode.Boolean, 1L == 2L);
            TestEvalSuccessVB("1L<>1L", TypeCode.Boolean, 1L != 1L);
            TestEvalSuccessVB("1L<>2L", TypeCode.Boolean, 1L != 2L);
            TestEvalSuccessVB("1F=1F", TypeCode.Boolean, 1f == 1f);
            TestEvalSuccessVB("1F=2F", TypeCode.Boolean, 1f == 2f);
            TestEvalSuccessVB("1F<>1F", TypeCode.Boolean, 1f != 1f);
            TestEvalSuccessVB("1F<>2F", TypeCode.Boolean, 1f != 2f);
            TestEvalSuccessVB("1R=1R", TypeCode.Boolean, 1d == 1d);
            TestEvalSuccessVB("1R=2R", TypeCode.Boolean, 1d == 2d);
            TestEvalSuccessVB("1R<>1R", TypeCode.Boolean, 1d != 1d);
            TestEvalSuccessVB("1R<>2R", TypeCode.Boolean, 1d != 2d);
            TestEvalSuccessVB("1D=1D", TypeCode.Boolean, 1m == 1m);
            TestEvalSuccessVB("1D=2D", TypeCode.Boolean, 1m == 2m);
            TestEvalSuccessVB("1D<>1D", TypeCode.Boolean, 1m != 1m);
            TestEvalSuccessVB("1D<>2D", TypeCode.Boolean, 1m != 2m);
            TestEvalSuccessVB("2<>'2'", TypeCode.Boolean, false);
            TestEvalSuccessVB("2D<>'2'", TypeCode.Boolean, false);
            TestEvalSuccessVB("2F<>'2'", TypeCode.Boolean, false);
            TestEvalSuccessVB("2R<>'2'", TypeCode.Boolean, false);
            TestEvalSuccessVB("'a'c='a'c", TypeCode.Boolean, true);
            TestEvalSuccessVB("'a'c<>'a'c", TypeCode.Boolean, false);
            TestEvalSuccessVB("'a'c='a'", TypeCode.Boolean, true);
            TestEvalSuccessVB("'a'c<>'a'", TypeCode.Boolean, false);

            TestEvalSuccessCS("null==null", TypeCode.Boolean, null == null);
            TestEvalSuccessCS("null!=null", TypeCode.Boolean, null != null);
            TestEvalSuccessCS("this==null", TypeCode.Boolean, _singleArray == null, _singleArray);
            TestEvalSuccessCS("this!=null", TypeCode.Boolean, _singleArray != null, _singleArray);
            TestEvalSuccessCS("null==this", TypeCode.Boolean, null == _singleArray, _singleArray);
            TestEvalSuccessCS("null!=this", TypeCode.Boolean, null != _singleArray, _singleArray);
            TestEvalSuccessCS("this==this", TypeCode.Boolean, true, _singleArray);
            TestEvalSuccessCS("this!=this", TypeCode.Boolean, false, _singleArray);
            TestEvalSuccessCS("this==this[12]", TypeCode.Boolean, _singleArray == _manyArray[12], _manyArray);
            TestEvalSuccessCS("this!=this[12]", TypeCode.Boolean, _singleArray != _manyArray[12], _manyArray);
            TestEvalSuccessCS("true==true", TypeCode.Boolean, true == true);
            TestEvalSuccessCS("true==false", TypeCode.Boolean, true == false);
            TestEvalSuccessCS("true!=true", TypeCode.Boolean, true != true);
            TestEvalSuccessCS("true!=false", TypeCode.Boolean, true != false);
            TestEvalSuccessCS("'a'=='a'", TypeCode.Boolean, 'a' == 'a');
            TestEvalSuccessCS("'a'=='d'", TypeCode.Boolean, 'a' == 'd');
            TestEvalSuccessCS("'a'==1", TypeCode.Boolean, 'a' == 1);
            TestEvalSuccessCS("'a'==1f", TypeCode.Boolean, 'a' == 1f);
            TestEvalSuccessCS("'a'==1d", TypeCode.Boolean, 'a' == 1d);
            TestEvalSuccessCS("'a'==1m", TypeCode.Boolean, 'a' == 1m);
            TestEvalSuccessCS("'a'!=1", TypeCode.Boolean, 'a' != 1);
            TestEvalSuccessCS("'a'!=1f", TypeCode.Boolean, 'a' != 1f);
            TestEvalSuccessCS("'a'!=1d", TypeCode.Boolean, 'a' != 1d);
            TestEvalSuccessCS("'a'!=1m", TypeCode.Boolean, 'a' != 1m);
            TestEvalSuccessCS("'a'!='a'", TypeCode.Boolean, 'a' != 'a');
            TestEvalSuccessCS("'a'!='d'", TypeCode.Boolean, 'a' != 'd');
            TestEvalSuccessCS("'abc'=='abc'", TypeCode.Boolean, "abc" == "abc");
            TestEvalSuccessCS("'abc'=='def'", TypeCode.Boolean, "abc" == "def");
            TestEvalSuccessCS("'abc'!='abc'", TypeCode.Boolean, "abc" != "abc");
            TestEvalSuccessCS("'abc'!='def'", TypeCode.Boolean, "abc" != "def");
            TestEvalSuccessCS("1==1", TypeCode.Boolean, 1 == 1);
            TestEvalSuccessCS("1==2", TypeCode.Boolean, 1 == 2);
            TestEvalSuccessCS("1!=1", TypeCode.Boolean, 1 != 1);
            TestEvalSuccessCS("1!=2", TypeCode.Boolean, 1 != 2);
            TestEvalSuccessCS("1u==1u", TypeCode.Boolean, 1u == 1u);
            TestEvalSuccessCS("1u==2u", TypeCode.Boolean, 1u == 2u);
            TestEvalSuccessCS("1u!=1u", TypeCode.Boolean, 1u != 1u);
            TestEvalSuccessCS("1u!=2u", TypeCode.Boolean, 1u != 2u);
            TestEvalSuccessCS("1ul==1ul", TypeCode.Boolean, 1ul == 1ul);
            TestEvalSuccessCS("1ul==2ul", TypeCode.Boolean, 1ul == 2ul);
            TestEvalSuccessCS("1ul!=1ul", TypeCode.Boolean, 1ul != 1ul);
            TestEvalSuccessCS("1ul!=2ul", TypeCode.Boolean, 1ul != 2ul);
            TestEvalSuccessCS("1l==1l", TypeCode.Boolean, 1L == 1L);
            TestEvalSuccessCS("1l==2l", TypeCode.Boolean, 1L == 2L);
            TestEvalSuccessCS("1l!=1l", TypeCode.Boolean, 1L != 1L);
            TestEvalSuccessCS("1l!=2l", TypeCode.Boolean, 1L != 2L);
            TestEvalSuccessCS("1f==1f", TypeCode.Boolean, 1f == 1f);
            TestEvalSuccessCS("1f==2f", TypeCode.Boolean, 1f == 2f);
            TestEvalSuccessCS("1f!=1f", TypeCode.Boolean, 1f != 1f);
            TestEvalSuccessCS("1f!=2f", TypeCode.Boolean, 1f != 2f);
            TestEvalSuccessCS("1d==1d", TypeCode.Boolean, 1d == 1d);
            TestEvalSuccessCS("1d==2d", TypeCode.Boolean, 1d == 2d);
            TestEvalSuccessCS("1d!=1d", TypeCode.Boolean, 1d != 1d);
            TestEvalSuccessCS("1d!=2d", TypeCode.Boolean, 1d != 2d);
            TestEvalSuccessCS("1m==1m", TypeCode.Boolean, 1m == 1m);
            TestEvalSuccessCS("1m==2m", TypeCode.Boolean, 1m == 2m);
            TestEvalSuccessCS("1m!=1m", TypeCode.Boolean, 1m != 1m);
            TestEvalSuccessCS("1m!=2m", TypeCode.Boolean, 1m != 2m);
            TestEvalSuccessCS("1==1m", TypeCode.Boolean, 1 == 1m);

            // Binary (C# ==,!=) (VB.NET =,<>) operations (negative tests)
            TestEvalFailedErrorVB("Nothing=100");
            TestEvalFailedErrorVB("Nothing<>100");
            TestEvalFailedErrorVB("Me=100", _singleArray);
            TestEvalFailedErrorVB("Me<>100", _singleArray);
            TestEvalFailedErrorVB("'a'c=1");
            TestEvalFailedErrorVB("'a'c=1F");
            TestEvalFailedErrorVB("'a'c=1R");
            TestEvalFailedErrorVB("'a'c=1D");
            TestEvalFailedErrorVB("'a'c<>1");
            TestEvalFailedErrorVB("'a'c<>1F");
            TestEvalFailedErrorVB("'a'c<>1R");
            TestEvalFailedErrorVB("'a'c<>1D");
            TestEvalFailedParseErrorVB("1==1", 1);
            TestEvalFailedParseErrorVB("Nothing!=100", 7);
            TestEvalFailedParseErrorVB("Me!=100", 2);

            TestEvalFailedErrorCS("null==100");
            TestEvalFailedErrorCS("null!=100");
            TestEvalFailedErrorCS("this==100", _singleArray);
            TestEvalFailedErrorCS("this!=100", _singleArray);
            TestEvalFailedErrorCS("true==1");
            TestEvalFailedErrorCS("true==1u");
            TestEvalFailedErrorCS("true==1ul");
            TestEvalFailedErrorCS("true==1l");
            TestEvalFailedErrorCS("true==1f");
            TestEvalFailedErrorCS("true==1d");
            TestEvalFailedErrorCS("true==1m");
            TestEvalFailedErrorCS("1f==1m");
            TestEvalFailedErrorCS("1d==1m");
            TestEvalFailedErrorCS("true!=1");
            TestEvalFailedErrorCS("true!=1u");
            TestEvalFailedErrorCS("true!=1ul");
            TestEvalFailedErrorCS("true!=1l");
            TestEvalFailedErrorCS("true!=1f");
            TestEvalFailedErrorCS("true!=1d");
            TestEvalFailedErrorCS("true!=1m");
            TestEvalFailedErrorCS("1f!=1m");
            TestEvalFailedErrorCS("1d!=1m");
        }

        public static void TestConditionalLogical()
        {
            // Binary (C# &&,||) (VB.NET AndAlso,OrElse) operations (positive tests)
            TestEvalSuccessVB("True AndAlso True", TypeCode.Boolean, true && true);
            TestEvalSuccessVB("True AndAlso False", TypeCode.Boolean, true && false);
            TestEvalSuccessVB("False AndAlso True", TypeCode.Boolean, false && true);
            TestEvalSuccessVB("False AndAlso False", TypeCode.Boolean, false && false);
            TestEvalSuccessVB("True OrElse True", TypeCode.Boolean, true || true);
            TestEvalSuccessVB("True OrElse False", TypeCode.Boolean, true || false);
            TestEvalSuccessVB("False OrElse True", TypeCode.Boolean, false || true);
            TestEvalSuccessVB("False OrElse False", TypeCode.Boolean, false || false);
            TestEvalSuccessVB("False AndAlso 1", TypeCode.Boolean, false);
            TestEvalSuccessVB("False AndAlso 1L", TypeCode.Boolean, false);
            TestEvalSuccessVB("False AndAlso 1S", TypeCode.Boolean, false);
            TestEvalSuccessVB("False AndAlso 1R", TypeCode.Boolean, false);
            TestEvalSuccessVB("False AndAlso 1D", TypeCode.Boolean, false);
            TestEvalSuccessVB("False AndAlso 'a'c", TypeCode.Boolean, false);
            TestEvalSuccessVB("False AndAlso 'abc'", TypeCode.Boolean, false);
            TestEvalSuccessVB("True OrElse 1", TypeCode.Boolean, true);
            TestEvalSuccessVB("True OrElse 1L", TypeCode.Boolean, true);
            TestEvalSuccessVB("True OrElse 1S", TypeCode.Boolean, true);
            TestEvalSuccessVB("True OrElse 1R", TypeCode.Boolean, true);
            TestEvalSuccessVB("True OrElse 1D", TypeCode.Boolean, true);
            TestEvalSuccessVB("True OrElse 'a'c", TypeCode.Boolean, true);
            TestEvalSuccessVB("True OrElse 'abc'", TypeCode.Boolean, true);
            
            TestEvalSuccessCS("true&&true", TypeCode.Boolean, true && true);
            TestEvalSuccessCS("true&&false", TypeCode.Boolean, true && false);
            TestEvalSuccessCS("false&&true", TypeCode.Boolean, false && true);
            TestEvalSuccessCS("false&&false", TypeCode.Boolean, false && false);
            TestEvalSuccessCS("true||true", TypeCode.Boolean, true || true);
            TestEvalSuccessCS("true||false", TypeCode.Boolean, true || false);
            TestEvalSuccessCS("false||true", TypeCode.Boolean, false || true);
            TestEvalSuccessCS("false||false", TypeCode.Boolean, false || false);
            TestEvalSuccessCS("false&&1", TypeCode.Boolean, false);
            TestEvalSuccessCS("false&&1u", TypeCode.Boolean, false);
            TestEvalSuccessCS("false&&1l", TypeCode.Boolean, false);
            TestEvalSuccessCS("false&&1ul", TypeCode.Boolean, false);
            TestEvalSuccessCS("false&&1f", TypeCode.Boolean, false);
            TestEvalSuccessCS("false&&1d", TypeCode.Boolean, false);
            TestEvalSuccessCS("false&&1m", TypeCode.Boolean, false);
            TestEvalSuccessCS("false&&'a'", TypeCode.Boolean, false);
            TestEvalSuccessCS("false&&'abc'", TypeCode.Boolean, false);
            TestEvalSuccessCS("true||1", TypeCode.Boolean, true);
            TestEvalSuccessCS("true||1u", TypeCode.Boolean, true);
            TestEvalSuccessCS("true||1l", TypeCode.Boolean, true);
            TestEvalSuccessCS("true||1ul", TypeCode.Boolean, true);
            TestEvalSuccessCS("true||1f", TypeCode.Boolean, true);
            TestEvalSuccessCS("true||1d", TypeCode.Boolean, true);
            TestEvalSuccessCS("true||1m", TypeCode.Boolean, true);
            TestEvalSuccessCS("true||'a'", TypeCode.Boolean, true);
            TestEvalSuccessCS("true||'abc'", TypeCode.Boolean, true);

            // Binary (C# &&,||) operations (negative tests)
            TestEvalFailedErrorVB("True AndAlso 'a'c");
            TestEvalFailedErrorVB("'a'c AndAlso 'a'c");
            TestEvalFailedErrorVB("True AndAlso 'abc'");
            TestEvalFailedErrorVB("False OrElse 'a'c");
            TestEvalFailedErrorVB("'a'c OrElse 'a'c");
            TestEvalFailedErrorVB("False OrElse 'abc'");
            TestEvalFailedParseErrorVB("True&&True", 5);
            TestEvalFailedParseErrorVB("True||True", 4);

            TestEvalFailedErrorCS("true&&1");
            TestEvalFailedErrorCS("true&&1u");
            TestEvalFailedErrorCS("true&&1l");
            TestEvalFailedErrorCS("true&&1ul");
            TestEvalFailedErrorCS("true&&1f");
            TestEvalFailedErrorCS("true&&1d");
            TestEvalFailedErrorCS("true&&1m");
            TestEvalFailedErrorCS("true&&'a'");
            TestEvalFailedErrorCS("true&&'abc'");
            TestEvalFailedErrorCS("false||1");
            TestEvalFailedErrorCS("false||1u");
            TestEvalFailedErrorCS("false||1l");
            TestEvalFailedErrorCS("false||1ul");
            TestEvalFailedErrorCS("false||1f");
            TestEvalFailedErrorCS("false||1d");
            TestEvalFailedErrorCS("false||1m");
            TestEvalFailedErrorCS("false||'a'");
            TestEvalFailedErrorCS("false||'abc'");
            TestEvalFailedParseErrorCS("true AndAlso true", 5);
            TestEvalFailedParseErrorCS("true OrElse true", 5);
        }

        public static void TestConditional()
        {
            // Binary (C# ?:,??) operation (positive tests)
            TestEvalSuccessCS("this ?? null", TypeCode.Object, _simpleObject ?? null, _simpleObject);
            TestEvalSuccessCS("this ?? null ?? null", TypeCode.Object, _simpleObject ?? null ?? null, _simpleObject);
            TestEvalSuccessCS("this??null", TypeCode.Object, _simpleObject ?? null, _simpleObject);
            TestEvalSuccessCS("this??null??null", TypeCode.Object, _simpleObject ?? null ?? null, _simpleObject);
            TestEvalSuccessCS("this[10]??this", TypeCode.Object, _manyArray[10] ?? _manyArray, _manyArray);
            TestEvalSuccessCS("this[12]??this", TypeCode.Object, _manyArray[12] ?? _manyArray, _manyArray);
            TestEvalSuccessCS("null??null??this", TypeCode.Object, null ?? null ?? _simpleObject, _simpleObject);
            TestEvalSuccessCS("null??null??null??this", TypeCode.Object, null ?? null ?? null ?? _simpleObject, _simpleObject);
            TestEvalSuccessCS("null??this", TypeCode.Object, null ?? _simpleObject, _simpleObject);

            TestEvalSuccessCS("true?1:2", TypeCode.Int32, true ? 1 : 2);
            TestEvalSuccessCS("false?1:2", TypeCode.Int32, false ? 1 : 2);
            TestEvalSuccessCS("true?1u:2u", TypeCode.UInt32, true ? 1u : 2u);
            TestEvalSuccessCS("false?1u:2u", TypeCode.UInt32, false ? 1u : 2u);
            TestEvalSuccessCS("true?1l:2l", TypeCode.Int64, true ? 1L : 2L);
            TestEvalSuccessCS("false?1l:2l", TypeCode.Int64, false ? 1L : 2L);
            TestEvalSuccessCS("true?1ul:2ul", TypeCode.UInt64, true ? 1ul : 2ul);
            TestEvalSuccessCS("false?1ul:2ul", TypeCode.UInt64, false ? 1ul : 2ul);
            TestEvalSuccessCS("true?1f:2f", TypeCode.Single, true ? 1f : 2f);
            TestEvalSuccessCS("false?1f:2f", TypeCode.Single, false ? 1f : 2f);
            TestEvalSuccessCS("true?1d:2d", TypeCode.Double, true ? 1d : 2d);
            TestEvalSuccessCS("false?1d:2d", TypeCode.Double, false ? 1d : 2d);
            TestEvalSuccessCS("true?1m:2m", TypeCode.Decimal, true ? 1m : 2m);
            TestEvalSuccessCS("false?1m:2m", TypeCode.Decimal, false ? 1m : 2m);
            TestEvalSuccessCS("true?'a':'b'", TypeCode.Char, true ? 'a' : 'b');
            TestEvalSuccessCS("false?'a':'b'", TypeCode.Char, false ? 'a' : 'b');
            TestEvalSuccessCS("true?'ab':'bc'", TypeCode.String, true ? "ab" : "bc");
            TestEvalSuccessCS("false?'ab':'bc'", TypeCode.String, false ? "ab" : "bc");

            // Binary (C# ?:) operation (negative tests)
            TestEvalFailedParseErrorVB("True?1:2", 4);
            TestEvalFailedParseErrorVB("Nothing??Nothing", 7);

            TestEvalFailedErrorCS("1??null");
            TestEvalFailedErrorCS("1u??null");
            TestEvalFailedErrorCS("1l??null");
            TestEvalFailedErrorCS("1ul??null");
            TestEvalFailedErrorCS("1f??null");
            TestEvalFailedErrorCS("1d??null");
            TestEvalFailedErrorCS("1m??null");
            TestEvalFailedErrorCS("'a'??null"); ;
            TestEvalFailedErrorCS("'abc'??null"); ;
            TestEvalFailedErrorCS("null??1");
            TestEvalFailedErrorCS("null??1u");
            TestEvalFailedErrorCS("null??1l");
            TestEvalFailedErrorCS("null??1ul");
            TestEvalFailedErrorCS("null??1f");
            TestEvalFailedErrorCS("null??1d");
            TestEvalFailedErrorCS("null??1m");
            TestEvalFailedErrorCS("null??'a'"); ;
            TestEvalFailedErrorCS("null??'abc'"); ;
            TestEvalFailedErrorCS("this[9]??this");
            TestEvalFailedErrorCS("1?1:2");
            TestEvalFailedErrorCS("1u?1:2");
            TestEvalFailedErrorCS("1l?1:2");
            TestEvalFailedErrorCS("1ul?1:2");
            TestEvalFailedErrorCS("1f?1:2");
            TestEvalFailedErrorCS("1d?1:2");
            TestEvalFailedErrorCS("1m?1:2");
            TestEvalFailedErrorCS("'a'?1:2");
            TestEvalFailedErrorCS("'abc'?1:2");
        }

        public static void TestIdentifier()
        {
            // Identifiers (positive tests)
            TestEvalSuccessVB("Nothing", TypeCode.Object, null);
            TestEvalSuccessVB("Date", TypeCode.Object, typeof(DateTime));
            TestEvalSuccessVB("Short", TypeCode.Object, typeof(Int16));
            TestEvalSuccessVB("Integer", TypeCode.Object, typeof(Int32));
            TestEvalSuccessVB("Long", TypeCode.Object, typeof(Int64));
            TestEvalSuccessVB("UShort", TypeCode.Object, typeof(UInt16));
            TestEvalSuccessVB("UInteger", TypeCode.Object, typeof(UInt32));
            TestEvalSuccessVB("ULong", TypeCode.Object, typeof(UInt64));
            TestEvalSuccessVB("Me", TypeCode.Object, _emptyArray, _emptyArray);
            TestEvalSuccessVB("Me", TypeCode.Object, _singleArray, _singleArray);
            TestEvalSuccessVB("Me", TypeCode.Object, _manyArray, _manyArray);

            TestEvalSuccessCS("null", TypeCode.Object, null);
            TestEvalSuccessCS("bool", TypeCode.Object, typeof(bool));
            TestEvalSuccessCS("byte", TypeCode.Object, typeof(byte));
            TestEvalSuccessCS("sbyte", TypeCode.Object, typeof(sbyte));
            TestEvalSuccessCS("char", TypeCode.Object, typeof(char));
            TestEvalSuccessCS("int", TypeCode.Object, typeof(int));
            TestEvalSuccessCS("uint", TypeCode.Object, typeof(uint));
            TestEvalSuccessCS("long", TypeCode.Object, typeof(long));
            TestEvalSuccessCS("ulong", TypeCode.Object, typeof(ulong));
            TestEvalSuccessCS("float", TypeCode.Object, typeof(float));
            TestEvalSuccessCS("double", TypeCode.Object, typeof(double));
            TestEvalSuccessCS("decimal", TypeCode.Object, typeof(decimal));
            TestEvalSuccessCS("string", TypeCode.Object, typeof(string));
            TestEvalSuccessCS("object", TypeCode.Object, typeof(object));
            TestEvalSuccessCS("this", TypeCode.Object, _emptyArray, _emptyArray);
            TestEvalSuccessCS("this", TypeCode.Object, _singleArray, _singleArray);
            TestEvalSuccessCS("this", TypeCode.Object, _manyArray, _manyArray);

            // Identifiers (negative tests)
            TestEvalFailedErrorVB("banana");
            TestEvalFailedErrorVB("Button");

            TestEvalFailedErrorCS("banana");
            TestEvalFailedErrorCS("Button");
        }

        public static void TestArrayIndex()
        {
            // Array Index (positive tests)
            TestEvalSuccessVB("Me(0)", TypeCode.Int32, _singleArray[0], _singleArray);
            TestEvalSuccessVB("Me(0)", TypeCode.Boolean, _manyArray[0], _manyArray);
            TestEvalSuccessVB("Me(1)", TypeCode.Int32, _manyArray[1], _manyArray);
            TestEvalSuccessVB("Me(2)", TypeCode.UInt32, _manyArray[2], _manyArray);
            TestEvalSuccessVB("Me(3)", TypeCode.UInt64, _manyArray[3], _manyArray);
            TestEvalSuccessVB("Me(4)", TypeCode.Int64, _manyArray[4], _manyArray);
            TestEvalSuccessVB("Me(5)", TypeCode.Single, _manyArray[5], _manyArray);
            TestEvalSuccessVB("Me(6)", TypeCode.Double, _manyArray[6], _manyArray);
            TestEvalSuccessVB("Me(7)", TypeCode.Decimal, _manyArray[7], _manyArray);
            TestEvalSuccessVB("Me(8)", TypeCode.Char, _manyArray[8], _manyArray);
            TestEvalSuccessVB("Me(9)", TypeCode.String, _manyArray[9], _manyArray);
            TestEvalSuccessVB("Me(10)", TypeCode.Object, _manyArray[10], _manyArray);
            TestEvalSuccessVB("Me(11)", TypeCode.DateTime, _manyArray[11], _manyArray);
            TestEvalSuccessVB("Me(12)", TypeCode.Object, _manyArray[12], _manyArray);
            TestEvalSuccessVB("Me(1F)", TypeCode.Int32, _manyArray[1], _manyArray);
            TestEvalSuccessVB("Me(1D)", TypeCode.Int32, _manyArray[1], _manyArray);
            TestEvalSuccessVB("Me(1R)", TypeCode.Int32, _manyArray[1], _manyArray);

            TestEvalSuccessVB("Me(12)(5)", TypeCode.String, ((SimpleObject)_manyArray[12])[5], _manyArray);
            TestEvalSuccessVB("Me(1)(0)", TypeCode.Int32, _arrayInsideArray[1][0], _arrayInsideArray);
            TestEvalSuccessVB("Me(2)(0)", TypeCode.Boolean, _arrayInsideArray[2][0], _arrayInsideArray);
            TestEvalSuccessVB("Me(2)(1)", TypeCode.Int32, _arrayInsideArray[2][1], _arrayInsideArray);
            TestEvalSuccessVB("Me(2)(7)", TypeCode.Decimal, _arrayInsideArray[2][7], _arrayInsideArray);
            TestEvalSuccessVB("Me(2)(8)", TypeCode.Char, _arrayInsideArray[2][8], _arrayInsideArray);
            TestEvalSuccessVB("Me(2)(9)", TypeCode.String, _arrayInsideArray[2][9], _arrayInsideArray);
            TestEvalSuccessVB("Me(2)(1F)", TypeCode.Int32, _arrayInsideArray[2][1], _arrayInsideArray);
            TestEvalSuccessVB("Me(2)(1D)", TypeCode.Int32, _arrayInsideArray[2][1], _arrayInsideArray);
            TestEvalSuccessVB("Me(2)(1R)", TypeCode.Int32, _arrayInsideArray[2][1], _arrayInsideArray);
            TestEvalSuccessVB("Me(2)(12)(5)", TypeCode.String, ((SimpleObject)_arrayInsideArray[2][12])[5], _arrayInsideArray);
            TestEvalSuccessVB("Me(2)(12)(100F)", TypeCode.String, ((SimpleObject)_arrayInsideArray[2][12])[100], _arrayInsideArray);
            TestEvalSuccessVB("Me(2)(12)(100F,10)", TypeCode.String, ((SimpleObject)_arrayInsideArray[2][12])[100, 10], _arrayInsideArray);
            TestEvalSuccessVB("Me(2)(12)(100F,10,10)", TypeCode.String, ((SimpleObject)_arrayInsideArray[2][12])[100, 10, 10], _arrayInsideArray);
            TestEvalSuccessVB("Me(0)(0,0)", TypeCode.Int32, ((int[,])_rankArray[0])[0, 0], _rankArray);
            TestEvalSuccessVB("Me(0)(0,1)", TypeCode.Int32, ((int[,])_rankArray[0])[0, 1], _rankArray);
            TestEvalSuccessVB("Me(0)(1,0)", TypeCode.Int32, ((int[,])_rankArray[0])[1, 0], _rankArray);
            TestEvalSuccessVB("Me(0)(1,1)", TypeCode.Int32, ((int[,])_rankArray[0])[1, 1], _rankArray);
            TestEvalSuccessVB("Me(0)(0F,0)", TypeCode.Int32, ((int[,])_rankArray[0])[0, 0], _rankArray);
            TestEvalSuccessVB("Me(0)(0,0F)", TypeCode.Int32, ((int[,])_rankArray[0])[0, 0], _rankArray);
            TestEvalSuccessVB("Me(0)((1-1),(1-1))", TypeCode.Int32, ((int[,])_rankArray[0])[(1 - 1), (1 - 1)], _rankArray);
            TestEvalSuccessVB("Me(1)(0,0,0)", TypeCode.Single, ((float[, ,])_rankArray[1])[0, 0, 0], _rankArray);
            TestEvalSuccessVB("Me(1)(1,1,1)", TypeCode.Single, ((float[, ,])_rankArray[1])[1, 1, 1], _rankArray);

            TestEvalSuccessCS("this[0]", TypeCode.Int32, _singleArray[0], _singleArray);
            TestEvalSuccessCS("this[0]", TypeCode.Boolean, _manyArray[0], _manyArray);
            TestEvalSuccessCS("this[1]", TypeCode.Int32, _manyArray[1], _manyArray);
            TestEvalSuccessCS("this[2]", TypeCode.UInt32, _manyArray[2], _manyArray);
            TestEvalSuccessCS("this[3]", TypeCode.UInt64, _manyArray[3], _manyArray);
            TestEvalSuccessCS("this[4]", TypeCode.Int64, _manyArray[4], _manyArray);
            TestEvalSuccessCS("this[5]", TypeCode.Single, _manyArray[5], _manyArray);
            TestEvalSuccessCS("this[6]", TypeCode.Double, _manyArray[6], _manyArray);
            TestEvalSuccessCS("this[7]", TypeCode.Decimal, _manyArray[7], _manyArray);
            TestEvalSuccessCS("this[8]", TypeCode.Char, _manyArray[8], _manyArray);
            TestEvalSuccessCS("this[9]", TypeCode.String, _manyArray[9], _manyArray);
            TestEvalSuccessCS("this[10]", TypeCode.Object, _manyArray[10], _manyArray);
            TestEvalSuccessCS("this[11]", TypeCode.DateTime, _manyArray[11], _manyArray);
            TestEvalSuccessCS("this[12]", TypeCode.Object, _manyArray[12], _manyArray);
            TestEvalSuccessCS("this[12][5]", TypeCode.String, ((SimpleObject)_manyArray[12])[5], _manyArray);
            TestEvalSuccessCS("this[1][0]", TypeCode.Int32, _arrayInsideArray[1][0], _arrayInsideArray);
            TestEvalSuccessCS("this[2][0]", TypeCode.Boolean, _arrayInsideArray[2][0], _arrayInsideArray);
            TestEvalSuccessCS("this[2][1]", TypeCode.Int32, _arrayInsideArray[2][1], _arrayInsideArray);
            TestEvalSuccessCS("this[2][7]", TypeCode.Decimal, _arrayInsideArray[2][7], _arrayInsideArray);
            TestEvalSuccessCS("this[2][8]", TypeCode.Char, _arrayInsideArray[2][8], _arrayInsideArray);
            TestEvalSuccessCS("this[2][9]", TypeCode.String, _arrayInsideArray[2][9], _arrayInsideArray);
            TestEvalSuccessCS("this[2][12][5]", TypeCode.String, ((SimpleObject)_arrayInsideArray[2][12])[5], _arrayInsideArray);
            TestEvalSuccessCS("this[0][0,0]", TypeCode.Int32, ((int[,])_rankArray[0])[0, 0], _rankArray);
            TestEvalSuccessCS("this[0][0,1]", TypeCode.Int32, ((int[,])_rankArray[0])[0, 1], _rankArray);
            TestEvalSuccessCS("this[0][1,0]", TypeCode.Int32, ((int[,])_rankArray[0])[1, 0], _rankArray);
            TestEvalSuccessCS("this[0][1,1]", TypeCode.Int32, ((int[,])_rankArray[0])[1, 1], _rankArray);
            TestEvalSuccessCS("this[0][(1-1),(1-1)]", TypeCode.Int32, ((int[,])_rankArray[0])[(1 - 1), (1 - 1)], _rankArray);
            TestEvalSuccessCS("this[1][0,0,0]", TypeCode.Single, ((float[, ,])_rankArray[1])[0, 0, 0], _rankArray);
            TestEvalSuccessCS("this[1][1,1,1]", TypeCode.Single, ((float[, ,])_rankArray[1])[1, 1, 1], _rankArray);

            // Array Index (negative tests)
            TestEvalFailedErrorVB("Me(Nothing)", _manyArray);
            TestEvalFailedErrorVB("Me(True)", _manyArray);
            TestEvalFailedErrorVB("Me('a')", _manyArray);
            TestEvalFailedErrorVB("Me('abc')", _manyArray);
            TestEvalFailedErrorVB("Me(2)(Nothing)", _arrayInsideArray);
            TestEvalFailedErrorVB("Me(2)(True)", _arrayInsideArray);
            TestEvalFailedErrorVB("Me(2)('a')", _arrayInsideArray);
            TestEvalFailedErrorVB("Me(2)('abc')", _arrayInsideArray);
            TestEvalFailedErrorVB("Me(-1)(0)", _arrayInsideArray);
            TestEvalFailedErrorVB("Me(3)(0)", _arrayInsideArray);
            TestEvalFailedErrorVB("Me(2)(9)(1)", _arrayInsideArray);
            TestEvalFailedErrorVB("Me(2)(12)('abc')", _arrayInsideArray);
            TestEvalFailedErrorVB("Me(0)(0)", _rankArray);
            TestEvalFailedErrorVB("Me(0)(0,0,0)", _rankArray);
            TestEvalFailedErrorVB("Me(1)(0,0)", _rankArray);
            TestEvalFailedErrorVB("Me(1)(0,0,0,0)", _rankArray);

            TestEvalFailedErrorCS("this[null]", _manyArray);
            TestEvalFailedErrorCS("this[true]", _manyArray);
            TestEvalFailedErrorCS("this[1f]", _manyArray);
            TestEvalFailedErrorCS("this[1d]", _manyArray);
            TestEvalFailedErrorCS("this[1m]", _manyArray);
            TestEvalFailedErrorCS("this['a']", _manyArray);
            TestEvalFailedErrorCS("this['abc']", _manyArray);
            TestEvalFailedErrorCS("this[2][null]", _arrayInsideArray);
            TestEvalFailedErrorCS("this[2][true]", _arrayInsideArray);
            TestEvalFailedErrorCS("this[2][1f]", _arrayInsideArray);
            TestEvalFailedErrorCS("this[2][1d]", _arrayInsideArray);
            TestEvalFailedErrorCS("this[2][1m]", _arrayInsideArray);
            TestEvalFailedErrorCS("this[2]['a']", _arrayInsideArray);
            TestEvalFailedErrorCS("this[2]['abc']", _arrayInsideArray);
            TestEvalFailedErrorCS("this[-1][0]", _arrayInsideArray);
            TestEvalFailedErrorCS("this[3][0]", _arrayInsideArray);
            TestEvalFailedErrorCS("this[2][9][1]", _arrayInsideArray);
            TestEvalFailedErrorCS("this[2][12][100f]", _arrayInsideArray);
            TestEvalFailedErrorCS("this[2][12]['abc']", _arrayInsideArray);
            TestEvalFailedErrorCS("this[0][0]", _rankArray);
            TestEvalFailedErrorCS("this[0][0f,0]", _rankArray);
            TestEvalFailedErrorCS("this[0][0,0f]", _rankArray);
            TestEvalFailedErrorCS("this[0][0,0,0]", _rankArray);
            TestEvalFailedErrorCS("this[1][0,0]", _rankArray);
            TestEvalFailedErrorCS("this[1][0,0,0,0]", _rankArray);
        }

        public static void TestFieldOrProperty()
        {
            // Field/Property access (positive tests)
            TestEvalSuccessVB("Me.Length", TypeCode.Int32, _singleArray.Length, _singleArray);
            TestEvalSuccessVB("Me(9).Length", TypeCode.Int32, ((string)_manyArray[9]).Length, _manyArray);
            TestEvalSuccessVB("Me(12).FieldByte", TypeCode.Byte, _simpleObject.FieldByte, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChar", TypeCode.Char, _simpleObject.FieldChar, _manyArray);
            TestEvalSuccessVB("Me(12).FieldInt32", TypeCode.Int32, _simpleObject.FieldInt32, _manyArray);
            TestEvalSuccessVB("Me(12).FieldInt64", TypeCode.Int64, _simpleObject.FieldInt64, _manyArray);
            TestEvalSuccessVB("Me(12).FieldSingle", TypeCode.Single, _simpleObject.FieldSingle, _manyArray);
            TestEvalSuccessVB("Me(12).FieldDouble", TypeCode.Double, _simpleObject.FieldDouble, _manyArray);
            TestEvalSuccessVB("Me(12).FieldDecimal", TypeCode.Decimal, _simpleObject.FieldDecimal, _manyArray);
            TestEvalSuccessVB("Me(12).FieldString", TypeCode.String, _simpleObject.FieldString, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChild", TypeCode.Object, _simpleObject.FieldChild, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChild.FieldByte", TypeCode.Byte, _simpleObject.FieldChild.FieldByte, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChild.FieldChar", TypeCode.Char, _simpleObject.FieldChild.FieldChar, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChild.FieldInt32", TypeCode.Int32, _simpleObject.FieldChild.FieldInt32, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChild.FieldInt64", TypeCode.Int64, _simpleObject.FieldChild.FieldInt64, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChild.FieldSingle", TypeCode.Single, _simpleObject.FieldChild.FieldSingle, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChild.FieldDouble", TypeCode.Double, _simpleObject.FieldChild.FieldDouble, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChild.FieldDecimal", TypeCode.Decimal, _simpleObject.FieldChild.FieldDecimal, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChild.FieldString", TypeCode.String, _simpleObject.FieldChild.FieldString, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChild.FieldChild", TypeCode.Object, _simpleObject.FieldChild.FieldChild, _manyArray);
            TestEvalSuccessVB("Me(12).PropertyChar", TypeCode.Char, _simpleObject.PropertyChar, _manyArray);
            TestEvalSuccessVB("Me(12).PropertyByte", TypeCode.Byte, _simpleObject.PropertyByte, _manyArray);
            TestEvalSuccessVB("Me(12).PropertyInt32", TypeCode.Int32, _simpleObject.PropertyInt32, _manyArray);
            TestEvalSuccessVB("Me(12).PropertyInt64", TypeCode.Int64, _simpleObject.PropertyInt64, _manyArray);
            TestEvalSuccessVB("Me(12).PropertySingle", TypeCode.Single, _simpleObject.PropertySingle, _manyArray);
            TestEvalSuccessVB("Me(12).PropertyDouble", TypeCode.Double, _simpleObject.PropertyDouble, _manyArray);
            TestEvalSuccessVB("Me(12).PropertyDecimal", TypeCode.Decimal, _simpleObject.PropertyDecimal, _manyArray);
            TestEvalSuccessVB("Me(12).PropertyString", TypeCode.String, _simpleObject.PropertyString, _manyArray);
            TestEvalSuccessVB("Me(12).PropertyChild", TypeCode.Object, _simpleObject.PropertyChild, _manyArray);
            TestEvalSuccessVB("Me(12).PropertyChar", TypeCode.Char, _simpleObject.PropertyChar, _manyArray);
            TestEvalSuccessVB("Me(12).PropertyChild", TypeCode.Object, _simpleObject.PropertyChild, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChild.PropertyByte", TypeCode.Byte, _simpleObject.FieldChild.PropertyByte, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChild.PropertyInt32", TypeCode.Int32, _simpleObject.FieldChild.PropertyInt32, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChild.PropertyInt64", TypeCode.Int64, _simpleObject.FieldChild.PropertyInt64, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChild.PropertySingle", TypeCode.Single, _simpleObject.FieldChild.PropertySingle, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChild.PropertyDouble", TypeCode.Double, _simpleObject.FieldChild.PropertyDouble, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChild.PropertyDecimal", TypeCode.Decimal, _simpleObject.FieldChild.PropertyDecimal, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChild.PropertyString", TypeCode.String, _simpleObject.FieldChild.PropertyString, _manyArray);
            TestEvalSuccessVB("Me(12).FieldChild.PropertyChild", TypeCode.Object, _simpleObject.FieldChild.PropertyChild, _manyArray);

            TestEvalSuccessCS("this.Length", TypeCode.Int32, _singleArray.Length, _singleArray);
            TestEvalSuccessCS("this[9].Length", TypeCode.Int32, ((string)_manyArray[9]).Length, _manyArray);
            TestEvalSuccessCS("this[12].FieldByte", TypeCode.Byte, _simpleObject.FieldByte, _manyArray);
            TestEvalSuccessCS("this[12].FieldChar", TypeCode.Char, _simpleObject.FieldChar, _manyArray);
            TestEvalSuccessCS("this[12].FieldInt32", TypeCode.Int32, _simpleObject.FieldInt32, _manyArray);
            TestEvalSuccessCS("this[12].FieldInt64", TypeCode.Int64, _simpleObject.FieldInt64, _manyArray);
            TestEvalSuccessCS("this[12].FieldSingle", TypeCode.Single, _simpleObject.FieldSingle, _manyArray);
            TestEvalSuccessCS("this[12].FieldDouble", TypeCode.Double, _simpleObject.FieldDouble, _manyArray);
            TestEvalSuccessCS("this[12].FieldDecimal", TypeCode.Decimal, _simpleObject.FieldDecimal, _manyArray);
            TestEvalSuccessCS("this[12].FieldString", TypeCode.String, _simpleObject.FieldString, _manyArray);
            TestEvalSuccessCS("this[12].FieldChild", TypeCode.Object, _simpleObject.FieldChild, _manyArray);
            TestEvalSuccessCS("this[12].FieldChild.FieldByte", TypeCode.Byte, _simpleObject.FieldChild.FieldByte, _manyArray);
            TestEvalSuccessCS("this[12].FieldChild.FieldChar", TypeCode.Char, _simpleObject.FieldChild.FieldChar, _manyArray);
            TestEvalSuccessCS("this[12].FieldChild.FieldInt32", TypeCode.Int32, _simpleObject.FieldChild.FieldInt32, _manyArray);
            TestEvalSuccessCS("this[12].FieldChild.FieldInt64", TypeCode.Int64, _simpleObject.FieldChild.FieldInt64, _manyArray);
            TestEvalSuccessCS("this[12].FieldChild.FieldSingle", TypeCode.Single, _simpleObject.FieldChild.FieldSingle, _manyArray);
            TestEvalSuccessCS("this[12].FieldChild.FieldDouble", TypeCode.Double, _simpleObject.FieldChild.FieldDouble, _manyArray);
            TestEvalSuccessCS("this[12].FieldChild.FieldDecimal", TypeCode.Decimal, _simpleObject.FieldChild.FieldDecimal, _manyArray);
            TestEvalSuccessCS("this[12].FieldChild.FieldString", TypeCode.String, _simpleObject.FieldChild.FieldString, _manyArray);
            TestEvalSuccessCS("this[12].FieldChild.FieldChild", TypeCode.Object, _simpleObject.FieldChild.FieldChild, _manyArray);
            TestEvalSuccessCS("this[12].PropertyChar", TypeCode.Char, _simpleObject.PropertyChar, _manyArray);
            TestEvalSuccessCS("this[12].PropertyByte", TypeCode.Byte, _simpleObject.PropertyByte, _manyArray);
            TestEvalSuccessCS("this[12].PropertyInt32", TypeCode.Int32, _simpleObject.PropertyInt32, _manyArray);
            TestEvalSuccessCS("this[12].PropertyInt64", TypeCode.Int64, _simpleObject.PropertyInt64, _manyArray);
            TestEvalSuccessCS("this[12].PropertySingle", TypeCode.Single, _simpleObject.PropertySingle, _manyArray);
            TestEvalSuccessCS("this[12].PropertyDouble", TypeCode.Double, _simpleObject.PropertyDouble, _manyArray);
            TestEvalSuccessCS("this[12].PropertyDecimal", TypeCode.Decimal, _simpleObject.PropertyDecimal, _manyArray);
            TestEvalSuccessCS("this[12].PropertyString", TypeCode.String, _simpleObject.PropertyString, _manyArray);
            TestEvalSuccessCS("this[12].PropertyChild", TypeCode.Object, _simpleObject.PropertyChild, _manyArray);
            TestEvalSuccessCS("this[12].PropertyChar", TypeCode.Char, _simpleObject.PropertyChar, _manyArray);
            TestEvalSuccessCS("this[12].PropertyChild", TypeCode.Object, _simpleObject.PropertyChild, _manyArray);
            TestEvalSuccessCS("this[12].FieldChild.PropertyByte", TypeCode.Byte, _simpleObject.FieldChild.PropertyByte, _manyArray);
            TestEvalSuccessCS("this[12].FieldChild.PropertyInt32", TypeCode.Int32, _simpleObject.FieldChild.PropertyInt32, _manyArray);
            TestEvalSuccessCS("this[12].FieldChild.PropertyInt64", TypeCode.Int64, _simpleObject.FieldChild.PropertyInt64, _manyArray);
            TestEvalSuccessCS("this[12].FieldChild.PropertySingle", TypeCode.Single, _simpleObject.FieldChild.PropertySingle, _manyArray);
            TestEvalSuccessCS("this[12].FieldChild.PropertyDouble", TypeCode.Double, _simpleObject.FieldChild.PropertyDouble, _manyArray);
            TestEvalSuccessCS("this[12].FieldChild.PropertyDecimal", TypeCode.Decimal, _simpleObject.FieldChild.PropertyDecimal, _manyArray);
            TestEvalSuccessCS("this[12].FieldChild.PropertyString", TypeCode.String, _simpleObject.FieldChild.PropertyString, _manyArray);
            TestEvalSuccessCS("this[12].FieldChild.PropertyChild", TypeCode.Object, _simpleObject.FieldChild.PropertyChild, _manyArray);

            // Field/Property access (negative tests)
            TestEvalFailedErrorVB("Me.Banana", _singleArray);
            TestEvalFailedErrorVB("Me(12).XXX", _manyArray);
            TestEvalFailedErrorVB("Me(12).FieldChild.XXX", _manyArray);

            TestEvalFailedErrorCS("this.Banana", _singleArray);
            TestEvalFailedErrorCS("this[12].XXX", _manyArray);
            TestEvalFailedErrorCS("this[12].FieldChild.XXX", _manyArray);
        }

        public static void TestMethod()
        {
            // Method access (positive tests)
            TestEvalSuccessVB("Me.MethodByte()", TypeCode.Byte, _simpleObject.MethodByte(), _simpleObject);
            TestEvalSuccessVB("Me.MethodChar()", TypeCode.Char, _simpleObject.MethodChar(), _simpleObject);
            TestEvalSuccessVB("Me.MethodInt32()", TypeCode.Int32, _simpleObject.MethodInt32(), _simpleObject);
            TestEvalSuccessVB("Me.MethodInt64()", TypeCode.Int64, _simpleObject.MethodInt64(), _simpleObject);
            TestEvalSuccessVB("Me.MethodSingle()", TypeCode.Single, _simpleObject.MethodSingle(), _simpleObject);
            TestEvalSuccessVB("Me.MethodDouble()", TypeCode.Double, _simpleObject.MethodDouble(), _simpleObject);
            TestEvalSuccessVB("Me.MethodDecimal()", TypeCode.Decimal, _simpleObject.MethodDecimal(), _simpleObject);
            TestEvalSuccessVB("Me.MethodString()", TypeCode.String, _simpleObject.MethodString(), _simpleObject);
            TestEvalSuccessVB("Me.MethodString(10)", TypeCode.String, _simpleObject.MethodString(10), _simpleObject);
            TestEvalSuccessVB("Me.MethodString(10,10)", TypeCode.String, _simpleObject.MethodString(10, 10), _simpleObject);
            TestEvalSuccessVB("Me.MethodString(10,10F)", TypeCode.String, _simpleObject.MethodString(10, 10f), _simpleObject);
            TestEvalSuccessVB("Me.MethodString(1*2)", TypeCode.String, _simpleObject.MethodString(1 * 2), _simpleObject);
            TestEvalSuccessVB("Me.MethodString(1*2+3)", TypeCode.String, _simpleObject.MethodString(1 * 2 + 3), _simpleObject);
            TestEvalSuccessVB("Me.MethodString(1*2+3,7-(2+1))", TypeCode.String, _simpleObject.MethodString(1 * 2 + 3, 7 - (2 + 1)), _simpleObject);
            TestEvalSuccessVB("Math.Min(Int32.MaxValue, Integer.Parse('100'))", TypeCode.Int32, Math.Min(Int32.MaxValue, int.Parse("100")));
            TestEvalSuccessVB("Integer.Parse('100')", TypeCode.Int32, int.Parse("100"));
            TestEvalSuccessVB("String.Format('{0:X} {1:X} {2:X} {3:X}', 10, 20, 30, 40)", TypeCode.String, String.Format("{0:X} {1:X} {2:X} {3:X}", 10, 20, 30, 40));
            TestEvalSuccessVB("String.Format('{0:X} {1:X} {2:X}', 10, 20, 30)", TypeCode.String, String.Format("{0:X} {1:X} {2:X}", 10, 20, 30));
            TestEvalSuccessVB("String.Format('{0:X} {1:X}', 10, 20)", TypeCode.String, String.Format("{0:X} {1:X}", 10, 20));
            TestEvalSuccessVB("String.Format('{0:X}', 10)", TypeCode.String, String.Format("{0:X}", 10));

            TestEvalSuccessCS("this.MethodByte()", TypeCode.Byte, _simpleObject.MethodByte(), _simpleObject);
            TestEvalSuccessCS("this.MethodChar()", TypeCode.Char, _simpleObject.MethodChar(), _simpleObject);
            TestEvalSuccessCS("this.MethodInt32()", TypeCode.Int32, _simpleObject.MethodInt32(), _simpleObject);
            TestEvalSuccessCS("this.MethodInt64()", TypeCode.Int64, _simpleObject.MethodInt64(), _simpleObject);
            TestEvalSuccessCS("this.MethodSingle()", TypeCode.Single, _simpleObject.MethodSingle(), _simpleObject);
            TestEvalSuccessCS("this.MethodDouble()", TypeCode.Double, _simpleObject.MethodDouble(), _simpleObject);
            TestEvalSuccessCS("this.MethodDecimal()", TypeCode.Decimal, _simpleObject.MethodDecimal(), _simpleObject);
            TestEvalSuccessCS("this.MethodString()", TypeCode.String, _simpleObject.MethodString(), _simpleObject);
            TestEvalSuccessCS("this.MethodString(10)", TypeCode.String, _simpleObject.MethodString(10), _simpleObject);
            TestEvalSuccessCS("this.MethodString(10,10)", TypeCode.String, _simpleObject.MethodString(10, 10), _simpleObject);
            TestEvalSuccessCS("this.MethodString(10,10f)", TypeCode.String, _simpleObject.MethodString(10, 10f), _simpleObject);
            TestEvalSuccessCS("this.MethodString(1*2)", TypeCode.String, _simpleObject.MethodString(1 * 2), _simpleObject);
            TestEvalSuccessCS("this.MethodString(1*2+3)", TypeCode.String, _simpleObject.MethodString(1 * 2 + 3), _simpleObject);
            TestEvalSuccessCS("this.MethodString(1*2+3,7-(2+1))", TypeCode.String, _simpleObject.MethodString(1 * 2 + 3, 7 - (2 + 1)), _simpleObject);
            TestEvalSuccessCS("Math.Min(Int32.MaxValue, int.Parse('100'))", TypeCode.Int32, Math.Min(Int32.MaxValue, int.Parse("100")));
            TestEvalSuccessCS("int.Parse('100')", TypeCode.Int32, int.Parse("100"));
            TestEvalSuccessCS("String.Format('{0:X} {1:X} {2:X} {3:X}', 10, 20, 30, 40)", TypeCode.String, String.Format("{0:X} {1:X} {2:X} {3:X}", 10, 20, 30, 40));
            TestEvalSuccessCS("String.Format('{0:X} {1:X} {2:X}', 10, 20, 30)", TypeCode.String, String.Format("{0:X} {1:X} {2:X}", 10, 20, 30));
            TestEvalSuccessCS("String.Format('{0:X} {1:X}', 10, 20)", TypeCode.String, String.Format("{0:X} {1:X}", 10, 20));
            TestEvalSuccessCS("String.Format('{0:X}', 10)", TypeCode.String, String.Format("{0:X}", 10));
            
            // Method access (negative tests)
            TestEvalFailedErrorVB("Me.MethodByte('a')", _simpleObject);
            TestEvalFailedErrorVB("Me.MethodInt32(32)", _simpleObject);
            TestEvalFailedErrorVB("Me.MethodString(1,2,3)", _simpleObject);

            TestEvalFailedErrorCS("this.MethodByte('a')", _simpleObject);
            TestEvalFailedErrorCS("this.MethodInt32(32)", _simpleObject);
            TestEvalFailedErrorCS("this.MethodString(1,2,3)", _simpleObject);
        }

        public static void TestPrecedence()
        {
            // Operator precedence (positive tests)
            TestEvalSuccessVB("Not False And True", TypeCode.Boolean, !false & true);
            TestEvalSuccessVB("True And Not False", TypeCode.Boolean, true & !false);
            TestEvalSuccessVB("Not True Or True", TypeCode.Boolean, !true | true);
            TestEvalSuccessVB("False Xor Not False", TypeCode.Boolean, false ^ !false);
            TestEvalSuccessVB("Not(False And True)", TypeCode.Boolean, !(false & true));
            TestEvalSuccessVB("Not(True Or True)", TypeCode.Boolean, !(true | true));
            TestEvalSuccessVB("Not(True Or Not True)", TypeCode.Boolean, !(true | !true));
            TestEvalSuccessVB("2>1 And 3>2 And 4>3", TypeCode.Boolean, 2 > 1 && 3 > 2 && 4 > 3);
            TestEvalSuccessVB("1>2 Or 2>3 Or 4>3", TypeCode.Boolean, 2 > 1 || 3 > 2 || 4 > 3);
            TestEvalSuccessVB("2>1 And 3>2 Or 4>3", TypeCode.Boolean, 2 > 1 && 3 > 2 || 4 > 3);
            TestEvalSuccessVB("1>2 Or 2>1 And 4>3", TypeCode.Boolean, 1 > 2 || 2 > 1 && 4 > 3);
            TestEvalSuccessVB("Me.Length+2*3", TypeCode.Int32, _singleArray.Length + 2 * 3, _singleArray);
            TestEvalSuccessVB("1+Me.Length*3", TypeCode.Int32, 1 + _singleArray.Length * 3, _singleArray);
            TestEvalSuccessVB("1+2*Me.Length", TypeCode.Int32, 1 + 2 * _singleArray.Length, _singleArray);
            TestEvalSuccessVB("4<90=False Or True", TypeCode.Boolean, 4<90==false || true);
            TestEvalSuccessVB("1<<2+3*4<90=False Or 1<<2+3*4<90=False And 1<<2+3*4<90=False", TypeCode.Boolean, 1 << 2 + 3 * 4 < 90 == false || 1 << 2 + 3 * 4 < 90 == false && 1 << 2 + 3 * 4 < 90 == false);

            TestEvalSuccessCS("1d+2d*3d", TypeCode.Double, 1d + 2d * 3d);
            TestEvalSuccessCS("1d+2d/3d", TypeCode.Double, 1d + 2d / 3d);
            TestEvalSuccessCS("1d+2d%3d", TypeCode.Double, 1d + 2d % 3d);
            TestEvalSuccessCS("1d-2d*3d", TypeCode.Double, 1d - 2d * 3d);
            TestEvalSuccessCS("1d-2d/3d", TypeCode.Double, 1d - 2d / 3d);
            TestEvalSuccessCS("1d-2d%3d", TypeCode.Double, 1d - 2d % 3d);
            TestEvalSuccessCS("1d*2d+3d", TypeCode.Double, 1d * 2d + 3d);
            TestEvalSuccessCS("1d/2d+3d", TypeCode.Double, 1d / 2d + 3d);
            TestEvalSuccessCS("1d%2d+3d", TypeCode.Double, 1d % 2d + 3d);
            TestEvalSuccessCS("1d*2d-3d", TypeCode.Double, 1d * 2d - 3d);
            TestEvalSuccessCS("1d/2d-3d", TypeCode.Double, 1d / 2d - 3d);
            TestEvalSuccessCS("1d%2d-3d", TypeCode.Double, 1d % 2d - 3d);
            TestEvalSuccessCS("1+2<<3", TypeCode.Int32, 1 + 2 << 3);
            TestEvalSuccessCS("1+2>>3", TypeCode.Int32, 1 + 2 >> 3);
            TestEvalSuccessCS("1-2<<3", TypeCode.Int32, 1 - 2 << 3);
            TestEvalSuccessCS("1-2>>3", TypeCode.Int32, 1 - 2 >> 3);
            TestEvalSuccessCS("1*2<<3", TypeCode.Int32, 1 * 2 << 3);
            TestEvalSuccessCS("1*2>>3", TypeCode.Int32, 1 * 2 >> 3);
            TestEvalSuccessCS("1/2<<3", TypeCode.Int32, 1 / 2 << 3);
            TestEvalSuccessCS("1/2>>3", TypeCode.Int32, 1 / 2 >> 3);
            TestEvalSuccessCS("1%2<<3", TypeCode.Int32, 1 % 2 << 3);
            TestEvalSuccessCS("1%2>>3", TypeCode.Int32, 1 % 2 >> 3);
            TestEvalSuccessCS("1<<2+3", TypeCode.Int32, 1 << 2 + 3);
            TestEvalSuccessCS("1<<2+3", TypeCode.Int32, 1 << 2 + 3);
            TestEvalSuccessCS("1>>2-3", TypeCode.Int32, 1 >> 2 - 3);
            TestEvalSuccessCS("1>>2-3", TypeCode.Int32, 1 >> 2 - 3);
            TestEvalSuccessCS("1>>2*3", TypeCode.Int32, 1 >> 2 * 3);
            TestEvalSuccessCS("1>>2*3", TypeCode.Int32, 1 >> 2 * 3);
            TestEvalSuccessCS("1>>2/3", TypeCode.Int32, 1 >> 2 / 3);
            TestEvalSuccessCS("1>>2/3", TypeCode.Int32, 1 >> 2 / 3);
            TestEvalSuccessCS("1>>2%3", TypeCode.Int32, 1 >> 2 % 3);
            TestEvalSuccessCS("1>>2%3", TypeCode.Int32, 1 >> 2 % 3);
            TestEvalSuccessCS("4+1*2<<3", TypeCode.Int32, 4 + 1 * 2 << 3);
            TestEvalSuccessCS("4-1*2>>3", TypeCode.Int32, 4 - 1 * 2 >> 3);
            TestEvalSuccessCS("1*2+4<<3", TypeCode.Int32, 1 * 2 + 4 << 3);
            TestEvalSuccessCS("1*2-4>>3", TypeCode.Int32, 1 * 2 - 4 >> 3);
            TestEvalSuccessCS("1*2<<3+4", TypeCode.Int32, 1 * 2 << 3 + 4);
            TestEvalSuccessCS("1*2>>3-4", TypeCode.Int32, 1 * 2 >> 3 - 4);
            TestEvalSuccessCS("1+2<1-2", TypeCode.Boolean, 1 + 2 < 1 - 2);
            TestEvalSuccessCS("1*2<1/2", TypeCode.Boolean, 1 * 2 < 1 / 2);
            TestEvalSuccessCS("1+2<1%2", TypeCode.Boolean, 1 + 2 < 1 % 2);
            TestEvalSuccessCS("1<<2<1>>2", TypeCode.Boolean, 1 << 2 < 1 >> 2);
            TestEvalSuccessCS("1+2<=1-2", TypeCode.Boolean, 1 + 2 <= 1 - 2);
            TestEvalSuccessCS("1*2<=1/2", TypeCode.Boolean, 1 * 2 <= 1 / 2);
            TestEvalSuccessCS("1+2<=1%2", TypeCode.Boolean, 1 + 2 <= 1 % 2);
            TestEvalSuccessCS("1<=2>>3", TypeCode.Boolean, 1 << 2 <= 1 >> 2);
            TestEvalSuccessCS("1<<2<=1>>2", TypeCode.Boolean, 1 << 2 <= 1 >> 2);
            TestEvalSuccessCS("1+2>1-2", TypeCode.Boolean, 1 + 2 > 1 - 2);
            TestEvalSuccessCS("1*2>1/2", TypeCode.Boolean, 1 * 2 > 1 / 2);
            TestEvalSuccessCS("1+2>1%2", TypeCode.Boolean, 1 + 2 > 1 % 2);
            TestEvalSuccessCS("1<<2>1>>2", TypeCode.Boolean, 1 << 2 > 1 >> 2);
            TestEvalSuccessCS("1+2>=1-2", TypeCode.Boolean, 1 + 2 >= 1 - 2);
            TestEvalSuccessCS("1*2>=1/2", TypeCode.Boolean, 1 * 2 >= 1 / 2);
            TestEvalSuccessCS("1+2>=1%2", TypeCode.Boolean, 1 + 2 >= 1 % 2);
            TestEvalSuccessCS("1<<2>=1>>2", TypeCode.Boolean, 1 << 2 >= 1 >> 2);
            TestEvalSuccessCS("1+2!=1-2", TypeCode.Boolean, 1 + 2 != 1 - 2);
            TestEvalSuccessCS("1*2!=1/2", TypeCode.Boolean, 1 * 2 != 1 / 2);
            TestEvalSuccessCS("1+2!=1%2", TypeCode.Boolean, 1 + 2 != 1 % 2);
            TestEvalSuccessCS("1<<2!=1>>2", TypeCode.Boolean, 1 << 2 != 1 >> 2);
            TestEvalSuccessCS("1+2==1-2", TypeCode.Boolean, 1 + 2 == 1 - 2);
            TestEvalSuccessCS("1*2==1/2", TypeCode.Boolean, 1 * 2 == 1 / 2);
            TestEvalSuccessCS("1+2==1%2", TypeCode.Boolean, 1 + 2 == 1 % 2);
            TestEvalSuccessCS("1<<2==1>>2", TypeCode.Boolean, 1 << 2 == 1 >> 2);
            TestEvalSuccessCS("1<2==2<3", TypeCode.Boolean, 1 < 2 == 2 < 3);
            TestEvalSuccessCS("1<2!=2<3", TypeCode.Boolean, 1 < 2 != 2 < 3);
            TestEvalSuccessCS("1<=2==2<=3", TypeCode.Boolean, 1 <= 2 == 2 <= 3);
            TestEvalSuccessCS("1<=2!=2<=3", TypeCode.Boolean, 1 <= 2 != 2 <= 3);
            TestEvalSuccessCS("1>=2==2>=3", TypeCode.Boolean, 1 >= 2 == 2 >= 3);
            TestEvalSuccessCS("1>=2!=2>=3", TypeCode.Boolean, 1 >= 2 != 2 >= 3);
            TestEvalSuccessCS("1>2==2>3", TypeCode.Boolean, 1 > 2 == 2 > 3);
            TestEvalSuccessCS("1>2!=2>3", TypeCode.Boolean, 1 > 2 != 2 > 3);
            TestEvalSuccessCS("1<<2+3*4==1>>2-3/4", TypeCode.Boolean, 1 << 2 + 3 * 4 == 1 >> 2 - 3 / 4);
            TestEvalSuccessCS("1<<2+3*4!=1>>2-3/4", TypeCode.Boolean, 1 << 2 + 3 * 4 != 1 >> 2 - 3 / 4);
            TestEvalSuccessCS("1<<2+3*4<90==80>1>>2-3/4", TypeCode.Boolean, 1 << 2 + 3 * 4 < 90 == 80 > 1 >> 2 - 3 / 4);
            TestEvalSuccessCS("1<<2+3*4<90==80>1>>2-3/4&false", TypeCode.Boolean, 1 << 2 + 3 * 4 < 90 == 80 > 1 >> 2 - 3 / 4 & false);
            TestEvalSuccessCS("1<<2+3*4<90==80>1>>2-3/4^false", TypeCode.Boolean, 1 << 2 + 3 * 4 < 90 == 80 > 1 >> 2 - 3 / 4 ^ false);
            TestEvalSuccessCS("1<<2+3*4<90==80>1>>2-3/4|false", TypeCode.Boolean, 1 << 2 + 3 * 4 < 90 == 80 > 1 >> 2 - 3 / 4 | false);
            TestEvalSuccessCS("1<<2+3*4<90==80>1>>2-3/4&8<4|2==1^false", TypeCode.Boolean, 1 << 2 + 3 * 4 < 90 == 80 > 1 >> 2 - 3 / 4 & 8 < 4 | 2 == 1 ^ false);
            TestEvalSuccessCS("1<<2+3*4<90==false||1<<2+3*4<90==false&&1<<2+3*4<90==false", TypeCode.Boolean, 1 << 2 + 3 * 4 < 90 == false || 1 << 2 + 3 * 4 < 90 == false && 1 << 2 + 3 * 4 < 90 == false);
            TestEvalSuccessCS("2>1&&3>2&&4>3", TypeCode.Boolean, 2 > 1 && 3 > 2 && 4 > 3);
            TestEvalSuccessCS("1>2||2>3||4>3", TypeCode.Boolean, 2 > 1 || 3 > 2 || 4 > 3);
            TestEvalSuccessCS("2>1&&3>2||4>3", TypeCode.Boolean, 2 > 1 && 3 > 2 || 4 > 3);
            TestEvalSuccessCS("1>2||2>1&&4>3", TypeCode.Boolean, 1 > 2 || 2 > 1 && 4 > 3);
            TestEvalSuccessCS("2>1&&3>2&&4>3?1>2!=2>3:1>2==2>3", TypeCode.Boolean, 2 > 1 && 3 > 2 && 4 > 3 ? 1 > 2 != 2 > 3 : 1 > 2 == 2 > 3);
            TestEvalSuccessCS("1+2*3<<4>50?1+2*3<<4>50:1+2*3<<4>50", TypeCode.Boolean, 1 + 2 * 3 << 4 > 50 ? 1 + 2 * 3 << 4 > 50 : 1 + 2 * 3 << 4 > 50);
            TestEvalSuccessCS("1>2?2>3?3>4?'a':'b':'c':'d'", TypeCode.Char, 1>2?2>3?3>4?'a':'b':'c':'d');
            TestEvalSuccessCS("((((1+2)*3)<<4)>50)?((((1+2)*3)<<4)>50):((((1+2)*3)<<4)>50)", TypeCode.Boolean, ((((1 + 2) * 3) << 4) > 50) ? ((((1 + 2) * 3) << 4) > 50) : ((((1 + 2) * 3) << 4) > 50));
            TestEvalSuccessCS("this.Length+2*3", TypeCode.Int32, _singleArray.Length + 2 * 3, _singleArray);
            TestEvalSuccessCS("1+this.Length*3", TypeCode.Int32, 1 + _singleArray.Length * 3, _singleArray);
            TestEvalSuccessCS("1+2*this.Length", TypeCode.Int32, 1 + 2 * _singleArray.Length, _singleArray);
        }

        public static void TestComment()
        {
            // (C# //, /* */) (positive tests)
            TestEvalSuccessCS("1//", TypeCode.Int32, 1);
            TestEvalSuccessCS("1 //", TypeCode.Int32, 1);
            TestEvalSuccessCS("1 // abcdefghijkl", TypeCode.Int32, 1);
            TestEvalSuccessCS("1 // 'comment'", TypeCode.Int32, 1);
            TestEvalSuccessCS("4+1*2<<3 // 'comment'", TypeCode.Int32, 4 + 1 * 2 << 3);
            TestEvalSuccessCS("1/**/", TypeCode.Int32, 1);
            TestEvalSuccessCS("1 /**/", TypeCode.Int32, 1);
            TestEvalSuccessCS("1 /* abcdef */", TypeCode.Int32, 1);
            TestEvalSuccessCS("1 /* 'comment' */", TypeCode.Int32, 1);
            TestEvalSuccessCS("/* 'comment' */1", TypeCode.Int32, 1);
            TestEvalSuccessCS("/* abc */ 1", TypeCode.Int32, 1);
            TestEvalSuccessCS("1+/* abc */ 1", TypeCode.Int32, 2);
            TestEvalSuccessCS("1/* abc */+1", TypeCode.Int32, 2);
            TestEvalSuccessCS("1/**/+1", TypeCode.Int32, 2);
            TestEvalSuccessCS("1/**/+1+/**/+1", TypeCode.Int32, 3);

            // (C# //) (negative tests)
            TestEvalFailedParseErrorVB("//", 0);
            TestEvalFailedParseErrorVB("1+//2", 2);
            TestEvalFailedParseErrorVB("/**/", 0);
            TestEvalFailedParseErrorVB("1+/**/2", 2);

            TestEvalFailedParseErrorCS("1+//2", 5);
        }

        public static void TestEvalSuccessCS(string input, TypeCode resultCode, object resultValue)
        {
            TestEvalSuccess(input, resultCode, resultValue, null, Language.CSharp);
        }

        public static void TestEvalSuccessCS(string input, TypeCode resultCode, object resultValue, object thisArray)
        {
            TestEvalSuccess(input, resultCode, resultValue, thisArray, Language.CSharp);
        }

        public static void TestEvalSuccessVB(string input, TypeCode resultCode, object resultValue)
        {
            TestEvalSuccess(input, resultCode, resultValue, null, Language.VBNet);
        }
        
        public static void TestEvalSuccessVB(string input, TypeCode resultCode, object resultValue, object thisArray)
        {
            TestEvalSuccess(input, resultCode, resultValue, thisArray, Language.VBNet);
        }

        public static void TestEvalSuccess(string input, TypeCode resultCode, object resultValue, object thisArray, Language language)
        {
            Eval eval = new Eval(input, language);
            EvalResult result = eval.Evaluate(thisArray);

            if ((resultCode != result.Type) ||                             
                ((resultValue == null) && (result.Value != null)) ||       
                ((result.Value != null) && (Type.GetTypeCode(resultValue.GetType()) != resultCode)))   
                throw new ApplicationException("Eval result type incorrect. Expected '" + resultCode.ToString() + "' but received '" + result.Type.ToString() + "' for input string '" + input + "'.");
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

                throw new ApplicationException("Eval result value incorrect. Expected '" + resultValue.ToString() + "' but received '" + result.Value.ToString() + "' for input string '" + input + "'.");
            }
        }

        public static void TestEvalFailedParseErrorCS(string input, int errorIndex)
        {
            TestEvalFailedParseError(input, errorIndex, Language.CSharp);
        }

        public static void TestEvalFailedParseErrorVB(string input, int errorIndex)
        {
            TestEvalFailedParseError(input, errorIndex, Language.VBNet);
        }

        public static void TestEvalFailedParseError(string input, int errorIndex, Language language)
        {
            try
            {
                Eval eval = new Eval(input, language);
                throw new ApplicationException("Eval parse error expected but not found. Input string '" + input + "' and expected error at index '" + errorIndex.ToString() + "'.");
            }
            catch (ParseException pe)
            {
                if (pe.Index == errorIndex)
                    return;
                else
                    throw new ApplicationException("Eval parse error at wrong index location. Input string '" + input + "' and expected error at index '" + errorIndex.ToString() + "' but found at '" + pe.Index.ToString() + "'.");
            }
        }

        public static void TestEvalFailedErrorCS(string input)
        {
            TestEvalFailedError(input, null, Language.CSharp);
        }

        public static void TestEvalFailedErrorCS(string input, object thisArray)
        {
            TestEvalFailedError(input, thisArray, Language.CSharp);
        }

        public static void TestEvalFailedErrorVB(string input)
        {
            TestEvalFailedError(input, null, Language.VBNet);
        }

        public static void TestEvalFailedErrorVB(string input, object thisArray)
        {
            TestEvalFailedError(input, thisArray, Language.VBNet);
        }

        public static void TestEvalFailedError(string input, object thisArray, Language language)
        {
            try
            {
                Eval eval = new Eval(input, language);
                eval.Evaluate(thisArray);
            }
            catch (ParseException pe)
            {
                throw new ApplicationException("Eval parse error not expected. Input string '" + input + "' and parse error at index '" + pe.Index.ToString() + "'.");
            }
            catch (Exception)
            {
                return;
            }

            throw new ApplicationException("Eval error expected but not found. Input string '" + input + "'.");
        }
    }
}

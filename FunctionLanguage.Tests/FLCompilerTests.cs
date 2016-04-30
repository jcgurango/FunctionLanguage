using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FunctionLanguage.Exceptions;

namespace FunctionLanguage.Tests
{
    [TestClass]
    public class FLCompilerTests
    {
        public static FLCompilerSettings BasicArithmeticTestCompilerSettings()
        {
            FLCompilerSettings settings = new FLCompilerSettings();
            settings.PushOperator("*");
            settings.PushOperator("/", true);
            settings.PushOperator("+");
            settings.PushOperator("-", true);
            return settings;
        }

        public static FLCompiler BasicArithmeticTestCompiler()
        {
            FLCompilerSettings settings = BasicArithmeticTestCompilerSettings();
            FLCompiler compiler = new FLCompiler(settings);
            return compiler;
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedEndException))]
        public void Execute_UnclosedParentheses_ExceptionThrown()
        {
            FLCompiler compiler = BasicArithmeticTestCompiler();
            compiler.Execute("10 + 10 + (15 + 15) + CALL(10, 10, CALL(10, 10, 0", new DebugContext());
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedTokenException))]
        public void Execute_NonMinusUnaryOperator_ExceptionThrown()
        {
            FLCompiler compiler = BasicArithmeticTestCompiler();
            compiler.Execute("10 + 10 + +10", new DebugContext());
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedTokenException))]
        public void Execute_MalformedFLCode_ExceptionThrown()
        {
            FLCompiler compiler = BasicArithmeticTestCompiler();
            compiler.Execute("10 + test_ thisIsWrong", new DebugContext());
        }

        [TestMethod]
        public void Execute_LongDoubleLiteral_PassedTextAsDouble()
        {
            FLCompiler compiler = BasicArithmeticTestCompiler();
            object value = compiler.Execute("1000350324.00000500032000", new DebugContext());
            Assert.AreEqual(1000350324.00000500032d, value);
        }

        [TestMethod]
        public void Execute_NumberAsStringLiteral_PassedTextAsString()
        {
            FLCompiler compiler = BasicArithmeticTestCompiler();
            object value = compiler.Execute("'1000350324.00000500032000'", new DebugContext());
            Assert.AreEqual("1000350324.00000500032000", value);
        }

        [TestMethod]
        public void Execute_NumberWithUnaryMinus_NegativeNumber()
        {
            FLCompiler compiler = BasicArithmeticTestCompiler();
            object value = compiler.Execute("-2500", new DebugContext());
            Assert.AreEqual(-2500d, value);
        }

        [TestMethod]
        public void Execute_StringWithMultipleOperations_OrderedOperations()
        {
            //PEMDAS
            FLCompiler compiler = BasicArithmeticTestCompiler();
            DebugContext context = new DebugContext();
            string operators = "";

            context.Operated += (symbol, left, right) =>
            {
                operators += symbol;
            };

            compiler.Execute("10 + 10 - (15 - 15 * 25 / 25 * (25 / 10)) / 100", context);

            Assert.AreEqual("+*//*-/-", operators);
        }

        [TestMethod]
        public void Execute_FunctionCallOnObject_ThisObjectIsReturnValue()
        {
            FLCompiler compiler = BasicArithmeticTestCompiler();
            FLRuntime runtime = new FLRuntime();
            object lastThis = null;

            runtime.RegisterFunction("add", (t, x) =>
            {
                double total = t == null ? 0f : (double)t;
                lastThis = t;

                foreach (object obj in x)
                {
                    total += Convert.ToSingle(obj);
                }

                return total;
            });

            object result = compiler.Execute("add(4, 8)->add(5)", runtime);
            Assert.AreEqual(12d, lastThis);
            Assert.AreEqual(17d, result);
        }
    }
}

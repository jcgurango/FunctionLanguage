using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunctionLanguage.Tests
{
    [TestClass]
    public class DefaultOperationHandlerTests
    {
        public DefaultOperationHandler GetOperationHandlerInstance()
        {
            return DefaultOperationHandler.Instance;
        }

        [TestMethod]
        public void Operate_ArithmeticOperatorsWithIntegerOperands_Results()
        {
            DefaultOperationHandler handler = GetOperationHandlerInstance();
            Assert.AreEqual(13d, handler.Operate("+", 5, 8));
            Assert.AreEqual(-3d, handler.Operate("-", 5, 8));
            Assert.AreEqual(40d, handler.Operate("*", 5, 8));
            Assert.AreEqual(0.625d, handler.Operate("/", 5, 8));
        }

        [TestMethod]
        public void Operate_ArithmeticOperatorsWithStringOperands_Results()
        {
            DefaultOperationHandler handler = GetOperationHandlerInstance();
            Assert.AreEqual(13d, handler.Operate("+", "5", "8"));
            Assert.AreEqual(-3d, handler.Operate("-", "5", "8"));
            Assert.AreEqual(40d, handler.Operate("*", "5", "8"));
            Assert.AreEqual(0.625d, handler.Operate("/", "5", "8"));
        }

        [TestMethod]
        public void Operate_ComparisonOperators_BooleanResults()
        {
            DefaultOperationHandler handler = GetOperationHandlerInstance();
            Assert.AreEqual(false, handler.Operate(">", 5, 8));
            Assert.AreEqual(true, handler.Operate("<", 5, 8));
            Assert.AreEqual(false, handler.Operate(">=", 5, 8));
            Assert.AreEqual(true, handler.Operate("<=", 5, 8));
            Assert.AreEqual(false, handler.Operate("==", 5, 8));
            Assert.AreEqual(true, handler.Operate("==", 5, 5));
            Assert.AreEqual(true, handler.Operate(">=", 5, 5));
            Assert.AreEqual(true, handler.Operate("<=", 5, 5));
        }
        
        [TestMethod]
        public void Operate_AndOperationOnBooleanValues_BooleanResults()
        {
            DefaultOperationHandler handler = GetOperationHandlerInstance();
            Assert.AreEqual(false, handler.Operate("&&", true, false));
            Assert.AreEqual(false, handler.Operate("&&", false, true));
            Assert.AreEqual(true, handler.Operate("&&", true, true));
            Assert.AreEqual(false, handler.Operate("&&", false, false));
        }

        [TestMethod]
        public void Operate_OrOperationOnBooleanValues_BooleanResults()
        {
            DefaultOperationHandler handler = GetOperationHandlerInstance();
            Assert.AreEqual(true, handler.Operate("||", true, false));
            Assert.AreEqual(true, handler.Operate("||", false, true));
            Assert.AreEqual(true, handler.Operate("||", true, true));
            Assert.AreEqual(false, handler.Operate("||", false, false));
        }
    }
}

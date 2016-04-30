using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FunctionLanguage.Tests
{
    [TestClass]
    public class DefaultTypeConverterTests
    {
        public static DefaultTypeConverter GetTypeConverterInstance()
        {
            return DefaultTypeConverter.Instance;
        }

        [TestMethod]
        public void TypeConvert_UnsupportedType_Null()
        {
            Assert.IsNull(GetTypeConverterInstance().TypeConvert("decimal", "number"));
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TypeConvert_UnsupportedNumberFormat_ExceptionThrown()
        {
            GetTypeConverterInstance().TypeConvert("number", "15.0004230fd");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TypeConvert_UnsupportedDateTimeFormat_ExceptionThrown()
        {
            GetTypeConverterInstance().TypeConvert("datetime", "2015/05/05/12");
        }

        [TestMethod]
        public void TypeConvert_NumbersToBooleans_Booleans()
        {
            ITypeConverter converter = GetTypeConverterInstance();
            Assert.AreEqual(false, converter.TypeConvert("boolean", 0));
            Assert.AreEqual(true, converter.TypeConvert("boolean", -1));
            Assert.AreEqual(true, converter.TypeConvert("boolean", -10));
            Assert.AreEqual(true, converter.TypeConvert("boolean", 1));
            Assert.AreEqual(true, converter.TypeConvert("boolean", 5));
            Assert.AreEqual(true, converter.TypeConvert("boolean", 10));
        }

        [TestMethod]
        public void TypeConvert_StringsToBooleans_Booleans()
        {
            ITypeConverter converter = GetTypeConverterInstance();
            Assert.AreEqual(true, converter.TypeConvert("boolean", "True"));
            Assert.AreEqual(true, converter.TypeConvert("boolean", "true"));
            Assert.AreEqual(false, converter.TypeConvert("boolean", "False"));
            Assert.AreEqual(false, converter.TypeConvert("boolean", "false"));
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TypeConvert_UnsupportedBooleanFormat_ExceptionThrown()
        {
            GetTypeConverterInstance().TypeConvert("boolean", "troo");
        }
    }
}

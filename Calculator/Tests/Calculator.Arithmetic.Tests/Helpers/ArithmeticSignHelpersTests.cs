using Calculator.Arithmetic.Enums;
using Calculator.Arithmetic.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Calculator.Arithmetic.Tests.Helpers
{
    [TestClass]
    public class ArithmeticSignHelpersTests
    {
        [DataTestMethod]
        [DataRow(ArithmeticSign.Mul, '*')]
        [DataRow(ArithmeticSign.Del, '/')]
        [DataRow(ArithmeticSign.Sum, '+')]
        [DataRow(ArithmeticSign.Sub, '-')]
        public void GetSign_CorrectSign(ArithmeticSign sign, char expectedSign)
        {
            //arrange
            char actualSign = sign.GetSign();

            //assert
            Assert.AreEqual(expectedSign, actualSign, $"Actual: {actualSign} != Expected: {expectedSign}");
        }

        [DataTestMethod]
        [DataRow(ArithmeticSign.Mul, '*', true)]
        [DataRow(ArithmeticSign.Del, '/', true)]
        [DataRow(ArithmeticSign.Sum, '+', true)]
        [DataRow(ArithmeticSign.Sub, '-', true)]
        [DataRow(ArithmeticSign.Sum, '*', false)]
        [DataRow(ArithmeticSign.Sum, '5', false)]
        [DataRow(ArithmeticSign.Mul, '.', false)]
        public void EqualsSign_CorrectValue(ArithmeticSign sign, char symbolSign, bool expectedValue)
        {
            //arrange
            bool actualValue = sign.EqualsSign(symbolSign);

            //assert
            Assert.AreEqual(expectedValue, actualValue, $"Actual: {actualValue} != Expected: {expectedValue}; DataRow: {sign} != {symbolSign}");
        }

        [DataTestMethod]
        [DataRow('*', ArithmeticSign.Mul)]
        [DataRow('/', ArithmeticSign.Del)]
        [DataRow('+', ArithmeticSign.Sum)]
        [DataRow('-', ArithmeticSign.Sub)]
        public void GetArithmeticSignType_CorrectArithmeticSign(char symbolSign, ArithmeticSign expectedSign)
        {
            //arrange
            var actualSign = ArithmeticSignHelpers.GetArithmeticSignType(symbolSign);

            //assert
            Assert.AreEqual(expectedSign, actualSign, $"Actual: {actualSign} != Expected: {expectedSign}");
        }

        [DataTestMethod]
        [DataRow('!')]
        [DataRow('q')]
        [DataRow(':')]
        [DataRow('\'')]
        [DataRow('^')]
        [DataRow('.')]
        [DataRow('|')]
        [DataRow('=')]
        [DataRow('(')]
        [DataRow(')')]
        [DataRow('?')]
        [ExpectedException(typeof(ArgumentException))]
        public void GetArithmeticSignType_Symbol_ThrowException(char symbol)
        {
            //arrange
            ArithmeticSignHelpers.GetArithmeticSignType(symbol);

            //assert
            //Throw ArgumentException
        }

        [DataTestMethod]
        [DataRow('*', true)]
        [DataRow('/', true)]
        [DataRow('-', true)]
        [DataRow('+', true)]
        [DataRow(')', false)]
        [DataRow('}', false)]
        [DataRow('>', false)]
        [DataRow(':', false)]
        [DataRow('=', false)]
        [DataRow('q', false)]
        [DataRow('!', false)]
        public void IsArithmeticSign_Symbol_BooleanValue(char symbol, bool expectedValue)
        {
            //arrange
            bool actualValue = ArithmeticSignHelpers.IsArithmeticSign(symbol);

            //assert
            Assert.AreEqual(expectedValue, actualValue, $"Actual: {actualValue}; Expected: {expectedValue}");
        }
    }
}

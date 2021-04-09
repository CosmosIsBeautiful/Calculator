using Calculator.Arithmetic.Enums;
using Calculator.Arithmetic.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Calculator.Arithmetic.Tests.Helpers
{
    [TestClass]
    public class ArithmeticSignHelpersTests
    {
        #region GetSign
        [DataTestMethod]
        [DataRow(ArithmeticSign.Mul, '*')]
        [DataRow(ArithmeticSign.Del, '/')]
        [DataRow(ArithmeticSign.Sum, '+')]
        [DataRow(ArithmeticSign.Sub, '-')]
        public void GetSignTest_CorrectSign(ArithmeticSign sign, char expectedSign)
        {
            char actualSign = sign.GetSign();

            Assert.AreEqual(expectedSign, actualSign, $"Invalid sign received: {expectedSign}");
        }
        #endregion

        #region GetPriority
        [DataTestMethod]
        [DataRow(ArithmeticSign.Mul, ArithmeticSign.Sum)]
        [DataRow(ArithmeticSign.Mul, ArithmeticSign.Sub)]
        [DataRow(ArithmeticSign.Del, ArithmeticSign.Sum)]
        [DataRow(ArithmeticSign.Del, ArithmeticSign.Sub)]
        public void GetPriorityTest_CorrectSeniorityPriority(ArithmeticSign signHigherPriority, ArithmeticSign signLoverPriority)
        {
            int actualHigherPriority = signHigherPriority.GetPriority();
            int actualLoverPriority = signLoverPriority.GetPriority();

            Assert.IsTrue(actualHigherPriority > actualLoverPriority, $"Sign priority: {signHigherPriority} > {signLoverPriority}");
        }

        [DataTestMethod]
        [DataRow(ArithmeticSign.Mul, ArithmeticSign.Del)]
        [DataRow(ArithmeticSign.Sum, ArithmeticSign.Sub)]
        public void GetPriorityTest_CorrectEqualityPriority(ArithmeticSign signPriority, ArithmeticSign signPriorityOther)
        {
            int actualPriority = signPriority.GetPriority();
            int actualPriorityOther = signPriorityOther.GetPriority();

            Assert.IsTrue(actualPriority == actualPriorityOther, $"Sign priority: {signPriority} == {signPriorityOther}");
        }

        [DataTestMethod]
        [DataRow(ArithmeticSign.Sum, ArithmeticSign.Mul)]
        [DataRow(ArithmeticSign.Sum, ArithmeticSign.Del)]
        [DataRow(ArithmeticSign.Sub, ArithmeticSign.Mul)]
        [DataRow(ArithmeticSign.Sub, ArithmeticSign.Del)]
        public void GetPriorityTest_CorrectJuniorPriority(ArithmeticSign signLoverPriority, ArithmeticSign signHigherPriority)
        {
            int actualLoverPriority = signLoverPriority.GetPriority();
            int actualHigherPriority = signHigherPriority.GetPriority();
            
            Assert.IsTrue(actualLoverPriority < actualHigherPriority, $"Sign priority: {signLoverPriority} < {actualHigherPriority}");
        }
        #endregion

        #region EqualsSign
        [DataTestMethod]
        [DataRow(ArithmeticSign.Mul, '*', true)]
        [DataRow(ArithmeticSign.Del, '/', true)]
        [DataRow(ArithmeticSign.Sum, '+', true)]
        [DataRow(ArithmeticSign.Sub, '-', true)]
        [DataRow(ArithmeticSign.Sum, '*', false)]
        [DataRow(ArithmeticSign.Sum, '5', false)]
        [DataRow(ArithmeticSign.Mul, '.', false)]
        public void EqualsSignTest_CorrectValue(ArithmeticSign sign, char symbolSign, bool expectedValue)
        {
            bool actualValue = sign.EqualsSign(symbolSign);

            Assert.AreEqual(expectedValue, actualValue, $"Sign {sign} != {symbolSign}");
        }
        #endregion

        #region GetArithmeticSignType
        [DataTestMethod]
        [DataRow('*', ArithmeticSign.Mul)]
        [DataRow('/', ArithmeticSign.Del)]
        [DataRow('+', ArithmeticSign.Sum)]
        [DataRow('-', ArithmeticSign.Sub)]
        public void GetArithmeticSignTypeTest_CorrectArithmeticSign(char symbolSign, ArithmeticSign expectedSign)
        {
            var actualSign = ArithmeticSignHelpers.GetArithmeticSignType(symbolSign);

            Assert.AreEqual(expectedSign, actualSign, $"Sign: {actualSign} != {expectedSign}");
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
        public void GetArithmeticSignTypeTest_IncorrectSymbol_ThrowException(char symbol)
        {
            ArithmeticSignHelpers.GetArithmeticSignType(symbol);

            //assert
            //throw ArgumentException
        }
        #endregion

        #region IsArithmeticSign
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
        public void IsArithmeticSignTest_CorrectSymbol(char symbol, bool expectedValue)
        {
            bool actualValue = ArithmeticSignHelpers.IsArithmeticSign(symbol);

            Assert.AreEqual(expectedValue, actualValue, $"Incorrectly recognized symbol: {symbol}");
        }
        #endregion

        #region IsNegativeNumber
        [DataTestMethod]
        [DataRow('-', true)]
        [DataRow('*', false)]
        [DataRow('/', false)]
        [DataRow('+', false)]
        [DataRow(')', false)]
        [DataRow('(', false)]
        public void IsNegativeNumberTest_CorrectSymbol(char symbol, bool expectedValue)
        {
            bool actualValue = ArithmeticSignHelpers.IsNegativeNumber(symbol);

            Assert.AreEqual(expectedValue, actualValue, $"Incorrectly recognized symbol: {symbol}");
        }
        #endregion
    }
}

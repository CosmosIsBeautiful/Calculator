using System.Collections.Generic;
using Calculator.Arithmetic.Helpers;
using Calculator.Arithmetic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Calculator.Arithmetic.Tests.Helpers
{
    [TestClass]
    public class ArithmeticCalculateHelpersTests
    {

        #region MakeOperation
        [DataTestMethod]
        [DynamicData(nameof(GetDataForMakeOperationTest), DynamicDataSourceType.Method)]
        public void MakeOperationTest_CorrectCalculate(NumberTerm firstNumber, NumberTerm secondNumber, NumberTerm expectedResult)
        {
            NumberTerm actualResult = firstNumber.MakeOperation(secondNumber);

            Assert.AreEqual(expectedResult, actualResult, $"Operation: {firstNumber}{secondNumber};");
        }

        private static IEnumerable<object[]> GetDataForMakeOperationTest()
        {
            yield return new object[] { new NumberTerm('+', 2), new NumberTerm('+', 2), new NumberTerm('+', 4) };
            yield return new object[] { new NumberTerm('+', -5), new NumberTerm('-', 2), new NumberTerm('+', -7) };
            yield return new object[] { new NumberTerm('-', 5), new NumberTerm('-', 2), new NumberTerm('-', 3) };
            yield return new object[] { new NumberTerm('+', 5), new NumberTerm('*', 2), new NumberTerm('+', 10) };
            yield return new object[] { new NumberTerm('+', -5), new NumberTerm('*', 2), new NumberTerm('+', -10) };
            yield return new object[] { new NumberTerm('+', 264), new NumberTerm('/', 7), new NumberTerm('+', 37.714285714285714285714285714M) };
            yield return new object[] { new NumberTerm('+', 42), new NumberTerm('/', 2), new NumberTerm('+', 21) };
            yield return new object[] { new NumberTerm('+', 54), new NumberTerm('*', -2), new NumberTerm('+', -108) };
            yield return new object[] { new NumberTerm('-', 17), new NumberTerm('*', 8), new NumberTerm('-', 136) };
            yield return new object[] { new NumberTerm('+', 94), new NumberTerm('+', 469), new NumberTerm('+', 563) };
            yield return new object[] { new NumberTerm('+', 168.856M), new NumberTerm('*', 4.4M), new NumberTerm('+', 742.9664M) };
        }
        #endregion

        #region IsBracket
        [DataTestMethod]
        [DataRow('(', true)]
        [DataRow(')', false)]
        [DataRow('{', false)]
        [DataRow('<', false)]
        [DataRow('[', false)]
        public void IsStartBracket_CorrectValue(char symbol, bool expectedValue)
        {
            bool actualValue = symbol.IsStartBracket();

            Assert.AreEqual(expectedValue, actualValue, $"Symbol: {symbol}; Actual: {actualValue} != Expected: {expectedValue}");
        }

        [DataTestMethod]
        [DataRow(')', true)]
        [DataRow('(', false)]
        [DataRow('}', false)]
        [DataRow('>', false)]
        [DataRow(']', false)]
        public void IsEndBracket_CorrectValue(char symbol, bool expectedValue)
        {
            bool actualValue = symbol.IsEndBracket();

            Assert.AreEqual(expectedValue, actualValue, $"Symbol: {symbol}; Actual: {actualValue} != Expected: {expectedValue}");
        }
        #endregion
    }
}

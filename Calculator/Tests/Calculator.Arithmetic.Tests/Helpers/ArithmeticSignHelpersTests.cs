using Calculator.Arithmetic.Enums;
using Calculator.Arithmetic.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

    }
}

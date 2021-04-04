using Calculator.Arithmetic.Enums;
using Calculator.Arithmetic.Models;

namespace Calculator.Arithmetic.Helpers
{
    public static class ArithmeticCalculateHelpers
    {
        public static NumberTerm MakeOperation(this NumberTerm firstNumber, NumberTerm secondNumber)
        {
            decimal result = 0;
            switch (secondNumber.Sign)
            {
                case ArithmeticSign.Mul:
                    result = firstNumber.Digit * secondNumber.Digit;
                    break;
                case ArithmeticSign.Del:
                    result = firstNumber.Digit / secondNumber.Digit;
                    break;
                case ArithmeticSign.Sum:
                    result = firstNumber.Digit + secondNumber.Digit;
                    break;
                case ArithmeticSign.Sub:
                    result = firstNumber.Digit - secondNumber.Digit;
                    break;
            }

            return new NumberTerm(firstNumber.Sign, result);
        }

        public static bool IsStartBracket(this char symbol)
        {
            return symbol.Equals('(');
        }

        public static bool IsEndBracket(this char symbol)
        {
            return symbol.Equals(')');
        }
    }
}

﻿using Calculator.Arithmetic.Enums;
using Calculator.Arithmetic.Models;

namespace Calculator.Arithmetic.Helpers
{
    public static class ArithmeticCalculateHelpers
    {
        public static NumberTerm MakeOperation(this NumberTerm firstNumber, NumberTerm secondNumber)
        {
            double result = 0;
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
    }
}

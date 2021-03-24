using Calculator.Arithmetic;
using Calculator.Arithmetic.Models;
using Calculator.Input;
using Calculator.Output;
using System;
using System.Collections.Generic;

namespace Calculator
{
    class Program
    {
        private static IInput Input { get; }
        private static IOutput Output { get; }
        private static IFormatting Formatting { get; }

        static Program()
        {
            Input = new ConsoleInput();
            Output = new ConsoleOutput();
            Formatting = new ArithmeticFormatting();
        }

        static void Main(string[] args)
        {
            var str = Input.Enter("Type the equation similarity \"(2 + 4 * 3) + 5\" and press enter");

            var normalizationExpressionString = Formatting.GetNormalizationExpressionString(str);
            bool isValidExpression = Formatting.IsValidExpression(normalizationExpressionString);

            if (isValidExpression)
            {
                List<ITerm> expression = Formatting.GetParsedExpression(normalizationExpressionString);
            }

            Output.Display("Return: " + str);
        }
    }
}

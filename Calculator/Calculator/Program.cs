using Calculator.Arithmetic;
using Calculator.Arithmetic.Models;
using Calculator.Input;
using Calculator.Output;
using System.Collections.Generic;

namespace Calculator
{
    class Program
    {
        private static IInput Input { get; }
        private static IOutput Output { get; }
        private static IFormatting Formatting { get; }
        private static ICalculate Calculate { get; }

        static Program()
        {
            Input = new ConsoleInput();
            Output = new ConsoleOutput();
            Formatting = new ArithmeticFormatting();
            Calculate = new ArithmeticCalculate();
        }

        static void Main(string[] args)
        {
            string validExpression = GetEnteredValidExpression();

            List<ITerm> expression = Formatting.GetParsedExpression(validExpression);
            decimal result = Calculate.Calculate(expression);

            Output.Display($"{validExpression} = {result}");
        }

        private static string GetEnteredValidExpression()
        {
            var str = Input.Enter("Type the equation similarity \"(2 + 4 * 3) + 5\" and press enter");

            string normalizationExpressionString;
            bool isValidExpression;

            do
            {
                normalizationExpressionString = Formatting.GetNormalizationExpressionString(str);
                isValidExpression = Formatting.IsValidExpression(normalizationExpressionString);

                if (isValidExpression == false)
                {
                    str = Input.Enter("Invalid expression entered please try again");
                }

            } while (isValidExpression == false);

            return normalizationExpressionString;
        }
    }
}

using Calculator.Arithmetic;
using Calculator.Input;
using Calculator.Output;

namespace Calculator.DI
{
    public class CalculatorArithmetic : ICalculator
    {
        private IInput Input { get; }
        private IOutput Output { get; }
        private IFormatting Formatting { get; }
        private ICalculate Calculate { get; }

        public CalculatorArithmetic(IFormatting formatting, ICalculate calculate, IInput input, IOutput output)
        {
            this.Input = input;
            this.Output = output;
            this.Formatting = formatting;
            this.Calculate = calculate;
        }

        public void CalculateExpression()
        {
            string validExpression = GetEnteredValidExpression();

            var expression = Formatting.GetParsedExpression(validExpression);
            decimal result = Calculate.Calculate(expression);

            Output.Display($"{validExpression} = {result}");
        }

        private string GetEnteredValidExpression()
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

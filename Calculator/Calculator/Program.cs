using Calculator.DI;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            CreatorCalculator creatorCalculators = new CreatorArithmeticCalculator();
            ICalculator calculator = creatorCalculators.FactoryMethod();

            calculator.CalculateExpression();
        }
    }
}

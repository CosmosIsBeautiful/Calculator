using Calculator.Arithmetic;
using Calculator.Input;
using Calculator.Output;

namespace Calculator.DI
{
    public class CreatorArithmeticCalculator : CreatorCalculator
    {
        public override ICalculator FactoryMethod()
        {
            //Poor Man's DI
            IFormatting formatting = new ArithmeticFormatting();
            ICalculate calculate = new ArithmeticCalculate();

            IInput input = new ConsoleInput();
            IOutput output = new ConsoleOutput();

            CalculatorArithmetic calculator = new CalculatorArithmetic(formatting, calculate, input, output);

            return calculator;
        }
    }
}

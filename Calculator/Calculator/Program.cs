using Calculator.Input;
using Calculator.Output;
using System;

namespace Calculator
{
    class Program
    {
        private static IInput Input { get; }
        private static IOutput Output { get; }

        static Program()
        {
            Input = new ConsoleInput();
            Output = new ConsoleOutput();
        }

        static void Main(string[] args)
        {
            var str = Input.Enter("Type the equation similarity \"(2 + 4 * 3) + 5\" and press enter");

            Output.Display("Return: " + str);
        }
    }
}

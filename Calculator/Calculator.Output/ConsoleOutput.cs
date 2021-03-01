using System;

namespace Calculator.Output
{
    public interface IOutput
    {
        void Display(string text);
    }

    public class ConsoleOutput : IOutput
    {
        public void Display(string text)
        {
            Console.WriteLine(text);
        }
    }
}

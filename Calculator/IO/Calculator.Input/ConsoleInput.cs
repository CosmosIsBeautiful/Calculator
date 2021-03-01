using System;

namespace Calculator.Input
{
    public interface IInput
    {
        string Enter();

        string Enter(string promptText);
    }

    public class ConsoleInput : IInput
    {
        public string Enter()
        {
            return Console.ReadLine();
        }

        public string Enter(string promptText)
        {
            Console.WriteLine(promptText);
            return this.Enter();
        }
    }
}

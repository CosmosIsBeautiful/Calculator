using System;

namespace Calculator.Arithmetic.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ArithmeticSignAttribute : Attribute
    {
        public readonly char Sign;

        public readonly int Priority;

        public ArithmeticSignAttribute(char sign, int priority)
        {
            this.Sign = sign;
            this.Priority = priority;
        }
    }
}

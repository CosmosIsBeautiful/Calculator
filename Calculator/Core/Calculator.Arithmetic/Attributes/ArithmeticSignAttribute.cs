using System;

namespace Calculator.Arithmetic.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ArithmeticSignAttribute : Attribute
    {
        public readonly char Sign;

        public ArithmeticSignAttribute(char sign)
        {
            this.Sign = sign;
        }
    }
}

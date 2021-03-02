using Calculator.Arithmetic.Enums;

namespace Calculator.Arithmetic.Models
{
    public class NumberTerm : ITerm
    {
        public ArithmeticSign Sign { get; set; }

        public int Digit { get; set; }

        public NumberTerm(ArithmeticSign sign, int digit)
        {
            this.Sign = sign;
            this.Digit = digit;
        }
    }
}

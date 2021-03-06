using Calculator.Arithmetic.Enums;
using Calculator.Arithmetic.Helpers;

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

        public NumberTerm(int digit, ArithmeticSign sign)
        {
            this.Digit = digit;
            this.Sign = sign;
        }
        public NumberTerm(char sign, int digit)
        {
            this.Digit = digit;
            this.Sign = ArithmeticSignHelpers.GetArithmeticSignType(sign);
        }
    }
}

using Calculator.Arithmetic.Enums;
using Calculator.Arithmetic.Helpers;
using System;

namespace Calculator.Arithmetic.Models
{
    public class NumberTerm : ITerm, IEquatable<NumberTerm>
    {
        public ArithmeticSign Sign { get; set; }

        public double Digit { get; set; }

        public NumberTerm(ArithmeticSign sign, double digit)
        {
            this.Sign = sign;
            this.Digit = digit;
        }

        public NumberTerm(int digit, ArithmeticSign sign)
        {
            this.Digit = digit;
            this.Sign = sign;
        }
        public NumberTerm(char sign, double digit)
        {
            this.Digit = digit;
            this.Sign = ArithmeticSignHelpers.GetArithmeticSignType(sign);
        }

        public bool Equals(NumberTerm otherNumber)
        {
            if (otherNumber == null)
            {
                return false;
            }

            return (this.Digit == otherNumber.Digit)
                && (this.Sign == otherNumber.Sign);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || (obj is NumberTerm == false))
            {
                return false;
            }

            var otherNumber = obj as NumberTerm;

            return (this.Digit == otherNumber.Digit)
                && (this.Sign == otherNumber.Sign);
        }

        public override int GetHashCode()
        {
            return this.Digit.GetHashCode() ^ this.Sign.GetHashCode();
        }
    }
}

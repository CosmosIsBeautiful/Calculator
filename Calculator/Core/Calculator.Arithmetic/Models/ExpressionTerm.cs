using Calculator.Arithmetic.Enums;
using System.Collections.Generic;

namespace Calculator.Arithmetic.Models
{
    public class ExpressionTerm : ITerm
    {
        public ArithmeticSign Sign { get; set; }

        public IList<ITerm> Expression { get; set; }

        public ExpressionTerm(ArithmeticSign sign, IList<ITerm> expression)
        {
            this.Sign = sign;
            this.Expression = expression;
        }
    }
}

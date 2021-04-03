using Calculator.Arithmetic.Enums;
using System.Collections.Generic;

namespace Calculator.Arithmetic.Models
{
    public class NestedExpression
    {
        public int Index { get; set; }

        public ArithmeticSign Sign { get; set; }

        public IList<ITerm> Expression { get; set; }

        public NestedExpression(int index, IList<ITerm> expression, ArithmeticSign sign = ArithmeticSign.Sum)
        {
            this.Index = index;
            this.Sign = sign;
            this.Expression = expression;
        }
    }
}

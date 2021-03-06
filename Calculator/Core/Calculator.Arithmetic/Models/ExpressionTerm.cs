using Calculator.Arithmetic.Enums;
using Calculator.Arithmetic.Helpers;
using System.Collections.Generic;
using System.Linq;

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

        public ExpressionTerm(ArithmeticSign sign)
        {
            this.Sign = sign;
            this.Expression = new List<ITerm>();
        }

        public ExpressionTerm(char sign, IEnumerable<ITerm> expression)
        {
            this.Sign = ArithmeticSignHelpers.GetArithmeticSignType(sign);
            this.Expression = expression.ToList();
        }
    }
}

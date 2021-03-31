using Calculator.Arithmetic.Helpers;
using Calculator.Arithmetic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator.Arithmetic
{
    public interface IArithmeticCalculate
    {
        double Calculate(IList<ITerm> expression);

        NumberTerm CalculateInnerExpression(IList<ITerm> expression);
    }

    public class ArithmeticCalculate : IArithmeticCalculate
    {

        public double Calculate(IList<ITerm> expression)
        {
            throw new NotImplementedException();
        }

        private int GetExpressionTermPositionNumber(IList<ITerm> expression)
        {
            for (int i = 0; i < expression.Count; i++)
            {
                if (expression[i] is ExpressionTerm)
                {
                    return i;
                }
            }

            return 0;
        }

        private IList<ITerm> GetInnerExpression(IList<ITerm> expression)
        {
            IList<ITerm> innerExpression = null;

            for (int i = 0; i < expression.Count; i++)
            {
                if (expression[i] is ExpressionTerm)
                {
                    innerExpression = ((ExpressionTerm)expression[i]).Expression;
                    break;
                }
            }

            return innerExpression;
        }

        public NumberTerm CalculateInnerExpression(IList<ITerm> expression)
        {
            var numbers = expression.Cast<NumberTerm>().ToList();

            for (int i = 0; numbers.Count != 1; i++)
            {
                var index = GetHighestProirityPositionNumber(numbers);

                numbers[index - 1] = numbers[index - 1].MakeOperation(numbers[index]);
                numbers.RemoveAt(index);
            }
            
            return numbers[0];
        }

        private int GetHighestProirityPositionNumber(IList<NumberTerm> numberExpression)
        {
            int maxPriority = 0;
            int index = 0;

            if (numberExpression.Count == 2)
            {
                return 1;
            }

            for (int i = 1; i < numberExpression.Count; i++)
            {
                var currentPriority = numberExpression[i].Sign.GetPriority();
               
                if (currentPriority > maxPriority)
                {
                    maxPriority = currentPriority;
                    index = i;
                }

            }

            return index;
        }
    }
}

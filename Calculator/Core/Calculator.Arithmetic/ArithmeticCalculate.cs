using Calculator.Arithmetic.Helpers;
using Calculator.Arithmetic.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calculator.Arithmetic.Enums;

namespace Calculator.Arithmetic
{
    public interface IArithmeticCalculate
    {
        decimal Calculate(IList<ITerm> expression);

        NumberTerm CalculateInnerExpression(IList<ITerm> expression, ArithmeticSign? signExpression = null);
    }

    public class ArithmeticCalculate : IArithmeticCalculate
    {
        public decimal Calculate(IList<ITerm> expression)
        {
            List<NestedExpression> nestedExpressions = CreateNestedExpressions(expression);

            while (nestedExpressions.Count != 1)  
            {
                int i = nestedExpressions.Count - 1;

                var signExpression = nestedExpressions[i].Sign;
                var innerExpressions = nestedExpressions[i].Expression;

                var calculateInnerExpression = CalculateInnerExpression(innerExpressions, signExpression);
                nestedExpressions = GetReplacedCalculatedNestedExpressions(nestedExpressions, i, calculateInnerExpression);

                nestedExpressions = CheckLastExpressionsOnNestedExpressions(nestedExpressions);
            }

            var resultExpression = CalculateInnerExpression(nestedExpressions[0].Expression);
            return resultExpression.Digit;
        }

        private List<NestedExpression> GetReplacedCalculatedNestedExpressions(List<NestedExpression> nestedExpressions, int i, NumberTerm calculateInnerExpression)
        {
            int innerIndex = nestedExpressions[i].Index;

            nestedExpressions[i - 1].Expression[innerIndex] = calculateInnerExpression;
            nestedExpressions.RemoveAt(i);

            return nestedExpressions;
        }

        private List<NestedExpression> CreateNestedExpressions(IList<ITerm> expression)
        {
            var index = -1;
            IList<ITerm> innerExpression = expression.ToList();

            var nestedExpressions = new List<NestedExpression>
            {
                new NestedExpression(index, innerExpression)
            };

            do
            {
                index = GetExpressionTermPositionNumber(innerExpression);
                if (index >= 0)
                {
                    var foundInnerExpression = (ExpressionTerm)innerExpression[index];
                    var innerSign = foundInnerExpression.Sign;
                    innerExpression = foundInnerExpression.Expression;

                    nestedExpressions.Add(new NestedExpression(index, innerExpression, innerSign));
                }
            } while (index >= 0);

            return nestedExpressions;
        }

        private List<NestedExpression> CheckLastExpressionsOnNestedExpressions(List<NestedExpression> nestedExpressions)
        {
            int index = nestedExpressions.Count - 1;
            var innerExpression = nestedExpressions[index].Expression;

            do
            {
                index = GetExpressionTermPositionNumber(innerExpression);
                if (index >= 0)
                {
                    var foundInnerExpression = innerExpression[index] as ExpressionTerm;
                    var innerSign = foundInnerExpression.Sign;
                    innerExpression = foundInnerExpression.Expression;

                    nestedExpressions.Add(new NestedExpression(index, innerExpression, innerSign));
                }

            } while (index >= 0);
          

            return nestedExpressions;
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

            return -1;
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

        public NumberTerm CalculateInnerExpression(IList<ITerm> expression, ArithmeticSign? signExpression = null)
        {
            var numbers = expression.Cast<NumberTerm>().ToList();

            for (int i = 0; numbers.Count != 1; i++)
            {
                var index = GetHighestProirityPositionNumber(numbers);

                numbers[index - 1] = numbers[index - 1].MakeOperation(numbers[index]);
                numbers.RemoveAt(index);
            }

            if (signExpression != null)
            {
                numbers[0].Sign = signExpression.Value;
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

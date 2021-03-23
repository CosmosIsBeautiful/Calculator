using Calculator.Arithmetic.Enums;
using Calculator.Arithmetic.Helpers;
using Calculator.Arithmetic.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.Arithmetic
{
    public interface IFormatting
    {
        List<ITerm> GetParsedExpression(string str);
    }

    public class ArithmeticFormatting: IFormatting
    {
        public List<ITerm> GetParsedExpression(string str)
        {
            str = AddSignIfMissing(str);

            List<ITerm> expression = new List<ITerm>();

            bool isNumber = false;
            int startNumber = 0;
            ArithmeticSign sign = ArithmeticSign.Sum;
            ArithmeticSign signExpression = ArithmeticSign.Sum;

            int inner = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (ArithmeticSignHelpers.IsArithmeticSign(str[i]) || str[i].Equals('(') || str[i].Equals(')'))
                {
                    if (str[i].Equals('('))
                    {
                        inner++;
                        signExpression = sign;
                        isNumber = false;
                        continue;
                    }

                    if (str[i].Equals(')') && str[i - 1].Equals(')'))
                    {
                        inner--;
                        continue;
                    }

                    if (str[i].Equals(')'))
                    {
                        isNumber = true;
                    }

                    if (isNumber)
                    {
                        var numberTerm = CreateNumberTerm(str, i, startNumber, sign);

                        if (inner == 0)
                        {
                            expression.Add(numberTerm);
                        }
                        else if (inner == 1)
                        {
                            ExpressionTerm innerExpression = GetOrCreateFirstInnerExpression(expression, signExpression);

                            innerExpression.Expression.Add(numberTerm);
                        }
                        else if (inner > 1)
                        {
                            IList<ITerm> innerExpression = GetOrCreateSubsequentInnerExpression(expression, inner, signExpression);

                            innerExpression.Add(numberTerm);
                        }

                        if (str[i].Equals(')'))
                        {
                            inner--;
                            i++;
                        }

                        isNumber = false;
                        i--;
                    }
                    else
                    {
                        isNumber = true;
                        startNumber = i + 1;

                        sign = ArithmeticSignHelpers.GetArithmeticSignType(str[i]);
                    }
                }
            }

            if (str[str.Length - 1].Equals(')') == false)
            {
                var lastNumberTerm = CreateNumberTerm(str, str.Length, startNumber, sign);
                expression.Add(lastNumberTerm);
            }

            return expression;
        }

        private string AddSignIfMissing(string str)
        {
            if (ArithmeticSignHelpers.IsArithmeticSign(str[0]) == false)
            {
                str = $"{ArithmeticSign.Sum.GetSign()}{str}";
            }

            return str;
        }

        private NumberTerm CreateNumberTerm(string str, int i, int startIndexNumber, ArithmeticSign sign)
        {
            string stringNumber = str.Substring(startIndexNumber, i - startIndexNumber);
            int number = Int32.Parse(stringNumber);

            return new NumberTerm(number, sign);
        }


        private ExpressionTerm GetOrCreateFirstInnerExpression(List<ITerm> expression, ArithmeticSign signExpression)
        {
            ExpressionTerm innerExpression;
            if (expression?.Any() == true && expression.Last() is ExpressionTerm expressionLast)
            {
                innerExpression = expressionLast;
            }
            else
            {
                innerExpression = new ExpressionTerm(signExpression);
                expression.Add(innerExpression);
            }

            return innerExpression;
        }

        private IList<ITerm> GetOrCreateSubsequentInnerExpression(List<ITerm> expression, int inner, ArithmeticSign signExpression)
        {
            IList<ITerm> innerExpression = ((ExpressionTerm)expression.Last()).Expression;

            for (int j = 1; j < inner; j++)
            {
                if (innerExpression.Last() is ExpressionTerm expressionLast)
                {
                    innerExpression = expressionLast.Expression;
                }
                else
                {
                    var newExpression = new ExpressionTerm(signExpression);
                    innerExpression.Add(newExpression);

                    innerExpression = newExpression.Expression;
                }
            }

            return innerExpression;
        }
    }
}

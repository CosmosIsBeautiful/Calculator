using Calculator.Arithmetic.Enums;
using Calculator.Arithmetic.Helpers;
using Calculator.Arithmetic.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Calculator.Arithmetic
{
    public interface IFormatting
    {
        string GetNormalizationExpressionString(string equation);

        bool IsValidExpression(string equation);

        List<ITerm> GetParsedExpression(string str);
    }

    public class ArithmeticFormatting: IFormatting
    {

        #region GetNormalizationExpressionString
        public string GetNormalizationExpressionString(string equationString)
        {
            var withoutSpacesEquation = equationString.Replace(" ", string.Empty);
            var equation = new StringBuilder(withoutSpacesEquation);

            equation = GetFixEquationFirstSign(equation);

            for (int i = 1; i < equation.Length; i++)
            {
                if (equation[i].IsStartBracket())
                {
                    equation = GetFixEquationBeforeBracket(equation, ref i);

                    equation = GetFixEquationAfterBracket(equation, ref i);
                }
            }
            
            return equation.ToString();
        }

        private static StringBuilder GetFixEquationFirstSign(StringBuilder equation)
        {
            if (equation[0].IsArithmeticSign() == false || equation[0].IsNegativeNumber())
            {
                return equation.Insert(0, '+');
            }

            return equation;
        }

        private static StringBuilder GetFixEquationBeforeBracket(StringBuilder equation, ref int i)
        {
            StringBuilder fixEquation = equation;
            if (equation[i - 1].IsArithmeticSign() == false)
            {
                fixEquation = equation.Insert(i, '*');
                i++;
                return fixEquation;
            }

            return fixEquation;
        }

        private static StringBuilder GetFixEquationAfterBracket(StringBuilder equation, ref int i)
        {
            StringBuilder fixEquation = equation;

            if (equation[i + 1].IsArithmeticSign() == false || equation[i + 1].IsNegativeNumber())
            {
                fixEquation = equation.Insert(i + 1, '+');
                i++;
                return fixEquation;
            }

            return fixEquation;
        }
        #endregion

        #region IsValidExpression
        public bool IsValidExpression(string equation)
        {
            Regex rgx = new Regex(@"^(([\+\-\*\/][(])*[\+\-\*\/][-]?\d+[,]?\d*[)]*)+$");
            bool isRgxValid = rgx.IsMatch(equation);

            bool isBracketsValid = IsBracketsValidExpression(equation);

            return (isRgxValid && isBracketsValid);
        }

        private static bool IsBracketsValidExpression(string equation)
        {
            int countBrackets = 0;
            foreach (char symbol in equation)
            {
                if (symbol.IsStartBracket())
                {
                    countBrackets++;
                }

                if (symbol.IsEndBracket())
                {
                    countBrackets--;
                }
            }

            return (countBrackets == 0);
        }
        #endregion

        #region GetParsedExpression
        public List<ITerm> GetParsedExpression(string str)
        {
            List<ITerm> expression = new List<ITerm>();

            bool isNumber = false;
            bool isNegativeNumber = false;

            int startNumberIndex = 0;

            var sign = ArithmeticSign.Sum;
            var signExpression = ArithmeticSign.Sum;

            int nestedExpressionCount = 0;
            bool isNewNestedExpression = false;

            for (int i = 0; i < str.Length; i++)
            {
                var isStartBracket = str[i].IsStartBracket();
                var isEndBracket = str[i].IsEndBracket();
                var isOperation = ( str[i].IsArithmeticSign() || isStartBracket || isEndBracket );

                if (isOperation)
                {
                    if (isStartBracket)
                    {
                        nestedExpressionCount++;
                        signExpression = sign;
                        isNumber = false;
                        continue;
                    }

                    if (isEndBracket && str[i - 1].IsEndBracket())
                    {
                        nestedExpressionCount--;
                        if (nestedExpressionCount == 0)
                        {
                            isNewNestedExpression = true;
                        }
                        continue;
                    }

                    if (isNumber || isEndBracket)
                    {
                        if (startNumberIndex == i)
                        {
                            startNumberIndex++;
                            isNegativeNumber = true;
                            continue;
                        }

                        var numberTerm = CreateNumberTerm(str, i, startNumberIndex, sign, isNegativeNumber);

                        if (nestedExpressionCount == 0)
                        {
                            expression.Add(numberTerm);
                        }
                        else if (nestedExpressionCount == 1)
                        {
                            ExpressionTerm innerExpression = GetOrCreateFirstInnerExpression(expression, signExpression, isNewNestedExpression);
                            innerExpression.Expression.Add(numberTerm);
                            isNewNestedExpression = false;
                        }
                        else if (nestedExpressionCount > 1)
                        {
                            IList<ITerm> innerExpression = GetOrCreateSubsequentInnerExpression(expression, nestedExpressionCount, signExpression);
                            innerExpression.Add(numberTerm);
                            isNewNestedExpression = false;
                        }

                        if (isEndBracket)
                        {
                            nestedExpressionCount--;
                            if (nestedExpressionCount == 0)
                            {
                                isNewNestedExpression = true;
                            }

                            i++;
                        }

                        isNumber = false;
                        i--;
                    }
                    else
                    {
                        isNumber = true;
                        isNegativeNumber = false;
                        startNumberIndex = i + 1;

                        sign = ArithmeticSignHelpers.GetArithmeticSignType(str[i]);
                    }
                }
            }

            if (str[^1].IsEndBracket() == false)
            {
                var lastNumberTerm = CreateNumberTerm(str, str.Length, startNumberIndex, sign, isNegativeNumber);
                expression.Add(lastNumberTerm);
            }

            return expression;
        }

        private static NumberTerm CreateNumberTerm(string str, int i, int startIndexNumber, ArithmeticSign sign, bool isNegativeNumber = false)
        {
            string stringNumber = str.Substring(startIndexNumber, i - startIndexNumber);
            int number = int.Parse(stringNumber);
            
            if (isNegativeNumber)
            {
                number *= -1;
            }

            return new NumberTerm(sign, number);
        }

        private static ExpressionTerm GetOrCreateFirstInnerExpression(List<ITerm> expression, ArithmeticSign signExpression, bool isNewNestedExpression)
        {
            ExpressionTerm innerExpression;
            if (expression?.Any() == true && expression.Last() is ExpressionTerm expressionLast && isNewNestedExpression == false)
            {
                innerExpression = expressionLast;
            }
            else
            {
                innerExpression = new ExpressionTerm(signExpression);
                expression!.Add(innerExpression);
            }

            return innerExpression;
        }

        private static IList<ITerm> GetOrCreateSubsequentInnerExpression(List<ITerm> expression, int inner, ArithmeticSign signExpression)
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
        #endregion
    }
}

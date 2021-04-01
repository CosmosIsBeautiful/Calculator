using Calculator.Arithmetic.Enums;
using Calculator.Arithmetic.Helpers;
using Calculator.Arithmetic.Models;
using System;
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
        public string GetNormalizationExpressionString(string equationString)
        {
            var withoutSpacesEquation = equationString.Replace(" ", string.Empty);
            StringBuilder equation = new StringBuilder(withoutSpacesEquation);

            equation = GetFixEquationFirstSign(equation);

            for (int i = 1; i < equation.Length; i++)
            {
                if (equation[i].Equals('('))
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

        public bool IsValidExpression(string equation)
        {
            Regex rgx = new Regex(@"^(([\+\-\*\/][(])*[\+\-\*\/][-]?\d+[,]?\d*[)]*)+$");
            bool isRgxValid = rgx.IsMatch(equation);

            bool isBracketsValid = IsBracketsValidExpression(equation);

            return (isRgxValid && isBracketsValid);
        }

        public List<ITerm> GetParsedExpression(string str)
        {
            str = AddSignIfMissing(str);

            List<ITerm> expression = new List<ITerm>();

            bool isNumber = false;
            int startNumber = 0;
            ArithmeticSign sign = ArithmeticSign.Sum;
            ArithmeticSign signExpression = ArithmeticSign.Sum;
            bool isNegativeNumber = false;

            int inner = 0;

            bool isOperation;
            bool isStartBracket;
            bool isEndBracket;

            for (int i = 0; i < str.Length; i++)
            {
                isStartBracket = str[i].Equals('(');
                isEndBracket = str[i].Equals(')');
                isOperation = ( str[i].IsArithmeticSign() || isStartBracket || isEndBracket );

                if (isOperation)
                {
                    if (isStartBracket)
                    {
                        inner++;
                        signExpression = sign;
                        isNumber = false;
                        continue;
                    }

                    if (isEndBracket && str[i - 1].Equals(')'))
                    {
                        inner--;
                        continue;
                    }

                    if (isNumber || isEndBracket)
                    {
                        if (startNumber == i)
                        {
                            startNumber++;
                            isNegativeNumber = true;
                            continue;
                        }

                        var numberTerm = CreateNumberTerm(str, i, startNumber, sign, isNegativeNumber);

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

                        if (isEndBracket)
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
                        isNegativeNumber = false;
                        startNumber = i + 1;

                        sign = ArithmeticSignHelpers.GetArithmeticSignType(str[i]);
                    }
                }
            }

            if (str[str.Length - 1].Equals(')') == false)
            {
                var lastNumberTerm = CreateNumberTerm(str, str.Length, startNumber, sign, isNegativeNumber);
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

        private NumberTerm CreateNumberTerm(string str, int i, int startIndexNumber, ArithmeticSign sign, bool isNegativeNumber = false)
        {
            string stringNumber = str.Substring(startIndexNumber, i - startIndexNumber);
            int number = Int32.Parse(stringNumber);
            
            if (isNegativeNumber)
            {
                number *= -1;
            }

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

        private bool IsBracketsValidExpression(string equation)
        {
            int countBrackets = 0;
            for (int i = 0; i < equation.Length; i++)
            {
                if (equation[i] == '(')
                {
                    countBrackets++;
                }

                if (equation[i] == ')')
                {
                    countBrackets--;
                }
            }

            return (countBrackets == 0);
        }
    }
}

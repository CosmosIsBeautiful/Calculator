using Calculator.Arithmetic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Calculator.Arithmetic.Tests
{
    [TestClass]
    public class ArithmeticFormattingTests
    {
        private static ArithmeticFormatting Formatting { get; set; }

        [AssemblyInitialize]
        public static void InitializeTests(TestContext testContext)
        {
            Formatting = new ArithmeticFormatting();
        }

        public Dictionary<string, List<ITerm>> CreateExpressionDictionary()
        {
            Dictionary<string, List<ITerm>> expressions = new Dictionary<string, List<ITerm>>
            {
                 { "+8+(+7/(+1+3)-5)-9",
                    new List<ITerm>
                    {   new NumberTerm('+', 8),
                        new ExpressionTerm('+',
                            new List<ITerm>
                            {
                                new NumberTerm('+', 7),
                                new ExpressionTerm('/',
                                    new List<ITerm>
                                    {
                                        new NumberTerm('+', 1),
                                        new NumberTerm('+', 3)
                                    }
                                ),
                                new NumberTerm('-', 5),
                            }
                        ),
                        new NumberTerm('-', 9)
                    }
                },
                { "+2+2", new List<ITerm> { new NumberTerm('+', 2), new NumberTerm('+', 2) } },
                { "+2*(-4/1+(-6/2))",
                    new List<ITerm>
                    {
                        new NumberTerm('+', 2), new ExpressionTerm('*',
                            new List<ITerm>
                            {
                                new NumberTerm('-', 4) , new NumberTerm('/', 1), new ExpressionTerm('+',
                                    new List<ITerm>
                                    {
                                        new NumberTerm('-', 6),  new NumberTerm('/',2)
                                    }
                                )
                            }
                        )
                    }
                },
                { "+(-5+7)*9/3",
                    new List<ITerm>
                    {
                        new ExpressionTerm('+',
                            new List<ITerm>
                            {
                                new NumberTerm('-', 5), new NumberTerm('+', 7)
                            }
                        ),
                        new NumberTerm('*', 9), new NumberTerm('/', 3)
                    }
                },
                { "+(-5+(+7*9)/3",
                    new List<ITerm>
                    {
                        new ExpressionTerm('+',
                            new List<ITerm>
                            {
                                new NumberTerm('-', 5), new ExpressionTerm('+',
                                    new List<ITerm>
                                    {
                                        new NumberTerm('+', 7), new NumberTerm('*', 9)
                                    }
                                )
                            }
                        ),
                        new NumberTerm('/', 3)
                    }
                },
                { "+8+(+2/1)-9",
                    new List<ITerm>
                    {   new NumberTerm('+', 8),
                        new ExpressionTerm('+',
                            new List<ITerm>
                            {
                                new NumberTerm('+', 2),
                                new NumberTerm('/', 1)
                            }
                        ),
                        new NumberTerm('-', 9)
                    }
                },
                { "+8+(+7/(+1+3))-9",
                    new List<ITerm>
                    {   new NumberTerm('+', 8),
                        new ExpressionTerm('+',
                            new List<ITerm>
                            {
                                new NumberTerm('+', 7),
                                new ExpressionTerm('/',
                                    new List<ITerm>
                                    {
                                        new NumberTerm('+', 1),
                                        new NumberTerm('+', 3)
                                    }
                                ),
                            }
                        ),
                        new NumberTerm('-', 9)
                    }
                },
                { "+(+100-(-5+(+7*9)/3)-100",
                    new List<ITerm>
                    {
                        new ExpressionTerm('+',
                            new List<ITerm>
                            {
                                new NumberTerm('+', 100),
                                new ExpressionTerm('-',
                                    new List<ITerm>
                                    {
                                        new NumberTerm('-', 5), new ExpressionTerm('+',
                                            new List<ITerm>
                                            {
                                                new NumberTerm('+', 7), new NumberTerm('*', 9)
                                            }
                                        ),
                                        new NumberTerm('/', 3)
                                    }
                                )
                            }
                        ), new NumberTerm('-', 100)
                    }
                },
                { "+105", new List<ITerm>  { new NumberTerm('+', 105) } },
                { "-5+7*9/3",
                    new List<ITerm>
                    {
                        new NumberTerm('-', 5), new NumberTerm('+', 7), new NumberTerm('*', 9), new NumberTerm('/', 3)
                    }
                },
                //{ "50*785+578001/453+1", new List<NumberTerm>  { new NumberTerm('+', 50), new NumberTerm('*', 785), new NumberTerm('+', 578001), new NumberTerm('/', 453), new NumberTerm('+', 1) } },
                //{ "-54/87+78*15", new List<NumberTerm>  { new NumberTerm('-', 54), new NumberTerm('/', 87), new NumberTerm('+', 78), new NumberTerm('*', 15) } }
            };

            return expressions;
        }

        [TestMethod]
        public void GetParsedExpression_CorrectlyParsingExpression()
        {
            //act
            Dictionary<string, List<ITerm>> expressions = CreateExpressionDictionary();

            foreach (var item in expressions)
            {
                //arrange
                var actual = Formatting.GetParsedExpression(item.Key);

                Assert.IsTrue(this.AreEqual(item.Value, actual, item.Key));
            }
        }

        private bool AreEqual(IList<ITerm> data, IList<ITerm> actual, string expression)
        {
            var flag = true;

            for (int i = 0; i < actual.Count; i++)
            {
                if (actual[i] is ExpressionTerm)
                {
                    flag = this.AreEqual(((ExpressionTerm)data[i]).Expression, ((ExpressionTerm)actual[i]).Expression, expression);
                }
                else
                {
                    Assert.AreEqual(data[i], actual[i], $"\nExpression: {expression}");
                }
            }

            return flag;
        }

        [DataTestMethod]
        [DataRow("+2+2", true)]
        [DataRow("+33/-5", true)]
        [DataRow("+70/-1,4", true)]
        [DataRow("+985/5000", true)]
        [DataRow("+(+2)/-2", true)]
        [DataRow("+(+(+2-1)-2)", true)]
        [DataRow("+(+7/1+(+2-1))", true)]
        [DataRow("+(+2+2)", true)]
        [DataRow("+(+5*4)*-65", true)]
        [DataRow("+(+(+2+2)+6)", true)]
        [DataRow("+(+1*2)/(+12/6)", true)]
        [DataRow("+(+5+(+2+2)-5)", true)]
        [DataRow("+(-2+2)-(+(+0+6))", true)]
        [DataRow("+8+(+7/(+1+3)-5)-9", true)]
        [DataRow("+42+18/(+6+12/-4)", true)]
        [DataRow("+12/6+10,34-(+5*(+6+2))+2,7", true)]
        [DataRow("-10+9*8/-4*60-90/-2*(-4/2)+(+55+9*6)+100", true)]
        [DataRow("+(+100/5+6*2+(+44-22/-10))+(+84*56/(+12/6)+90-(-5*-5-1))", true)]
        public void IsValidExpression_ValidExpressions(string str, bool isValidExpected)
        {
            //arrange
            var isValidActual = Formatting.IsValidExpression(str);

            //assert
            Assert.AreEqual(isValidExpected, isValidActual, $"Expression: {str}. Actual: {isValidActual} != Expected: {isValidExpected}");
        }

        [DataTestMethod]
        [DataRow("2+2", false)]
        [DataRow(" 2 + 2", false)]
        [DataRow("+70/- 1,4", false)]
        [DataRow("(+2)-2", false)]
        [DataRow("+(+(+5)-5", false)]
        [DataRow("+(+5)-5)", false)]
        [DataRow("+(+(2-1)+-2)", false)]
        [DataRow("(7/1+(2-1))", false)]
        [DataRow("+( +7 / 1 + ( +2 - 1 ) )", false)]
        [DataRow("+(+5*(+9-(+15-(+20+20)-5)", false)]
        public void IsValidExpression_InvalidExpressions(string str, bool isValidExpected)
        {
            //arrange
            var isValidActual = Formatting.IsValidExpression(str);

            //assert
            Assert.AreEqual(isValidExpected, isValidActual, $"Expression: {str}. Actual: {isValidActual} != Expected: {isValidExpected}");
        }

        [DataTestMethod]
        [DataRow("+ 2 + 2 ", "+2+2")]
        [DataRow(" 5 + 5 ", "+5+5")]
        [DataRow("-5+-5 ", "+-5+-5")]
        [DataRow("(-7*-7)", "+(+-7*-7)")]
        [DataRow("+4 (+4 + 4 )", "+4+(+4+4)")]
        [DataRow("-1 (+14 + 45 )", "+-1+(+14+45)")]
        [DataRow("  1 (  14 + 45 )", "+1+(+14+45)")]
        [DataRow("  9 (  9 + 9 ) + 9", "+9+(+9+9)+9")]
        public void GetNormalizationExpressionString_CorrectNormalization(string str, string expectedEquation)
        {
            //arrange
            var actualEquation = Formatting.GetNormalizationExpressionString(str);

            //assert
            Assert.AreEqual(expectedEquation, actualEquation, $"Actual: {actualEquation} != Expected: {expectedEquation}");

        }
    }
}

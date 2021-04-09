using Calculator.Arithmetic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Calculator.Arithmetic.Tests
{
    [TestClass]
    public class ArithmeticFormattingTests
    {
        private static ArithmeticFormatting Formatting { get; set; }

        [ClassInitialize]
        public static void InitializeTests(TestContext testContext)
        {
            Formatting = new ArithmeticFormatting();
        }

        #region GetParsedExpression
        [DataTestMethod]
        [DynamicData(nameof(GetDataForGetParsedExpressionTest), DynamicDataSourceType.Method)]
        public void GetParsedExpressionTest_CorrectlyParsingExpression(string expressionString, List<ITerm> expectedExpression)
        {
            var actualExpression = Formatting.GetParsedExpression(expressionString);

            Assert.IsTrue(AreEqualExpression(expectedExpression, actualExpression), $"Incorrect expression: {expressionString}");
        }
        private static IEnumerable<object[]> GetDataForGetParsedExpressionTest()
        {
            yield return new object[]
            { 
                "+-2*-2",
                new List<ITerm> { new NumberTerm('+', -2), new NumberTerm('*', -2) }
            };
            yield return new object[]
            { 
                "+2+2",
                new List<ITerm> {new NumberTerm('+', 2), new NumberTerm('+', 2)}
            };
            yield return new object[]
            { 
                "+5*(+-2*-2)",
                new List<ITerm>
                {
                    new NumberTerm('+', 5),
                    new ExpressionTerm('*',
                        new List<ITerm>
                        {
                            new NumberTerm('+', -2),
                            new NumberTerm('*', -2),
                        }
                    )
                }
            };
            yield return new object[]
            { 
                "+8*-9",
                new List<ITerm>
                {   new NumberTerm('+', 8),
                    new NumberTerm('*', -9)
                }
            };
            yield return new object[]
            { 
                "+8+(+7/(+1+3)*-5)/-9",
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
                            new NumberTerm('*', -5),
                        }
                    ),
                    new NumberTerm('/', -9)
                }
            };
            yield return new object[]
            { 
                "+2*(+-4/-1+(-6/-2))",
                new List<ITerm>
                {
                    new NumberTerm('+', 2), new ExpressionTerm('*',
                        new List<ITerm>
                        {
                            new NumberTerm('+', -4) , new NumberTerm('/', -1), new ExpressionTerm('+',
                                new List<ITerm>
                                {
                                    new NumberTerm('-', 6),  new NumberTerm('/', -2)
                                }
                            )
                        }
                    )
                }
            };
            yield return new object[]
            { 
                "+(+-5+7)*9/3",
                new List<ITerm>
                {
                    new ExpressionTerm('+',
                        new List<ITerm>
                        {
                            new NumberTerm('+', -5), new NumberTerm('+', 7)
                        }
                    ),
                    new NumberTerm('*', 9), new NumberTerm('/', 3)
                }
            };
            yield return new object[]
            { 
                "+(+-5+(+7*9)/3",
                new List<ITerm>
                {
                    new ExpressionTerm('+',
                        new List<ITerm>
                        {
                            new NumberTerm('+', -5), new ExpressionTerm('+',
                                new List<ITerm>
                                {
                                    new NumberTerm('+', 7), new NumberTerm('*', 9)
                                }
                            )
                        }
                    ),
                    new NumberTerm('/', 3)
                }
            };
            yield return new object[]
            { 
                "+8+(+2/1)-9",
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
            };
            yield return new object[]
            { 
                "+8+(+7/(+1+3))-9",
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
            };
            yield return new object[]
            { 
                "+(+100-(+-5+(+7*9)/3)-100",
                new List<ITerm>
                {
                    new ExpressionTerm('+',
                        new List<ITerm>
                        {
                            new NumberTerm('+', 100),
                            new ExpressionTerm('-',
                                new List<ITerm>
                                {
                                    new NumberTerm('+', -5), new ExpressionTerm('+',
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
            };
            yield return new object[]
            { 
                "+105",
                new List<ITerm>  { new NumberTerm('+', 105) }
            };
            yield return new object[]
            {
                "+-5+7*9/3",
                new List<ITerm>
                {
                    new NumberTerm('+', -5), new NumberTerm('+', 7), new NumberTerm('*', 9), new NumberTerm('/', 3)
                }
            };
        }

        private static bool AreEqualExpression(IList<ITerm> expectedExpression, IList<ITerm> actualExpression)
        {
            var flag = true;

            for (int i = 0; i < actualExpression.Count; i++)
            {
                if (actualExpression[i] is ExpressionTerm)
                {
                    flag = AreEqualExpression(((ExpressionTerm)expectedExpression[i]).Expression, ((ExpressionTerm)actualExpression[i]).Expression);
                }
                else
                {
                    Assert.AreEqual(expectedExpression[i], actualExpression[i]);
                }
            }

            return flag;
        }
        #endregion

        #region IsValidExpression
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
        public void IsValidExpressionTest_ValidExpressions(string str, bool isValidExpected)
        {
            var isValidActual = Formatting.IsValidExpression(str);

            Assert.AreEqual(isValidExpected, isValidActual, $"Invalid expression validation");
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
        public void IsValidExpressionTest_InvalidExpressions(string str, bool isValidExpected)
        {
            var isValidActual = Formatting.IsValidExpression(str);

            Assert.AreEqual(isValidExpected, isValidActual, $"Invalid expression validation");
        }
        #endregion

        #region GetNormalizationExpressionString
        [DataTestMethod]
        [DataRow("+ 2 + 2 ", "+2+2")]
        [DataRow(" 5 + 5 ", "+5+5")]
        [DataRow("-5+-5 ", "+-5+-5")]
        [DataRow("-5-5 ", "+-5-5")]
        [DataRow("(-7*-7)", "+(+-7*-7)")]
        [DataRow("54/(5*2)", "+54/(+5*2)")]
        [DataRow("+4 (+4 + 4 )", "+4*(+4+4)")]
        [DataRow("-1 (+14 + 45 )", "+-1*(+14+45)")]
        [DataRow("  1 (14 + 45 )", "+1*(+14+45)")]
        [DataRow("  9 (  9 + 9 ) + 9", "+9*(+9+9)+9")]
        [DataRow(" + 5(78,74*-8/(45-20/2)(2+5))-8", "+5*(+78,74*-8/(+45-20/2)*(+2+5))-8")]
        public void GetNormalizationExpressionStringTest_CorrectNormalization(string str, string expectedEquation)
        {
            var actualEquation = Formatting.GetNormalizationExpressionString(str);

            Assert.AreEqual(expectedEquation, actualEquation, $"Incorrect expression normalization");
        }
        #endregion
    }
}

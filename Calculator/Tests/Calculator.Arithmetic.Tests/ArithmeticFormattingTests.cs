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

        [TestMethod]
        public void GetParsedExpression_CorrectlyParsingExpression()
        {
            //act
            Dictionary<string, List<ITerm>> data = new Dictionary<string, List<ITerm>>
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

            foreach (var item in data)
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
    }
}

using Calculator.Arithmetic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Calculator.Arithmetic.Tests
{
    [TestClass]
    public class ArithmeticCalculateTests
    {
        private static ICalculate Calculate { get; set; }

        [ClassInitialize]
        public static void InitializeTests(TestContext testContext)
        {
            Calculate = new ArithmeticCalculate();
        }

        #region CalculateInnerExpression
        [TestMethod]
        public void CalculateInnerExpression_CorrectSolve()
        {
            Dictionary<NumberTerm, List<ITerm>> expressions = new Dictionary<NumberTerm, List<ITerm>>
            {
                { new NumberTerm('+', 65), new List<ITerm>
                    {
                        new NumberTerm('+', 50),
                        new NumberTerm('+', 5),
                        new NumberTerm('*', 3),
                    }
                },
                { new NumberTerm('+', 5), new List<ITerm>
                    {
                        new NumberTerm('+', -12),
                        new NumberTerm('*', 7),
                        new NumberTerm('+', 3),
                        new NumberTerm('*', 30),
                        new NumberTerm('-', 1),
                    }
                },
                { new NumberTerm('+', -78.8M), new List<ITerm>
                    {
                        new NumberTerm('+', -12),
                        new NumberTerm('-', 4),
                        new NumberTerm('*', 91),
                        new NumberTerm('/', 5),
                        new NumberTerm('+', 10),
                        new NumberTerm('-', 2),
                        new NumberTerm('*', 2),
                    }
                },
                { new NumberTerm('+', 29.6M), new List<ITerm>
                    {

                        new NumberTerm('+', 74),
                        new NumberTerm('/', 2),
                        new NumberTerm('/', 10),
                        new NumberTerm('*', 8),
                    }
                },
                { new NumberTerm('+', 240), new List<ITerm>
                    {
                        new NumberTerm('+', -60),
                        new NumberTerm('*', -18),
                        new NumberTerm('/', 2),
                        new NumberTerm('-', 300),
                    }
                },
            };

            foreach (var item in expressions)
            {
                //arrange
                var actual = Calculate.CalculateInnerExpression(item.Value);

                Assert.AreEqual(item.Key, actual, $" Actual: {actual.Sign} {actual.Digit} ! = Expression: {item.Key.Sign} {item.Key.Digit}");
            }
        }
        #endregion

        #region Calculate
        [TestMethod]
        public void Calculate_CorrectSolve()
        {
            Dictionary<decimal, List<ITerm>> expressions = CreateAnswerAndExpressionDictionary();

            foreach (var item in expressions)
            {
                //arrange
                var actualSolved = Calculate.Calculate(item.Value);

                //assert
                Assert.AreEqual(item.Key, actualSolved, $"Expert solved: {expressions.Keys} != Actual solved {actualSolved}");
            }
        }

        private Dictionary<decimal, List<ITerm>> CreateAnswerAndExpressionDictionary()
        {
            Dictionary<decimal, List<ITerm>> expressions = new Dictionary<decimal, List<ITerm>>
            {
                //+2+2
                { 4, new List<ITerm> { new NumberTerm('+', 2), new NumberTerm('+', 2) } },
                //-118*(7+15+(9*(4*3))*6*(7-2))/4
                { -96229,
                    new List<ITerm>
                    {   new NumberTerm('+', -118),
                        new ExpressionTerm('*',
                            new List<ITerm>
                            {
                                new NumberTerm('+', 7),
                                new NumberTerm('+', 15),
                                new ExpressionTerm('+',
                                    new List<ITerm>
                                    {
                                        new NumberTerm('+', 9),
                                        new ExpressionTerm('*',
                                            new List<ITerm>
                                            {
                                                new NumberTerm('+', 4),
                                                new NumberTerm('*', 3)
                                            }
                                        ),
                                    }
                                ),
                                new NumberTerm('*', 6),
                                new ExpressionTerm('*',
                                    new List<ITerm>
                                    {
                                        new NumberTerm('+', 7),
                                        new NumberTerm('-', 2)
                                    }
                                ),
                            }
                        ),
                        new NumberTerm('/', 4),
                    }
                },
                //-105
                { -105, new List<ITerm>  { new NumberTerm('+', -105) } },
                //+88+(2*10)-94/2+25*(200+15+(2+1))+(985/10)-42
                { 5567.5M,
                    new List<ITerm>
                    {   new NumberTerm('+', 88),
                        new ExpressionTerm('+',
                            new List<ITerm>
                            {
                                new NumberTerm('+', 2),
                                new NumberTerm('*', 10)
                            }
                        ),
                        new NumberTerm('-', 94),
                        new NumberTerm('/', 2),
                        new NumberTerm('+', 25),
                        new ExpressionTerm('*',
                            new List<ITerm>
                            {
                                new NumberTerm('+', 200),
                                new NumberTerm('+', 15),
                                new ExpressionTerm('+',
                                    new List<ITerm>
                                    {
                                        new NumberTerm('+', 2),
                                        new NumberTerm('+', 1)
                                    }
                                ),
                            }
                        ),
                        new ExpressionTerm('+',
                            new List<ITerm>
                            {
                                new NumberTerm('+', 985),
                                new NumberTerm('/', 10)
                            }
                        ),
                        new NumberTerm('-', 42),
                    }
                },
                //8+(7/(50+5*3)-5)-9
                { -5.8923076923076923076923076923M,
                    new List<ITerm>
                    {
                        new NumberTerm('+', 8),
                        new ExpressionTerm('+',
                            new List<ITerm>
                            {
                                new NumberTerm('+', 7),
                                new ExpressionTerm('/',
                                    new List<ITerm>
                                    {
                                        new NumberTerm('+', 50),
                                        new NumberTerm('+', 5),
                                        new NumberTerm('*', 3),
                                    }
                                ),
                                new NumberTerm('-', 5),
                            }
                        ),
                        new NumberTerm('-', 9)
                    }
                },
                //2*(-4/1+(-6/2))
                { -14,
                    new List<ITerm>
                    {
                        new NumberTerm('+', 2), new ExpressionTerm('*',
                            new List<ITerm>
                            {
                                new NumberTerm('+', -4) , new NumberTerm('/', 1), new ExpressionTerm('+',
                                    new List<ITerm>
                                    {
                                        new NumberTerm('+', -6),  new NumberTerm('/',2)
                                    }
                                )
                            }
                        )
                    }
                },
                //(-5+7)*9/3
                { 6,
                    new List<ITerm>
                    {
                        new ExpressionTerm('+',
                            new List<ITerm>
                            {
                                new NumberTerm('+', -5), new NumberTerm('+', 7)
                            }
                        ),
                        new NumberTerm('*', 9),
                        new NumberTerm('/', 3)
                    }
                },
                //(-5+(7*9))/3
                { 3,
                    new List<ITerm>
                    {
                        new ExpressionTerm('+',
                            new List<ITerm>
                            {
                                new NumberTerm('+', -5), new ExpressionTerm('+',
                                    new List<ITerm>
                                    {
                                        new NumberTerm('+', 7), new NumberTerm('*', 2)
                                    }
                                )
                            }
                        ),
                        new NumberTerm('/', 3)
                    }
                },
                //8+(33/11)-9*43
                { -376,
                    new List<ITerm>
                    {   new NumberTerm('+', 8),
                        new ExpressionTerm('+',
                            new List<ITerm>
                            {
                                new NumberTerm('+', 33),
                                new NumberTerm('/', 11)
                            }
                        ),
                        new NumberTerm('-', 9),
                        new NumberTerm('*', 43),
                    }
                },
                //+8900+(25/(-100+34))*98
                { 8862.878787878787878787878788M,
                    new List<ITerm>
                    {   new NumberTerm('+', 8900),
                        new ExpressionTerm('+',
                            new List<ITerm>
                            {
                                new NumberTerm('+', 25),
                                new ExpressionTerm('/',
                                    new List<ITerm>
                                    {
                                        new NumberTerm('+', -100),
                                        new NumberTerm('+', 34)
                                    }
                                ),
                            }
                        ),
                        new NumberTerm('*', 98)
                    }
                },
                //(100-(-5+(+7/9)/3)-100
                { 4.74074074074074074074074074M,
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
                                                new NumberTerm('+', 7), new NumberTerm('/', 9)
                                            }
                                        ),
                                        new NumberTerm('/', 3)
                                    }
                                )
                            }
                        ),
                        new NumberTerm('-', 100)
                    }
                },
                //-10+7*9/3
                { 11,
                    new List<ITerm>
                    {
                        new NumberTerm('+', -10),
                        new NumberTerm('+', 7),
                        new NumberTerm('*', 9),
                        new NumberTerm('/', 3)
                    }
                },
            };

            return expressions;
        }
        #endregion
    }
}

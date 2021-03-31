using Calculator.Arithmetic.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;

namespace Calculator.Arithmetic.Tests
{
    [TestClass]
    public class ArithmeticCalculateTests
    {
        private static IArithmeticCalculate Calculate { get; set; }

        [ClassInitialize]
        public static void InitializeTests(TestContext testContext)
        {
            Calculate = new ArithmeticCalculate();
        }

        [TestMethod]
        public void CalculateInnerExpression()
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
                { new NumberTerm('+', -78.8), new List<ITerm>
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
                { new NumberTerm('+', 29.6), new List<ITerm>
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
    }
}

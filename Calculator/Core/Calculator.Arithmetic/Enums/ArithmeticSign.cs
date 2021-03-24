using Calculator.Arithmetic.Attributes;

namespace Calculator.Arithmetic.Enums
{
    public enum ArithmeticSign
    {
        [ArithmeticSign('*', priority: 1000)]
        Mul = 1,

        [ArithmeticSign('/', priority: 1000)]
        Del,

        [ArithmeticSign('+', priority: 900)]
        Sum,

        [ArithmeticSign('-', priority: 900)]
        Sub,
    }
}

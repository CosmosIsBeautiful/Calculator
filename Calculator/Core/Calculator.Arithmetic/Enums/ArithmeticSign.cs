using Calculator.Arithmetic.Attributes;

namespace Calculator.Arithmetic.Enums
{
    public enum ArithmeticSign
    {
        [ArithmeticSign('*')]
        Mul = 1,

        [ArithmeticSign('/')]
        Del,

        [ArithmeticSign('+')]
        Sum,

        [ArithmeticSign('-')]
        Sub,
    }
}

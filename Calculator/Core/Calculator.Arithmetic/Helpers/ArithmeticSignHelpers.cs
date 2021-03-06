using Calculator.Arithmetic.Attributes;
using Calculator.Arithmetic.Enums;
using System;
using System.Reflection;

namespace Calculator.Arithmetic.Helpers
{
    public static class ArithmeticSignHelpers
    {
        public static char GetSign(this ArithmeticSign enumElement)
        {
            Type type = enumElement.GetType();
            MemberInfo[] memInfo = type.GetMember(enumElement.ToString());

            if (memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(ArithmeticSignAttribute), false);

                if (attrs.Length > 0)
                {
                    char sign = ((ArithmeticSignAttribute)attrs[0]).Sign;
                    return sign;
                }
            }

            throw new NullReferenceException("The sign has no ArithmeticSign attribute");
        }

        public static bool EqualsSign(this ArithmeticSign enumElement, char symbol)
        {
            return symbol.Equals(enumElement.GetSign());
        }

        public static bool IsArithmeticSign(char symbol)
        {
            if (ArithmeticSign.Sub.EqualsSign(symbol)
                || ArithmeticSign.Sum.EqualsSign(symbol)
                || ArithmeticSign.Mul.EqualsSign(symbol)
                || ArithmeticSign.Del.EqualsSign(symbol))
            {
                return true;
            }

            return false;
        }

        public static ArithmeticSign GetArithmeticSignType(char symbol)
        {
            if (ArithmeticSign.Sub.EqualsSign(symbol))
            {
                return ArithmeticSign.Sub;
            }
            else if (ArithmeticSign.Sum.EqualsSign(symbol))
            {
                return ArithmeticSign.Sum;
            }
            else if (ArithmeticSign.Mul.EqualsSign(symbol))
            {
                return ArithmeticSign.Mul;
            }
            else if (ArithmeticSign.Del.EqualsSign(symbol))
            {
                return ArithmeticSign.Del;
            }

            throw new ArgumentException($"The symbol \'{symbol}\' not recognized");
        }
    }
}

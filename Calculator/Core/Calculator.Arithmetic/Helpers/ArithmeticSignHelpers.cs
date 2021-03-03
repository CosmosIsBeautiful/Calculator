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
    }
}

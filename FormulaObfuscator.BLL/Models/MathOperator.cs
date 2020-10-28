using FormulaObfuscator.BLL.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaObfuscator.BLL.Models
{
    public class MathOperator
    {
        public char Value { get; set; }

        public MathOperator(char value)
        {
            Value = value;
        }

        public static MathOperator operator !(MathOperator a)
        {
            switch (a.Value)
            {
                case '+':
                    return new MathOperator('-');
                case '-':
                    return new MathOperator('+');

            }
            throw new UnhandledOperatorException();
        }
    }
}

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
            return a.Value switch
            {
                '+' => new MathOperator('-'),
                '-' => new MathOperator('+'),
                _ => throw new UnhandledOperatorException(),
            };
        }
    }
}

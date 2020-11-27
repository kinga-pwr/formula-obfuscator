using FormulaObfuscator.BLL.Exceptions;

namespace FormulaObfuscator.BLL.Models
{
    public class MathOperator
    {
        private char Value { get; }

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

        override public string ToString()
        {
            return Value.ToString();
        }
    }
}

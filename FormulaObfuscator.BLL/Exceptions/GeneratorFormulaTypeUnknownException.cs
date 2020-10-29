using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaObfuscator.BLL.Exceptions
{
    class GeneratorFormulaTypeUnknownException : Exception
    {
        public GeneratorFormulaTypeUnknownException()
        {
        }

        public GeneratorFormulaTypeUnknownException(string message)
            : base(message)
        {
        }

        public GeneratorFormulaTypeUnknownException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

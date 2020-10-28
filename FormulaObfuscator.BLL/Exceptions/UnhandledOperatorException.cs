using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaObfuscator.BLL.Exceptions
{
    class UnhandledOperatorException : Exception
    {
        public UnhandledOperatorException()
        {
        }

        public UnhandledOperatorException(string message)
            : base(message)
        {
        }

        public UnhandledOperatorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

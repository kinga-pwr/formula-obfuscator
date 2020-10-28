using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaObfuscator.BLL.Exceptions
{
    class GeneratorComplexityLevelOutOfBoundsException : Exception
    {
        public GeneratorComplexityLevelOutOfBoundsException()
        {
        }

        public GeneratorComplexityLevelOutOfBoundsException(string message)
            : base(message)
        {
        }

        public GeneratorComplexityLevelOutOfBoundsException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaObfuscator.BLL.Models
{
    public enum TypeOfMethod
    {
        Polynomial,
        Fraction,
        Root,
        Equation,
        Trigonometry, // should be always last because it's recursive
        Integral, // should be always last because it's recursive
        TrigonometryRedundancy, // should be always last because it's recursive
    }
}

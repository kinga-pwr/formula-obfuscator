using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaObfuscator.BLL.Models
{
    public enum TypeOfFormula
    {
        Polynomial,
        Fraction,
        Trigonometry, // should be always last because it's recursive
        Integral, // should be always last because it's recursive
        Root,
        TrigonometryRedundancy, // should be always last because it's recursive
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace FormulaObfuscator.Helpers
{
    public class MinTenLengthOfFieldValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var result = value != null  &&  (new HashSet<char>((value as string).ToCharArray()).Count > 10);

            return new ValidationResult(result, "Min 10 different letters");
        }
    }
}

using System.Globalization;
using System.Windows.Controls;

namespace FormulaObfuscator.Helpers
{
    public class ObligatoryFieldValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var result = value != null && (value as string).Length > 0;

            return new ValidationResult(result, "Field obligatory");
        }
    }
}

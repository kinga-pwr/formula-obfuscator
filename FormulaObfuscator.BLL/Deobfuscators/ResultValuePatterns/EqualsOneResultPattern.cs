using FormulaObfuscator.BLL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Deobfuscators.ResultValuePatterns
{
    public class EqualsOneResultPattern : IResultValuePattern
    {
        private static List<Trigonometry> ValidTrigonometricSymbols => new List<Trigonometry> { Trigonometry.cos };

        public bool ValidateResultValue(XElement element)
        {
            if (CheckPolynomialPattern(element))
            {
                return ValidatePolynomialPattern(element);
            }
            if (CheckTrigonometricPattern(element))
            {
                return ValidateTrigonometricPattern(element);
            }
            return false;
        }

        #region Polynomial
        private bool CheckPolynomialPattern(XElement element)
        {
            var initialCheck = element.Name == MathMLTags.Row
                && element.Elements().All(e => e.Name == MathMLTags.Number || e.Name == MathMLTags.Operator
                || (e.Name == MathMLTags.Power && e.Elements().First().Name == MathMLTags.Identifier && e.Elements().Last().Name == MathMLTags.Number));

            if (initialCheck)
            {
                var operatorsQty = element.Elements().Where(e => e.Name == MathMLTags.Operator).Count();
                var powerQty = element.Elements().Where(e => e.Name == MathMLTags.Power).Count();
                var numbersQty = element.Elements().Where(e => e.Name == MathMLTags.Number).Count();
                return numbersQty == operatorsQty + 1 && powerQty == operatorsQty - 1;
            }
            return false;
        }

        private bool ValidatePolynomialPattern(XElement element)
        {
            try
            {
                var firstNumber = int.Parse(element.Elements().First().Value);
                var lastNumber = int.Parse(element.Elements().Last().Value);
                if (firstNumber - lastNumber == 1)
                {
                    var valuesDictionary = new Dictionary<string, int>();
                    int i = 2;
                    while (i < element.Elements().Count() - 2)
                    {
                        var number = int.Parse(element.Elements().ElementAt(i).Value);
                        if (element.Elements().ElementAt(i - 1).Value == "-") number *= -1;
                        var key = element.Elements().ElementAt(i + 1).Value;
                        if (!valuesDictionary.TryAdd(key, number))
                        {
                            if (valuesDictionary[key] + number == 0)
                            {
                                valuesDictionary.Remove(key);
                            }
                            else
                            {
                                valuesDictionary[key] = valuesDictionary[key] + number;
                            }
                        }
                        i += 3;
                    }
                    return !valuesDictionary.Any() || valuesDictionary.All(vd => vd.Value == 0);
                }
            }
            catch { }
            return false;
        }
        #endregion Polynomial

        #region Trigonometry
        private bool CheckTrigonometricPattern(XElement element)
        {
            return element.Name == MathMLTags.Row
                && element.Elements().First().Name == MathMLTags.Identifier
                && ValidTrigonometricSymbols.Any(s => s.ToString() == element.Elements().First().Value)
                && element.Elements().ElementAt(1).Value == "(" && element.Elements().Last().Value == ")";
        }

        private bool ValidateTrigonometricPattern(XElement element)
        {
            var variable = element.Elements().ElementAt(2);
            var equalsZeroPattern = new EqualsZeroResultPattern();
            if (equalsZeroPattern.CheckPolynomialPattern(variable))
            {
                return equalsZeroPattern.ValidatePolynomialPattern(variable);
            }
            // todo fraction
            return false;
        }
        #endregion Trigonometry
    }
}

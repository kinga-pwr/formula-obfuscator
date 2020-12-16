using FormulaObfuscator.BLL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Deobfuscators.ResultValuePatterns
{
    public class EqualsZeroResultPattern : IResultValuePattern
    {
        private static List<Trigonometry> ValidTrigonometricSymbols => new List<Trigonometry> { Trigonometry.sin, Trigonometry.tg };

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
            if (CheckFractionPattern(element))
            {
                return ValidateFractionPattern(element);
            }
            if (CheckIntegralPattern(element))
            {
                return ValidateIntegralPattern(element);
            }
            return false;
        }

        #region Polynomial
        public bool CheckPolynomialPattern(XElement element)
        {
            var initialCheck = element.Name == MathMLTags.Row
                && element.Elements().All(e => e.Name == MathMLTags.Number || e.Name == MathMLTags.Operator
                || (e.Name == MathMLTags.Power && e.Elements().First().Name == MathMLTags.Identifier && e.Elements().Last().Name == MathMLTags.Number));

            if (initialCheck)
            {
                var operatorsQty = element.Elements().Where(e => e.Name == MathMLTags.Operator).Count();
                var powerQty = element.Elements().Where(e => e.Name == MathMLTags.Power).Count();
                var numbersQty = element.Elements().Where(e => e.Name == MathMLTags.Number).Count();
                return numbersQty == powerQty && powerQty == operatorsQty + 1;
            }
            return false;
        }

        public bool ValidatePolynomialPattern(XElement element)
        {
            try
            {
                var valuesDictionary = new Dictionary<string, int>();
                int i = 0;
                while (i < element.Elements().Count())
                {
                    var number = int.Parse(element.Elements().ElementAt(i).Value);
                    if (i > 0 && element.Elements().ElementAt(i - 1).Value == "-") number *= -1;
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
                return !valuesDictionary.Any();
            }
            catch { }
            return false;
        }
        #endregion Polynomial

        #region Trigonometry
        private bool CheckTrigonometricPattern(XElement element)
        {
            return element.Name == MathMLTags.Row
                && element.Elements().Count() == 4
                && element.Elements().First().Name == MathMLTags.Identifier 
                && ValidTrigonometricSymbols.Any(s => s.ToString() == element.Elements().First().Value)
                && element.Elements().ElementAt(1).Value == "(" && element.Elements().Last().Value == ")";
        }

        private bool ValidateTrigonometricPattern(XElement element)
        {
            var expression = element.Elements().ElementAt(2);
            return ValidateResultValue(expression);
        }
        #endregion Trigonometry

        #region Fraction
        private bool CheckFractionPattern(XElement element)
        {
            return element.Name == MathMLTags.Fraction
                && element.Elements().First().Name == MathMLTags.Row
                && element.Elements().ElementAt(1).Name == MathMLTags.Row;
        }

        private bool ValidateFractionPattern(XElement element)
        {
            var nominator = element.Elements().First().Elements().First();
            var denominator = element.Elements().ElementAt(1).Elements().First();
            return ValidateResultValue(nominator) && new EqualsOneResultPattern().ValidateResultValue(denominator);
        }
        #endregion Fraction

        #region Integral
        private bool CheckIntegralPattern(XElement element)
        {
            return element.Name == MathMLTags.Row
                && element.Elements().Count() == 5
                && element.Elements().First().Name == MathMLTags.Integral
                && element.Elements().First().Elements().Count() == 3
                && element.Elements().First().Elements().First().Value == MathMLSymbols.Integral
                && element.Elements().First().Elements().ElementAt(1).Value == "0"
                && element.Elements().First().Elements().ElementAt(2).Value == MathMLSymbols.Infinite
                && element.Elements().ElementAt(1).Value == "("
                && element.Elements().ElementAt(3).Value == ")"
                && element.Elements().ElementAt(4).Value == "dx";
        }

        private bool ValidateIntegralPattern(XElement element)
        {
            var expression = element.Elements().ElementAt(2);
            return ValidateResultValue(expression);
        }
        #endregion Integral
    }
}

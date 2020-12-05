using FormulaObfuscator.BLL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Deobfuscators.ResultValuePatterns
{
    public class EqualsZeroResultPattern : IResultValuePattern
    {
        public bool ValidateResultValue(XElement element)
        {
            if (CheckPolynomialPattern(element))
            {
                return ValidatePolynomialPattern(element);
            }
            return false;
        }

        private bool CheckPolynomialPattern(XElement element)
        {
            var initialCount = element.Name == MathMLTags.Row
                && element.Elements().All(e => e.Name == MathMLTags.Number || e.Name == MathMLTags.Operator
                || (e.Name == MathMLTags.Power && e.Elements().First().Name == MathMLTags.Identifier && e.Elements().Last().Name == MathMLTags.Number));

            if (initialCount)
            {
                var operatorsQty = element.Elements().Where(e => e.Name == MathMLTags.Operator).Count();
                var powerQty = element.Elements().Where(e => e.Name == MathMLTags.Power).Count();
                var numbersQty = element.Elements().Where(e => e.Name == MathMLTags.Number).Count();
                return numbersQty == powerQty && powerQty == operatorsQty + 1;
            }
            return false;
        }

        private bool ValidatePolynomialPattern(XElement element)
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
    }
}

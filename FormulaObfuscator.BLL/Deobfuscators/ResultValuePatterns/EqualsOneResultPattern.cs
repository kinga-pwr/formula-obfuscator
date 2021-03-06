﻿using FormulaObfuscator.BLL.Models;
using System;
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
            if (CheckFractionPattern(element))
            {
                return ValidateFractionPattern(element);
            }
            if (CheckRootPattern(element))
            {
                return ValidateRootPattern(element);
            }
            if (CheckEquationPattern(element))
            {
                return ValidateEquationPattern(element);
            }
            if (CheckTrigonometricRedundancyPattern(element))
            {
                return ValidateTrigonometricRedundancyPattern(element);
            }
            return false;
        }

        #region Polynomial
        private bool CheckPolynomialPattern(XElement element)
        {
            return element.Name == MathMLTags.Row
                && element.Elements().All(e => e.Name == MathMLTags.Number || e.Name == MathMLTags.Operator
                || (e.Name == MathMLTags.Power && e.Elements().First().Name == MathMLTags.Identifier && e.Elements().Last().Name == MathMLTags.Number));
        }

        public bool ValidatePolynomialPattern(XElement element)
        {
            var valuesDictionary = new Dictionary<string, int>();
            int i = 0;
            while (i < element.Elements().Count())
            {
                try
                {
                    if (i == 0 && element.Elements().ElementAt(i).Name == MathMLTags.Operator)
                    {
                        i++;
                    }

                    // case 2x or 2
                    if (element.Elements().ElementAt(i).Name == MathMLTags.Number)
                    {
                        var number = int.Parse(element.Elements().ElementAt(i).Value);
                        if (i > 0 && element.Elements().ElementAt(i - 1).Value == "-") number *= -1;
                        string key = "null"; // case 2
                        if (i < element.Elements().Count() - 1 && element.Elements().ElementAt(i + 1).Name != MathMLTags.Operator)
                        {
                            key = element.Elements().ElementAt(i + 1).Value; // case 2x
                            i += 3;
                        }
                        else
                        {
                            i += 2;
                        }
                        TryAddElement(number, key);
                    }
                    // case x
                    else if (i == element.Elements().Count() - 1 || element.Elements().ElementAt(i + 1).Name == MathMLTags.Operator)
                    {
                        var number = 1;
                        if (i > 0 && element.Elements().ElementAt(i - 1).Value == "-") number *= -1;
                        var key = element.Elements().ElementAt(i).Value;
                        TryAddElement(number, key);
                        i += 2;
                    }
                    // case x2
                    else if (element.Elements().ElementAt(i + 1).Name == MathMLTags.Number)
                    {
                        var number = int.Parse(element.Elements().ElementAt(i + 1).Value);
                        if (i > 0 && element.Elements().ElementAt(i - 1).Value == "-") number *= -1;
                        var key = element.Elements().ElementAt(i).Value;
                        TryAddElement(number, key);
                        i += 3;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return valuesDictionary.All(vd => vd.Key == "null" && vd.Value == 1);

            void TryAddElement(int number, string key)
            {
                if (number != 0 && !valuesDictionary.TryAdd(key, number))
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
            }
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
            return new EqualsZeroResultPattern().ValidateResultValue(expression);
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
            return ValidateResultValue(nominator) && ValidateResultValue(denominator);
        }
        #endregion Fraction

        #region Root
        private bool CheckRootPattern(XElement element)
        {
            return element.Name == MathMLTags.Root
                && element.Elements().First().Name == MathMLTags.Row
                && element.Elements().ElementAt(1).Name == MathMLTags.Row;
        }

        private bool ValidateRootPattern(XElement element)
        {
            var expression = element.Elements().First().Elements().First();
            var degree = element.Elements().ElementAt(1).Elements().First();
            return ValidateResultValue(expression) && ValidateResultValue(degree);
        }
        #endregion Root

        #region Equation
        private bool CheckEquationPattern(XElement element)
        {
            return element.Name == MathMLTags.Row
                && element.Elements().Count() == 11
                && element.Elements().First().Value == "("
                && element.Elements().ElementAt(1).Name == MathMLTags.Number
                && element.Elements().ElementAt(2).Value == "("
                && element.Elements().ElementAt(4).Value == ")"
                && element.Elements().ElementAt(5).Value == "-"
                && element.Elements().ElementAt(6).Name == MathMLTags.Number
                && element.Elements().ElementAt(7).Value == "("
                && element.Elements().ElementAt(9).Value == ")"
                && element.Elements().ElementAt(10).Value == ")";
        }

        private bool ValidateEquationPattern(XElement element)
        {
            if (int.TryParse(element.Elements().ElementAt(1).Value, out int firstValue)
                && int.TryParse(element.Elements().ElementAt(6).Value, out int secondValue)
                && firstValue == secondValue + 1)
            {
                var expression1 = element.Elements().ElementAt(3);
                var expression2 = element.Elements().ElementAt(8);
                return ValidateResultValue(expression1) && ValidateResultValue(expression2);
            }
            return false;
        }
        #endregion Equation

        #region TrigonometricRedundancy
        private bool CheckTrigonometricRedundancyPattern(XElement element)
        {
            var options = new[] {
                (Trigonometry.sin, "+", Trigonometry.cos, 2),
                (Trigonometry.tg, MathMLSymbols.Multiply, Trigonometry.ctg, 1)
            };

            return element.Name == MathMLTags.Row
                && element.Elements().Count() == 3

                && element.Elements().ElementAt(1).Name == MathMLTags.Operator
                && options.Any(o => o.Item2 == element.Elements().ElementAt(1).Value)

                && element.Elements().First().Name == MathMLTags.Row
                && element.Elements().First().Elements().Count() == 4
                && ((options.FirstOrDefault(o => o.Item2 == element.Elements().ElementAt(1).Value).Item4 > 1
                && element.Elements().First().Elements().First().Name == MathMLTags.Power
                && element.Elements().First().Elements().First().Elements().First().Value == Enum.GetName(typeof(Trigonometry), options.FirstOrDefault(o => o.Item2 == element.Elements().ElementAt(1).Value).Item1)
                && element.Elements().First().Elements().First().Elements().ElementAt(1).Value == options.FirstOrDefault(o => o.Item2 == element.Elements().ElementAt(1).Value).Item4.ToString())
                || (options.FirstOrDefault(o => o.Item2 == element.Elements().ElementAt(1).Value).Item4 == 1
                && element.Elements().First().Elements().First().Name == MathMLTags.Identifier
                && element.Elements().First().Elements().First().Value == Enum.GetName(typeof(Trigonometry), options.FirstOrDefault(o => o.Item2 == element.Elements().ElementAt(1).Value).Item1)))
                && element.Elements().First().Elements().ElementAt(1).Value == "("
                && element.Elements().First().Elements().ElementAt(3).Value == ")"

                && element.Elements().ElementAt(2).Name == MathMLTags.Row
                && element.Elements().ElementAt(2).Elements().Count() == 4
                && ((options.FirstOrDefault(o => o.Item2 == element.Elements().ElementAt(1).Value).Item4 > 1
                && element.Elements().ElementAt(2).Elements().First().Name == MathMLTags.Power
                && element.Elements().ElementAt(2).Elements().First().Elements().First().Value == Enum.GetName(typeof(Trigonometry), options.FirstOrDefault(o => o.Item2 == element.Elements().ElementAt(1).Value).Item3)
                && element.Elements().ElementAt(2).Elements().First().Elements().ElementAt(1).Value == options.FirstOrDefault(o => o.Item2 == element.Elements().ElementAt(1).Value).Item4.ToString())
                || (options.FirstOrDefault(o => o.Item2 == element.Elements().ElementAt(1).Value).Item4 == 1
                && element.Elements().ElementAt(2).Elements().First().Name == MathMLTags.Identifier
                && element.Elements().ElementAt(2).Elements().First().Value == Enum.GetName(typeof(Trigonometry), options.FirstOrDefault(o => o.Item2 == element.Elements().ElementAt(1).Value).Item3)))
                && element.Elements().ElementAt(2).Elements().ElementAt(1).Value == "("
                && element.Elements().ElementAt(2).Elements().ElementAt(3).Value == ")";
        }

        private bool ValidateTrigonometricRedundancyPattern(XElement element)
        {
            var expression1 = element.Elements().First().Elements().ElementAt(2);
            var expression2 = element.Elements().ElementAt(2).Elements().ElementAt(2);
            return new EqualsZeroResultPattern().ValidateResultValue(expression1) 
                && new EqualsZeroResultPattern().ValidateResultValue(expression2);
        }
        #endregion TrigonometricRedundancy
    }
}

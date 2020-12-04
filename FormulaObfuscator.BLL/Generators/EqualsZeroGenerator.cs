using FormulaObfuscator.BLL.Exceptions;
using FormulaObfuscator.BLL.Helpers;
using FormulaObfuscator.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Generators
{
    public class EqualsZeroGenerator : IGenerator
    {
        public static TypeOfMethod[] PossibleFormulas =>
            new[] 
            {
                TypeOfMethod.Polynomial,
                TypeOfMethod.Fraction,
                TypeOfMethod.Trigonometry,
                TypeOfMethod.Integral 
            };

        public static TypeOfMethod[] AvailableFormulas => Settings.CurrentSettings.MethodsForZeroGenerator.ToArray();

        public TypeOfMethod[] GetPossibleFormulas() => AvailableFormulas;

        public XElement Generate(TypeOfMethod formula)
        {
            if (Randoms.RecursionDepth <= 0) return Polynomial();

            return formula switch
            {
                TypeOfMethod.Polynomial => Polynomial(),
                TypeOfMethod.Fraction => Fraction(),
                TypeOfMethod.Trigonometry => Trigonometry(),
                TypeOfMethod.Integral => Integral(),
                _ => throw new GeneratorFormulaTypeUnknownException(),
            };
        }

        /// <summary>
        /// <code>
        /// (msup)
        ///     (mi)x(/mi)
        ///     (mn)3(/mn)
        /// (/msup)
        /// </code>
        /// </summary>
        /// <returns></returns>
        private XElement Polynomial()
        {
            var MAX_AMOUNT_OF_VARIABLES = 10;
            var MAX_POWER = 10;
            var MAX_COUNT_OF_GEN_FORMULA = 3;
            var MAX_CONST_VALUE = 20;

            var elementsCollection = new List<List<XElement>>();

            // a : {2: 6, 6: -2 } == 6a^2 - 2a^6
            // <varName> : {<power>: <const>, <power2>: <const>}
            var logs = new Dictionary<char, Dictionary<int, int>>();

            var amountOfVariables = Randoms.Int(1, MAX_AMOUNT_OF_VARIABLES);

            // Generate variable names
            while (logs.Keys.Count < amountOfVariables)
            {
                logs.TryAdd(Randoms.Char(), new Dictionary<int, int>());
            }

            //var formula = "";
            var formulaRoot = new XElement(MathMLTags.Row);
            var isFirst = true;

            // create formula
            for (int power = 0; power < MAX_COUNT_OF_GEN_FORMULA; power++)
            {
                var currPower = Randoms.Int(MAX_POWER);
                if (currPower % 2 != 0) currPower++; // only even powers
                var currVarName = logs.Keys.ElementAt(Randoms.Int(amountOfVariables));
                var currConst = Randoms.Int(1, MAX_CONST_VALUE);

                //formula += (currConst >= 0 ? "+" : "") + currConst + currVarName + "^" + currPower;
                if (!isFirst)
                {
                    var piecesContainer = new List<XElement>
                    {
                        new XElement(MathMLTags.Operator, currConst >= 0 ? "+" : "-"),
                        new XElement(MathMLTags.Number, Math.Abs(currConst)),
                        MathMLStructures.Power(currPower, new XElement(MathMLTags.Identifier, currVarName))
                    };
                    elementsCollection.Add(piecesContainer);
                }
                else
                {
                    isFirst = false;
                    formulaRoot.Add(new XElement(MathMLTags.Number, Math.Abs(currConst)));
                    formulaRoot.Add(MathMLStructures.Power(currPower, new XElement(MathMLTags.Identifier, currVarName)));
                }

                var varHistory = logs[currVarName];

                if (varHistory.ContainsKey(currPower))
                    varHistory[currPower] += currConst;
                else
                    varHistory.Add(currPower, currConst);
            }

            // create opposed formula
            foreach (char variable in logs.Keys)
            {
                var powerHistory = logs[variable];

                foreach (int power in powerHistory.Keys)
                {
                    var currConst = powerHistory[power] * -1;

                    // original const is split into two parts
                    // example equation: 4x-1x-3x instead of: 4x-4x
                    var constPart1 = currConst / 3;
                    var constPart2 = currConst - constPart1;

                    var piecesContainer = new List<XElement>
                    {
                        //formula += (currConst >= 0 ? "+" : "") + currConst + variable + "^" + power;
                        new XElement(MathMLTags.Operator, constPart1 >= 0 ? "+" : "-"),
                        new XElement(MathMLTags.Number, Math.Abs(constPart1)),
                        MathMLStructures.Power(power, new XElement(MathMLTags.Identifier, variable))
                    };
                    elementsCollection.Add(piecesContainer);

                    piecesContainer = new List<XElement>
                    {
                        //formula += (currConst >= 0 ? "+" : "") + currConst + variable + "^" + power;
                        new XElement(MathMLTags.Operator, constPart2 >= 0 ? "+" : "-"),
                        new XElement(MathMLTags.Number, Math.Abs(constPart2)),
                        MathMLStructures.Power(power, new XElement(MathMLTags.Identifier, variable))
                    };
                    elementsCollection.Add(piecesContainer);
                }
            }

            // shuffle order of elements
            foreach(var element in elementsCollection.OrderBy(e => Guid.NewGuid()))
            {
                foreach(var elementPart in element)
                {
                    formulaRoot.Add(elementPart);
                }
            }

            return formulaRoot;
        }

        /// <summary>
        /// <code>
        /// (mfrac)
        ///     (mrow)
        ///         (mn)0(/mn)
        ///     (/mrow)
        ///     (mrow)
        ///         (mn)6(/mn)
        ///     (/mrow)
        /// (/mfrac)
        /// </code>
        /// </summary>
        /// <returns></returns>
        private XElement Fraction()
        {
            Randoms.RecursionDepth--;

            XElement fraction = new XElement(MathMLTags.Fraction);
            XElement nominator = new XElement(MathMLTags.Row);
            nominator.Add(Polynomial());
            XElement denominator = new XElement(MathMLTags.Row);
            denominator.Add(new EqualsOneGenerator().Generate(EqualsOneGenerator.RandomFormula));

            fraction.Add(nominator);
            fraction.Add(denominator);

            return fraction;
        }

        /// <summary>
        /// <code>
        /// (mrow)
        ///     (mi)sin(/mi)
        ///     (mn)0(/mn)
        /// (/mrow)
        /// </code>
        /// </summary>
        /// <returns></returns>
        private XElement Trigonometry()
        {
            Randoms.RecursionDepth--;
            return MathMLStructures.Trigonometric((Trigonometry)Randoms.Int(0, 1), Generate((TypeOfMethod)Randoms.Int(0, 2)));
        }
        /// <summary>
        /// <code>
        /// (mrow)
        ///     (munderover)
        ///         (mo)#integral sign#(/mo)
        ///         (mn)0(/mn)
        ///     (/munderover)
        /// (/mrow)
        /// </code>
        /// </summary>
        /// <returns></returns>
        private XElement Integral()
        {
            Randoms.RecursionDepth--;
            var upperLimit = new XElement(MathMLTags.Identifier, MathMLSymbols.Infinite);
            var lowerLimit = new XElement(MathMLTags.Number, 0);
            return MathMLStructures.Integral(Generate((TypeOfMethod)Randoms.Int(0, 3)), upperLimit, lowerLimit);
        }
    }
}

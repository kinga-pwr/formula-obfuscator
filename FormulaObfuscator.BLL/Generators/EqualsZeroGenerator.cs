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

        public TypeOfMethod[] GetPossibleFormulas() => PossibleFormulas;

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

        private XElement Polynomial()
        {
            var MAX_AMOUNT_OF_VARIABLES = 10;
            var MAX_POWER = 10;
            var MAX_COUNT_OF_GEN_FORMULA = 3;
            var MAX_CONST_VALUE = 20;

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
                    formulaRoot.Add(new XElement(MathMLTags.Operator, currConst >= 0 ? "+" : "-"));
                else
                    isFirst = false;

                formulaRoot.Add(new XElement(MathMLTags.Number, Math.Abs(currConst)));
                formulaRoot.Add(MathMLStructures.Power(currPower, new XElement(MathMLTags.Identifier, currVarName)));

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
                    //formula += (currConst >= 0 ? "+" : "") + currConst + variable + "^" + power;
                    formulaRoot.Add(new XElement(MathMLTags.Operator, currConst >= 0 ? "+" : "-"));
                    formulaRoot.Add(new XElement(MathMLTags.Number, Math.Abs(currConst)));
                    formulaRoot.Add(MathMLStructures.Power(power, new XElement(MathMLTags.Identifier, variable)));
                }
            }

            return formulaRoot;
        }

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
        /// (container)
        /// <para>(mi)sin(/mi)</para>
        /// <para>(mn)0(/mn)</para>
        /// (/container)
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
        /// (container)
        /// (munderover)
        /// <para>(mo)#integral sign#(/mo)</para>
        /// <para>(mn)0(/mn)</para>
        /// (/munderover)
        /// (/container)
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

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
        public TypeOfFormula[] GetPossibleFormulas()
        {
            return new[] { TypeOfFormula.Polynomial, TypeOfFormula.Fraction, TypeOfFormula.Trigonometry, TypeOfFormula.Integral };
        }

        public XElement Generate(TypeOfFormula formula)
        {
            if (Randoms.RecursionDepth <= 0) return LevelOne();

            return formula switch
            {
                TypeOfFormula.Polynomial => Polynomial(),
                TypeOfFormula.Fraction => Fraction(),
                TypeOfFormula.Trigonometry => Trigonometry(),
                TypeOfFormula.Integral => Integral(),
                _ => throw new GeneratorFormulaTypeUnknownException(),
            };
        }

        private XElement LevelOne() => new XElement(MathMLTags.Number, "0");

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
            XElement nominator = new XElement(MathMLTags.Row, Polynomial());
            XElement denominator = new XElement(MathMLTags.Row, Randoms.ComplexExpression());

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
            return MathMLStructures.Trigonometric((Trigonometry)Randoms.Int(0, 1), new EqualsZeroGenerator().Generate((TypeOfFormula)Randoms.Int(0, 2)));
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
            return MathMLStructures.Integral(new EqualsZeroGenerator().Generate((TypeOfFormula)Randoms.Int(0, 3)));
        }
    }
}

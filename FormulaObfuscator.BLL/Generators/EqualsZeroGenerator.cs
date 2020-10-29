﻿using FormulaObfuscator.BLL.Exceptions;
using FormulaObfuscator.BLL.Helpers;
using FormulaObfuscator.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Generators
{
    public class EqualsZeroGenerator
    {
        public XElement Generate(TypeOfFormula formula)
        {
            switch (formula)
            {
                case TypeOfFormula.Polynomial:
                    return Polynomial();
            }
            throw new GeneratorComplexityLevelOutOfBoundsException();
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

            var amountOfVariables = Randoms.Int(MAX_AMOUNT_OF_VARIABLES);

            // Generate variable names
            while (logs.Keys.Count < amountOfVariables)
            {
                logs.TryAdd(Randoms.Char(), new Dictionary<int, int>());
            }

            var formula = "";
            var formulaRoot = new XElement(MathMLTags.Row);

            // create formula
            for (int power = 0; power < MAX_COUNT_OF_GEN_FORMULA; power++)
            {
                var currPower = Randoms.Int(MAX_POWER);
                var currVarName = logs.Keys.ElementAt(Randoms.Int(amountOfVariables));
                var currConst = Randoms.Int(MAX_CONST_VALUE);

                formula += (currConst >= 0 ? "+" : "") + currConst + currVarName + "^" + currPower;
                formulaRoot.Add(new XElement(MathMLTags.Operator, currConst >= 0 ? "+" : "-"));
                formulaRoot.Add(new XElement(MathMLTags.Number, Math.Abs(currConst)));
                formulaRoot.Add(MakePowerNode(currPower, currVarName));

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
                    formula += (currConst >= 0 ? "+" : "") + currConst + variable + "^" + power;
                    formulaRoot.Add(new XElement(MathMLTags.Operator, currConst >= 0 ? "+" : "-"));
                    formulaRoot.Add(new XElement(MathMLTags.Number, Math.Abs(currConst)));
                    formulaRoot.Add(MakePowerNode(power, variable));
                }
            }

            return formulaRoot;
        }

        private XElement MakePowerNode(int power, char varName)
        {
            var powerNode = new XElement(MathMLTags.Power);
            powerNode.Add(new XElement(MathMLTags.Identifier, varName));
            powerNode.Add(new XElement(MathMLTags.Number, power));

            return powerNode;
        }
    }
}
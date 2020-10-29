using FormulaObfuscator.BLL.Exceptions;
using FormulaObfuscator.BLL.Helpers;
using FormulaObfuscator.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Generators
{
    public class EqualsOneGenerator
    {
        public XElement Generate(int complexityLevel = 2)
        {
            switch (complexityLevel)
            {
                case 1:
                    return LevelOne();
                case 2:
                    return Polynomial();
                    
            }
            throw new GeneratorComplexityLevelOutOfBoundsException();
        }

        private XElement LevelOne()
        {
            XElement element = new XElement("container");
            for (int i = 0; i < 3; i++)
            {
                // Initialize XML elements
                //XElement numOperator = new XElement(MathMLTags.Operator);
                //XElement letterOperator = new XElement(MathMLTags.Operator, );
                //XElement num = new XElement(MathMLTags.Number);
                //XElement letter = new XElement(MathMLTags.Number);

                // Set numberic value and its operator (-6)
                var numOp = Randoms.Operator();
                //numOperator.Value = numOp.Value.ToString();
                //num.Value = Randoms.Int().ToString();

                // Set character value and its operator (+s)
                var letterOp = Randoms.Operator();
                //letter.Value = Randoms.Char().ToString();
                //letterOperator.Value = letterOp.Value.ToString();

                // Add XML elements to one root
                element.Add(new XElement(MathMLTags.Operator), numOp.Value.ToString());
                element.Add(new XElement(MathMLTags.Number), Randoms.Int().ToString());

                element.Add(new XElement(MathMLTags.Operator, numOp.Value.ToString()));
                element.Add(new XElement(MathMLTags.Number), Randoms.Char().ToString());

                // Add same elements with inverteed operators

                element.Add(new XElement(MathMLTags.Operator), (!numOp).Value.ToString());
                element.Add(new XElement(MathMLTags.Number), Randoms.Int().ToString());

                element.Add(new XElement(MathMLTags.Operator, (!numOp).Value.ToString()));
                element.Add(new XElement(MathMLTags.Number), Randoms.Char().ToString());
            }

            return element;
        }

        private XElement Polynomial()
        {
            var MAX_AMOUNT_OF_VARIABLES = 10;
            var MAX_POWER = 10;
            var MAX_COUNT_OF_GEN_FORMULA = 1;
            var MAX_CONST_VALUE = 7;

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

            // create formula
            for (int power = 0; power < MAX_COUNT_OF_GEN_FORMULA; power++)
            {
                var currPower = Randoms.Int(MAX_POWER);
                var currVarName = logs.Keys.ElementAt(Randoms.Int(amountOfVariables));
                var currConst = Randoms.Int(MAX_CONST_VALUE);

                formula += (currConst >= 0 ? "+": "") + currConst + currVarName + "^" + currPower;

                var varHistory = logs[currVarName];

                if (varHistory.ContainsKey(currPower))
                    varHistory[currPower] += currConst;
                else
                    varHistory.Add(currPower, currConst);
            }

            // create opposed formula
            foreach(char variable in logs.Keys)
            {
                var powerHistory = logs[variable];

                foreach (int power in powerHistory.Keys)
                {
                    var currConst = powerHistory[power] * -1;
                    formula += (currConst >= 0 ? "+" : "") + currConst + variable + "^" + power;
                }
            }

            return new XElement(formula.Substring(1));
        }

    }
}

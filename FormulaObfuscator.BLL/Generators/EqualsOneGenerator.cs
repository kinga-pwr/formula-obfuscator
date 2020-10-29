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
        public XElement Generate(TypeOfFormula type)
        {
            switch (type)
            {
                case TypeOfFormula.Polynomial:
                    return Polynomial();
                default:
                    return LevelOne();
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
            var CONST_TO_ADD = 1;
            var formulaNode = new EqualsZeroGenerator().Generate(TypeOfFormula.Polynomial);
            formulaNode.Add(new XElement(MathMLTags.Operator, "+"));
            formulaNode.Add(new XElement(MathMLTags.Number, CONST_TO_ADD));

            return formulaNode;
        }
    }
}

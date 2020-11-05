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
            return type switch
            {
                TypeOfFormula.Polynomial => Polynomial(),
                TypeOfFormula.Fraction => Fraction(),
                TypeOfFormula.Root => Root(),
                TypeOfFormula.Trigonometry => Trigonometric(),
                TypeOfFormula.TrigonometryRedundancy => TrigonometricRedundancy(),
                _ => LevelOne(),
            };
            throw new GeneratorFormulaTypeUnknownException();
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

        /// <summary>
        /// <code>
        /// (mroot)
        /// <para>(mi)x(/mi)</para>
        /// <para>(mn)3(/mn)</para>
        /// <para>(/mroot) </para>
        /// </code>
        /// </summary>
        /// <returns></returns>
        private XElement Root()
        {
            XElement root = new XElement(MathMLTags.Root);
            XElement degree = new XElement(MathMLTags.Identifier, Randoms.ComplexExpression());
            XElement element = new XElement(MathMLTags.Number, Polynomial());

            root.Add(degree);
            root.Add(element);

            return root;
        }

        /// <summary>
        /// <code>
        /// <para>(mfrac)</para>
        /// <para>    (mrow)</para>
        /// <para>        (mn)6(/mn)</para>
        /// <para>    (/mrow)</para>
        /// <para>    (mrow)</para>
        /// <para>        (mn)6(/mn)</para>
        /// <para>    (/mrow)</para>
        /// <para>(/mfrac)</para>
        /// </code>
        /// </summary>
        /// <returns></returns>
        private XElement Fraction()
        {
            XElement fraction = new XElement(MathMLTags.Fraction);
            XElement nominator = new XElement(MathMLTags.Row, Root());
            XElement denominator = new XElement(MathMLTags.Row, Polynomial());

            fraction.Add(nominator);
            fraction.Add(denominator);

            return fraction;
        }

        /// <summary>
        /// <code>
        /// (container)
        /// <para>(mi)cos(/mi)</para>
        /// <para>(mn)3(/mn)</para>
        /// (/container)
        /// </code>
        /// </summary>
        /// <returns></returns>
        private XElement Trigonometric()
            => MathMLStructures.Trigonometric(Trigonometry.cos, new EqualsZeroGenerator().Generate((TypeOfFormula)Randoms.Int(0, 2)));

        /// <summary>
        /// <code>
        /// (container)
        ///    (msup)
        ///      (mi)sin(/mi)
        ///      (mn)2(/mn)
        ///    (/msup)
        ///    +
        ///    (msup)
        ///      (mi)cos(/mi)
        ///      (mn)2(/mn)
        ///    (/msup)
        /// (/container)
        /// </code>
        /// </summary>
        /// <returns></returns>
        private XElement TrigonometricRedundancy()
        {
            var options = new[] {
                (Trigonometry.sin, "+", Trigonometry.cos, 2),
                (Trigonometry.tg, MathMLSymbols.Multiply, Trigonometry.ctg, 1)
            };
            var formula = new EqualsZeroGenerator().Generate((TypeOfFormula)Randoms.Int(0, 2));
            var option = options[Randoms.Int(0, 1)];
            XElement element = new XElement("container");
            var part1 = MathMLStructures.Trigonometric(option.Item1, formula, option.Item4);
            var part2 = MathMLStructures.Trigonometric(option.Item3, formula, option.Item4);
            element.Add(part1);
            element.Add(new XElement(MathMLTags.Operator, option.Item2));
            element.Add(part2);
            return element;
        }
    }
}

using FormulaObfuscator.BLL.Exceptions;
using FormulaObfuscator.BLL.Helpers;
using FormulaObfuscator.BLL.Models;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Generators
{
    public class EqualsOneGenerator : IGenerator
    {
        public TypeOfFormula[] GetPossibleFormulas()
        {
            return new[] { TypeOfFormula.Polynomial, TypeOfFormula.Fraction, TypeOfFormula.Trigonometry, TypeOfFormula.Root, TypeOfFormula.TrigonometryRedundancy };
        }

        public XElement Generate(TypeOfFormula type)
        {
            if (Randoms.RecursionDepth <= 0) return Polynomial();

            return type switch
            {
                TypeOfFormula.Polynomial => Polynomial(),
                TypeOfFormula.Fraction => Fraction(),
                TypeOfFormula.Root => Root(),
                TypeOfFormula.Trigonometry => Trigonometric(),
                TypeOfFormula.TrigonometryRedundancy => TrigonometricRedundancy(),
                _ => Polynomial(),
            };
            throw new GeneratorFormulaTypeUnknownException();
        }

        /// <summary>
        /// <code>
        /// (msup)
        /// <para>(mi)x(/mi)</para>
        /// <para>(mn)3(/mn)</para>
        /// <para>(/msup) </para>
        /// </code>
        /// </summary>
        /// <returns></returns>
        private XElement Polynomial()
        {
            Randoms.RecursionDepth--;

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
            Randoms.RecursionDepth--;

            XElement root = new XElement(MathMLTags.Root);
            XElement degree = new XElement(MathMLTags.Row);
            degree.Add(Randoms.ComplexExpression());
            XElement element = new XElement(MathMLTags.Row);
            element.Add(Polynomial());

            root.Add(element);
            root.Add(degree);

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
            Randoms.RecursionDepth--;

            XElement fraction = new XElement(MathMLTags.Fraction);
            XElement nominator = new XElement(MathMLTags.Row);
            nominator.Add(Root());
            XElement denominator = new XElement(MathMLTags.Row);
            denominator.Add(Polynomial());

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
        {
            Randoms.RecursionDepth--;
            return MathMLStructures.Trigonometric(Trigonometry.cos, new EqualsZeroGenerator().Generate((TypeOfFormula)Randoms.Int(0, 2)));
        }

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
            Randoms.RecursionDepth--;

            var options = new[] {
                (Trigonometry.sin, "+", Trigonometry.cos, 2),
                (Trigonometry.tg, MathMLSymbols.Multiply, Trigonometry.ctg, 1)
            };
            var formula = new EqualsZeroGenerator().Generate((TypeOfFormula)Randoms.Int(0, 2));
            var option = options[Randoms.Int(0, 1)];
            XElement element = new XElement(MathMLTags.Row);
            var part1 = MathMLStructures.Trigonometric(option.Item1, formula, option.Item4);
            var part2 = MathMLStructures.Trigonometric(option.Item3, formula, option.Item4);
            element.Add(part1);
            element.Add(new XElement(MathMLTags.Operator, option.Item2));
            element.Add(part2);
            return element;
        }
    }
}

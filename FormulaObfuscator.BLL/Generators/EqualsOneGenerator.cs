using FormulaObfuscator.BLL.Helpers;
using FormulaObfuscator.BLL.Models;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Generators
{
    public class EqualsOneGenerator : IGenerator
    {
        public static TypeOfMethod[] PossibleFormulas => 
            new[]
            {
                TypeOfMethod.Polynomial,
                TypeOfMethod.Fraction,
                TypeOfMethod.Trigonometry,
                TypeOfMethod.Root,
                TypeOfMethod.TrigonometryRedundancy,
                TypeOfMethod.Equation
            };

        public static TypeOfMethod[] AvailableFormulas => Settings.CurrentSettings.MethodsForOneGenerator.ToArray();

        public TypeOfMethod[] GetPossibleFormulas() => AvailableFormulas;

        public static TypeOfMethod RandomFormula => AvailableFormulas[Randoms.Int(AvailableFormulas.Length)];

        public XElement Generate(TypeOfMethod type)
        {
            if (Randoms.RecursionDepth <= 0) return Polynomial();

            return type switch
            {
                TypeOfMethod.Polynomial => Polynomial(),
                TypeOfMethod.Fraction => Fraction(),
                TypeOfMethod.Root => Root(),
                TypeOfMethod.Trigonometry => Trigonometric(),
                TypeOfMethod.TrigonometryRedundancy => TrigonometricRedundancy(),
                TypeOfMethod.Equation => Equation(),
                _ => Polynomial(),
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
            Randoms.RecursionDepth--;

            var CONST_TO_ADD = 1;
            var formulaNode = new EqualsZeroGenerator().Generate(TypeOfMethod.Polynomial);
            formulaNode.Add(new XElement(MathMLTags.Operator, "+"));
            formulaNode.Add(new XElement(MathMLTags.Number, CONST_TO_ADD));

            return formulaNode;
        }

        /// <summary>
        /// <code>
        /// (mroot)
        ///     (mi)x(/mi)
        ///     (mn)3(/mn)
        /// (/mroot)
        /// </code>
        /// </summary>
        /// <returns></returns>
        private XElement Root()
        {
            Randoms.RecursionDepth--;

            XElement root = new XElement(MathMLTags.Root);
            XElement degree = new XElement(MathMLTags.Row);
            degree.Add(Generate(RandomFormula));
            XElement element = new XElement(MathMLTags.Row);
            element.Add(Polynomial());

            root.Add(element);
            root.Add(degree);

            return root;
        }

        /// <summary>
        /// <code>
        /// (mfrac)
        ///     (mrow)
        ///         (mn)6(/mn)
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
            nominator.Add(Root());
            XElement denominator = new XElement(MathMLTags.Row);
            denominator.Add(Polynomial());

            fraction.Add(nominator);
            fraction.Add(denominator);

            return fraction;
        }

        /// <summary>
        /// <code>
        /// (mrow)
        ///     (mi)cos(/mi)
        ///     (mn)3(/mn)
        /// (/mrow)
        /// </code>
        /// </summary>
        /// <returns></returns>
        private XElement Trigonometric()
        {
            Randoms.RecursionDepth--;
            return MathMLStructures.Trigonometric(Trigonometry.cos, new EqualsZeroGenerator().Generate((TypeOfMethod)Randoms.Int(0, 2)));
        }

        /// <summary>
        /// <code>
        /// (mrow)
        ///    (msup)
        ///      (mi)sin(/mi)
        ///      (mn)2(/mn)
        ///    (/msup)
        ///    +
        ///    (msup)
        ///      (mi)cos(/mi)
        ///      (mn)2(/mn)
        ///    (/msup)
        /// (/mrow)
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
            var option = options[Randoms.Int(0, 1)];
            XElement element = new XElement(MathMLTags.Row);
            var part1 = MathMLStructures.Trigonometric(option.Item1, new EqualsZeroGenerator().Generate((TypeOfMethod)Randoms.Int(0, 2)), option.Item4);
            var part2 = MathMLStructures.Trigonometric(option.Item3, new EqualsZeroGenerator().Generate((TypeOfMethod)Randoms.Int(0, 2)), option.Item4);
            element.Add(part1);
            element.Add(new XElement(MathMLTags.Operator, option.Item2));
            element.Add(part2);
            return element;
        }

        /// <summary>
        /// <code>
        /// (mrow)
        ///      (mn)2(/mn)
        ///      (mrow)(/mrow)
        ///      (mo)-(/mo)
        ///      (mrow)(/mrow)
        /// (/mrow)
        /// </code>
        /// </summary>
        /// <returns></returns>
        private XElement Equation()
        {
            Randoms.RecursionDepth--;

            var number = Randoms.Int(2, 100);

            var container = new XElement(MathMLTags.Row);
            container.Add(new XElement(MathMLTags.Operator, "("));
            container.Add(new XElement(MathMLTags.Number, number.ToString()));
            container.Add(new XElement(MathMLTags.Operator, "("));
            container.Add(Generate(RandomFormula));
            container.Add(new XElement(MathMLTags.Operator, ")"));
            container.Add(new XElement(MathMLTags.Operator, "-"));
            container.Add(new XElement(MathMLTags.Number, (number-1).ToString()));
            container.Add(new XElement(MathMLTags.Operator, "("));
            container.Add(Generate(RandomFormula));
            container.Add(new XElement(MathMLTags.Operator, ")"));
            container.Add(new XElement(MathMLTags.Operator, ")"));

            return container;
        }
    }
}

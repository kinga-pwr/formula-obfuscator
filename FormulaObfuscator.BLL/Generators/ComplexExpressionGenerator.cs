using FormulaObfuscator.BLL.Helpers;
using FormulaObfuscator.BLL.Models;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Generators
{
    public static class ComplexExpressionGenerator
    {
        private static int _maxLength => 5;

        public static XElement Fraction()
        {
            XElement fraction = new XElement(MathMLTags.Fraction);
            XElement nominator = new XElement(MathMLTags.Row);
            XElement denominator = new XElement(MathMLTags.Row);

            var length = Randoms.Int(_maxLength);
            for (int i = 0; i < length; i++)
            {
                nominator.Add(Randoms.SimpleExpression());
                if (i != length-1)
                {
                    nominator.Add(Randoms.OperatorXElement());
                }
            }

            length = Randoms.Int(_maxLength);
            for (int i = 0; i < length; i++)
            {
                denominator.Add(Randoms.SimpleExpression());
                if (i != length-1)
                {
                    denominator.Add(Randoms.OperatorXElement());
                }
            }

            fraction.Add(nominator);
            fraction.Add(denominator);

            return fraction;
        }

        public static XElement Root()
        {

            XElement root = new XElement(MathMLTags.Root);
            XElement degree = new XElement(MathMLTags.Identifier);
            XElement element = new XElement(MathMLTags.Number);

            var length = Randoms.Int(_maxLength);
            for (int i = 0; i < length; i++)
            {
                degree.Add(Randoms.SimpleExpression());
                if (i != length - 1)
                {
                    degree.Add(Randoms.OperatorXElement());
                }
            }

            length = Randoms.Int(_maxLength);
            for (int i = 0; i < length; i++)
            {
                element.Add(Randoms.SimpleExpression());
                if (i != length - 1)
                {
                    element.Add(Randoms.OperatorXElement());
                }
            }

            root.Add(degree);
            root.Add(element);
            return root;

        }
    }
}

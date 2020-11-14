using FormulaObfuscator.BLL.Helpers;
using FormulaObfuscator.BLL.Models;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Generators
{
    public static class SimpleExpressionGenerator
    {
        public static XElement FractionNumberNumber()
        {
            XElement main = new XElement(MathMLTags.Fraction);
            XElement nominator = new XElement(MathMLTags.Number, Randoms.Int());
            XElement denominator = new XElement(MathMLTags.Number, Randoms.Int());

            main.Add(nominator);
            main.Add(denominator);

            return main;
        }

        public static XElement PowerCharNumber()
        {
            XElement main = new XElement(MathMLTags.Sub);
            XElement basis = new XElement(MathMLTags.Identifier, Randoms.Char());
            XElement power = new XElement(MathMLTags.Number, Randoms.Int());

            main.Add(basis);
            main.Add(power);

            return main;
        }

        public static XElement RootCharNumber()
        {
            XElement main = new XElement(MathMLTags.Sub);
            XElement degree = new XElement(MathMLTags.Identifier, Randoms.Char());
            XElement element = new XElement(MathMLTags.Number, Randoms.Int());

            main.Add(degree);
            main.Add(element);

            return main;
        }

    }
}

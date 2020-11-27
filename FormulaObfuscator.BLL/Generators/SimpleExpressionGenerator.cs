using FormulaObfuscator.BLL.Helpers;
using FormulaObfuscator.BLL.Models;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Generators
{
    public static class SimpleExpressionGenerator
    {
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
        public static XElement FractionNumberNumber()
        {
            XElement main = new XElement(MathMLTags.Fraction);
            XElement nominator = new XElement(MathMLTags.Number, Randoms.Int());
            XElement denominator = new XElement(MathMLTags.Number, Randoms.Int());

            main.Add(nominator);
            main.Add(denominator);

            return new XElement(MathMLTags.Math, main);
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
        public static XElement PowerCharNumber()
        {
            XElement main = new XElement(MathMLTags.Power);
            XElement basis = new XElement(MathMLTags.Identifier, Randoms.Char());
            XElement power = new XElement(MathMLTags.Number, Randoms.Int());

            main.Add(basis);
            main.Add(power);

            return new XElement(MathMLTags.Math, main);
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
        public static XElement RootCharNumber()
        {
            XElement main = new XElement(MathMLTags.Sqrt);
            XElement degree = new XElement(MathMLTags.Identifier, Randoms.Char());
            XElement element = new XElement(MathMLTags.Number, Randoms.Int());

            main.Add(degree);
            main.Add(element);

            return new XElement(MathMLTags.Math, main);
        }
    }
}

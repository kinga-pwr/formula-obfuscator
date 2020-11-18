using System;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Models
{
    public class MathMLStructures
    {
        public static XElement Trigonometric(Trigonometry type, XElement variable, int power = 1)
        {
            XElement container = new XElement(MathMLTags.Row);
            XElement trigonometricSymbol = new XElement(MathMLTags.Identifier, Enum.GetName(typeof(Trigonometry), type));

            container.Add(Power(power, trigonometricSymbol));
            container.Add(new XElement(MathMLTags.Operator, "("));
            container.Add(variable);
            container.Add(new XElement(MathMLTags.Operator, ")"));
            return container;
        }
        public static XElement Power(int power, XElement variable)
        {
            var powerNode = new XElement(MathMLTags.Power);
            powerNode.Add(variable);
            powerNode.Add(new XElement(MathMLTags.Number, power));

            return power != 1 ? powerNode : variable;
        }

        public static XElement Integral(XElement expression, XElement upperLimit = null, XElement lowerLimit = null)
        {
            var container = new XElement(MathMLTags.Row);
            var integral = new XElement(MathMLTags.Integral);
            var integralSymbol = new XElement(MathMLTags.Operator, MathMLSymbols.Integral);
            integral.Add(integralSymbol);
            if (upperLimit != null && lowerLimit != null)
            {
                integral.Add(lowerLimit);
                integral.Add(upperLimit);
            }
            else
            {
                integral.Add(new XElement(MathMLTags.Number));
                integral.Add(new XElement(MathMLTags.Number));
            }
            container.Add(integral);
            container.Add(new XElement(MathMLTags.Operator, "("));
            container.Add(expression);
            container.Add(new XElement(MathMLTags.Operator, ")"));
            return container;
        }
    }

    public enum Trigonometry
    {
        sin,
        tg,
        cos,
        ctg  
    }
}

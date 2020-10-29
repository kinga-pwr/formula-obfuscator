using FormulaObfuscator.BLL.Helpers;
using FormulaObfuscator.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Generators
{
    public static class ComplexExpressionGenerator
    {
        private static int _maxDepth => 5;

        public static XElement Fraction()
        {
            XElement fraction = new XElement(MathMLTags.Fraction);
            XElement nominator = new XElement(MathMLTags.Row);
            XElement denominator = new XElement(MathMLTags.Row);

            for (int i = 0; i < Randoms.Int(_maxDepth); i++)
            {
                nominator.Add(Randoms.SimpleExpression());
                if (i != _maxDepth-1)
                {
                    nominator.Add(Randoms.OperatorXElement());
                }
            }
            
            for (int i = 0; i < Randoms.Int(_maxDepth); i++)
            {
                denominator.Add(Randoms.SimpleExpression());
                if (i != _maxDepth-1)
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

            for (int i = 0; i < Randoms.Int(_maxDepth); i++)
            {
                degree.Add(Randoms.SimpleExpression());
                if (i != _maxDepth - 1)
                {
                    degree.Add(Randoms.OperatorXElement());
                }
            }

            for (int i = 0; i < Randoms.Int(_maxDepth); i++)
            {
                element.Add(Randoms.SimpleExpression());
                if (i != _maxDepth - 1)
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

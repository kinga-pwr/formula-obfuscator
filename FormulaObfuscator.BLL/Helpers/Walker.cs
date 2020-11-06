using FormulaObfuscator.BLL.Algorithms;
using FormulaObfuscator.BLL.Generators;
using FormulaObfuscator.BLL.Models;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Helpers
{
    public static class Walker
    {
        public static void WalkWithAlgorithm(XElement node, IAlgorithm algorithm)
        {
            foreach (XElement child in node.Elements())
            {
                WalkWithAlgorithm(child, algorithm);
            }
            // condition when we want to add obfuscate node
            // for now find <mi>
            if (node.Name.ToString().Contains(MathMLTags.Identifier))
            {
                if (IsToObfuscate())
                {
                    var operation = GetTypeOfOperation();
                    var container = new XElement(MathMLTags.Row);
                    IGenerator generator;

                    switch (operation)
                    {
                        case TypeOfOperation.PlusZero:
                            container.Add(new XElement(MathMLTags.Operator, "+"));
                            generator = new EqualsZeroGenerator();
                            break;
                        case TypeOfOperation.MinusZero:
                            container.Add(new XElement(MathMLTags.Operator, "-"));
                            generator = new EqualsZeroGenerator();
                            break;
                        case TypeOfOperation.MultiplyByOne:
                            container.Add(new XElement(MathMLTags.Operator, "x"));
                            generator = new EqualsOneGenerator();
                            break;
                        default:
                            container.Add(new XElement(MathMLTags.Operator, "+"));
                            generator = new EqualsZeroGenerator();
                            break;
                    }
                    var formulaToGenerate = GetFormula(generator);
                    container.Add(new XElement(MathMLTags.Operator, "("));
                    container.Add(generator.Generate(formulaToGenerate));
                    container.Add(new XElement(MathMLTags.Operator, ")"));
                    node.Add(container);
                }
            }
        }

        private static TypeOfOperation GetTypeOfOperation()
        {
            Array operations = Enum.GetValues(typeof(TypeOfOperation));

            return (TypeOfOperation) operations.GetValue(Randoms.Int(operations.Length));
        }

        private static TypeOfFormula GetFormula(IGenerator generator)
        {
            var operations = generator.GetPossibleFormulas();

            return operations[(Randoms.Int(operations.Length))];
        }

        private static bool IsToObfuscate()
        {
            var num = Randoms.Int(1, 10);
            return num < 2;
        }

        public static void FindTrees(XElement node, string value, List<XElement> outputTrees)
        {
            if (node.Name.ToString().Contains(value))
            {
                outputTrees.Add(node);
            }

            foreach (XElement child in node.Elements())
            {
                FindTrees(child, value, outputTrees);
            }
        }

        public static XElement SubstituteObfuscatedTrees(XElement node, string value, Queue<XElement> outputTrees)
        {
            if (node.Name.ToString().Contains(value))
            {
                node.ReplaceNodes(outputTrees.Dequeue().Elements());
            }

            foreach (XElement child in node.Elements())
            {
                SubstituteObfuscatedTrees(child, value, outputTrees);
            }
            return node;
        }
    }
}

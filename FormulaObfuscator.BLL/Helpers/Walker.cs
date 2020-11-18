using FormulaObfuscator.BLL.Generators;
using FormulaObfuscator.BLL.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Helpers
{
    public static class Walker
    {
        public static XElement WalkWithAlgorithmForRootVariables(XElement root)
        {
            var level = 1;
            var currentLevel = 0;
            for (int i = 0; i < GetMaxLevel(root); i++)
            {
                foreach (XElement child in root.Elements())
                {
                    currentLevel = 0;
                    WalkWithAlgorithmForVariables(child, level, ref currentLevel);
                }
                level++;
            }
            return root;
        }

        public static int GetMaxLevel(XElement root)
        {
            foreach (XElement child in root.Elements())
            {
                return GetMaxLevel(child) + 1;
            }
            return 0;
        }

        public static XElement WalkWithAlgorithmForVariables(XElement node, int level, ref int currentLevel)
        {
            currentLevel++;

            // condition when we want to add obfuscate node
            // for now find <mi>
            if (currentLevel == level && node.Name.ToString().Contains(MathMLTags.Identifier))
            {
                if (IsToObfuscate())
                {
                    var operation = GetTypeOfOperation();

                    if (operation != TypeOfOperation.DivideByOne)
                    {
                        node.AddAfterSelf(Obfuscate(operation));
                    }
                    else
                    {
                        return ObfuscateWithDivide(node, operation);
                    }
                }
            }

            foreach (XElement child in node.Elements())
            {
                if(!child.Name.ToString().Contains(MathMLTags.Power))
                    WalkWithAlgorithmForVariables(child, level, ref currentLevel);
            }

            return node;
        }

        public static XElement WalkWithAlgorithmForWholeFormula(XElement root)
        {
            //var operation = GetTypeOfOperation();
            var operation = TypeOfOperation.DivideByOne;

            if (operation != TypeOfOperation.DivideByOne)
            {
                var node = root.FirstNode;
                var last = root.LastNode;
                node.AddBeforeSelf(new XElement(MathMLTags.Operator, "("));
                last.AddAfterSelf(Obfuscate(operation));
                last.AddAfterSelf(new XElement(MathMLTags.Operator, ")"));
                return root;

            } else
            {
                return ObfuscateRootWithDivide(root, operation);
            }
        }

        public static XElement WalkWithAlgorithmForAllFractionsInRoot(XElement root)
        {
            foreach (XElement child in root.Elements())
            {
                if (!child.Name.ToString().Contains(MathMLTags.Power))
                    WalkWithAlgorithmForAllFractions(child);
            }

            return root;
        }

        public static XElement WalkWithAlgorithmForAllFractions(XElement node)
        {
            foreach (XElement child in node.Elements())
            {
                if (!child.Name.ToString().Contains(MathMLTags.Power))
                    WalkWithAlgorithmForAllFractions(child);
            }
            // condition when we want to add obfuscate node
            // for now find <mi>
            if (node.Name.ToString().Contains(MathMLTags.Fraction))
            {
                if (IsToObfuscate())
                {
                    var operation = GetTypeOfOperationWithoutDivide();
                    node.AddBeforeSelf(new XElement(MathMLTags.Operator, "("));
                    node.AddAfterSelf(Obfuscate(operation));
                    node.AddAfterSelf(new XElement(MathMLTags.Operator, ")"));
                }
            }

            return node;
        }

        private static XElement ObfuscateRootWithDivide(XElement root, TypeOfOperation operation)
        {
            if (ifContainsEqualities(root.Value))
            {
                var leftSide = new List<XElement>();
                var rightSide = new List<XElement>();
                XElement equalifier = null;
                FindTreeWithEqualities(root, ref equalifier); // find equalifier
                var childrens = equalifier.Parent.Elements(); // get all childrens
                bool isRightSide = false;

                foreach (XElement child in childrens)
                {
                    if (child.Name.ToString().Contains(MathMLTags.Operator) && ifContainsEqualities(child.Value.ToString()))
                    {
                        isRightSide = true;
                        continue;
                    }

                    if(isRightSide)
                    {
                        rightSide.Add(child);
                    } else
                    {
                        leftSide.Add(child);
                    }
                }

                var leftFraction = makeFraction(leftSide, operation);
                var rightFraction = makeFraction(rightSide, operation);
                root.RemoveAll();
                root.Add(leftFraction);
                root.Add(equalifier);
                root.Add(rightFraction);

                return root;
            }

            var elements = root.Elements();
            root.RemoveAll();
            root.Add(makeFraction(elements, operation));
            return root;
        }

        private static XElement makeFraction(IEnumerable<XElement> elements, TypeOfOperation operation)
        {
            var fraction = new XElement(MathMLTags.Fraction);
            var nominator = new XElement(MathMLTags.Row);

            nominator.Add(new XElement(MathMLTags.Operator, "("));
            nominator.Add(elements);
            nominator.Add(new XElement(MathMLTags.Operator, ")"));
            fraction.Add(nominator);
            fraction.Add(Obfuscate(operation));

            return fraction;
        }

        private static bool ifContainsEqualities(string value)
        {
            var equalities = new List<string>{ MathMLSymbols.Equal }; // TODO add rest symbols
            return equalities.Any(equality => value.Contains(equality));
        }

        private static XElement ObfuscateWithDivide(XElement node, TypeOfOperation operation)
        {
            var fraction = new XElement(MathMLTags.Fraction);
            var nominator = new XElement(MathMLTags.Row);
            nominator.Add(new XElement(MathMLTags.Operator, "("));
            nominator.Add(node);
            nominator.Add(new XElement(MathMLTags.Operator, ")"));
            fraction.Add(nominator);
            fraction.Add(Obfuscate(operation));

            return fraction;
        }

        private static XElement Obfuscate(TypeOfOperation operation)
        {
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
                    container.Add(new XElement(MathMLTags.Operator, MathMLSymbols.Multiply));
                    generator = new EqualsOneGenerator();
                    break;
                case TypeOfOperation.DivideByOne:
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

            return container;
        }

        private static TypeOfOperation GetTypeOfOperation()
        {
            Array operations = Enum.GetValues(typeof(TypeOfOperation));

            return (TypeOfOperation) operations.GetValue(Randoms.Int(operations.Length));
        }

        private static TypeOfOperation GetTypeOfOperationWithoutDivide()
        {
            TypeOfOperation[] operations = { TypeOfOperation.PlusZero, TypeOfOperation.MinusZero, TypeOfOperation.MultiplyByOne};

            return operations[(Randoms.Int(operations.Length))];
        }

        private static TypeOfFormula GetFormula(IGenerator generator)
        {
            var operations = generator.GetPossibleFormulas();

            return operations[(Randoms.Int(operations.Length))];
        }

        private static bool IsToObfuscate()
        {
            var num = Randoms.Int(1, 10);
            return num < 7;
        }

        public static void FindTreeWithEqualities(XElement node, ref XElement outputTree)
        {
            if (node.Name.ToString().Contains(MathMLTags.Operator) && ifContainsEqualities(node.Value.ToString()))
            {
                outputTree = node;
            }

            foreach (XElement child in node.Elements())
            {
                FindTreeWithEqualities(child, ref outputTree);
            }
        }

        public static void FindTrees(XElement node, string value, List<XElement> outputTrees)
        {
            if (node.Name.ToString().Contains(value))
            {
                outputTrees.Add(node);
            }
            else
            {
                foreach (XElement child in node.Elements())
                {
                    FindTrees(child, value, outputTrees);
                }
            }          
        }

        public static XElement SubstituteObfuscatedTrees(XElement node, string value, Queue<XElement> outputTrees)
        {
            if (node.Name.ToString().Contains(value))
            {
                node.ReplaceNodes(outputTrees.Dequeue().Elements());
            }
            else
            {
                foreach (XElement child in node.Elements())
                {
                    SubstituteObfuscatedTrees(child, value, outputTrees);
                }
            }
            return node;
        }
    }
}

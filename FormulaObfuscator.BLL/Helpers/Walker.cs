using FormulaObfuscator.BLL.Deobfuscators;
using FormulaObfuscator.BLL.Generators;
using FormulaObfuscator.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Helpers
{
    public static class Walker
    {
        static readonly List<string> TAGS_WITH_VALUES = new List<string> { MathMLTags.Identifier, MathMLTags.Operator, MathMLTags.Number };
        static readonly List<TypeOfOperation> OPERATIONS_WITH_EXTRA_BRACKETS = new List<TypeOfOperation> { TypeOfOperation.MultiplyByOne, TypeOfOperation.MinusZero };

        public static XElement WalkWithAlgorithmForVariables(XElement node, XElement copyTree)
        {
            // copy current node to copyTree
            if (TAGS_WITH_VALUES.Any(i => node.Name.ToString().Contains(i)))
            {
                copyTree.Add(new XElement(node.Name, node.Value));
            }
            else
            {
                copyTree.Add(new XElement(node.Name));
            }

            var copy = copyTree.Elements().Last();

            // condition when we want to add obfuscate node
            // for now find <mi>
            if (node.Name.ToString().Contains(MathMLTags.Identifier))
            {
                if (IsToObfuscate())
                {
                    var operation = GetTypeOfOperation();

                    if (operation != TypeOfOperation.DivideByOne)
                    {
                        var obfuscated = Obfuscate(operation);
                        var isOperationWithExtraBrackets = OPERATIONS_WITH_EXTRA_BRACKETS.Contains(operation);

                        if (!isOperationWithExtraBrackets)
                            obfuscated.Elements().ElementAt(1).Remove();

                        obfuscated.AddFirst(copy);
                        obfuscated.AddFirst(new XElement(MathMLTags.Operator, "("));

                        if (isOperationWithExtraBrackets)
                            obfuscated.Add(new XElement(MathMLTags.Operator, ")"));

                        copyTree.Elements().Last().Remove();

                        copyTree.Add(obfuscated);
                        // get added copy of identifier
                        // identifier is second element in obfuscated node
                        copy = copyTree.Elements().Last().Elements().ElementAt(1);
                    }
                    else
                    {
                        copyTree.Elements().Last().Remove();
                        copyTree.Add(ObfuscateWithDivide(node, operation));
                    }
                }
            }

            for (int i = 0; i < node.Elements().Count(); i++)
            {
                var child = node.Elements().ElementAt(i);
                WalkWithAlgorithmForVariables(child, copy);
            }

            return node;
        }

        public static XElement WalkWithAlgorithmForRootVariables(XElement root)
        {
            XElement copy = new XElement(root.Name);
            WalkWithAlgorithmForVariables(root, copy);

            return copy;
        }

        public static int GetMaxLevel(XElement root)
        {
            foreach (XElement child in root.Elements())
            {
                return GetMaxLevel(child) + 1;
            }
            return 0;
        }

        public static XElement WalkWithAlgorithmForWholeFormula(XElement root)
        {
            var operation = GetTypeOfOperation();

            if (operation != TypeOfOperation.DivideByOne)
            {
                var node = root.FirstNode;
                var last = root.LastNode;

                if (IfContainsEqualities(root.Value))
                {
                    XElement equalifier = null;
                    FindTreeWithEqualities(root, ref equalifier); // find equalifier
                    node = equalifier.ElementsAfterSelf().First();
                    last = equalifier.ElementsAfterSelf().Last();
                }

                node.AddBeforeSelf(new XElement(MathMLTags.Operator, "("));
                last.AddAfterSelf(Obfuscate(operation));
                last.AddAfterSelf(new XElement(MathMLTags.Operator, ")"));
                return root;

            }
            else
            {
                return ObfuscateRootWithDivide(root, operation);
            }
        }

        public static XElement WalkWithAlgorithmForAllFractionsInRoot(XElement root)
        {
            XElement copy = new XElement(root.Name);
            WalkWithAlgorithmForAllFractions(root, copy);

            return copy;
        }

        public static XElement WalkWithAlgorithmForAllFractions(XElement node, XElement copyTree)
        {
            // copy current node to copyTree
            if (TAGS_WITH_VALUES.Any(i => node.Name.ToString().Contains(i)))
            {
                copyTree.Add(new XElement(node.Name, node.Value));
            }
            else
            {
                copyTree.Add(new XElement(node.Name));
            }

            var copy = copyTree.Elements().Last();

            // condition when we want to add obfuscate node
            // for now find <fraction>
            if (node.Name.ToString().Contains(MathMLTags.Fraction))
            {
                if (IsToObfuscate())
                {
                    var operation = GetTypeOfOperationWithoutDivide();

                    var obfuscated = Obfuscate(operation);

                    obfuscated.AddFirst(copy);
                    obfuscated.AddFirst(new XElement(MathMLTags.Operator, "("));
                    obfuscated.Add(new XElement(MathMLTags.Operator, ")"));
                    copyTree.Elements().Last().Remove();

                    copyTree.Add(obfuscated);
                    // get added copy of fraction
                    // fraction is second element in obfuscated node
                    copy = copyTree.Elements().Last().Elements().ElementAt(1);
                }
            }

            for (int i = 0; i < node.Elements().Count(); i++)
            {
                var child = node.Elements().ElementAt(i);
                WalkWithAlgorithmForAllFractions(child, copy);
            }

            return node;
        }

        private static XElement ObfuscateRootWithDivide(XElement root, TypeOfOperation operation)
        {
            if (IfContainsEqualities(root.Value))
            {
                var leftSide = new List<XElement>();
                var rightSide = new List<XElement>();
                XElement equalifier = null;
                FindTreeWithEqualities(root, ref equalifier); // find equalifier
                var childrens = equalifier.Parent.Elements(); // get all childrens
                bool isRightSide = false;

                foreach (XElement child in childrens)
                {
                    if (child.Name.ToString().Contains(MathMLTags.Operator) && IfContainsEqualities(child.Value))
                    {
                        isRightSide = true;
                        continue;
                    }

                    if (isRightSide)
                    {
                        rightSide.Add(child);
                    }
                    else
                    {
                        leftSide.Add(child);
                    }
                }

                var leftFraction = MakeFraction(leftSide, operation);
                var rightFraction = MakeFraction(rightSide, operation);
                root.RemoveAll();
                root.Add(leftFraction);
                root.Add(equalifier);
                root.Add(rightFraction);

                return root;
            }

            var elements = MakeFraction(root.Elements(), operation);
            root.RemoveAll();
            root.Add(elements);
            return root;
        }

        private static XElement MakeFraction(IEnumerable<XElement> elements, TypeOfOperation operation)
        {
            var fraction = new XElement(MathMLTags.Fraction);
            var nominator = new XElement(MathMLTags.Row);
            var formula = new XElement(MathMLTags.Row);

            formula.Add(new XElement(MathMLTags.Operator, "("));
            nominator.Add(elements);
            fraction.Add(nominator);
            fraction.Add(Obfuscate(operation));
            formula.Add(fraction);
            formula.Add(new XElement(MathMLTags.Operator, ")"));

            return formula;
        }

        private static bool IfContainsEqualities(string value)
        {
            var equalities = new List<string> { MathMLSymbols.Equal };
            return equalities.Any(equality => value.Contains(equality));
        }

        private static XElement ObfuscateWithDivide(XElement node, TypeOfOperation operation)
        {
            var fraction = new XElement(MathMLTags.Fraction);
            var nominator = new XElement(MathMLTags.Row);
            var formula = new XElement(MathMLTags.Row);

            formula.Add(new XElement(MathMLTags.Operator, "("));
            nominator.Add(node);
            fraction.Add(nominator);
            fraction.Add(Obfuscate(operation));
            formula.Add(fraction);
            formula.Add(new XElement(MathMLTags.Operator, ")"));

            return formula;
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

            return (TypeOfOperation)operations.GetValue(Randoms.Int(operations.Length));
        }

        private static TypeOfOperation GetTypeOfOperationWithoutDivide()
        {
            TypeOfOperation[] operations = { TypeOfOperation.MultiplyByOne };

            return operations[(Randoms.Int(operations.Length))];
        }

        private static TypeOfMethod GetFormula(IGenerator generator)
        {
            var operations = generator.GetPossibleFormulas();

            return operations[(Randoms.Int(operations.Length))];
        }

        private static bool IsToObfuscate()
        {
            var num = Randoms.Int(0, 100);
            return num < Settings.CurrentSettings.ObfucateProbability;
        }

        public static void FindTreeWithEqualities(XElement node, ref XElement outputTree)
        {
            if (node.Name.ToString().Contains(MathMLTags.Operator) && IfContainsEqualities(node.Value))
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

        public static XElement WalkWithAlgorithmForDeobfuscation(XElement element)
        {
            int i = 0;
            while (i < element.Elements().Count())
            {
                foreach (var deobfuscationPattern in DeobfuscationManager.AvailableVariableStructurePatterns)
                {
                    var isObfuscatingElem = deobfuscationPattern.DetectObfuscation(element.Elements().ElementAt(i));
                    if (isObfuscatingElem)
                    {
                        var deobfuscatedElem = deobfuscationPattern.RemoveObfuscation(element.Elements().ElementAt(i));
                        element.Elements().ElementAt(i).ReplaceWith(deobfuscatedElem);
                        break;
                    }
                }

                foreach (var deobfuscationPattern in DeobfuscationManager.AvailableFormulaStructurePatterns)
                {
                    var isObfuscatingElem = deobfuscationPattern.DetectObfuscation(element.Elements().ElementAt(i));
                    if (isObfuscatingElem)
                    {
                        var deobfuscatedElem = deobfuscationPattern.RemoveObfuscation(element.Elements().ElementAt(i));
                        element = deobfuscatedElem;
                        return element;
                    }
                }
                WalkWithAlgorithmForDeobfuscation(element.Elements().ElementAt(i));
                i++;
            }
            return element;
        }

        public static XElement SubstituteModifiedTrees(XElement node, string value, Queue<XElement> outputTrees)
        {
            if (node.Name.ToString().Contains(value))
            {
                node.ReplaceNodes(outputTrees.Dequeue().Elements());
            }
            else
            {
                foreach (XElement child in node.Elements())
                {
                    SubstituteModifiedTrees(child, value, outputTrees);
                }
            }
            return node;
        }
    }
}

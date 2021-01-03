using FormulaObfuscator.BLL.Deobfuscators.ResultValuePatterns;
using FormulaObfuscator.BLL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Deobfuscators.StructurePatterns
{
    class FormulaStructureFractionPattern : IStructurePattern
    {
        private static readonly Dictionary<string, IResultValuePattern> OperatorValueDictionary = new Dictionary<string, IResultValuePattern>()
        {
            { MathMLSymbols.Multiply, new EqualsOneResultPattern() },
            { "+", new EqualsZeroResultPattern() },
            { "-", new EqualsZeroResultPattern() }
        };
        public bool DetectObfuscation(XElement element)
        {
            return DetectFormulaObfucationStructure(element);
        }

        public XElement RemoveObfuscation(XElement element)
        {
            return XElement.Parse(element.Elements().ElementAt(1).FirstNode.ToString());
        }

        private bool DetectFormulaObfucationStructure(XElement element)
        {
            // mrow
            // surrounded by brackets
            // next is mfrac
            return element.Elements().Count() > 2
                && element.Elements().ElementAt(0).Name == MathMLTags.Operator
                && element.Elements().ElementAt(0).Value == "("
                && element.Elements().ElementAt(1).Name == MathMLTags.Fraction
                && DetectVariablePattern(AfterClosingBracketNode(element)); ;

        }


        private bool DetectVariablePattern(XElement element)
        {
            XElement nextElement = (element.NextNode as XElement);
            var elementOperator = nextElement.Elements().ElementAt(0).Value;
            bool detected = false;
            int i = 0;
            while (i < nextElement.Elements().Count())
            {
                foreach (var deobfuscationPattern in DeobfuscationManager.AvailableVariableStructurePatterns)
                {
                    detected = detected || OperatorValueDictionary[elementOperator].ValidateResultValue(nextElement.Elements().ElementAt(2));
                }
                i++;
            }
            return detected;
        }

        private XElement AfterClosingBracketNode(XElement element)
        {
            if (element.Value == ")")
                return element;
            else
                if (element.NextNode != null)
                return (ClosingBracketNode(element.NextNode) as XElement);
            else return null;
        }

        private XNode ClosingBracketNode(XNode element)
        {
            if ((element as XElement).Value == ")")
                return element;
            else
                if (element.NextNode != null)
                return ClosingBracketNode(element.NextNode);
            else return null;
        }
    }
}

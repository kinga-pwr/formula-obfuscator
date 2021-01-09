using FormulaObfuscator.BLL.Deobfuscators.ResultValuePatterns;
using FormulaObfuscator.BLL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Deobfuscators.StructurePatterns
{
    class FormulaStructureFlatPattern : IStructurePattern
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
            List<XElement> list = element.Parent.Elements().ToList();
            XElement startBracket = list.Where(e => e.Value == "(").FirstOrDefault();
            XElement endBracket = list.Where(e => e.Value == ")").FirstOrDefault();
            int startIndex = list.IndexOf(startBracket);
            int endIndex = list.IndexOf(endBracket);
            XElement result = new XElement(MathMLTags.Row);
            for (int i = 0; i < list.Count; i++)
            {
                if (i == startIndex || i >= endIndex)
                    continue;
                result.Add(list[i]);
            }
            return result;
        }

        private bool DetectFormulaObfucationStructure(XElement element)
        {
            // mrow
            // surrounded by brackets
            // brakcethas attribute xmlns
            return element.Name == MathMLTags.Operator
                && element.Value == "("
                && element.Attribute("xmlns") != null
                && DetectVariablePattern(AfterClosingBracketNode(element));

        }

        private bool DetectVariablePattern(XElement element)
        {
            XElement nextElement = (element.NextNode as XElement);
            var elementOperator = nextElement.Elements().ElementAt(0).Value;
            return OperatorValueDictionary[elementOperator].ValidateResultValue(nextElement.Elements().ElementAt(2));
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

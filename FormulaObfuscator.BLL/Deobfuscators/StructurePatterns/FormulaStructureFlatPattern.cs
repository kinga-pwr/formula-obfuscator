using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using FormulaObfuscator.BLL.Models;
using System.Linq;

namespace FormulaObfuscator.BLL.Deobfuscators.StructurePatterns
{
    class FormulaStructureFlatPattern : IStructurePattern
    {
        public bool DetectObfuscation(XElement element)
        {
            return DetectFormulaObfucationStructure(element);
        }

        public XElement RemoveObfuscation(XElement element)
        {
            //return XElement.Parse(element.NextNode.ToString());

            var list = element.Parent.Elements().ToList();
            var startBracket = list.Where(e => e.Value == "(").FirstOrDefault();
            var endBracket = list.Where(e => e.Value == ")").FirstOrDefault();
            var startIndex = list.IndexOf(startBracket);
            var endIndex = list.IndexOf(endBracket);
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
                && element.Attribute("xmlns") != null;

        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using FormulaObfuscator.BLL.Models;
using System.Linq;

namespace FormulaObfuscator.BLL.Deobfuscators.StructurePatterns
{
    class FormulaStructureFractionPattern : IStructurePattern
    {
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
            return element.Elements().ElementAt(0).Name == MathMLTags.Operator
                && element.Elements().ElementAt(0).Value == "("
                && element.Elements().ElementAt(1).Name == MathMLTags.Fraction;

        }
    }
}

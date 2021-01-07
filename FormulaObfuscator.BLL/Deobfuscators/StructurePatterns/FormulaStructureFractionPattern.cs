using FormulaObfuscator.BLL.Deobfuscators.ResultValuePatterns;
using FormulaObfuscator.BLL.Models;
using System.Linq;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Deobfuscators.StructurePatterns
{
    class FormulaStructureFractionPattern : IStructurePattern
    {
        public bool DetectObfuscation(XElement element)
        {
            return DetectFormulaObfucationStructure(element) && ValidateFractionValue(element);
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
            return element.Elements().Count() == 3
                && element.Elements().ElementAt(0).Name == MathMLTags.Operator
                && element.Elements().ElementAt(0).Value == "("
                && element.Elements().ElementAt(1).Name == MathMLTags.Fraction
                && element.Elements().ElementAt(2).Name == MathMLTags.Operator
                && element.Elements().ElementAt(2).Value == ")";

        }

        private bool ValidateFractionValue(XElement element)
        {
            if (element.Elements().ElementAt(1).Elements().ElementAt(1).Elements().First().Value == "("
                && element.Elements().ElementAt(1).Elements().ElementAt(1).Elements().Last().Value == ")")
            {
                return new EqualsOneResultPattern().ValidateResultValue(element.Elements().ElementAt(1).Elements().ElementAt(1).Elements().ElementAt(1));
            }
            else return new EqualsOneResultPattern().ValidateResultValue(element.Elements().ElementAt(1).Elements().ElementAt(1));
        }
    }
}

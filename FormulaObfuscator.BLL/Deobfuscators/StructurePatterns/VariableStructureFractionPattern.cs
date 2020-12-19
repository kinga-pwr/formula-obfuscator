using FormulaObfuscator.BLL.Deobfuscators.ResultValuePatterns;
using FormulaObfuscator.BLL.Models;
using System.Linq;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Deobfuscators.StructurePatterns
{
    public class VariableStructureFractionPattern : IStructurePattern
    {
        public bool DetectObfuscation(XElement element)
        {
            return DetectVariableObfucationFractionStructure(element) && ValidateFractionValue(element);
        }

        private bool DetectVariableObfucationFractionStructure(XElement element)
        {
            // mrow
            // surrounded by brackets
            // 3rd is frac
            // 4th is mrow with original value
            // 5th is mrow with polynomial or polynomial in brackets
            return element.Name == MathMLTags.Row
                && element.Elements().Count() == 3
                && element.Elements().First().Value == "(" && element.Elements().Last().Value == ")"
                && element.Elements().ElementAt(1).Name == MathMLTags.Fraction
                && element.Elements().ElementAt(1).Elements().Count() == 2
                && element.Elements().ElementAt(1).Elements().ElementAt(0).Name == MathMLTags.Row
                && element.Elements().ElementAt(1).Elements().ElementAt(1).Name == MathMLTags.Row;
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

        public XElement RemoveObfuscation(XElement element) => element.Elements().ElementAt(1).Elements().ElementAt(0);
    }
}

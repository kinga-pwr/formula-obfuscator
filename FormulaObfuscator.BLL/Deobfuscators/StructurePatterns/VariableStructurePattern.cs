using FormulaObfuscator.BLL.Deobfuscators.ResultValuePatterns;
using FormulaObfuscator.BLL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Deobfuscators.StructurePatterns
{
    public class VariableStructurePattern : IStructurePattern
    {
        private static readonly List<string> PossibleOperators = new List<string>() { MathMLSymbols.Multiply, "+", "-" };
        private static readonly Dictionary<string, IResultValuePattern> OperatorValueDictionary = new Dictionary<string, IResultValuePattern>() 
        {
            { MathMLSymbols.Multiply, new EqualsOneResultPattern() },
            { "+", new EqualsZeroResultPattern() },
            { "-", new EqualsZeroResultPattern() }
        };

        public bool DetectObfuscation(XElement element)
        {
            return DetectVariableObfucationStructure(element) && ValidateValue(element);
        }

        private bool DetectVariableObfucationStructure(XElement element)
        {
            // mrow
            // surrounded by brackets
            // 3rd is operator 
            // 4th is another set of brackets
            return element.Name == MathMLTags.Row
                && element.Elements().First().Value == "(" && element.Elements().Last().Value == ")"
                && element.Elements().ElementAt(2).Name == MathMLTags.Operator && PossibleOperators.Contains(element.Elements().ElementAt(2).Value)
                && element.Elements().ElementAt(3).Value == "(" && element.Elements().ElementAt(5).Value == ")";
        }

        private bool ValidateValue(XElement element)
        {
            var elementOperator = element.Elements().ElementAt(2).Value;
            return OperatorValueDictionary[elementOperator].ValidateResultValue(element.Elements().ElementAt(4));
        }

        public XElement RemoveObfuscation(XElement element) => element.Elements().ElementAt(1);
    }
}

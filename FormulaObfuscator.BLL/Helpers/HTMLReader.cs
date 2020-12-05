using System.Collections.Generic;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Helpers
{
    public class HTMLReader
    {
        private const string MATHML_TAG = "math";

        public string Text { get; set; }

        public HTMLReader(string text)
        {
            Text = text;
        }

        public void ConvertToMathMLTree(List<XElement> outputTrees)
        {
            var tree = XElement.Parse(Text);
            Walker.FindTrees(tree, MATHML_TAG, outputTrees);
        }

        public string SubstituteModifiedMathMLTree(Queue<XElement> obfuscatedTrees)
        {
            var tree = XElement.Parse(Text);
            Walker.SubstituteModifiedTrees(tree, MATHML_TAG, obfuscatedTrees);
            return tree.ToString().Replace("&amp;", "&");
        }
    }
}

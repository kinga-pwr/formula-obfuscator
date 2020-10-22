using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Helpers
{
    public class HTMLReader
    {
        private const string MATHML_TAG = "math";

        public string Text { get; set; }

        public HTMLReader(string text)
        {
            this.Text = text;
        }

        public Holder<XElement> convertToMathMLTree()
        {
            try
            {
                var tree = XElement.Parse(Text);
                var mathMLTreeHolder = Walker.findTreeWithValue(tree, MATHML_TAG);

                return mathMLTreeHolder;

            } catch (System.Xml.XmlException e)
            {
                Console.WriteLine(e.Message);
            }

            return Holder<XElement>.Fail(ErrorMsgs.CONVERT_FAILED_MSG);
        }

        private XElement getMathML(XElement tree)
        {

            throw new System.Xml.XmlException("MathML not found");
        }
    }
}

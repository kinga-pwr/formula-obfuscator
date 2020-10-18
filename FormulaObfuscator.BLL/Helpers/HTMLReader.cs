using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Helpers
{
    public class HTMLReader
    {
        private const string CONVERT_FAILED_MSG = "Nie udało się przekonwertować tekstu, proszę spróbować jeszcze raz...";

        public string Text { get; set; }

        public HTMLReader(string text)
        {
            this.Text = text;
        }

        public Holder<XElement> convertToTree()
        {
            try
            {
                var tree = XElement.Parse(Text);

                return Holder<XElement>.Success(tree);

            } catch (System.Xml.XmlException e)
            {
                Console.WriteLine(e.Message);
            }

            return Holder<XElement>.Fail(CONVERT_FAILED_MSG);
        }
    }
}

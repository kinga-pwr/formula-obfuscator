using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FormulaObfuscator.BLL.Algorithms
{

    public class FractionAlgorithm
    {
        public string TagName => "mfrac";
        public void Obfuscate(List<XElement> leaf)
        {
            XElement main = new XElement(TagName);
            XElement nominator = new XElement("row");
            XElement denominator = new XElement("row", 1);

            leaf[0].Parent.Add(main);

            main.Add(nominator);
            main.Add(denominator);

            leaf.ForEach(l =>
            {
                l.Remove();
                nominator.Add(l);
//                denominator.Add(l);
            });
        }

        /// <summary>
        /// Finds and extracts elements that are inside of ()
        /// </summary>
        /// <param name="leaf"></param>
        private List<XElement> ExtractElementsInParenthesis(XElement leaf)
        {
      

            bool copy = false;
            List<XElement> list = new List<XElement>();
            foreach (XElement item in leaf.Elements())
            {
                if (copy == false)
                    copy = item.Value.Equals("(");
                else
                {
                    list.Add(item);
                    copy = !item.Value.Equals(")");
                }
            }
            return list;
        }
    }
}

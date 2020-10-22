using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Algorithms
{
    public class SimpleAlgorithm : IAlgorithm
    {
        // add 0 to leaf, for ex: a-a

        private XElement Node { get; set; }

        Func<string, string> Algorithm = name => $"<mrow><mo>+</mo><mi>{name}</mi><mo>-</mo><mi>{name}</mi></mrow>";

        public SimpleAlgorithm(){}

        public void makeObfuscate(XElement leaf)
        {
            var obfuscateNode = XElement.Parse(Algorithm("a"));
            leaf.Add(obfuscateNode);
        }
    };
}

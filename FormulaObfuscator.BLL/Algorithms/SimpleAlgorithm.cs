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

        Func<char, string> Algorithm = name => $"<mrow><mo>+</mo><mi>{name}</mi><mo>-</mo><mi>{name}</mi></mrow>";

        public SimpleAlgorithm(){}

        public void makeObfuscate(XElement leaf)
        {
            int num = new Random().Next(0, 26);
            char let = (num > 13) ? ((char)('a' + num)) : (char)('a' + num); // get random letter
            var obfuscateNode = XElement.Parse(Algorithm(let));
            leaf.AddBeforeSelf(obfuscateNode);
        }
    };
}

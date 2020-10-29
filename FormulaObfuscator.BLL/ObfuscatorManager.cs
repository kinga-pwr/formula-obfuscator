using FormulaObfuscator.BLL.Algorithms;
using FormulaObfuscator.BLL.Generators;
using FormulaObfuscator.BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL
{
    public class ObfuscatorManager
    {
        public string UploadedText { get; set; }

        public ObfuscatorManager(string uploadedText)
        {
            this.UploadedText = uploadedText;
        }

        public Holder obfuscate()
        {
            var reader = new HTMLReader(UploadedText);
            var mathmlTreeHolder = reader.convertToMathMLTree();

            if (mathmlTreeHolder.WasSuccessful)
                obfuscateWithSimple(mathmlTreeHolder.Value);

            return mathmlTreeHolder;
        }

        private void obfuscateWithSimple(XElement node)
        {
            Walker.walkWithAlgorithm(node, new SimpleAlgorithm());
        }

        // taki jakby main
    //    XElement root = new XElement("");
    //    Random random = new Random();
    //    List<IAlgorithm> ListaAlgorytmów = new List<IAlgorithm>();
    //        for (int i = 0; i< 7; i++)
    //        {
    //            ListaAlgorytmów[random.Next(ListaAlgorytmów.Count)].Obfuscate(root);
    //}

}
}

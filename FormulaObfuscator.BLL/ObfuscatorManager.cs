using FormulaObfuscator.BLL.Algorithms;
using FormulaObfuscator.BLL.Helpers;
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

        public Holder runObfuscate()
        {
            var obfuscateCount = 2;
            var reader = new HTMLReader(UploadedText);
            var mathmlTreeHolder = reader.convertToMathMLTree();

            if (mathmlTreeHolder.WasSuccessful)
                while (obfuscateCount > 0)
                {
                    obfuscate(mathmlTreeHolder.Value);
                    obfuscateCount--;
                }

            return mathmlTreeHolder;
        }

        private void obfuscate(XElement node)
        {
            Walker.WalkWithAlgorithm(node, new SimpleAlgorithm());
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

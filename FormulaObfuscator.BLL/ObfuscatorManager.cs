using FormulaObfuscator.BLL.Algorithms;
using FormulaObfuscator.BLL.Helpers;
using FormulaObfuscator.BLL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL
{
    public class ObfuscatorManager
    {
        public string UploadedText { get; set; }

        public ObfuscatorManager(string uploadedText)
        {
            UploadedText = uploadedText;
        }

        public Holder RunObfuscate(Settings settings)
        {
            ApplySettings(settings);
            var reader = new HTMLReader(UploadedText);
            var mathmlTrees = new List<XElement>();
            
            try
            {
                reader.ConvertToMathMLTree(mathmlTrees);
            }
            catch
            {
                return Holder.Fail(ErrorMsgs.CONVERT_FAILED_MSG);
            }

            if (mathmlTrees.Any())
            {
                foreach (var tree in mathmlTrees)
                {
                    var obfuscateCount = 2;

                    while (obfuscateCount > 0)
                    {
                        Obfuscate(tree);
                        obfuscateCount--;
                    }
                }
            }

            return Holder.Success(reader.SubstituteObfuscatedMathMLTree(new Queue<XElement>(mathmlTrees)));
        }

        private void Obfuscate(XElement node)
        {
            Walker.WalkWithAlgorithm(node, new SimpleAlgorithm());
        }

        private void ApplySettings(Settings settings)
        {
            Randoms.RecursionDepth = settings.RecursionDepth;
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

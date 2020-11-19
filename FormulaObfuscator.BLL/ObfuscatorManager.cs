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
            var obfuscated = new List<XElement>();
            
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
                    var obfuscateCount = 1;
                    var level = Level.Variables;

                    while (obfuscateCount > 0)
                    {
                        obfuscated.Add(Obfuscate(tree, level));
                        obfuscateCount--;
                    }
                }
            }

            return Holder.Success(reader.SubstituteObfuscatedMathMLTree(new Queue<XElement>(obfuscated)));
        }

        private XElement Obfuscate(XElement node, Level level)
        {
            switch(level)
            {
                case Level.Full:
                    return Walker.WalkWithAlgorithmForWholeFormula(node);
                case Level.Fraction:
                    return Walker.WalkWithAlgorithmForAllFractionsInRoot(node);
                case Level.Variables:
                    return Walker.WalkWithAlgorithmForRootVariables(node);
            }

            return node;
        }

        private void ApplySettings(Settings settings)
        {
            Randoms.RecursionDepth = settings.RecursionDepth;
        }

}
}

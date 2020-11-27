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

        private void ApplySettings(Settings settings)
        {
            Settings.CurrentSettings = settings;
            Randoms.ResetSettings();
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
                    var obfuscateCount = Settings.CurrentSettings.ObfucateCount;
                    while (obfuscateCount > 0)
                    {
                        obfuscated.Add(Obfuscate(tree, Settings.CurrentSettings.ObfuscateLevel));
                        obfuscateCount--;
                    }
                }
            }

            return Holder.Success(reader.SubstituteObfuscatedMathMLTree(new Queue<XElement>(obfuscated)));
        }

        private static XElement Obfuscate(XElement node, ObfuscateLevel level)
        {
            return level switch
            {
                ObfuscateLevel.Full => Walker.WalkWithAlgorithmForWholeFormula(node),
                ObfuscateLevel.Fraction => Walker.WalkWithAlgorithmForAllFractionsInRoot(node),
                ObfuscateLevel.Variables => Walker.WalkWithAlgorithmForRootVariables(node),
                _ => node,
            };
        }
    }
}

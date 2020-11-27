using FormulaObfuscator.BLL.TestSamplesGenerator;
using System.Collections.Generic;

namespace FormulaObfuscator.BLL.Models
{
    public class Settings
    {
        public int RecursionDepth { get; set; }
        public ObfuscateLevel ObfuscateLevel { get; set; }
        public string Letters { get; set; } 
        public string GreekLetters { get; set; }
        public int MinNumber { get; set; }
        public int MaxNumber { get; set; }
        public List<SimpleGeneratorMethod> SimpleMethods { get; set; }
        public List<ComplexGeneratorMethod> ComplexMethods { get; set; }
        public List<TypeOfMethod> MethodsForZeroGenerator { get; set; }
        public List<TypeOfMethod> MethodsForOneGenerator { get; set; }
        public List<SamplesGeneratorMethod> MethodsForSamplesGenerator { get; set; }
    }
}

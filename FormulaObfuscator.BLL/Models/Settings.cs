using FormulaObfuscator.BLL.Generators;
using FormulaObfuscator.BLL.TestSamplesGenerator;
using System.Collections.Generic;

namespace FormulaObfuscator.BLL.Models
{
    public class Settings
    {
        public int RecursionDepth { get; set; } = 3;
        public ObfuscateLevel ObfuscateLevel { get; set; } = ObfuscateLevel.Variables;
        public int ObfucateCount { get; set; } = 1;
        public int ObfucateProbability { get; set; } = 70;
        public string Letters { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0abcdefghijklmnopqrstuwyz";
        public string GreekLetters { get; set; } = "αβγδεζηθικλμνξοπρσςτυφχψanω";
        public int MinNumber { get; set; } = 0;
        public int MaxNumber { get; set; } = 100;
        public List<SimpleGeneratorMethod> SimpleMethods { get; set; } = new List<SimpleGeneratorMethod> { SimpleGeneratorMethod.Fraction, SimpleGeneratorMethod.Power, SimpleGeneratorMethod.Root };
        public List<ComplexGeneratorMethod> ComplexMethods { get; set; } = new List<ComplexGeneratorMethod> { ComplexGeneratorMethod.Fraction, ComplexGeneratorMethod.Root };
        public List<TypeOfMethod> MethodsForZeroGenerator { get; set; } = new List<TypeOfMethod>(EqualsZeroGenerator.PossibleFormulas);
        public List<TypeOfMethod> MethodsForOneGenerator { get; set; } = new List<TypeOfMethod>(EqualsOneGenerator.PossibleFormulas);
        public List<SamplesGeneratorMethod> MethodsForSamplesGenerator { get; set; } = new List<SamplesGeneratorMethod> { SamplesGeneratorMethod.DetSumPi, SamplesGeneratorMethod.Fi, SamplesGeneratorMethod.FuncBracket, SamplesGeneratorMethod.Integral, SamplesGeneratorMethod.Limit, SamplesGeneratorMethod.SqrtRecursive, SamplesGeneratorMethod.Sum };

        public static Settings CurrentSettings { get; set; } = new Settings();
    }
}

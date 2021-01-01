using FormulaObfuscator.BLL.Deobfuscators.StructurePatterns;
using System.Collections.Generic;

namespace FormulaObfuscator.BLL.Deobfuscators
{
    public static class DeobfuscationManager
    {
        public static readonly List<IStructurePattern> AvailableVariableStructurePatterns = new List<IStructurePattern>()
        {
            new VariableStructureFlatPattern(),
            new VariableStructureFractionPattern()
        };

        public static readonly List<IStructurePattern> AvailableFormulaStructurePatterns = new List<IStructurePattern>()
        {
            new FormulaStructureFlatPattern(),
            new FormulaStructureFractionPattern()
        };
    }
}

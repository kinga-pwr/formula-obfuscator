using FormulaObfuscator.BLL.Deobfuscators.StructurePatterns;
using System.Collections.Generic;

namespace FormulaObfuscator.BLL.Deobfuscators
{
    public static class DeobfuscationManager
    {
        public static readonly List<IStructurePattern> AvailableStructurePatterns = new List<IStructurePattern>()
        {
            new VariableStructurePattern()
        };
    }
}

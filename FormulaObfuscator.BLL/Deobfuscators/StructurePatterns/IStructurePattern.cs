using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Deobfuscators.StructurePatterns
{
    public interface IStructurePattern
    {
        bool DetectObfuscation(XElement element);
        XElement RemoveObfuscation(XElement element);
    }
}

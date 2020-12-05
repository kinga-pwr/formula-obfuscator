using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Deobfuscators.ResultValuePatterns
{
    public interface IResultValuePattern
    {
        bool ValidateResultValue(XElement element);
    }
}

using FormulaObfuscator.BLL.Models;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Generators
{
    public interface IGenerator
    {
        XElement Generate(TypeOfMethod type);
        TypeOfMethod[] GetPossibleFormulas();
    }
}

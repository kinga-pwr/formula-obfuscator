using FormulaObfuscator.BLL.Models;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Generators
{
    public interface IGenerator
    {
        XElement Generate(TypeOfFormula type);
        TypeOfFormula[] GetPossibleFormulas();
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Algorithms
{
    public interface IAlgorithm
    {
        void makeObfuscate(XElement leaf);
    }
}

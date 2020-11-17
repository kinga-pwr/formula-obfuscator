using FormulaObfuscator.BLL.Helpers;
using FormulaObfuscator.BLL.Models;
using FormulaObfuscator.BLL.TestSamplesGenerator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.TestGenerator
{
    public class SamplesGenerator
    {
        List<ITestSample> testSamples;
        public SamplesGenerator()
        {
            testSamples = new List<ITestSample>();
            testSamples.Add(new DetSumPi());
            testSamples.Add(new Fi());
            testSamples.Add(new FuncBracket());
            testSamples.Add(new Integral());
            testSamples.Add(new Limit());
            testSamples.Add(new SqrtRecursive());
            testSamples.Add(new Sum());
        }

        public string DrawSample()
        {
            string result = @"<math display=""block""
                            xmlns=""http://www.w3.org/1998/Math/MathML"">";
            result += testSamples[Randoms.Int(0, testSamples.Count - 1)].Generate();
            result += "</math>";
            return result;
        }

        public string DrawMany(int n)
        {
            string result = @"<math display=""block""
                            xmlns=""http://www.w3.org/1998/Math/MathML"">";
            for (int i = 0; i < n; i++)
            {
                result += testSamples[Randoms.Int(0, testSamples.Count - 1)].Generate();
                if (i != n - 1)
                    result += new XElement(MathMLTags.Operator, Randoms.Operator().ToString());
            }
            result += "</math>";
            return result;
        }


    }
}

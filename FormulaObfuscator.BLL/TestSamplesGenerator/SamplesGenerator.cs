﻿using FormulaObfuscator.BLL.Helpers;
using FormulaObfuscator.BLL.Models;
using FormulaObfuscator.BLL.TestSamplesGenerator;
using System.Collections.Generic;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.TestGenerator
{
    public class SamplesGenerator
    {
        List<ITestSample> testSamples = new List<ITestSample>();

        private void ApplySettings(Settings settings)
        {
            Settings.CurrentSettings = settings;

            testSamples = new List<ITestSample>();
            foreach (var generatorMethod in settings.MethodsForSamplesGenerator)
            {
                switch (generatorMethod)
                {
                    case SamplesGeneratorMethod.Integral:
                        testSamples.Add(new Integral());
                        break;
                    case SamplesGeneratorMethod.Limit:
                        testSamples.Add(new Limit());
                        break;
                    case SamplesGeneratorMethod.SqrtRecursive:
                        testSamples.Add(new SqrtRecursive());
                        break;
                    case SamplesGeneratorMethod.Sum:
                        testSamples.Add(new Sum());
                        break;
                }
            }
        }

        public string DrawSample(Settings settings)
        {
            ApplySettings(settings);
            int index = Randoms.Int(0, testSamples.Count);

            string mathML = @"<html><math display=""block"" xmlns=""http://www.w3.org/1998/Math/MathML"">";
            mathML += testSamples[index].Generate();
            System.Console.WriteLine(testSamples[index].ToString());
            mathML += "</math></html>";

            XDocument result = XDocument.Parse(mathML);
            return result.ToString();
        }

        public string DrawMany(int n)
        {
            string mathML = @"<html><math display=""block""
                            xmlns=""http://www.w3.org/1998/Math/MathML"">";
            for (int i = 0; i < n; i++)
            {
                mathML += testSamples[Randoms.Int(0, testSamples.Count - 1)].Generate();
                if (i != n - 1)
                    mathML += new XElement(MathMLTags.Operator, Randoms.Operator().ToString());
            }
            mathML += "</math></html>";

            XDocument result = XDocument.Parse(mathML);
            return result.ToString();
        }
    }
}

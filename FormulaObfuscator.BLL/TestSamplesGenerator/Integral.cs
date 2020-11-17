using System;
using FormulaObfuscator.BLL.Models;
using System.Collections.Generic;
using System.Text;
using FormulaObfuscator.BLL.Helpers;

namespace FormulaObfuscator.BLL.TestSamplesGenerator
{
    class Integral : ITestSample
    {
        public string Generate()
        {
            string D = Randoms.Char().ToString().ToUpper();
            string x = Randoms.Char().ToString().ToLower();
            string y;
            do { y = Randoms.Char().ToString().ToLower(); } while (y == x);

            return @$"
                    <mrow>
                        <msub>
                            <mo>{MathMLSymbols.Integral}</mo>
                            <mi>D</mi>
                        </msub>
                        <mi>d</mi>
                        <mi>{x}</mi>
                        <mspace width=""thinmathspace""></mspace>
                        <mi>d</mi>
                        <mi>{y}</mi>
                    </mrow>
                ";
        }
    }
}

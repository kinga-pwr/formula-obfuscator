using FormulaObfuscator.BLL.Helpers;
using FormulaObfuscator.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaObfuscator.BLL.TestSamplesGenerator
{
    class Limit : ITestSample
    {
        public string Generate()
        {
            string n = Randoms.Char().ToString().ToLower();
            string pi = Randoms.GreekLetter().ToString().ToLower();
            string e;
            do { e = Randoms.GreekLetter().ToString().ToLower(); } while (e == pi);
            

            return $@"
                    <mrow>
                        <munder>
                            <mo lspace=""0em""
                                rspace=""0em"">lim</mo>
                            <mrow>
                                <mi>{n}</mi>
                                <mo stretchy=""false"">→</mo>
                                <mo>{Randoms.Operator().ToString()}</mo>
                                <mn>{MathMLSymbols.Infinite}</mn>
                            </mrow>
                        </munder>
                        <mfrac>
                            <msqrt>
                                <mrow>
                                    <mn>{Randoms.Int(1, 25).ToString()}</mn>
                                    <mi>{pi}</mi>
                                    <mi>{n}</mi>
                                </mrow>
                            </msqrt>
                            <mrow>
                                <mi>{n}</mi>
                                <mo>!</mo>
                            </mrow>
                        </mfrac>
                        <msup>
                            <mrow>
                                <mo>(</mo>
                                <mfrac>
                                    <mi>{n}</mi>
                                    <mi>{e}</mi>
                                </mfrac>
                                <mo>)</mo>
                            </mrow>
                            <mi>{n}</mi>
                        </msup>
                    </mrow>
                    <mo>=</mo>
                    {Randoms.ComplexExpression().ToString()}";
        }
    }
}

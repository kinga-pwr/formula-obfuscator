using FormulaObfuscator.BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaObfuscator.BLL.TestSamplesGenerator
{
    class FuncBracket : ITestSample
    {
        public string Generate()
        {
            string f = Randoms.GreekLetter();
            string x;
            x = Randoms.Char().ToString();
            int int1 = Randoms.Int(0, 25);
            int int2 = Randoms.Int(int1, int1 + 10);
            int int3 = Randoms.Int(int2, int2 + 10);
            int int4 = Randoms.Int(int3, int3 + 10);
            int int5 = Randoms.Int(int4, int4 + 10);


            return $@"
                    <mrow>
                        <mi>{f}</mi>
                        <mo stretchy=""false"">(</mo>
                        <mi>{x}</mi>
                        <mo stretchy=""false"">)</mo>
                        <mo>=</mo>
                        <mrow>
                            <mo>{{</mo>
                            <mtable>
                                <mtr>
                                    <mtd columnalign=""center"">
                                        <mrow>
                                            <mn>{Randoms.SimpleExpression().ToString()}</mn>
                                            <mo>{Randoms.MultiplicityOperator()}</mo>
                                            <mn>{Randoms.SimpleExpression().ToString()}</mn>
                                        </mrow>
                                    </mtd>
                                    <mtd columnalign=""left"">
                                        <mrow>
                                            <mtext>if </mtext>
                                            <mn>{int1}</mn>
                                            <mo>≤</mo>
                                            <mi>{x}</mi>
                                            <mo>≤</mo>
                                            <mn>{int2}</mn>
                                            <mo>;</mo>
                                        </mrow>
                                    </mtd>
                                </mtr>
                                <mtr>
                                    <mtd columnalign=""center"">
                                        <mrow>
                                            <mn>{Randoms.ComplexExpression().ToString()}</mn>
                                            <mo>{Randoms.MultiplicityOperator()}</mo>
                                            <mn>{Randoms.ComplexExpression().ToString()}</mn>
                                        </mrow>
                                    </mtd>
                                    <mtd columnalign=""center"">
                                        <mrow>
                                            <mtext>if </mtext>
                                            <mn>{int3}</mn>
                                            <mo>≤</mo>
                                            <mi>{x}</mi>
                                            <mo>≤</mo>
                                            <mn>{int4}</mn>
                                            <mo>;</mo>
                                        </mrow>
                                    </mtd>
                                </mtr>
                                <mtr>
                                    <mtd columnalign=""center"">
                                        <mn>{Randoms.ComplexExpression().ToString()}</mn>
                                    </mtd>
                                    <mtd columnalign=""center"">
                                        <mrow>
                                            <mtext>if  </mtext>
                                            <mi>{int4}</mi>
                                            <mo>≤</mo>
                                            <mn>{x}</mn>
                                            <mo>;</mo>
                                        </mrow>
                                    </mtd>
                                </mtr>
                            </mtable>
                        </mrow>
                    </mrow>
                ";
        }
    }
}

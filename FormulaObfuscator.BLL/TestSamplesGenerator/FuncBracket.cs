using FormulaObfuscator.BLL.Helpers;

namespace FormulaObfuscator.BLL.TestSamplesGenerator
{
    class FuncBracket : ITestSample
    {
        public string Generate()
        {
            string f = Randoms.GreekLetter();
            string x;
            x = Randoms.Char().ToString();
            int int1 = Randoms.Int();
            int int2 = Randoms.Int(int1, int1 + 10);
            int int3 = Randoms.Int(int2, int2 + 10);
            int int4 = Randoms.Int(int3, int3 + 10);


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
                                            <mn>{Randoms.SimpleExpression()}</mn>
                                            <mo>{Randoms.MultiplicityOperator()}</mo>
                                            <mn>{Randoms.SimpleExpression()}</mn>
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
                                            <mn>{Randoms.ComplexExpression()}</mn>
                                            <mo>{Randoms.MultiplicityOperator()}</mo>
                                            <mn>{Randoms.ComplexExpression()}</mn>
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
                                        <mn>{Randoms.ComplexExpression()}</mn>
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

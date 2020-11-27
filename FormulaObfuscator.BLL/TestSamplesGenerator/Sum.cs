using FormulaObfuscator.BLL.Helpers;

namespace FormulaObfuscator.BLL.TestSamplesGenerator
{
    class Sum : ITestSample
    {
        public string Generate()
        {
            string i = Randoms.Char().ToString().ToLower(), j;
            do { j = Randoms.Char().ToString().ToLower(); } while (j == i);
            return $@"
                    <mrow>
                        <munder>
                            <mo>∑</mo>
                            <mrow>
                                <mfrac linethickness=""0px"">
                                    <mrow>
                                        <mn>{Randoms.SimpleExpression().ToString()}</mn>
                                        <mo>≤</mo>
                                        <mi>{i}</mi>
                                        <mo>≤</mo>
                                        <mi>{Randoms.SimpleExpression().ToString()}</mi>
                                    </mrow>
                                    <mrow>
                                        <mn>{Randoms.SimpleExpression().ToString()}</mn>
                                        <mo>&lt;</mo>
                                        <mi>{j}</mi>
                                        <mo>&lt;</mo>
                                        <mi>{Randoms.SimpleExpression().ToString()}</mi>
                                    </mrow>
                                </mfrac>
                            </mrow>
                        </munder>
                        <mi>{Randoms.Char().ToString().ToUpper()}</mi>
                        <mo stretchy=""false"">(</mo>
                        <mi>{i}</mi>
                        <mo>,</mo>
                        <mi>{j}</mi>
                        <mo stretchy=""false"">)</mo>
                    </mrow>
";
        }
    }
}

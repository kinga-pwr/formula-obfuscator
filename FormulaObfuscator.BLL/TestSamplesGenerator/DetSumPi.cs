using FormulaObfuscator.BLL.Helpers;

namespace FormulaObfuscator.BLL.TestSamplesGenerator
{
    class DetSumPi : ITestSample
    {
        public string Generate()
        {
            string A = Randoms.Char().ToString().ToUpper();
            string n = Randoms.Char().ToString().ToLower();
            string S;
            do { S = Randoms.Char().ToString().ToUpper(); } while (S == A);
            string i;
            do { i = Randoms.Char().ToString().ToLower(); } while (i == n);
            string greek = Randoms.GreekLetter().ToUpper();

            return $@"<mrow>
                        <mrow>
                            <mo lspace='0em'
                                rspace='0em'>det</mo>
                            <mo stretchy='false'>(</mo>
                            <mi>{A}</mi>
                            <mo stretchy='false'>)</mo>
                        </mrow>
                        <mo>=</mo>
                        <munder>
                            <mo>∑</mo>
                            <mrow>
                                <mi>{greek}</mi>
                                <mo>∊</mo>
                                <msub>
                                    <mi>{S}</mi>
                                    <mi>{n}</mi>
                                </msub>
                            </mrow>
                        </munder>
                        <mrow>
                            <mi>ϵ</mi>
                            <mo stretchy='false'>(</mo>
                            <mi>{greek}</mi>
                            <mo stretchy='false'>)</mo>
                        </mrow>
                        <mrow>
                            <munderover>
                                <mo>∏</mo>
                                <mrow>
                                    <mi>{i}</mi>
                                    <mo>=</mo>
                                    <mn>1</mn>
                                </mrow>
                                <mi>{n}</mi>
                            </munderover>
                            <msub>
                                <mi>{A}</mi>
                                <mrow>
                                    <mi>{i}</mi>
                                    <mo>,</mo>
                                    <msub>
                                        <mi>{greek}</mi>
                                        <mi>{i}</mi>
                                    </msub>
                                </mrow>
                            </msub>
                        </mrow>
                    </mrow>";
        }
    }
}

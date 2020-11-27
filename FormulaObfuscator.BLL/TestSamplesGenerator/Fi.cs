using FormulaObfuscator.BLL.Helpers;

namespace FormulaObfuscator.BLL.TestSamplesGenerator
{
    class Fi : ITestSample
    {
        public string Generate()
        {
            string greek = Randoms.GreekLetter();
            string greek2;
            string x, y, i;
            x = Randoms.Char().ToString().ToLower();
            do { y = Randoms.Char().ToString().ToLower(); } while (y == x);
            do { i = Randoms.Char().ToString().ToLower(); } while (i == x && i == y);
            do { greek2 = Randoms.GreekLetter(); } while (greek2 == greek);

            return $@"
                    <mrow>
                          <mrow>
                            <mo>(</mo>
                            <mfrac>
                              <msup>
                                <mo>{greek}</mo>
                                <mn>{Randoms.Int()}</mn>
                              </msup>
                              <mrow>
                                <mo>{greek}</mo>
                                <msup>
                                  <mi>{x}</mi>
                                  <mn>{Randoms.Int()}</mn>
                                </msup>
                              </mrow>
                            </mfrac>
                            <mo>{Randoms.Operator()}</mo>
                            <mfrac>
                              <msup>
                                <mo>{greek}</mo>
                                <mn>{Randoms.Int()}</mn>
                              </msup>
                              <mrow>
                                <mo>{greek}</mo>
                                <msup>
                                  <mi>{y}</mi>
                                  <mn>{Randoms.Int()}</mn>
                                </msup>
                              </mrow>
                            </mfrac>
                            <mo>)</mo>
                          </mrow>
                          <msup>
                            <mrow>
                              <mo minsize=""150%"">|</mo>
                              <mi>{greek2}</mi>
                              <mo stretchy=""false"">(</mo>
                              <mi>{x}</mi>
                              <mo>{Randoms.Operator()}</mo>
                              <mi mathvariant=""normal"">{i}</mi>
                              <mi>{y}</mi>
                              <mo stretchy=""false"">)</mo>
                              <mo minsize=""150%"">|</mo>
                            </mrow>
                            <mn>{Randoms.Int()}</mn>
                          </msup>
                          <mo>=</mo>
                          <mn>{Randoms.SimpleExpression()}</mn>
                        </mrow>";
        }
    }
}

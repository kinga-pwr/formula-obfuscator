using FormulaObfuscator.BLL.Helpers;

namespace FormulaObfuscator.BLL.TestSamplesGenerator
{
    class SqrtRecursive : ITestSample
    {
        public string Generate()
        {
            return $@"
                    <mrow>
                        <msqrt>
                            {Randoms.SimpleExpression().ToString()}
                            <mo>{Randoms.Operator().ToString()}</mo>
                            <msqrt>
                                {Randoms.SimpleExpression().ToString()}
                                <mo>{Randoms.Operator().ToString()}</mo>
                                <msqrt>
                                    {Randoms.SimpleExpression().ToString()}
                                    <mo>{Randoms.Operator().ToString()}</mo>
                                    <msqrt>
                                        {Randoms.SimpleExpression().ToString()}
                                        <mo>{Randoms.Operator().ToString()}</mo>
                                        <msqrt>
                                            {Randoms.SimpleExpression().ToString()}
                                            <mo>{Randoms.Operator().ToString()}</mo>
                                            <msqrt>
                                                {Randoms.SimpleExpression().ToString()}
                                                <mo>{Randoms.Operator().ToString()}</mo>
                                                <msqrt>
                                                    {Randoms.SimpleExpression().ToString()}
                                                    <mo>{Randoms.Operator().ToString()}</mo>
                                                    {Randoms.SimpleExpression().ToString()}
                                                </msqrt>
                                            </msqrt>
                                        </msqrt>
                                    </msqrt>
                                </msqrt>
                            </msqrt>
                        </msqrt>
                    </mrow>";
        }
    }
}

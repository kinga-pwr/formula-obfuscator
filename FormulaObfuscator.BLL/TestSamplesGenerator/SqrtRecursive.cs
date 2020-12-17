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
                            {Randoms.SimpleExpression()}
                            <mo>{Randoms.Operator()}</mo>
                            <msqrt>
                                {Randoms.SimpleExpression()}
                                <mo>{Randoms.Operator()}</mo>
                                <msqrt>
                                    {Randoms.SimpleExpression()}
                                    <mo>{Randoms.Operator()}</mo>
                                    <msqrt>
                                        {Randoms.SimpleExpression()}
                                        <mo>{Randoms.Operator()}</mo>
                                        <msqrt>
                                            {Randoms.SimpleExpression()}
                                            <mo>{Randoms.Operator()}</mo>
                                            <msqrt>
                                                {Randoms.SimpleExpression()}
                                                <mo>{Randoms.Operator()}</mo>
                                                <msqrt>
                                                    {Randoms.SimpleExpression()}
                                                    <mo>{Randoms.Operator()}</mo>
                                                    {Randoms.SimpleExpression()}
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

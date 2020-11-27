﻿using FormulaObfuscator.BLL.Helpers;

namespace FormulaObfuscator.BLL.TestSamplesGenerator
{
    class Matrix : ITestSample
    {
        public string Generate()
        {
            string c = Randoms.Char().ToString();
            string n = c;
            do { n = Randoms.Char().ToString(); } while (n == c);

            return $@"
                    <mrow>
                        <mi>det</mi>
                        <mo>|</mo>
                        <mtable>
                            <mtr>
                                <mtd columnalign='center'>
                                    <msub>
                                        <mi>{c}</mi>
                                        <mn>0</mn>
                                    </msub>
                                </mtd>
                                <mtd columnalign=center'>
                                    <msub>
                                        <mi>{c}</mi>
                                        <mn>1</mn>
                                    </msub>
                                </mtd>
                                <mtd columnalign='center'>
                                    <msub>
                                        <mi>{c}</mi>
                                        <mn>2</mn>
                                    </msub>
                                </mtd>
                                <mtd columnalign='center'>
                                    <mo>…</mo>
                                </mtd>
                                <mtd columnalign='center'>
                                    <msub>
                                        <mi>{c}</mi>
                                        <mi>{n}</mi>
                                    </msub>
                                </mtd>
                            </mtr>
                            <mtr>
                                <mtd columnalign='center'>
                                    <msub>
                                        <mi>{c}</mi>
                                        <mn>1</mn>
                                    </msub>
                                </mtd>
                                <mtd columnalign='center'>
                                    <msub>
                                        <mi>{c}</mi>
                                        <mn>2</mn>
                                    </msub>
                                </mtd>
                                <mtd columnalign='center'>
                                    <msub>
                                        <mi>{c}</mi>
                                        <mn>3</mn>
                                    </msub>
                                </mtd>
                                <mtd columnalign='center'>
                                    <mo>…</mo>
                                </mtd>
                                <mtd columnalign='center'>
                                    <msub>
                                        <mi>{c}</mi>
                                        <mrow>
                                            <mi>{n}</mi>
                                            <mo>+</mo>
                                            <mn>1</mn>
                                        </mrow>
                                    </msub>
                                </mtd>
                            </mtr>
                            <mtr>
                                <mtd columnalign='center'>
                                    <msub>
                                        <mi>{c}</mi>
                                        <mn>2</mn>
                                    </msub>
                                </mtd>
                                <mtd columnalign='center'>
                                    <msub>
                                        <mi>{c}</mi>
                                        <mn>3</mn>
                                    </msub>
                                </mtd>
                                <mtd columnalign='center'>
                                    <msub>
                                        <mi>{c}</mi>
                                        <mn>4</mn>
                                    </msub>
                                </mtd>
                                <mtd columnalign='center'>
                                    <mo>…</mo>
                                </mtd>
                                <mtd columnalign='center'>
                                    <msub>
                                        <mi>{c}</mi>
                                        <mrow>
                                            <mi>{n}</mi>
                                            <mo>+</mo>
                                            <mn>2</mn>
                                        </mrow>
                                    </msub>
                                </mtd>
                            </mtr>
                            <mtr>
                                <mtd columnalign='center'>
                                    <mo>⋮</mo>
                                </mtd>
                                <mtd columnalign='center'>
                                    <mo>⋮</mo>
                                </mtd>
                                <mtd columnalign='center'>
                                    <mo>⋮</mo>
                                </mtd>
                                <mtd columnalign='center'>
                                </mtd>
                                <mtd columnalign='center'>
                                    <mo>⋮</mo>
                                </mtd>
                            </mtr>
                            <mtr>
                                <mtd columnalign='center'>
                                    <msub>
                                        <mi>{c}</mi>
                                        <mi>{n}</mi>
                                    </msub>
                                </mtd>
                                <mtd columnalign='center'>
                                    <msub>
                                        <mi>{c}</mi>
                                        <mrow>
                                            <mi>{n}</mi>
                                            <mo>+</mo>
                                            <mn>1</mn>
                                        </mrow>
                                    </msub>
                                </mtd>
                                <mtd columnalign='center'>
                                    <msub>
                                        <mi>{c}</mi>
                                        <mrow>
                                            <mi>{n}</mi>
                                            <mo>+</mo>
                                            <mn>2</mn>
                                        </mrow>
                                    </msub>
                                </mtd>
                                <mtd columnalign='center'>
                                    <mo>…</mo>
                                </mtd>
                                <mtd columnalign='center'>
                                    <msub>
                                        <mi>{c}</mi>
                                        <mrow>
                                            <mn>2</mn>
                                            <mi>{n}</mi>
                                        </mrow>
                                    </msub>
                                </mtd>
                            </mtr>
                        </mtable>
                        <mo>|</mo>
                        <mo>&gt;</mo>
                        <mn>0</mn>
                    </mrow>";

        }
    }
}

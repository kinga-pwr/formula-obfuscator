using FormulaObfuscator.BLL.Helpers;
using FormulaObfuscator.BLL.Models;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Generators
{
    public static class SimpleExpressionGenerator
    {
        public static XElement FractionNumberNumber()
        {
            XElement main = new XElement(MathMLTags.Fraction);
            XElement nominator = new XElement(MathMLTags.Number, Randoms.Int(31, 40));
            XElement denominator = new XElement(MathMLTags.Number, Randoms.Int(21, 30));

            main.Add(nominator);
            main.Add(denominator);

            return new XElement(MathMLTags.Math, main);
        }

        public static XElement PowerCharNumber()
        {
            XElement main = new XElement(MathMLTags.Power);
            XElement basis = new XElement(MathMLTags.Identifier, Randoms.Char());
            XElement power = new XElement(MathMLTags.Number, Randoms.Int(11, 20));

            main.Add(basis);
            main.Add(power);

            return new XElement(MathMLTags.Math, main);
        }

        public static XElement RootCharNumber()
        {
            XElement main = new XElement(MathMLTags.Sqrt);
            XElement degree = new XElement(MathMLTags.Identifier, Randoms.Char());
            XElement element = new XElement(MathMLTags.Number, Randoms.Int(1, 10));

            main.Add(degree);
            main.Add(element);

            return new XElement(MathMLTags.Math, main);
        }
        /// <summary>
        /// <code>
        /// <para>(math display="block")               </para>
        /// <para>    (mrow)                           </para>
        /// <para>        (munderover)                 </para>
        /// <para>            (mo)∑(/mo)               </para>
        /// <para>            (mrow)                   </para>
        /// <para>                (mi)k(/mi)           </para>
        /// <para>                (mo)=(/mo)           </para>
        /// <para>                (mn)1(/mn)           </para>
        /// <para>            (/mrow)                  </para>
        /// <para>            (mi)r(/mi)               </para>
        /// <para>        (/munderover)                </para>
        /// <para>        (msub)                       </para>
        /// <para>            (mi)c(/mi)               </para>
        /// <para>            (mrow)                   </para>
        /// <para>                (mi)k(/mi)           </para>
        /// <para>            (/mrow)                  </para>
        /// <para>        (/msub)                      </para>
        /// <para>    (/mrow)                          </para>
        /// <para>(/math)                              </para>
        ///</code>
        /// </summary>
        /// <returns></returns>
        public static XElement Sum()
        {
            XElement rootRow = new XElement(MathMLTags.Row);
            XElement underOver = new XElement(MathMLTags.Underover);
            #region underover
            XElement epsilon = new XElement(MathMLTags.Operator, MathMLSymbols.Epsilon);
            underOver.Add(epsilon);

            XElement underEpsilon = new XElement(MathMLTags.Row);

            string k = Randoms.Char().ToString();

            //k
            underEpsilon.Add(new XElement(MathMLTags.Identifier, k));
            //=
            underEpsilon.Add(new XElement(MathMLTags.Operator, MathMLSymbols.Equal));
            //1
            underEpsilon.Add(new XElement(MathMLTags.Identifier, MathMLSymbols.One));
            //r
            #endregion
            underOver.Add(underEpsilon);
            underOver.Add(new XElement(MathMLTags.Identifier, Randoms.CharOrInt()));
            #region sub
            XElement expressionRoot = new XElement(MathMLTags.Sub);
            //c
            expressionRoot.Add(new XElement(MathMLTags.Identifier, Randoms.Char()));
            // k
            XElement subrow = new XElement(MathMLTags.Row);
            subrow.Add(new XElement(MathMLTags.Identifier, k));
            expressionRoot.Add(subrow);
            #endregion
            rootRow.Add(underOver);
            rootRow.Add(expressionRoot);
            return new XElement(MathMLTags.Math, rootRow);
        }
    }
}

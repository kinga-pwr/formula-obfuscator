using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaObfuscator.BLL.Models
{


    public static class MathMLTags
    {
        /// <value>
        /// <para>The top-level element in MathML is math. </para>
        /// <para>Every valid MathML instance must be wrapped in math tags. </para>
        /// <para>In addition you must not nest a second math element in another, but you can have an arbitrary number of other child elements in it.</para>
        /// </value>
        public static string Math => "math";
        /// <value>
        /// <para>The MathML mn element represents a numeric literal which is normally a sequence of digits with a possible separator (a dot or a comma).</para>
        /// <para>However,  it is also allowed to have arbitrary text in it which is actually a numeric quantity, for example "eleven".</para>
        /// </value>
        public static string Number => "mn";
        /// <value>
        /// <para>The MathML mo element represents an operator in a broad sense. </para>
        /// <para>Besides operators in strict mathematical meaning, this element also includes "operators" like parentheses, separators like comma and semicolon, or "absolute value" bars.</para>
        /// </value>
        public static string Operator => "mo";
        /// <value>
        /// <para>The MathML mroot element is used to display roots with an explicit index. </para>
        /// <para>Two arguments are accepted, which leads to the syntax: mroot base index mroot.</para>
        /// </value>
        public static string Root => "mroot";
        /// <value>
        /// <para>The MathML mrow element is used to group sub-expressions, which usually contain one or more operators with their respective operands (such as mi and mn). </para>
        /// <para>This element renders as a horizontal row containing its arguments.</para>
        /// <para>When writing a MathML expression, you should group elements within anmrow in the same way as they are grouped in the mathematical interpretation of the expression.  </para>
        /// </value>
        public static string Row => "mrow";
        /// <value>
        /// <para>The MathML mspace element is used to display a blank space, whose size is set by its attributes.</para>
        /// </value>
        public static string Space => "mspace";
        /// <value>
        /// <para>The MathML msqrt element is used to display square roots (no index is displayed). </para>
        /// <para>The square root accepts only one argument, which leads to the following syntax: msqrt base /msqrt.</para>
        /// </value>

        public static string Sqrt => "msqrt";
        /// <value>
        /// <para>The MathML mfrac element is used to display fractions.</para>
        /// </value>

        public static string Fraction => "mfrac";
        /// <value>
        /// <para>The MathML msub element is used to attach a subscript to an expression.</para>
        /// <para>It uses the following syntax: msub base subscript /msub.</para>
        /// </value>

        public static string Sub => "msub";
        /// <value>
        /// <para>The MathML mi element indicates that the content should be rendered as an identifier such as function names, variables or symbolic constants. </para>
        /// <para>You can also have arbitrary text in it to mark up terms.</para>
        /// </value>
        public static string Identifier => "mi";


        /// <value>
        /// <para>The MathML msup element is used to attach a superscript to an expression.</para>
        /// <para>It uses the following syntax: msup base superscript /msup.</para>
        /// </value>
        public static string Power => "msup";


        /// <summary>
        /// Element used to create a set of elements with one in the middle and one above and below.
        /// It allows to create an integral structure.
        /// </summary>
        public static string Integral => "munderover";
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaObfuscator.BLL.Models
{
    public static class MathMLSymbols
    {
        public static string Multiply => "&sdot;";
        public static string Integral => "&int;";

        /// <summary>
        /// >
        /// </summary>
        public static string LessThan => "&gt;";

        public static string Epsilon => "∑";

        public static string Equal => "=";

        public static string One => "1";
        public static string Divide => "/";
        public static string Infinite => "∞";
        public static string PI => "π";
    }
}

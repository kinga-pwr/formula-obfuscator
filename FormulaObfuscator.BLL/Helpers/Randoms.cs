using FormulaObfuscator.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FormulaObfuscator.BLL.Helpers
{
    public static class Randoms
    {
        private static Random random = new Random();
        private static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static string operators = "+-";

        public static string String(int length = 1)
        {
            return new string(Enumerable.Repeat(chars.ToLower(), length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static char Char()
        {
            return (char)random.Next('a', 'z');
        }
        public static int Int()
        {
            return random.Next();
        }
        public static int Int(int max)
        {
            return random.Next(max);
        }
        public static int Int(int min, int max)
        {
            return random.Next(min, max);
        }

        public static MathOperator Operator()
        {
            return new MathOperator(operators[Randoms.Int(operators.Length)]);
        }

    }
}

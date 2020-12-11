using FormulaObfuscator.BLL.Generators;
using FormulaObfuscator.BLL.Models;
using System;
using System.Linq;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Helpers
{
    public static class Randoms
    {
        private static readonly Random random = new Random();
        public static string Chars => Settings.CurrentSettings.Letters;
        public static string Greek => Settings.CurrentSettings.GreekLetters;
        private static readonly string operators = "+-";

        public static void ResetSettings()
        {
            RecursionDepth = Settings.CurrentSettings.RecursionDepth;
        }

        public static int RecursionDepth { get; set; } = Settings.CurrentSettings.RecursionDepth;

        public static char Char() => Chars[random.Next(Chars.Length)];

        public static int Int() => random.Next(Settings.CurrentSettings.MinNumber, Settings.CurrentSettings.MaxNumber);

        public static int Int(int max) => random.Next(max);

        public static int Int(int min, int max) => random.Next(min, max);

        public static MathOperator Operator() => new MathOperator(operators[Int(operators.Length)]);

        public static XElement OperatorXElement() => new XElement(MathMLTags.Operator, Operator().ToString());

        public static XElement SimpleExpression()
        {
            var possibleExpressions = typeof(SimpleExpressionGenerator).GetMethods();
            var chosenMethod = Settings.CurrentSettings.SimpleMethods[Int(0, Settings.CurrentSettings.SimpleMethods.Count)];
            return (XElement)possibleExpressions[(int)chosenMethod].Invoke(null, null);
        }
        public static XElement ComplexExpression()
        {
            var possibleExpressions = typeof(ComplexExpressionGenerator).GetMethods();
            var chosenMethod = Settings.CurrentSettings.ComplexMethods[Int(0, Settings.CurrentSettings.ComplexMethods.Count)];
            return (XElement)possibleExpressions[(int)chosenMethod].Invoke(null, null);
        }

        public static string GreekLetter(int length = 1)
            => new string(Enumerable.Repeat(Greek.ToLower(), length).Select(s => s[random.Next(s.Length)]).ToArray());

        public static string MultiplicityOperator()
        {
            if (Int(0, 4) % 2 == 0)
            {
                return MathMLSymbols.Divide;
            }

            return MathMLSymbols.Multiply;
        }
    }
}

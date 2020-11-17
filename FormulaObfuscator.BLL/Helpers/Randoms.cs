using FormulaObfuscator.BLL.Generators;
using FormulaObfuscator.BLL.Models;
using System;
using System.Linq;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Helpers
{
    public static class Randoms
    {
        private static Random random = new Random();
        private static string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static string greek = "αβγδεζηθικλμνξοπρσςτυφχψanω";
        private static string operators = "+-";

        public static int RecursionDepth { get; set; } = 3;

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

        public static string CharOrInt()
        {
            if (Int(0, 4) % 2 == 0)
            {
                return Int(0, 145).ToString();
            }

            return Char().ToString();
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
            return new MathOperator(operators[Int(operators.Length)]);
        }

        public static XElement OperatorXElement()
        {
            return new XElement(MathMLTags.Operator, Operator().ToString());
        }

        public static XElement SimpleExpression()
        {
            var possibleExpressions = typeof(SimpleExpressionGenerator).GetMethods();
            return (XElement)possibleExpressions[Int(possibleExpressions.Count() - 4)].Invoke(null, null);
            //switch (possibleExpressions[Randoms.Int(possibleExpressions.GetLength(0))].Name)
            //{
            //    case "FractionNumberNumber":
            //        return SimpleExpressionGenerator.FractionNumberNumber();
            //    default:
            //        break;
            //}
            //return null;
        }
        public static XElement ComplexExpression()
        {
            var possibleExpressions = typeof(ComplexExpressionGenerator).GetMethods();
            return (XElement)possibleExpressions[Int(possibleExpressions.Count() - 4)].Invoke(null, null);
        }

        public static string GreekLetter(int length = 1)
        {
            return new string(Enumerable.Repeat(greek.ToLower(), length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

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

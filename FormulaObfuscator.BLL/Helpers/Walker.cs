﻿using FormulaObfuscator.BLL.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Helpers
{
    public static class Walker
    {
        public static void walkWithAlgorithm(XElement node, IAlgorithm algorithm)
        {
            foreach (XElement child in node.Elements())
            {
                walkWithAlgorithm(child, algorithm);
            }
            // condition when we want to add obfuscate node
            // for now find ) in <mrow>
            if (new List<string>() { ")", "}", "]" }.Contains(node.Value.ToString()))
            {
                algorithm.Obfuscate(node);
            }
        }

        public static Holder<XElement> findTreeWithValue(XElement node, string value)
        {
            if (node.Name.ToString().Contains(value))
            {
                return Holder<XElement>.Success(node);
            }

            foreach (XElement child in node.Elements())
            {
                var res = findTreeWithValue(child, value);
                if (res.WasSuccessful)
                    return res;
            }

            return Holder<XElement>.Fail(ErrorMsgs.CONVERT_FAILED_MATHML_MSG); ;
        }
    }
}

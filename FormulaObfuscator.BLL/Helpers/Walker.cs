using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Helpers
{
    public static class Walker
    {
        public static void walk(XElement node)
        {
            foreach (XElement child in node.Elements())
            {
                if (child.HasElements)
                {
                    walk(child);
                }
            }
        }

        public static Holder<XElement> findTreeWithValue(XElement node, string value)
        {
            if (node.Name == value)
                return Holder<XElement>.Success(node);

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

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace FormulaObfuscator.BLL.Helpers
{
    public class Holder
    {
        private Holder(XElement value)
        {
            WasSuccessful = true;
            Value = value;
        }

        private Holder(string errorMsg)
        {
            WasSuccessful = false;
            ErrorMsg = errorMsg;
        }

        public bool WasSuccessful { get; }
        public XElement Value { get; }
        public string ErrorMsg { get; }

        public static Holder Success(XElement value)
        {
            return new Holder(value);
        }

        public static Holder Fail(string errorMsg)
        {
            return new Holder(errorMsg);
        }
    }
}

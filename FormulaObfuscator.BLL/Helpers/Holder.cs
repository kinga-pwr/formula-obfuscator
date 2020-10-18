using System;
using System.Collections.Generic;
using System.Text;

namespace FormulaObfuscator.BLL.Helpers
{
    public class Holder<T>
    {
        private Holder(T value)
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
        public T Value { get; }
        public string ErrorMsg { get; }

        public static Holder<T> Success(T value)
        {
            return new Holder<T>(value);
        }

        public static Holder<T> Fail(string errorMsg)
        {
            return new Holder<T>(errorMsg);
        }
    }
}

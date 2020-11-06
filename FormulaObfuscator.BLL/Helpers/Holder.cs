namespace FormulaObfuscator.BLL.Helpers
{
    public class Holder
    {
        public bool WasSuccessful { get; private set; }
        public string Value { get; private set; }
        public string ErrorMsg { get; private set; }

        public static Holder Success(string value)
        {
            return new Holder()
            {
                WasSuccessful = true,
                Value = value
            };
        }

        public static Holder Fail(string errorMsg)
        {
            return new Holder()
            {
                WasSuccessful = false,
                ErrorMsg = errorMsg
            };
        }
    }
}

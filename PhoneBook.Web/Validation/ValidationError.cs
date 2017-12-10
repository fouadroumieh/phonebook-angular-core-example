namespace PhoneBook.Web.Validation
{
    public class ValidationError
    {
        public static class Type
        {
            public static string Input = "input";
            public static string Domain = "domain";
        }

        public string ErrorType { get; set; }
        public string ErrorCode { get; set; }
        public object Source { get; set; }
        public string ErrorMessage { get; set; }

        public ValidationError()
        {
        }

        public ValidationError(string type, string code, object source, string message)
        {
            ErrorType = type;
            ErrorCode = code;
            Source = source;
            ErrorMessage = message;
        }
    }
}

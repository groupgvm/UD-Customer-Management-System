using System.Globalization;

namespace CM.Middleware
{
    public class CustomException : Exception
    {
        public CustomException() : base() { }
        public CustomException(string message) : base(message) { }
        public CustomException(string message, Exception innerException) : base(message, innerException) { }
        public CustomException(string message, params object[] args)
        : base(String.Format(CultureInfo.CurrentCulture, message, args)) { }
    }
}

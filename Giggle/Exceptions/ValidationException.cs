using System;

namespace Giggle.Exceptions
{
    public class ValidationException : Exception
    {
        public string? PropertyName { get; }
        public string? UserMessage { get; }

        public ValidationException()
        {
        }

        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public ValidationException(string propertyName, string message, string userMessage)
            : base(message)
        {
            PropertyName = propertyName;
            UserMessage = userMessage;
        }
    }
}

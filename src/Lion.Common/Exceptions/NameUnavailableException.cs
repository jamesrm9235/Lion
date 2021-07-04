using System;

namespace Lion.Common.Exceptions
{
    public class NameUnavailableException : Exception
    {
        public NameUnavailableException()
        {
        }

        public NameUnavailableException(string message)
            : base(message)
        {
        }

        public NameUnavailableException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

using System;

namespace Lion.Abstractions
{
    public class BundleNameUnavailableException : Exception
    {
        public BundleNameUnavailableException()
        {

        }
        public BundleNameUnavailableException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

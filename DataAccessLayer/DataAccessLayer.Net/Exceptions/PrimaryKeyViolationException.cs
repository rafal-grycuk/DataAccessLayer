using System;

namespace DataAccessLayer.Net.Exceptions
{
    public class PrimaryKeyViolationException : Exception
    {
        public PrimaryKeyViolationException()
        {
        }

        public PrimaryKeyViolationException(string message)
            : base(message)
        {
        }

        public PrimaryKeyViolationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
using System;

namespace DataAccessLayer.Core.EntityFramework.Exceptions
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
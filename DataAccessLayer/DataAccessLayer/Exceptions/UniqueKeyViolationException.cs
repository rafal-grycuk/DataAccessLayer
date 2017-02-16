using System;

namespace DataAccessLayer.Exceptions
{
    public class UniqueKeyViolationException : Exception
    {
        public UniqueKeyViolationException() 
            : base("You cannot remove this object beacuse another object is related to it.")
        {
        }

        public UniqueKeyViolationException(string message)
            : base(message)
        {
        }

        public UniqueKeyViolationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
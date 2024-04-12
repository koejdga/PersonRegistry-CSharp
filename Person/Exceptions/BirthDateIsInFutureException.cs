using System;

namespace Person.Exceptions
{
    public class BirthDateIsInFutureException : Exception
    {
        public BirthDateIsInFutureException()
        {
        }

        public BirthDateIsInFutureException(string message)
            : base(message)
        {
        }

        public BirthDateIsInFutureException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

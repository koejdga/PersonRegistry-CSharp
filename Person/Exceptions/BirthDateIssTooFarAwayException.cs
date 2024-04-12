using System;

namespace Person.Exceptions
{
    class BirthDateIssTooFarAwayException : Exception
    {
        public BirthDateIssTooFarAwayException()
        {
        }

        public BirthDateIssTooFarAwayException(string message)
            : base(message)
        {
        }

        public BirthDateIssTooFarAwayException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

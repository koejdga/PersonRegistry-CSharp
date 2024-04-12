using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Person.Exceptions
{
    class InvalidEmailAddressException : Exception
    {
        public InvalidEmailAddressException()
        {
        }

        public InvalidEmailAddressException(string message)
            : base(message)
        {
        }

        public InvalidEmailAddressException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

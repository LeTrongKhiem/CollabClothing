using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollabClothing.Utilities.Exceptions
{
    public class CollabException : Exception
    {
        public CollabException()
        {
        }

        public CollabException(string message)
            : base(message)
        {
        }

        public CollabException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

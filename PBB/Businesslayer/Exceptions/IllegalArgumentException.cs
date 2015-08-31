using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PBB.Businesslayer.Exceptions
{
    public class IllegalArgumentException : Exception
    {
        public IllegalArgumentException(string message)
            : base(message)
        {
        }
    }
}

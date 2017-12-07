using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Platform.EF.Exceptions
{
    public class ConcurrentModificationException : Exception
    {
        public ConcurrentModificationException()
        {
        }

        public ConcurrentModificationException(string message)
            : base(message)
        {
        }

        public ConcurrentModificationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}

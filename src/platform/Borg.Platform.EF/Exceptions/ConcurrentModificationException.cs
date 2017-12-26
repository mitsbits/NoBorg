using System;

namespace Borg.Platform.EF.Exceptions
{
    public class ConcurrentModificationException : BorgEfException
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
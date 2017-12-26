using System;

namespace Borg.Platform.EF.Exceptions
{
    public abstract class BorgEfException : Exception
    {
        protected BorgEfException()
        {
        }

        protected BorgEfException(string message)
            : base(message)
        {
        }

        protected BorgEfException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
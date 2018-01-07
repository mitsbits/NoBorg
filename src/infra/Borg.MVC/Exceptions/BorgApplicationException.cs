using System;

namespace Borg.MVC.Exceptions
{
    public class BorgApplicationException : Exception
    {
        public BorgApplicationException(string message) : base(message)
        {
        }

        public BorgApplicationException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
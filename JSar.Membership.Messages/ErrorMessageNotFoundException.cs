using System;

namespace JSar.Membership.Messages
{
    public class ErrorMessageNotFoundException : Exception
    {
        public ErrorMessageNotFoundException()
        {
        }

        public ErrorMessageNotFoundException(string message)
            : base(message)
        {
        }

        public ErrorMessageNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

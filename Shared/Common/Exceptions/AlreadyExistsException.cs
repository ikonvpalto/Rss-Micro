using System;
using System.Net;

namespace Common.Exceptions
{
    public sealed class AlreadyExistsException : BaseHttpException
    {
        public AlreadyExistsException(string message)
            : base(message, HttpStatusCode.Conflict)
        {
        }

        public AlreadyExistsException(string message, Exception innerException)
            : base(message, innerException, HttpStatusCode.Conflict)
        {
        }
    }
}

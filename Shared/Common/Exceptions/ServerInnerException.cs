using System;
using System.Net;

namespace Common.Exceptions
{
    public sealed class ServerInnerException : BaseHttpException
    {
        public ServerInnerException(string message)
            : base(message, HttpStatusCode.InternalServerError)
        {
        }

        public ServerInnerException(string message, Exception innerException)
            : base(message, innerException, HttpStatusCode.InternalServerError)
        {
        }
    }
}
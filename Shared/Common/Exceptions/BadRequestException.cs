using System;
using System.Net;

namespace Common.Exceptions
{
    public sealed class BadRequestException : BaseHttpException
    {
        public BadRequestException(string message)
            : base(message, HttpStatusCode.BadRequest)
        {
        }

        public BadRequestException(string message, Exception innerException)
            : base(message, innerException, HttpStatusCode.BadRequest)
        {
        }
    }
}

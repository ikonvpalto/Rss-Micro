using System;
using System.Net;

namespace Common.Exceptions
{
    public class BaseHttpException : Exception
    {
        public BaseHttpException(string message, HttpStatusCode errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public BaseHttpException(string message, Exception innerException, HttpStatusCode errorCode)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public HttpStatusCode ErrorCode { get; }
    }
}

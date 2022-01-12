using System;
using System.Diagnostics;
using System.Net;

namespace Common.Exceptions
{
    public class BaseHttpException : Exception
    {
        public static BaseHttpException Create(string message, HttpStatusCode errorCode)
        {
            return errorCode switch
            {
                HttpStatusCode.Conflict => new AlreadyExistsException(message),
                HttpStatusCode.BadRequest => new BadRequestException(message),
                HttpStatusCode.NotFound => new NotFoundException(message),
                HttpStatusCode.InternalServerError => new ServerInnerException(message),
                _ => new ($"{errorCode:G}: {message}", errorCode)
            };
        }

        protected BaseHttpException(string message, HttpStatusCode errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        protected BaseHttpException(string message, Exception innerException, HttpStatusCode errorCode)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        public HttpStatusCode ErrorCode { get; }
    }
}

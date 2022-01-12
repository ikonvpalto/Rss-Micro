using System;
using System.Net;
using System.Threading.Tasks;
using Common.Exceptions;
using Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Api.Common.Middlewares
{
    // https://jasonwatmore.com/post/2020/10/02/aspnet-core-31-global-error-handler-tutorial
    public sealed class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
                _logger.LogTrace("Incoming request {Path} completed with {Code} status code", context.Request.Path.Value, context.Response.StatusCode);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch(error)
                {
                    case BaseHttpException e:
                        response.StatusCode = (int) e.ErrorCode;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                _logger.LogError("Incoming request {0} failed with {1} status code, error:{2}\n", context.Request.Path.Value, context.Response.StatusCode, error);

                var result = JsonConvert.SerializeObject(new ErrorResponse{ ErrorMessage = error.Message });
                await response.WriteAsync(result);
            }
        }
    }
}

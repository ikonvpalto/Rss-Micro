using System;
using Auth.API.Models;

namespace Auth.API.Exceptions;

public class Auth0Exception : Exception
{
    public Auth0Exception(Auth0ErrorModel error, string message, string requestedPath)
        : base($"{message}; error: {error.Error}, description: {error.ErrorDescription}, requested path: {requestedPath}")
    {
    }
}

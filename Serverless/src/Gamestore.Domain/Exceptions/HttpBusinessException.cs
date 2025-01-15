using System.Net;

namespace Gamestore.Domain.Exceptions;

public class HttpBusinessException : HttpRequestException
{
    public HttpBusinessException()
    { }

    public HttpBusinessException(string? message, HttpStatusCode? statusCode)
        : this(message, null, statusCode)
    { }

    public HttpBusinessException(string? message, Exception? inner, HttpStatusCode? statusCode)
        : base(message, inner, statusCode)
    { }
}
using System.Net;

namespace Gamestore.Serverless.HttpApi.Exceptions
{
    internal class ServiceException : HttpRequestException
    {
        public ServiceException()
        { }

        public ServiceException(string? message, HttpStatusCode? statusCode)
            : this(message, null, statusCode)
        { }

        public ServiceException(string? message, Exception? inner, HttpStatusCode? statusCode)
            : base(message, inner, statusCode)
        { }
    }
}

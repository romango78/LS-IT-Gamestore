using System.Net;
using System.Net.Mime;
using Amazon.Lambda.Annotations.APIGateway;
using Gamestore.Serverless.Exceptions;
using Microsoft.Net.Http.Headers;

namespace Gamestore.Serverless.Extensions
{
    internal static class HttpResultExtensions
    {
        public static IHttpResult AddCorsHeaders(this IHttpResult httpResult)
        {
            ArgumentNullException.ThrowIfNull(httpResult);

            httpResult
                .AddHeader(HeaderNames.AccessControlAllowHeaders, HeaderNames.ContentType)
                .AddHeader(HeaderNames.AccessControlAllowOrigin, "*")
                .AddHeader(HeaderNames.AccessControlAllowMethods, "GET, OPTIONS")
                .AddHeader(HeaderNames.AccessControlMaxAge, "60");

            return httpResult;
        }

        public static IHttpResult AddJsonContentType(this IHttpResult httpResult)
        {
            ArgumentNullException.ThrowIfNull(httpResult);

            httpResult.AddHeader(HeaderNames.ContentType, MediaTypeNames.Application.Json);

            return httpResult;
        }

        public static IHttpResult AddTextContentType(this IHttpResult httpResult)
        {
            ArgumentNullException.ThrowIfNull(httpResult);

            httpResult.AddHeader(HeaderNames.ContentType, MediaTypeNames.Text.Plain);

            return httpResult;
        }

        public static IHttpResult GetHttpResult(this ServiceException exception)
        {
            switch (exception.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return HttpResults.BadRequest(exception.Message)
                        .AddCorsHeaders().AddTextContentType();
                case HttpStatusCode.NotFound:
                    return HttpResults.NotFound(exception.Message)
                        .AddCorsHeaders().AddTextContentType();
                case HttpStatusCode.InternalServerError:
                    return HttpResults.InternalServerError(exception.Message);
                default:
                    return HttpResults.NewResult((HttpStatusCode)exception.StatusCode!, exception.Message);
            }
        }
    }
}

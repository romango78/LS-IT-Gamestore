using System.Net.Mime;
using Amazon.Lambda.Annotations.APIGateway;
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
    }
}

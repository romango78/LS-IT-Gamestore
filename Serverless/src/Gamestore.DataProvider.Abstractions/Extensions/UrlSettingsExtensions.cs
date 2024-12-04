using Gamestore.DataProvider.Abstractions.Models;
using System.Web;

namespace Gamestore.DataProvider.Abstractions.Extensions
{
    public static class UrlSettingsExtensions
    {

        public static Uri Build(this UrlSettings settings)
        {
            return settings.Build(mapping: null);
        }

        public static Uri Build(this UrlSettings settings, IReadOnlyDictionary<string,string>? mapping)
        {
            var url = new Uri(settings.BaseUrl);

            foreach (var parameter in settings.QueryParameters)
            {
                if (mapping?.TryGetValue(parameter.Mapping ?? string.Empty, out string? value) ?? false)
                {
                    url = url.AddParameter(parameter.Name, value);
                }
                else
                {
                    url = url.AddParameter(parameter.Name, parameter.Value);
                }
            }

            return url;
        }

        public static Uri AddParameter(this Uri url, string paramName, string paramValue)
        {
            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query[paramName] = paramValue;
            uriBuilder.Query = query.ToString();

            return uriBuilder.Uri;
        }
    }
}

using Gamestore.DataProvider.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gamestore.DataProvider.Steam.Extensions
{
    public static class DependencyExtensions
    {
        public static void AddSteamDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);

            var section = configuration.GetSection(nameof(DataProviderSettings));
            var settings = section.Get<DataProviderSettings>();

            services.AddSteamHttpClient(settings);
        }

        private static void AddSteamHttpClient(this IServiceCollection services, DataProviderSettings? settings)
        {
            ArgumentNullException.ThrowIfNull(settings);
            if (string.IsNullOrEmpty(settings.HttpClientName))
            {
                throw new ArgumentException("The HttpClient name is not specified for Steam Data Provider.");
            }

            services.AddHttpClient(settings.HttpClientName)
                .ConfigureHttpClient(client =>
                {
                    client.Timeout = TimeSpan.FromSeconds(settings.TimeoutInSeconds);
                })
                .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
                {
                    PooledConnectionLifetime = TimeSpan.FromMinutes(settings.PooledConnectionLifetimeInMinutes),
                    PooledConnectionIdleTimeout = TimeSpan.FromMinutes(settings.PooledConnectionIdleTimeoutInMinutes),
                    MaxConnectionsPerServer = settings.MaxConnectionsPerServer
                })
                .SetHandlerLifetime(TimeSpan.FromSeconds(settings.HandlerLifetimeInSeconds));
        }
    }
}

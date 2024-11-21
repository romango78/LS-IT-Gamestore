using FluentAssertions;
using Gamestore.DataProvider.Models;
using Gamestore.DataProvider.Steam.Extensions;
using Gamestore.DataProvider.Steam.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Gamestore.DataProvider.Steam.Tests.Services
{
    public class SteamDataProviderIntegrationTests
    {
#pragma warning disable NUnit1032
        private readonly IServiceProvider _serviceProvider;
#pragma warning restore NUnit1032

        public SteamDataProviderIntegrationTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var serviceCollection = new ServiceCollection();

            serviceCollection.Configure<DataProviderSettings>(configuration.GetSection(nameof(DataProviderSettings)));
            serviceCollection.AddSteamDependencies(configuration);

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Test]
        public async Task ShouldGetTenFirstGames_FromSteamSource()
        {
            // Arrange

            var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            var options = _serviceProvider.GetRequiredService<IOptions<DataProviderSettings>>();

            var sut = new SteamDataProvider(httpClientFactory, options);

            // Act

            var result = await sut.GetAvailableGameListAsync(CancellationToken.None)
                .Where(m => !string.IsNullOrWhiteSpace(m.Name))
                .OrderByDescending(m => m.GameId)
                .Take(10)
                .ToListAsync(CancellationToken.None);

            // Asserts
            result.Should().NotBeEmpty().And.HaveCount(10);
        }
    }
}
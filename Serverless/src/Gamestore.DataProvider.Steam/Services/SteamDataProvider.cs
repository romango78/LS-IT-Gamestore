using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Gamestore.DataProvider.Models;
using Gamestore.DataProvider.Services;
using Gamestore.DataProvider.Steam.Models;
using Microsoft.Extensions.Options;

namespace Gamestore.DataProvider.Steam.Services
{
    public class SteamDataProvider : IDataProvider
    {
        private const string GameIdPrefix = "EXT_STEAM_";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DataProviderSettings _settings;

        public SteamDataProvider(IHttpClientFactory clientFactory, IOptions<DataProviderSettings> options)
        {
            ArgumentNullException.ThrowIfNull(clientFactory);
            ArgumentNullException.ThrowIfNull(options);
            ArgumentNullException.ThrowIfNull(options.Value);

            _settings = options.Value;
            _httpClientFactory = clientFactory;
        }

        public async IAsyncEnumerable<AvailableGame> GetAvailableGameListAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            using var httpClient = _httpClientFactory.CreateClient(_settings.HttpClientName!);
            using var response = await httpClient.GetAsync(_settings.AvailableGamesUrl, cancellationToken)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var root = await response.Content
                .ReadFromJsonAsync<SteamAppListRootObject>(options: null, cancellationToken).ConfigureAwait(false);

            foreach (var item in root?.List?.Data ?? [])
            {
                yield return new AvailableGame
                {
                    GameId = string.Concat(GameIdPrefix, item.Id),
                    Name = item.Name
                };
            }
        }
    }
}

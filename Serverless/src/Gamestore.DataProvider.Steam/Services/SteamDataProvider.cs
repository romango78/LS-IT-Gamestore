using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Gamestore.DataProvider.Abstractions.Extensions;
using Gamestore.DataProvider.Abstractions.Models;
using Gamestore.DataProvider.Abstractions.Services;
using Gamestore.DataProvider.Steam.Models;
using Microsoft.Extensions.Options;

namespace Gamestore.DataProvider.Steam.Services
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>See https://steamcommunity.com/dev for Steam API documentation</remarks>
    public class SteamDataProvider : IDataProvider
    {
        private const string GameIdPrefix = "EXT_STEAM_";
        private const string GameIdParserPattern = $"^{GameIdPrefix}(?<id>\\d+)$";

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
            var url = _settings.AvailableGamesUrl.Build();

            using var httpClient = _httpClientFactory.CreateClient(_settings.HttpClientName!);
            using var response = await httpClient.GetAsync(url, cancellationToken)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var root = await response.Content
                .ReadFromJsonAsync<SteamAppListRootObject>(options: null, cancellationToken).ConfigureAwait(false);

            foreach (var item in root?.AppList?.Data ?? [])
            {
                yield return new AvailableGame
                {
                    GameId = string.Concat(GameIdPrefix, item.Id),
                    Name = item.Name
                };
            }
        }

        public async Task<GameNews?> GetGameNewsAsync(string gameId, CancellationToken cancellationToken)
        {
            var regex = new Regex(GameIdParserPattern, RegexOptions.IgnoreCase);
            var match = regex.Match(gameId);
            if (!match.Success)
            {
                return null;
            }

            var url = _settings.GameNewsUrl.Build(mapping: new Dictionary<string, string>
            {
                { "gameId", match.Groups["id"].Value }
            });

            using var httpClient = _httpClientFactory.CreateClient(_settings.HttpClientName!);
            using var response = await httpClient.GetAsync(url, cancellationToken)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var root = await response.Content
                .ReadFromJsonAsync<SteamAppNewsRoot>(options: null, cancellationToken).ConfigureAwait(false);

            var news = root?.Root?.NewsItems?.FirstOrDefault();
            if (news is null)
            {
                return null;
            }

            if (!Uri.TryCreate(news.Url, UriKind.Absolute, out var readMoreLink))
            {
                readMoreLink = null;
            }

            return new GameNews
            {
                GameId = gameId,
                Source = "Steam",
                Title = news.Title,
                Contents = news.Contents,
                ReadMoreLink = readMoreLink
            };
        }
    }
}

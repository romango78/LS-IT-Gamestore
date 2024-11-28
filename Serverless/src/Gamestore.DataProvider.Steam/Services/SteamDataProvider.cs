using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using Gamestore.DataProvider.Abstractions.Models;
using Gamestore.DataProvider.Abstractions.Services;
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

        public Task<GameNews?> GetGameNewsAsync(string gameId, CancellationToken cancellationToken)
        {
            return Task.FromResult(new GameNews
            {
                GameId = gameId,
                Source = "Steam",
                Title =
                    "Valve giveth, and Valve taketh away: Team Fortress 2's BLU Scout is once again wearing the 'wrong' pants after a 17 years-in-the-making fix was reversed a day later",
                Contents =
                    "Signs of life from Team Fortress 2 are rare and precious these days, so when Valve updated the in-game model of the Scout, \u003Ca href=\"https://www.pcgamer.com/games/fps/17-years-later-valve-fixes-team-fortress-2-bug-that-made-scouts-pants-the-wrong-color-bug-so-old-it-could-have-enlisted-with-parental-consent/\" target=\"_blank\"\u003Efixing a visual bug that's existed since TF2's release\u003C/a\u003E in 2007, we were paying attention. But as reported by YouTuber \u003Ca href=\"https://www.youtube.com/watch?v=jS9R32G2-lo&ab_channel=shounic\" target=\"_blank\"\u003Eshounic\u003C/a\u003E, the visual tweak was not to last: A \u003Ca href=\"https://store.steampowered.com/news/app/440/view/4520017657931497816?l=english\" target=\"_blank\"\u003Efollow-up patch\u003C/a\u003E has reverted the BLU Scout's pants to their original, incorrect khaki from a smart, team-appropriate navy...",
                ReadMoreLink = new Uri(
                    "https://www.pcgamer.com/games/fps/valve-giveth-and-valve-taketh-away-team-fortress-2s-blu-scout-is-once-again-wearing-the-wrong-pants-after-a-17-years-in-the-making-fix-was-reversed-a-day-later?utm_source=steam&utm_medium=referral")
            })!;
        }
    }
}

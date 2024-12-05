using FluentAssertions;
using Gamestore.DataProvider.Abstractions.Models;
using Gamestore.DataProvider.Steam.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;

namespace Gamestore.DataProvider.Steam.Tests.Services;

public class SteamDataProviderTests
{
    [TestCase("123")]
    [TestCase("EXT_STEAM_")]
    [TestCase("EXT_STEAM_AB")]
    [TestCase("EXT_STEAM_123AB")]
    [TestCase("EXT_STEAM_AB123")]
    public async Task GetNews_ShouldReturnNull_WhenGameIdIsNotRelatedToSteam(string gameId)
    {
        // Arrange
        var httpClientFactory = Mock.Of<IHttpClientFactory>();
        var logger = NullLogger<SteamDataProvider>.Instance;

        var options = Mock.Of<IOptions<DataProviderSettings>>();
        Mock.Get(options)
            .Setup(m => m.Value)
            .Returns(new DataProviderSettings());

        var sut = new SteamDataProvider(httpClientFactory, logger, options);

        // Act
        var result = await sut.GetGameNewsAsync(gameId, CancellationToken.None).ConfigureAwait(false);

        // Asserts
        result.Should().BeNull();
    }
}

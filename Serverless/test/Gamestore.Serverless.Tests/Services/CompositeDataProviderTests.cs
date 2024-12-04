using FluentAssertions;
using Gamestore.DataProvider.Abstractions.Models;
using Gamestore.DataProvider.Abstractions.Services;
using Gamestore.Serverless.Services;
using Moq;
using Xunit;

namespace Gamestore.Serverless.Tests.Services
{
    public class CompositeDataProviderTests
    {
        [Fact]
        public async Task GetGameNews_ShouldReturnNews_WhenGameExists()
        {
            // Arrange
            const string TestGameId = "EXT_SRC_737364A";

            var expectedResult = new GameNews
            {
                GameId = TestGameId,
                Source = "Steam",
                Title =
                    "Valve giveth, and Valve taketh away: Team Fortress 2's BLU Scout is once again wearing the 'wrong' pants after a 17 years-in-the-making fix was reversed a day later",
                Contents =
                    "Signs of life from Team Fortress 2 are rare and precious these days, so when Valve updated the in-game model of the Scout, \u003Ca href=\"https://www.pcgamer.com/games/fps/17-years-later-valve-fixes-team-fortress-2-bug-that-made-scouts-pants-the-wrong-color-bug-so-old-it-could-have-enlisted-with-parental-consent/\" target=\"_blank\"\u003Efixing a visual bug that's existed since TF2's release\u003C/a\u003E in 2007, we were paying attention. But as reported by YouTuber \u003Ca href=\"https://www.youtube.com/watch?v=jS9R32G2-lo&ab_channel=shounic\" target=\"_blank\"\u003Eshounic\u003C/a\u003E, the visual tweak was not to last: A \u003Ca href=\"https://store.steampowered.com/news/app/440/view/4520017657931497816?l=english\" target=\"_blank\"\u003Efollow-up patch\u003C/a\u003E has reverted the BLU Scout's pants to their original, incorrect khaki from a smart, team-appropriate navy...",
                ReadMoreLink = new Uri(
                    "https://www.pcgamer.com/games/fps/valve-giveth-and-valve-taketh-away-team-fortress-2s-blu-scout-is-once-again-wearing-the-wrong-pants-after-a-17-years-in-the-making-fix-was-reversed-a-day-later?utm_source=steam&utm_medium=referral")
            };

            var provider1 = Mock.Of<IDataProvider>();
            Mock.Get(provider1)
                .Setup(m => m.GetGameNewsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GameNews?)null);

            var provider2 = Mock.Of<FakeDataProvider>();
            Mock.Get(provider2)
                .Setup(m => m.GetGameNewsAsync(
                    It.Is<string>(p => p.Equals(TestGameId, StringComparison.OrdinalIgnoreCase)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var sut = new CompositeDataProvider();
            sut.RegisterDataProvider(provider1);
            sut.RegisterDataProvider(provider2);

            // Act
            var result = await sut.GetGameNewsAsync(TestGameId, CancellationToken.None);

            //Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public async Task GetGameNews_ShouldReturnNull_WhenGameDoesNotExist()
        {
            // Arrange
            const string TestGameId = "EXT_SRC_737364A";

            var expectedResult = new GameNews
            {
                GameId = TestGameId,
                Source = "Steam",
                Title =
                    "Valve giveth, and Valve taketh away: Team Fortress 2's BLU Scout is once again wearing the 'wrong' pants after a 17 years-in-the-making fix was reversed a day later",
                Contents =
                    "Signs of life from Team Fortress 2 are rare and precious these days, so when Valve updated the in-game model of the Scout, \u003Ca href=\"https://www.pcgamer.com/games/fps/17-years-later-valve-fixes-team-fortress-2-bug-that-made-scouts-pants-the-wrong-color-bug-so-old-it-could-have-enlisted-with-parental-consent/\" target=\"_blank\"\u003Efixing a visual bug that's existed since TF2's release\u003C/a\u003E in 2007, we were paying attention. But as reported by YouTuber \u003Ca href=\"https://www.youtube.com/watch?v=jS9R32G2-lo&ab_channel=shounic\" target=\"_blank\"\u003Eshounic\u003C/a\u003E, the visual tweak was not to last: A \u003Ca href=\"https://store.steampowered.com/news/app/440/view/4520017657931497816?l=english\" target=\"_blank\"\u003Efollow-up patch\u003C/a\u003E has reverted the BLU Scout's pants to their original, incorrect khaki from a smart, team-appropriate navy...",
                ReadMoreLink = new Uri(
                    "https://www.pcgamer.com/games/fps/valve-giveth-and-valve-taketh-away-team-fortress-2s-blu-scout-is-once-again-wearing-the-wrong-pants-after-a-17-years-in-the-making-fix-was-reversed-a-day-later?utm_source=steam&utm_medium=referral")
            };

            var provider1 = Mock.Of<IDataProvider>();
            Mock.Get(provider1)
                .Setup(m => m.GetGameNewsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GameNews?)null);

            var provider2 = Mock.Of<FakeDataProvider>();
            Mock.Get(provider2)
                .Setup(m => m.GetGameNewsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((GameNews?)null);

            var sut = new CompositeDataProvider();
            sut.RegisterDataProvider(provider1);
            sut.RegisterDataProvider(provider2);

            // Act
            var result = await sut.GetGameNewsAsync(TestGameId, CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetGameNews_ShouldThrowInvalidOperationException_WhenTryAddSameProviderTwice()
        {
            // Arrange
            var provider1 = Mock.Of<IDataProvider>();
            var provider2 = Mock.Of<IDataProvider>();

            var sut = new CompositeDataProvider();
            sut.RegisterDataProvider(provider1);

            // Act
            var action = () => sut.RegisterDataProvider(provider2);

            //Assert
            action.Should().Throw<InvalidOperationException>();
        }
    }

    public abstract class FakeDataProvider : IDataProvider
    {
        protected FakeDataProvider() { }

        public abstract IAsyncEnumerable<AvailableGame> GetAvailableGameListAsync(CancellationToken cancellationToken);

        public abstract Task<GameNews?> GetGameNewsAsync(string gameId, CancellationToken cancellationToken);
    }
}

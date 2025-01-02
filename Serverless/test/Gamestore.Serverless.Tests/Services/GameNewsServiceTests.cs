using Gamestore.DataProvider.Abstractions.Models;
using Gamestore.DataProvider.Abstractions.Services;
using Gamestore.Serverless.Properties;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using FluentAssertions;
using Gamestore.Serverless.Exceptions;
using Gamestore.Serverless.Services;
using Xunit;

namespace Gamestore.Serverless.Tests.Services;

public class GameNewsServiceTests
{
    [Fact]
    public async Task GetNews_ShouldReturnGameNews_WhenNewsExist()
    {
        // Arrange
        const string GameId = "1212";

        var expectedResult = new GameNews
        {
            GameId = GameId,
            Source = "Steam",
            Title = "Game Title",
            Contents = "Game News contents",
            ReadMoreLink = new Uri("http://localhost")
        };

        var dataProvider = Mock.Of<IDataProvider>();
        Mock.Get(dataProvider)
            .Setup(m => m.GetGameNewsAsync(GameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var logger = Mock.Of<ILogger<Functions>>();

        var sut = new GameNewsService(dataProvider, logger);

        // Act
        var result = await sut.GetNewsAsync(GameId, CancellationToken.None);

        // Asserts
        result.Should().BeEquivalentTo(expectedResult);

        Mock.Get(logger)
            .Verify(
                m => m.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Exactly(2));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void GetNews_ShouldThrowServiceException_WithBadRequest_WhenProvidedInvalidGameId(string gameId)
    {
        // Arrange
        var dataProvider = Mock.Of<IDataProvider>();
        var logger = Mock.Of<ILogger<Functions>>();

        var sut = new GameNewsService(dataProvider, logger);

        // Act
        var action = async () => await sut.GetNewsAsync(gameId, CancellationToken.None);

        // Asserts
        action.Should().ThrowAsync<ServiceException>().Where(e => e.StatusCode == HttpStatusCode.BadRequest)
            .WithMessage(ErrorsRes.InvalidGameIdParam);

        Mock.Get(logger)
            .Verify(
                m => m.Log(LogLevel.Warning, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }

    [Fact]
    public void GetNews_ShouldThrowServiceException_WithNotFound_WhenNewsDoesNotExist()
    {
        // Arrange
        const string GameId = "1212";

        var dataProvider = Mock.Of<IDataProvider>();
        Mock.Get(dataProvider)
            .Setup(m => m.GetGameNewsAsync(GameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((GameNews?)null);

        var logger = Mock.Of<ILogger<Functions>>();

        var sut = new GameNewsService(dataProvider, logger);

        // Act
        var action = async () => await sut.GetNewsAsync(GameId, CancellationToken.None);

        // Asserts
        action.Should().ThrowAsync<ServiceException>().Where(e => e.StatusCode == HttpStatusCode.NotFound)
            .WithMessage(ErrorsRes.GameNotFound.Replace("{gameId}", GameId));

        Mock.Get(logger)
            .Verify(
                m => m.Log(LogLevel.Warning, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }

    [Fact]
    public void GetNews_ShouldThrowServiceException_WithInternalServerError_WhenExceptionOccurred()
    {
        // Arrange
        var innerException = new Exception("Something went wrong");

        var dataProvider = Mock.Of<IDataProvider>();
        Mock.Get(dataProvider)
            .Setup(m => m.GetGameNewsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Throws(innerException);
        var logger = Mock.Of<ILogger<Functions>>();

        var sut = new GameNewsService(dataProvider, logger);

        // Act
        var action = async () => await sut.GetNewsAsync("3232", CancellationToken.None);

        // Asserts
        action.Should().ThrowAsync<ServiceException>().Where(e =>
                e.StatusCode == HttpStatusCode.InternalServerError && e.InnerException!.Equals(innerException))
            .WithMessage(ErrorsRes.GeneralError);

        Mock.Get(logger)
            .Verify(
                m => m.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }
}
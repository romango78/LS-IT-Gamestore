using System.Net;
using Amazon.Lambda.Annotations.APIGateway;
using Xunit;
using Amazon.Lambda.TestUtilities;
using FluentAssertions;
using Gamestore.DataProvider.Abstractions.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using System.Text.Json;
using Amazon.Lambda.Serialization.SystemTextJson;
using Gamestore.DataProvider.Abstractions.Models;
using Gamestore.Serverless.Exceptions;
using Gamestore.Serverless.Properties;
using Gamestore.Serverless.Services;

namespace Gamestore.Serverless.Tests;

public class FunctionsTest
{
    [Fact]
    public async Task GetAvailableGameList_ShouldReturnOkResponse_WhenAvailableGamesExist()
    {
        // Arrange
        var context = new TestLambdaContext
        {
            AwsRequestId = Guid.NewGuid().ToString(),
            FunctionName = "GamestoreServerlessGetAvailableGames",
            FunctionVersion = "${LATEST}",
            RemainingTime = TimeSpan.FromSeconds(30)
        };

        var expectedBody = new[]
        {
            new AvailableGame
            {
                GameId = "26353",
                Name = "Game 1"
            },
            new AvailableGame
            {
                GameId = "76328",
                Name = "Game 2"
            }
        };
        var expectedResult = new
        {
            statusCode = (int)HttpStatusCode.OK,
            multiValueHeaders = new Dictionary<string, IList<string>>
            {
                { "access-control-allow-headers", new[] { "Content-Type" } },
                { "access-control-allow-origin", new[] { "*" } },
                { "access-control-allow-methods", new[] { "GET, OPTIONS" } },
                { "access-control-max-age", new[] { "60" } },
                { "content-type", new[] { "application/json" } }
            },
            body = JsonSerializer.Serialize(expectedBody.OrderByDescending(m => m.GameId)),
            isBase64Encoded = false
        };

        var dataProvider = Mock.Of<IDataProvider>();
        Mock.Get(dataProvider)
            .Setup(m => m.GetAvailableGameListAsync(It.IsAny<CancellationToken>()))
            .Returns(expectedBody.ToAsyncEnumerable());

        var logger = Mock.Of<ILogger<Functions>>();

        var sut = new Functions(dataProvider, Mock.Of<IGameNewsService>(), logger);

        // Act
        var result = await sut.GetAvailableGamesAsync(context);
        var responseJson = await GetHttpResponseAsJsonAsync(result);

        // Asserts
        result.Should().NotBeNull()
            .And.Match<IHttpResult>(m => m.StatusCode == HttpStatusCode.OK);
        responseJson.Should().NotBeNull()
            .And.BeEquivalentTo(JsonSerializer.Serialize(expectedResult));

        Mock.Get(logger)
            .Verify(m =>
                m.BeginScope(It.Is<Dictionary<string, object>>(p =>
                    p.ContainsKey(LoggerRes.ScopeCorrelationId) && 
                    p.ContainsKey(LoggerRes.ScopeFunctionName) &&
                    p.ContainsKey(LoggerRes.ScopeFunctionVersion))), Times.Once);
        Mock.Get(logger)
            .Verify(
                m => m.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetAvailableGameList_ShouldReturnOkResponse_WhenAvailableGamesDoesNotExist()
    {
        // Arrange
        var context = new TestLambdaContext
        {
            AwsRequestId = Guid.NewGuid().ToString(),
            FunctionName = "GamestoreServerlessGetAvailableGames",
            FunctionVersion = "${LATEST}",
            RemainingTime = TimeSpan.FromSeconds(30)
        };

        var expectedResult = new
        {
            statusCode = (int)HttpStatusCode.OK,
            multiValueHeaders = new Dictionary<string, IList<string>>
            {
                { "access-control-allow-headers", new[] { "Content-Type" } },
                { "access-control-allow-origin", new[] { "*" } },
                { "access-control-allow-methods", new[] { "GET, OPTIONS" } },
                { "access-control-max-age", new[] { "60" } },
                { "content-type", new[] { "application/json" } }
            },
            body = JsonSerializer.Serialize(Array.Empty<AvailableGame>()),
            isBase64Encoded = false
        };

        var dataProvider = Mock.Of<IDataProvider>();
        Mock.Get(dataProvider)
            .Setup(m => m.GetAvailableGameListAsync(It.IsAny<CancellationToken>()))
            .Returns(Array.Empty<AvailableGame>().ToAsyncEnumerable());

        var logger = Mock.Of<ILogger<Functions>>();

        var sut = new Functions(dataProvider, Mock.Of<IGameNewsService>(), logger);

        // Act
        var result = await sut.GetAvailableGamesAsync(context);
        var responseJson = await GetHttpResponseAsJsonAsync(result);

        // Asserts
        result.Should().NotBeNull()
            .And.Match<IHttpResult>(m => m.StatusCode == HttpStatusCode.OK);
        responseJson.Should().NotBeNull()
            .And.BeEquivalentTo(JsonSerializer.Serialize(expectedResult));

        Mock.Get(logger)
            .Verify(m =>
                m.BeginScope(It.Is<Dictionary<string, object>>(p =>
                    p.ContainsKey(LoggerRes.ScopeCorrelationId) &&
                    p.ContainsKey(LoggerRes.ScopeFunctionName) &&
                    p.ContainsKey(LoggerRes.ScopeFunctionVersion))), Times.Once);
        Mock.Get(logger)
            .Verify(
                m => m.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Exactly(2));
    }

    [Fact]
    public async Task GetAvailableGameList_ShouldReturnInternalServerError_WhenExceptionOccurred()
    {
        // Arrange
        var context = new TestLambdaContext
        {
            AwsRequestId = Guid.NewGuid().ToString(),
            FunctionName = "GamestoreServerlessGetAvailableGames",
            FunctionVersion = "${LATEST}",
            RemainingTime = TimeSpan.FromSeconds(30)
        };

        var expectedErrorMessage = "Something went wrong";
        var expectedResult = new
        {
            statusCode = (int)HttpStatusCode.InternalServerError,
            multiValueHeaders = new Dictionary<string, IList<string>>
            {
                { "content-type", new[] { "text/plain" } }
            },
            body = expectedErrorMessage,
            isBase64Encoded = false
        };

        var dataProvider = Mock.Of<IDataProvider>();
        Mock.Get(dataProvider)
            .Setup(m => m.GetAvailableGameListAsync(It.IsAny<CancellationToken>()))
            .Throws(new Exception(expectedErrorMessage));
        var logger = Mock.Of<ILogger<Functions>>();

        var sut = new Functions(dataProvider, Mock.Of<IGameNewsService>(), logger);

        // Act
        var result = await sut.GetAvailableGamesAsync(context);
        var responseJson = await GetHttpResponseAsJsonAsync(result);

        // Asserts
        result.Should().NotBeNull()
            .And.Match<IHttpResult>(m => m.StatusCode == HttpStatusCode.InternalServerError);
        responseJson.Should().NotBeNull()
            .And.BeEquivalentTo(JsonSerializer.Serialize(expectedResult));

        Mock.Get(logger)
            .Verify(
                m => m.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }

    [Fact]
    public async Task GetNews_ShouldReturnOkResponse_WhenNewsExist()
    {
        // Arrange
        const string GameId = "1212";

        var context = new TestLambdaContext
        {
            AwsRequestId = Guid.NewGuid().ToString(),
            FunctionName = "GamestoreServerlessGetAvailableGames",
            FunctionVersion = "${LATEST}",
            RemainingTime = TimeSpan.FromSeconds(30)
        };

        var expectedBody = new GameNews
        {
            GameId = GameId,
            Source = "Steam",
            Title = "Game Title",
            Contents = "Game News contents",
            ReadMoreLink = new Uri("http://localhost")
        };
        var expectedResult = new
        {
            statusCode = (int)HttpStatusCode.OK,
            multiValueHeaders = new Dictionary<string, IList<string>>
            {
                { "access-control-allow-headers", new[] { "Content-Type" } },
                { "access-control-allow-origin", new[] { "*" } },
                { "access-control-allow-methods", new[] { "GET, OPTIONS" } },
                { "access-control-max-age", new[] { "60" } },
                { "content-type", new[] { "application/json" } }
            },
            body = JsonSerializer.Serialize(expectedBody),
            isBase64Encoded = false
        };

        var gameNewsService = Mock.Of<IGameNewsService>();
        Mock.Get(gameNewsService)
            .Setup(m => m.GetNewsAsync(GameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedBody);

        var logger = Mock.Of<ILogger<Functions>>();

        var sut = new Functions(Mock.Of<IDataProvider>(), gameNewsService, logger);

        // Act
        var result = await sut.GetNewsHttpApiAsync(GameId, context);
        var responseJson = await GetHttpResponseAsJsonAsync(result);

        // Asserts
        result.Should().NotBeNull()
            .And.Match<IHttpResult>(m => m.StatusCode == HttpStatusCode.OK);
        responseJson.Should().NotBeNull()
            .And.BeEquivalentTo(JsonSerializer.Serialize(expectedResult));

        Mock.Get(logger)
            .Verify(m =>
                m.BeginScope(It.Is<Dictionary<string, object>>(p =>
                    p.ContainsKey(LoggerRes.ScopeCorrelationId) &&
                    p.ContainsKey(LoggerRes.ScopeFunctionName) &&
                    p.ContainsKey(LoggerRes.ScopeFunctionVersion))), Times.Once);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public async Task GetNews_ShouldReturnBadRequest_WhenProvidedInvalidGameId(string gameId)
    {
        // Arrange
        var context = new TestLambdaContext
        {
            AwsRequestId = Guid.NewGuid().ToString(),
            FunctionName = "GamestoreServerlessGetAvailableGames",
            FunctionVersion = "${LATEST}",
            RemainingTime = TimeSpan.FromSeconds(30)
        };

        var expectedResult = new
        {
            statusCode = (int)HttpStatusCode.BadRequest,
            multiValueHeaders = new Dictionary<string, IList<string>>
            {
                { "access-control-allow-headers", new[] { "Content-Type" } },
                { "access-control-allow-origin", new[] { "*" } },
                { "access-control-allow-methods", new[] { "GET, OPTIONS" } },
                { "access-control-max-age", new[] { "60" } },
                { "content-type", new[] { "text/plain" } }
            },
            body = ErrorsRes.InvalidGameIdParam,
            isBase64Encoded = false
        };

        var gameNewsService = Mock.Of<IGameNewsService>();
        Mock.Get(gameNewsService)
            .Setup(m => m.GetNewsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ServiceException(ErrorsRes.InvalidGameIdParam, HttpStatusCode.BadRequest));
        var logger = Mock.Of<ILogger<Functions>>();

        var sut = new Functions(Mock.Of<IDataProvider>(), gameNewsService, logger);

        // Act
        var result = await sut.GetNewsHttpApiAsync(gameId, context);
        var responseJson = await GetHttpResponseAsJsonAsync(result);

        // Asserts
        result.Should().NotBeNull()
            .And.Match<IHttpResult>(m => m.StatusCode == HttpStatusCode.BadRequest);
        responseJson.Should().NotBeNull()
            .And.BeEquivalentTo(JsonSerializer.Serialize(expectedResult));
    }

    [Fact]
    public async Task GetNews_ShouldReturnNotFound_WhenNewsDoesNotExist()
    {
        // Arrange
        const string GameId = "1212";

        var context = new TestLambdaContext
        {
            AwsRequestId = Guid.NewGuid().ToString(),
            FunctionName = "GamestoreServerlessGetAvailableGames",
            FunctionVersion = "${LATEST}",
            RemainingTime = TimeSpan.FromSeconds(30)
        };

        var expectedResult = new
        {
            statusCode = (int)HttpStatusCode.NotFound,
            multiValueHeaders = new Dictionary<string, IList<string>>
            {
                { "access-control-allow-headers", new[] { "Content-Type" } },
                { "access-control-allow-origin", new[] { "*" } },
                { "access-control-allow-methods", new[] { "GET, OPTIONS" } },
                { "access-control-max-age", new[] { "60" } },
                { "content-type", new[] { "text/plain" } }
            },
            body = ErrorsRes.GameNotFound.Replace("{gameId}", GameId),
            isBase64Encoded = false
        };

        var gameNewsService = Mock.Of<IGameNewsService>();
        Mock.Get(gameNewsService)
            .Setup(m => m.GetNewsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ServiceException(ErrorsRes.GameNotFound.Replace("{gameId}", GameId), HttpStatusCode.NotFound));
        var logger = Mock.Of<ILogger<Functions>>();

        var sut = new Functions(Mock.Of<IDataProvider>(), gameNewsService, logger);

        // Act
        var result = await sut.GetNewsHttpApiAsync(GameId, context);
        var responseJson = await GetHttpResponseAsJsonAsync(result);

        // Asserts
        result.Should().NotBeNull()
            .And.Match<IHttpResult>(m => m.StatusCode == HttpStatusCode.NotFound);
        responseJson.Should().NotBeNull()
            .And.BeEquivalentTo(JsonSerializer.Serialize(expectedResult));
    }

    [Fact]
    public async Task GetNews_ShouldReturnInternalServerError_WhenExceptionOccurred()
    {
        // Arrange
        var context = new TestLambdaContext
        {
            AwsRequestId = Guid.NewGuid().ToString(),
            FunctionName = "GamestoreServerlessGetAvailableGames",
            FunctionVersion = "${LATEST}",
            RemainingTime = TimeSpan.FromSeconds(30)
        };

        var expectedErrorMessage = "Something went wrong";
        var expectedResult = new
        {
            statusCode = (int)HttpStatusCode.InternalServerError,
            multiValueHeaders = new Dictionary<string, IList<string>>
            {
                { "content-type", new[] { "text/plain" } }
            },
            body = expectedErrorMessage,
            isBase64Encoded = false
        };

        var gameNewsService = Mock.Of<IGameNewsService>();
        Mock.Get(gameNewsService)
            .Setup(m => m.GetNewsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ServiceException(expectedErrorMessage, new Exception(expectedErrorMessage),
                HttpStatusCode.InternalServerError));
        var logger = Mock.Of<ILogger<Functions>>();

        var sut = new Functions(Mock.Of<IDataProvider>(), gameNewsService, logger);

        // Act
        var result = await sut.GetNewsHttpApiAsync("3232", context);
        var responseJson = await GetHttpResponseAsJsonAsync(result);

        // Asserts
        result.Should().NotBeNull()
            .And.Match<IHttpResult>(m => m.StatusCode == HttpStatusCode.InternalServerError);
        responseJson.Should().NotBeNull()
            .And.BeEquivalentTo(JsonSerializer.Serialize(expectedResult));
    }

    private async Task<string> GetHttpResponseAsJsonAsync(IHttpResult? result)
    {
        if (result == null)
        {
            return string.Empty;
        }

        await using var esStream = result.Serialize(new HttpResultSerializationOptions
        {
            Format = HttpResultSerializationOptions.ProtocolFormat.HttpApi,
            Version = HttpResultSerializationOptions.ProtocolVersion.V1,
            Serializer = new DefaultLambdaJsonSerializer()
        });
        using var reader = new StreamReader(esStream, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }
}

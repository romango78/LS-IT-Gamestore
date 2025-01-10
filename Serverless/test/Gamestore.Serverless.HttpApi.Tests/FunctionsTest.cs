using System.Net;
using System.Text;
using System.Text.Json;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.TestUtilities;
using FluentAssertions;
using Gamestore.DataProvider.Abstractions.Models;
using Gamestore.Serverless.HttpApi.Exceptions;
using Gamestore.Serverless.HttpApi.Models;
using Gamestore.Serverless.HttpApi.Properties;
using Gamestore.Serverless.HttpApi.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Gamestore.Serverless.HttpApi.Tests;

public class FunctionsTest
{
    [Fact]
    public async Task GetGames_ShouldReturnOkResponse_WhenAvailableGamesExist()
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
            new Game
            {
                GameId = "26353",
                Name = "Game 1"
            },
            new Game
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
            body = JsonSerializer.Serialize(expectedBody),
            isBase64Encoded = false
        };

        var gamesService = Mock.Of<IGamesService>();
        Mock.Get(gamesService)
            .Setup(m => m.GetGamesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedBody);

        var logger = Mock.Of<ILogger<Functions>>();

        var sut = new Functions(gamesService, Mock.Of<IGameNewsService>(), Mock.Of<ICartService>(), logger);

        // Act
        var result = await sut.GetGamesAsync(context);
        var responseJson = await GetHttpResponseAsJsonAsync(result);

        // Asserts
        result.Should().NotBeNull()
            .And.Match<IHttpResult>(m => m.StatusCode == HttpStatusCode.OK);
        responseJson.Should().NotBeNull()
            .And.BeEquivalentTo(JsonSerializer.Serialize(expectedResult));
    }

    [Fact]
    public async Task GetGames_ShouldReturnOkResponse_WhenAvailableGamesDoesNotExist()
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

        var gamesService = Mock.Of<IGamesService>();
        Mock.Get(gamesService)
            .Setup(m => m.GetGamesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Array.Empty<Game>());

        var logger = Mock.Of<ILogger<Functions>>();

        var sut = new Functions(gamesService, Mock.Of<IGameNewsService>(), Mock.Of<ICartService>(), logger);

        // Act
        var result = await sut.GetGamesAsync(context);
        var responseJson = await GetHttpResponseAsJsonAsync(result);

        // Asserts
        result.Should().NotBeNull()
            .And.Match<IHttpResult>(m => m.StatusCode == HttpStatusCode.OK);
        responseJson.Should().NotBeNull()
            .And.BeEquivalentTo(JsonSerializer.Serialize(expectedResult));
    }

    [Fact]
    public async Task GetGames_ShouldReturnInternalServerError_WhenExceptionOccurred()
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

        var gamesService = Mock.Of<IGamesService>();
        Mock.Get(gamesService)
            .Setup(m => m.GetGamesAsync(It.IsAny<CancellationToken>()))
            .Throws(new Exception(expectedErrorMessage));
        var logger = Mock.Of<ILogger<Functions>>();

        var sut = new Functions(gamesService, Mock.Of<IGameNewsService>(), Mock.Of<ICartService>(), logger);

        // Act
        var result = await sut.GetGamesAsync(context);
        var responseJson = await GetHttpResponseAsJsonAsync(result);

        // Asserts
        result.Should().NotBeNull()
            .And.Match<IHttpResult>(m => m.StatusCode == HttpStatusCode.InternalServerError);
        responseJson.Should().NotBeNull()
            .And.BeEquivalentTo(JsonSerializer.Serialize(expectedResult));

        Mock.Get(logger)
            .Verify(
                m => m.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }

    [Fact]
    public async Task GetNews_ShouldReturnOkResponse_WhenNewsExist()
    {
        // Arrange
        const string gameId = "1212";

        var context = new TestLambdaContext
        {
            AwsRequestId = Guid.NewGuid().ToString(),
            FunctionName = "GamestoreServerlessGetAvailableGames",
            FunctionVersion = "${LATEST}",
            RemainingTime = TimeSpan.FromSeconds(30)
        };

        var expectedBody = new GameNews
        {
            GameId = gameId,
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
            .Setup(m => m.GetNewsAsync(gameId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedBody);

        var logger = Mock.Of<ILogger<Functions>>();

        var sut = new Functions(Mock.Of<IGamesService>(), gameNewsService, Mock.Of<ICartService>(), logger);

        // Act
        var result = await sut.GetNewsAsync(gameId, context);
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
#pragma warning disable xUnit1012
    [InlineData(null)]
#pragma warning restore xUnit1012
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

        var sut = new Functions(Mock.Of<IGamesService>(), gameNewsService, Mock.Of<ICartService>(), logger);

        // Act
        var result = await sut.GetNewsAsync(gameId, context);
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
        const string gameId = "1212";

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
            body = ErrorsRes.GameNotFound.Replace("{gameId}", gameId),
            isBase64Encoded = false
        };

        var gameNewsService = Mock.Of<IGameNewsService>();
        Mock.Get(gameNewsService)
            .Setup(m => m.GetNewsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ServiceException(ErrorsRes.GameNotFound.Replace("{gameId}", gameId), HttpStatusCode.NotFound));
        var logger = Mock.Of<ILogger<Functions>>();

        var sut = new Functions(Mock.Of<IGamesService>(), gameNewsService, Mock.Of<ICartService>(), logger);

        // Act
        var result = await sut.GetNewsAsync(gameId, context);
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

        var sut = new Functions(Mock.Of<IGamesService>(), gameNewsService, Mock.Of<ICartService>(), logger);

        // Act
        var result = await sut.GetNewsAsync("3232", context);
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
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Core;
using Gamestore.Domain.Exceptions;
using Gamestore.Serverless.Extensions;
using Gamestore.Serverless.HttpApi.Extensions;
using Gamestore.Serverless.HttpApi.Services;
using Gamestore.Serverless.Resources;
using Microsoft.Extensions.Logging;
using Cart = Gamestore.Serverless.HttpApi.Models.Cart;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Gamestore.Serverless.HttpApi;

/// <summary>
/// A collection of sample Lambda functions that provide a HTTP/REST api handlers. 
/// </summary>
public class Functions
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(120);
    private static readonly TimeSpan CancellingReservedTime = TimeSpan.FromSeconds(5);

    private readonly IGamesService _gamesService;
    private readonly ILogger<Functions> _logger;
    private readonly IGameNewsService _gameNewsService;
    private readonly ICartService _cartService;

    public Functions(IGamesService gamesService, IGameNewsService gameNewsService, ICartService cartService,
        ILogger<Functions> logger)
    {
        ArgumentNullException.ThrowIfNull(gamesService);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(gameNewsService);
        ArgumentNullException.ThrowIfNull(cartService);

        _gamesService = gamesService;
        _logger = logger;
        _gameNewsService = gameNewsService;
        _cartService = cartService;
    }

    [LambdaFunction(ResourceName = "GamestoreServerlessGetGames", MemorySize = 128, Timeout = 60)]
    [HttpApi(LambdaHttpMethod.Get, "/games")]
    public Task<IHttpResult> GetGamesAsync(ILambdaContext context)
    {
        return PerformActionAsync(async (cancellationToken) =>
        {
            var gameNews = await _gamesService.GetGamesAsync(cancellationToken).ConfigureAwait(false);

            return HttpResults.Ok(gameNews).AddCorsHeaders().AddJsonContentType();
        }, context);
    }

    [LambdaFunction(ResourceName = "GamestoreServerlessGetNews", MemorySize = 128, Timeout = 60)]
    [HttpApi(LambdaHttpMethod.Get, "/news/{gameId}")]
    public Task<IHttpResult> GetNewsAsync(string gameId, ILambdaContext context)
    {
        return PerformActionAsync(async (cancellationToken) =>
        {
            var gameNews = await _gameNewsService.GetNewsAsync(gameId, cancellationToken).ConfigureAwait(false);

            return HttpResults.Ok(gameNews).AddCorsHeaders().AddJsonContentType();
        }, context);
    }

    [LambdaFunction(ResourceName = "GamestoreServerlessCartSubmit", MemorySize = 128, Timeout = 60)]
    [HttpApi(LambdaHttpMethod.Post, "/cart/submit")]
    public Task<IHttpResult> CartSubmitAsync([FromBody] Cart request, ILambdaContext context)
    {
        return PerformActionAsync(async (cancellationToken) =>
        {
            await _cartService.SubmitAsync(request, cancellationToken).ConfigureAwait(false);

            return HttpResults.Accepted()
                .AddCorsHeaders();
        }, context);
    }

    private async Task<IHttpResult> PerformActionAsync(Func<CancellationToken, Task<IHttpResult>> performer, ILambdaContext context)
    {
        using var logScope = _logger.BeginScope(new Dictionary<string, object>
        {
            [LoggerRes.ScopeCorrelationId] = context.AwsRequestId,
            [LoggerRes.ScopeFunctionName] = context.FunctionName,
            [LoggerRes.ScopeFunctionVersion] = context.FunctionVersion
        });
        using var cts = CancellationTokenExtensions.CreateCancellationTokenSource(context);

        try
        {
            return await performer(cts.Token).ConfigureAwait(false);
        }
        catch (HttpBusinessException e)
        {
            return e.GetHttpResult();
        }
        catch (Exception e)
        {
            _logger.LogError(e, LoggerRes.GeneralError, e.Message);
            return HttpResults.InternalServerError(e.Message);
        }
    }
}
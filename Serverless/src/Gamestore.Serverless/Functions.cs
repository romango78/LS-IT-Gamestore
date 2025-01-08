using Amazon.Lambda.Core;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Gamestore.DataProvider.Abstractions.Services;
using Gamestore.Serverless.Extensions;
using Gamestore.Serverless.Properties;
using Gamestore.Serverless.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Gamestore.Serverless.Exceptions;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Gamestore.Serverless;

/// <summary>
/// A collection of sample Lambda functions that provide a REST api for doing simple math calculations. 
/// </summary>
public class Functions
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(120);
    private static readonly TimeSpan CancellingReservedTime = TimeSpan.FromSeconds(5);

    private readonly IDataProvider _dataProvider;
    private readonly ILogger<Functions> _logger;
    private readonly IGameNewsService _gameNewsService;

    public Functions([FromKeyedServices(nameof(CompositeDataProvider))] IDataProvider dataProvider,
        IGameNewsService gameNewsService, ILogger<Functions> logger)
    {
        ArgumentNullException.ThrowIfNull(dataProvider);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(gameNewsService);

        _dataProvider = dataProvider;
        _logger = logger;
        _gameNewsService = gameNewsService;
    }

    [LambdaFunction(ResourceName = "GamestoreServerlessGetAvailableGames", MemorySize = 128, Timeout = 60)]
    [HttpApi(LambdaHttpMethod.Get, "/availablegames")]
    public async Task<IHttpResult> GetAvailableGamesAsync(ILambdaContext context)
    {
        using var logScope = _logger.BeginScope(new Dictionary<string, object>
        {
            [LoggerRes.ScopeCorrelationId] = context.AwsRequestId,
            [LoggerRes.ScopeFunctionName] = context.FunctionName,
            [LoggerRes.ScopeFunctionVersion] = context.FunctionVersion
        });
        using var cts = CreateCancellationTokenSource(context);

        _logger.LogInformation(InfoRes.StartRequestMessage);

        try
        {
            var availableGameList = await _dataProvider.GetAvailableGameListAsync(cts.Token)
                .Where(m => !string.IsNullOrWhiteSpace(m.Name))
                .OrderByDescending(m => m.GameId)
                .Distinct()
                .Take(30).ToListAsync(cancellationToken: cts.Token).ConfigureAwait(false);

            _logger.LogInformation(InfoRes.EndRequestMessage);

            return HttpResults.Ok(availableGameList)
                .AddCorsHeaders().AddJsonContentType();
        }
        catch (Exception e)
        {
            _logger.LogError(e, ErrorsRes.GeneralError, e.Message);
            return HttpResults.InternalServerError(e.Message);
        }
    }

    [LambdaFunction(ResourceName = "GamestoreServerlessGetNews", MemorySize = 128, Timeout = 60)]
    [HttpApi(LambdaHttpMethod.Get, "/news/{gameId}")]
    public Task<IHttpResult> GetNewsHttpApiAsync(string gameId, ILambdaContext context)
    {
        return GetNewsAsync(gameId, context);
    }

    [LambdaFunction(ResourceName = "GamestoreServerlessGetNewsRestApi", MemorySize = 128, Timeout = 60)]
    [RestApi(LambdaHttpMethod.Get, "/news/{gameId}")]
    public Task<IHttpResult> GetNewsRestApiAsync(string gameId, ILambdaContext context)
    {
        return GetNewsAsync(gameId, context);
    }

    private async Task<IHttpResult> GetNewsAsync(string gameId, ILambdaContext context)
    {
        using var logScope = _logger.BeginScope(new Dictionary<string, object>
        {
            [LoggerRes.ScopeCorrelationId] = context.AwsRequestId,
            [LoggerRes.ScopeFunctionName] = context.FunctionName,
            [LoggerRes.ScopeFunctionVersion] = context.FunctionVersion
        });
        using var cts = CreateCancellationTokenSource(context);

        try
        {
            var gameNews = await _gameNewsService.GetNewsAsync(gameId, cts.Token).ConfigureAwait(false);

            return HttpResults.Ok(gameNews)
                .AddCorsHeaders().AddJsonContentType();
        }
        catch (ServiceException e)
        {
            return e.GetHttpResult();
        }
        catch (Exception e)
        {
            _logger.LogError(e, ErrorsRes.GeneralError, e.Message);
            return HttpResults.InternalServerError(e.Message);
        }
    }

    private CancellationTokenSource CreateCancellationTokenSource(ILambdaContext context)
    {
        var timeout = context.RemainingTime.Subtract(CancellingReservedTime);
        if (timeout.TotalSeconds - CancellingReservedTime.TotalSeconds <= double.Epsilon)
        {
            timeout = DefaultTimeout;
        }
        return new CancellationTokenSource(timeout);
    }
}
using Amazon.Lambda.Core;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Gamestore.DataProvider.Abstractions.Services;
using Gamestore.Serverless.Extensions;
using Gamestore.Serverless.Properties;
using Gamestore.Serverless.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Gamestore.Serverless;

/// <summary>
/// A collection of sample Lambda functions that provide a REST api for doing simple math calculations. 
/// </summary>
public class Functions
{
    private readonly IDataProvider _dataProvider;
    private readonly ILogger<Functions> _logger;

    public Functions([FromKeyedServices(nameof(CompositeDataProvider))] IDataProvider dataProvider,
        ILogger<Functions> logger)
    {
        ArgumentNullException.ThrowIfNull(dataProvider);
        ArgumentNullException.ThrowIfNull(logger);

        _dataProvider = dataProvider;
        _logger = logger;
    }

    [LambdaFunction(ResourceName = "GamestoreServerlessGetAvailableGames", MemorySize = 128, Timeout = 60)]
    [HttpApi(LambdaHttpMethod.Get, "/availablegames")]
    public async Task<IHttpResult> GetAvailableGamesAsync(ILambdaContext context)
    {
        using var logScope = _logger.BeginScope(new Dictionary<string, object>()
        {
            [LoggerRes.ScopeCorrelationId] = context.AwsRequestId,
            [LoggerRes.ScopeFunctionName] = context.FunctionName,
            [LoggerRes.ScopeFunctionVersion] = context.FunctionVersion
        });
        
        _logger.LogInformation(InfoRes.StartRequestMessage);

        try
        {
            var availableGameList = await _dataProvider.GetAvailableGameListAsync(CancellationToken.None)
                .Where(m => !string.IsNullOrWhiteSpace(m.Name))
                .OrderByDescending(m => m.GameId)
                //.Distinct()
                .Take(30).ToListAsync().ConfigureAwait(false);

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
    [HttpApi(LambdaHttpMethod.Get, "/news")]
    public async Task<IHttpResult> GetNewsAsync([FromQuery] string gameId, ILambdaContext context)
    {
        using var logScope = _logger.BeginScope(new Dictionary<string, object>()
        {
            [LoggerRes.ScopeCorrelationId] = context.AwsRequestId,
            [LoggerRes.ScopeFunctionName] = context.FunctionName,
            [LoggerRes.ScopeFunctionVersion] = context.FunctionVersion
        });

        _logger.LogInformation(InfoRes.StartRequestMessage);

        if (string.IsNullOrWhiteSpace(gameId))
        {
            _logger.LogWarning(ErrorsRes.InvalidGameIdParam);
            return HttpResults.BadRequest(ErrorsRes.InvalidGameIdParam)
                .AddCorsHeaders().AddTextContentType();
        }

        try
        {
            var gameNews = await _dataProvider.GetGameNewsAsync(gameId, CancellationToken.None).ConfigureAwait(false);

            if (gameNews == null)
            {
                _logger.LogWarning(ErrorsRes.GameNotFound, gameId);
                return HttpResults.NotFound(ErrorsRes.GameNotFound.Replace("{gameId}",gameId))
                    .AddCorsHeaders().AddTextContentType();
            }

            _logger.LogInformation(InfoRes.EndRequestMessage);

            return HttpResults.Ok(gameNews)
                .AddCorsHeaders().AddJsonContentType();
        }
        catch (Exception e)
        {
            _logger.LogError(e, ErrorsRes.GeneralError, e.Message);
            return HttpResults.InternalServerError(e.Message);
        }
    }
}
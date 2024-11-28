using Amazon.Lambda.Core;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Gamestore.DataProvider.Abstractions.Services;
using Gamestore.Serverless.Services;
using Microsoft.Extensions.DependencyInjection;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Gamestore.Serverless;

/// <summary>
/// A collection of sample Lambda functions that provide a REST api for doing simple math calculations. 
/// </summary>
public class Functions
{
    private readonly IDataProvider _dataProvider;

    public Functions([FromKeyedServices(nameof(CompositeDataProvider))]IDataProvider dataProvider)
    {
        ArgumentNullException.ThrowIfNull(dataProvider);

        _dataProvider = dataProvider;
    }

    [LambdaFunction(ResourceName = "GamestoreServerlessGetAvailableGames", MemorySize = 128, Timeout = 60)]
    [HttpApi(LambdaHttpMethod.Get, "/availablegames")]
    public async Task<IHttpResult> GetAvailableGamesAsync(ILambdaContext context)
    {
        // Log the incoming request
        context.Logger.LogInformation($"Processing request data for request {context.AwsRequestId}");

        var availableGameList = await _dataProvider.GetAvailableGameListAsync(CancellationToken.None)
            .Where(m => !string.IsNullOrWhiteSpace(m.Name))
            .OrderByDescending(m => m.GameId)
            //.Distinct()
            .Take(30).ToListAsync().ConfigureAwait(false);

        return HttpResults.Ok(availableGameList)
            .AddHeader("Access-Control-Allow-Headers", "Content-Type")
            .AddHeader("Access-Control-Allow-Origin", "*")
            .AddHeader("Access-Control-Allow-Methods", "GET, OPTIONS")
            .AddHeader("Access-Control-Max-Age", "60")
            .AddHeader("Content-Type", "application/json");
    }

    [LambdaFunction(ResourceName = "GamestoreServerlessGetNews", MemorySize = 128, Timeout = 60)]
    [HttpApi(LambdaHttpMethod.Get, "/news")]
    public async Task<IHttpResult> GetNewsAsync([FromQuery] string gameId, ILambdaContext context)
    {
        // Log the incoming request
        context.Logger.LogInformation($"Processing request data for request {context.AwsRequestId}");

        if (string.IsNullOrWhiteSpace(gameId))
        {
            return HttpResults.BadRequest("Invalid value for 'gameId'")
                .AddHeader("Access-Control-Allow-Headers", "Content-Type")
                .AddHeader("Access-Control-Allow-Origin", "*")
                .AddHeader("Access-Control-Allow-Methods", "GET, OPTIONS")
                .AddHeader("Access-Control-Max-Age", "60")
                .AddHeader("Content-Type", "application/json");
        }

        var gameNews = await _dataProvider.GetGameNewsAsync(gameId, CancellationToken.None).ConfigureAwait(false);

        if (gameNews == null)
        {
            return HttpResults.NotFound($"The game with id '{gameId}' is not found.")
                .AddHeader("Access-Control-Allow-Headers", "Content-Type")
                .AddHeader("Access-Control-Allow-Origin", "*")
                .AddHeader("Access-Control-Allow-Methods", "GET, OPTIONS")
                .AddHeader("Access-Control-Max-Age", "60")
                .AddHeader("Content-Type", "application/json");
        }

        return HttpResults.Ok(gameNews)
            .AddHeader("Access-Control-Allow-Headers", "Content-Type")
            .AddHeader("Access-Control-Allow-Origin", "*")
            .AddHeader("Access-Control-Allow-Methods", "GET, OPTIONS")
            .AddHeader("Access-Control-Max-Age", "60")
            .AddHeader("Content-Type", "application/json");
    }
}
using Amazon.Lambda.Core;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Gamestore.DataProvider.Services;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Gamestore.Serverless;

/// <summary>
/// A collection of sample Lambda functions that provide a REST api for doing simple math calculations. 
/// </summary>
public class Functions
{
    private readonly IDataProvider _dataProvider;

    public Functions(IDataProvider dataProvider)
    {
        ArgumentNullException.ThrowIfNull(dataProvider);

        _dataProvider = dataProvider;
    }

    [LambdaFunction(ResourceName = "GetAvailableGames", MemorySize = 128, Timeout = 60)]
    [HttpApi(LambdaHttpMethod.Get, "/availablegames")]
    public async Task<IHttpResult> GetAvailableGamesAsync(ILambdaContext context)
    {
        // Log the incoming request
        context.Logger.LogInformation($"Processing request data for request {context.AwsRequestId}");

        var availableGameList = await _dataProvider.GetAvailableGameListAsync(CancellationToken.None)
            .Where(m => !string.IsNullOrWhiteSpace(m.Name))
            .OrderByDescending(m => m.GameId)
            .Take(10).ToListAsync().ConfigureAwait(false);

        return HttpResults.Ok(availableGameList)
            .AddHeader("Access-Control-Allow-Headers", "Content-Type")
            .AddHeader("Access-Control-Allow-Origin", "*")
            .AddHeader("Access-Control-Allow-Methods", "GET, OPTIONS")
            .AddHeader("Access-Control-Max-Age", "60")
            .AddHeader("Content-Type", "application/json");
    }
}
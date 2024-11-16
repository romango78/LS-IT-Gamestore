using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Net;
using Gamestore.Serverless.GetAvailableGames.Models;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Gamestore.Serverless.GetAvailableGames;

public class Function
{

    /// <summary>
    /// Get list of available games
    /// </summary>
    /// <param name="request">The API Gateway request.</param>
    /// <param name="context">Lambda execution context.</param>
    /// <returns>The API Gateway response.</returns>
    public APIGatewayProxyResponse Handler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        // Log the incoming request
        context.Logger.LogLine($"Processing request data for request {context.AwsRequestId}");

        var statusCode = HttpStatusCode.OK;

        var availableGameList = new[]
        {
            new AvailableGame
            {
                GameId = "EXT-STEAM-239860",
                Name = "Agarest - Basic Pack DLC"
            },
            new AvailableGame
            {
                GameId = "EXT-STEAM-237550",
                Name = "Realms of Arkania: Blade of Destiny"
            }
        };

        // Create response
        return new APIGatewayProxyResponse
        {
            StatusCode = (int)statusCode,
            Body = JsonSerializer.Serialize(availableGameList),
            Headers = new Dictionary<string, string>
            {
                { "Access-Control-Allow-Headers", "Content-Type" },
                { "Access-Control-Allow-Origin", "*" },
                { "Access-Control-Allow-Methods", "GET, OPTIONS" },
                { "Access-Control-Max-Age", "60" },
                { "Content-Type", "text/plain" }
            }
        };
    }
}

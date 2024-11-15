using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using System.Net;
using Gamestore.Serverless.GetNews.Models;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Gamestore.Serverless.GetNews;

public class Function
{
    private const string GameIdParam = "gameId";

    /// <summary>
    /// Example function that processes an API Gateway request and returns a response
    /// </summary>
    /// <param name="request">The API Gateway request.</param>
    /// <param name="context">Lambda execution context.</param>
    /// <returns>The API Gateway response.</returns>
    public APIGatewayProxyResponse Handler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        // Log the incoming request
        context.Logger.LogLine($"Processing request data for request {context.AwsRequestId}");
        context.Logger.LogLine(
            $"Query: {string.Join(", ", request.QueryStringParameters.Select(m => $"{m.Key}={m.Value}"))}");

        string responseBody;
        var statusCode = HttpStatusCode.OK;

        // Predefined parameters with default values
        var predefinedParams = new Dictionary<string, string>
        {
            { GameIdParam, "" }
        };

        var queryParams = new[] { request.QueryStringParameters, predefinedParams }
            .SelectMany(dict => dict)
            .ToLookup(pair => pair.Key, pair => pair.Value)
            .ToDictionary(group => group.Key, group => group.First());


        // Validate query parameters
        if (string.IsNullOrWhiteSpace(queryParams[GameIdParam]))
        {
            statusCode = HttpStatusCode.BadRequest;
            responseBody = "Invalid value for 'gameId'";
        }
        else
        {
            var gameInfo = new GameInfo
            {
                GameId = queryParams[GameIdParam],
                Source = "Steam",
                Title =
                    "Valve giveth, and Valve taketh away: Team Fortress 2's BLU Scout is once again wearing the 'wrong' pants after a 17 years-in-the-making fix was reversed a day later",
                Contents =
                    "Signs of life from Team Fortress 2 are rare and precious these days, so when Valve updated the in-game model of the Scout, \u003Ca href=\"https://www.pcgamer.com/games/fps/17-years-later-valve-fixes-team-fortress-2-bug-that-made-scouts-pants-the-wrong-color-bug-so-old-it-could-have-enlisted-with-parental-consent/\" target=\"_blank\"\u003Efixing a visual bug that's existed since TF2's release\u003C/a\u003E in 2007, we were paying attention. But as reported by YouTuber \u003Ca href=\"https://www.youtube.com/watch?v=jS9R32G2-lo&ab_channel=shounic\" target=\"_blank\"\u003Eshounic\u003C/a\u003E, the visual tweak was not to last: A \u003Ca href=\"https://store.steampowered.com/news/app/440/view/4520017657931497816?l=english\" target=\"_blank\"\u003Efollow-up patch\u003C/a\u003E has reverted the BLU Scout's pants to their original, incorrect khaki from a smart, team-appropriate navy...",
                ReadMore =
                    "https://www.pcgamer.com/games/fps/valve-giveth-and-valve-taketh-away-team-fortress-2s-blu-scout-is-once-again-wearing-the-wrong-pants-after-a-17-years-in-the-making-fix-was-reversed-a-day-later?utm_source=steam&utm_medium=referral"
            };

            responseBody = JsonSerializer.Serialize(gameInfo);
        }

        // Create response
        var response = new APIGatewayProxyResponse
        {
            StatusCode = (int)statusCode,
            Body = responseBody,
            Headers = new Dictionary<string, string>
            {
                { "Access-Control-Allow-Headers", "Content-Type" },
                { "Access-Control-Allow-Origin", "*" },
                { "Access-Control-Allow-Methods", "GET, OPTIONS" },
                { "Access-Control-Max-Age", "60" },
                { "Content-Type", "text/plain" }
            }
        };

        return response;
    }
}

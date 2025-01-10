using System.Text.Json.Serialization;

namespace Gamestore.Serverless.HttpApi.Models;

public record Game
{
    [JsonPropertyName("gameId")]
    public string? GameId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
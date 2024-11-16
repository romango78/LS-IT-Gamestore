using System.Text.Json.Serialization;

namespace Gamestore.Serverless.GetAvailableGames.Models
{
    internal record AvailableGame
    {
        [JsonPropertyName("game-id")]
        public string? GameId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}

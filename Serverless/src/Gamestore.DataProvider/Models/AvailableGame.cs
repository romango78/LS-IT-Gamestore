using System.Text.Json.Serialization;

namespace Gamestore.DataProvider.Models
{
    public record AvailableGame
    {
        [JsonPropertyName("game-id")]
        public string? GameId { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}

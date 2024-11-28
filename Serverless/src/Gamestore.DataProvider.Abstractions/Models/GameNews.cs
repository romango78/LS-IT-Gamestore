using System.Text.Json.Serialization;

namespace Gamestore.DataProvider.Abstractions.Models
{
    public record GameNews
    {
        [JsonPropertyName("game-id")]
        public string? GameId { get; set; }

        [JsonPropertyName("source")]
        public string? Source { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("contents")]
        public string? Contents { get; set; }

        [JsonPropertyName("read-more")]
        public Uri? ReadMoreLink { get; set; }
    }
}

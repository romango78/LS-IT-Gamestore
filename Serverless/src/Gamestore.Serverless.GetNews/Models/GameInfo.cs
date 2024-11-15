using System.Text.Json.Serialization;

namespace Gamestore.Serverless.GetNews.Models
{
    internal record GameInfo
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
        public string? ReadMore { get; set; }
    }
}

using System.Text.Json.Serialization;
using Gamestore.DataProvider.Steam.Extensions;

namespace Gamestore.DataProvider.Steam.Models;

internal record SteamNewsItem
{
    [JsonPropertyName("appid")]
    public int Appid { get; set; }

    [JsonPropertyName("author")]
    public string? Author { get; set; }

    [JsonPropertyName("contents")]
    public string? Contents { get; set; }

    [JsonPropertyName("date")]
    [JsonConverter(typeof(UnixEpochDateTimeConverter))]
    public DateTime Date { get; set; }

    [JsonPropertyName("feed_type")]
    public int FeedType { get; set; }

    [JsonPropertyName("feedlabel")]
    public string? FeedLabel { get; set; }

    [JsonPropertyName("feedname")]
    public string? FeedName { get; set; }

    [JsonPropertyName("gid")]
    public string? Gid { get; set; }

    [JsonPropertyName("is_external_url")]
    public bool IsExternalUrl { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }
}

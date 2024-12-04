using System.Text.Json.Serialization;

namespace Gamestore.DataProvider.Steam.Models;

internal record SteamAppNews
{
    [JsonPropertyName("appid")]
    public int Appid { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("newsitems")]
    public SteamNewsItem[]? NewsItems { get; set; }
}

using System.Text.Json.Serialization;

namespace Gamestore.DataProvider.Steam.Models;

internal record SteamApp
{
    [JsonPropertyName("appid")]
    public int? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

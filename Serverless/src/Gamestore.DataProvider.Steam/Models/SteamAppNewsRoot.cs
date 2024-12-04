using System.Text.Json.Serialization;

namespace Gamestore.DataProvider.Steam.Models;

internal record SteamAppNewsRoot
{
    [JsonPropertyName("appnews")]
    public SteamAppNews? Root { get; set; }
}

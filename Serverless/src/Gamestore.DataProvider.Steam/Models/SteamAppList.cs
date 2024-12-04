using System.Text.Json.Serialization;

namespace Gamestore.DataProvider.Steam.Models;

internal record SteamAppList
{
    [JsonPropertyName("apps")] 
    public SteamApp[]? Data { get; set; }
}

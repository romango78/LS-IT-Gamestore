using System.Text.Json.Serialization;

namespace Gamestore.DataProvider.Steam.Models;

internal record SteamAppListRootObject
{
    [JsonPropertyName("applist")]
    public SteamAppList? AppList { get; set; }
}

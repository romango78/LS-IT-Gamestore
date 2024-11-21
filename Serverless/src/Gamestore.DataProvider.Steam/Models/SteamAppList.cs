using System.Text.Json.Serialization;

namespace Gamestore.DataProvider.Steam.Models
{
    internal record SteamAppListRootObject
    {
        [JsonPropertyName("applist")]
        public SteamAppList? List { get; set; }
    }

    internal record SteamAppList
    {
        [JsonPropertyName("apps")]
        public SteamApp[]? Data { get; set; }
    }
}

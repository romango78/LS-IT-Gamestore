using System.Text.Json.Serialization;

namespace Gamestore.Serverless.HttpApi.Models;

public record CartItem
{
    [JsonPropertyName("productId")]
    public string? ProductId { get; set; }

    [JsonPropertyName("quantity")]
    public int? Quantity {get; set; }
}
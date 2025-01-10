using System.Text.Json.Serialization;

namespace Gamestore.Serverless.HttpApi.Models;

public record CartItem
{
    [JsonPropertyName("product_id")]
    public string? ProductId { get; set; }

    [JsonPropertyName("quantity")]
    public int? Quantity {get; set; }
}
using System.Text.Json.Serialization;

namespace Gamestore.Serverless.HttpApi.Models;

public record Cart
{
    [JsonPropertyName("customerId")]
    public string? CustomerId { get; set; }

    [JsonPropertyName("details")]
    public IEnumerable<CartItem>? Details { get; set; } = Enumerable.Empty<CartItem>();
}
using System.Text.Json.Serialization;

namespace Gamestore.Serverless.HttpApi.Models;

public record Cart
{
    [JsonPropertyName("customer_id")]
    public string? CustomerId { get; set; }

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [JsonPropertyName("email")]
    public string? EmailAddress { get; set; }

    [JsonPropertyName("products")]
    public IEnumerable<CartItem>? Details { get; set; } = Enumerable.Empty<CartItem>();
}
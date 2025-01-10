using System.Text.Json.Serialization;
using MediatR;

namespace Gamestore.Domain.Events;

public class CartSubmitEvent : INotification
{
    #region Customer Info

    [JsonPropertyName("customer_id")]
    public string? CustomerId { get; }

    [JsonPropertyName("first_name")]
    public string? FirstName { get; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; }

    [JsonPropertyName("email")]
    public string? EmailAddress { get; }

    #endregion Customer Info

    #region Shipping Address

    // TBD

    #endregion Shipping Address

    #region Products

    [JsonPropertyName("products")]
    public IEnumerable<(string ProductId, int Quantity)> Products { get; }

    #endregion Products

    public CartSubmitEvent(string? customerId, string? firstName, string? lastName, string? emailAddress,
        IEnumerable<(string ProductId, int Quantity)> products)
    {
        CustomerId = customerId;
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        Products = products;
    }
}
using Gamestore.Domain.Events;
using Gamestore.Serverless.HttpApi.Models;

namespace Gamestore.Serverless.HttpApi.Extensions;

internal static class ModelToDomainEventExtensions
{
    public static CartSubmitEvent ToDomainEvent(this Cart cart)
    {
        return new CartSubmitEvent(cart.CustomerId, cart.FirstName, cart.LastName, cart.EmailAddress,
            cart.Details?.Select(x => new CartSubmitEvent.Product(x.ProductId ?? "", x.Quantity ?? 0)).ToHashSet() ??
            []);
    }
}
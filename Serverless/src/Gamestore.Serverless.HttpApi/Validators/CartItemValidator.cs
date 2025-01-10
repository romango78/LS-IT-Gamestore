using FluentValidation;
using Gamestore.Serverless.HttpApi.Models;
using Gamestore.Serverless.HttpApi.Properties;

namespace Gamestore.Serverless.HttpApi.Validators;

internal class CartItemValidator : AbstractValidator<CartItem>
{
    public CartItemValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage(ValidationRes.CartItem_ProductId_Empty);
        RuleFor(x => x.Quantity).NotEmpty().WithMessage(ValidationRes.CartItem_Quantity_Invalid);
        RuleFor(x => x.Quantity).InclusiveBetween(1, int.MaxValue).WithMessage(ValidationRes.CartItem_Quantity_Invalid);
    }
}
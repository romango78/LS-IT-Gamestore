using FluentValidation;
using Gamestore.Serverless.HttpApi.Models;
using Gamestore.Serverless.HttpApi.Properties;

namespace Gamestore.Serverless.HttpApi.Validators;

internal class CartValidator : AbstractValidator<Cart>
{
    public CartValidator(IValidator<CartItem> cartItemValidator)
    {
        RuleFor(x => x.CustomerId).NotEmpty().WithMessage(ValidationRes.Cart_CustomerId_Empty);
        RuleFor(x => x.Details).Must(x => x.Any()).WithMessage(ValidationRes.Cart_Details_Empty);
        RuleForEach(x => x.Details).SetValidator(cartItemValidator);
    }
}
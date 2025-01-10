using FluentValidation;

namespace Gamestore.Domain.Events.Validators;

internal class CartSubmitEventValidator : AbstractValidator<CartSubmitEvent>
{
    public CartSubmitEventValidator()
    {

    }
}
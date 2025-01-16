using Gamestore.Domain.Events;
using Gamestore.Domain.Exceptions;
using Gamestore.Domain.Properties;
using Gamestore.Domain.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Gamestore.Application.UseCases;

/// <summary>
/// The <c>ValidateOrAddCustomerWhenCartSubmittedEventHandler</c> handler performs the following use case:
/// 1. If event contains 'CustomerId' then check if customer is registered
/// 1.1 If customer is not registered then throw an error
/// 1.2 otherwise create a new event <c>TBD</c> and send it to 'TBD' queue
/// 2. Register a new customer using data in 'first name', 'last name' and 'email' fields. All those fields are required.
/// 3. Create a new event <c>TBD</c> ('CustomerId' for just registered customer is used) and send it to 'TBD' queue
/// </summary>
internal class ValidateOrAddCustomerWhenCartSubmittedEventHandler :INotificationHandler<CartSubmitEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<ValidateOrAddCustomerWhenCartSubmittedEventHandler> _logger;

    public ValidateOrAddCustomerWhenCartSubmittedEventHandler(IMediator mediator,
        ILogger<ValidateOrAddCustomerWhenCartSubmittedEventHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        ArgumentNullException.ThrowIfNull(logger);

        _mediator = mediator;
        _logger = logger;
    }

    public async Task Handle(CartSubmitEvent domainEvent, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        if (!string.IsNullOrWhiteSpace(domainEvent.CustomerId))
        {
            var request = new CustomerDetailsByIdQuery { CustomerId = domainEvent.CustomerId };
            var customer = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);
            if (customer == null)
            {
                throw new BusinessException(BusinessError.CustomerDoesNotExist(request.CustomerId));
            }

            _logger.LogDebug("The customer (ID: {CustomerId}) exists...", request.CustomerId);
        }

        throw new NotImplementedException();
    }
}
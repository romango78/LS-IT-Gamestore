using Gamestore.Domain.Entities;
using Gamestore.Domain.Queries;
using Gamestore.Domain.Services.Repositories;
using MediatR;

namespace Gamestore.Domain.Services.Queries;

internal class CustomerDetailsByIdQueryHandler : IRequestHandler<CustomerDetailsByIdQuery, CustomerEntity?>
{
    private readonly ICustomerRepository _repository;

    public CustomerDetailsByIdQueryHandler(ICustomerRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        _repository = repository;
    }

    public Task<CustomerEntity?> Handle(CustomerDetailsByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.CustomerId);

        return _repository.GetByIdAsync(request.CustomerId);
    }
}
using Gamestore.Domain.Entities;
using MediatR;

namespace Gamestore.Domain.Queries;

public class CustomerDetailsByIdQuery : IRequest<CustomerEntity?>
{
    public string? CustomerId { get; set; }
}
using Gamestore.Domain.Entities;

namespace Gamestore.Domain.Services.Repositories;

public interface ICustomerRepository : IGenericRepository<CustomerEntity, string>
{
}
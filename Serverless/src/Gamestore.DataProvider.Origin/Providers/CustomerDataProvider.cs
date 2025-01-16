using Gamestore.DataProvider.Abstractions.Providers;
using Gamestore.Domain.Entities;

namespace Gamestore.DataProvider.Origin.Providers;

public class CustomerDataProvider : ICustomerDataProvider
{
    public Task<CustomerEntity?> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }
}
using Gamestore.DataProvider.Abstractions.Providers;
using Gamestore.Domain.Entities;
using Gamestore.Domain.Services.Repositories;

namespace Gamestore.DataProvider.Abstractions.Repositories;

public class CustomerRepository : AbstractRepository<ICustomerRepository>, ICustomerRepository
{
    public CustomerRepository(IEnumerable<IDataProvider<ICustomerRepository>> dataProviders) 
        : base(dataProviders)
    {
    }

    public async Task<CustomerEntity?> GetByIdAsync(string id)
    {
        foreach (var dataProvider in RegisteredProviders.Values)
        {
            var result = await ((ICustomerDataProvider)dataProvider).GetByIdAsync(id).ConfigureAwait(false);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}
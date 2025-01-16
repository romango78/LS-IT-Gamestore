using Gamestore.Domain.Entities;
using Gamestore.Domain.Services.Repositories;

namespace Gamestore.DataProvider.Abstractions.Providers;

public interface IDataProvider<TRepository>
    where TRepository: IRepository
{
}

public interface IGenericDataProvider<TEntity, TKey> : IDataProvider<IGenericRepository<TEntity, TKey>>
    where TEntity : class
{
    Task<TEntity?> GetByIdAsync(TKey id);
}

public interface ICustomerDataProvider : IDataProvider<ICustomerRepository>,
    IGenericDataProvider<CustomerEntity, string>
{
}

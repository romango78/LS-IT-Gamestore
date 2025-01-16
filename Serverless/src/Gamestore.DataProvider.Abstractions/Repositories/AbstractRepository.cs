using System.Collections.Concurrent;
using Gamestore.DataProvider.Abstractions.Providers;
using Gamestore.Domain.Services.Repositories;

namespace Gamestore.DataProvider.Abstractions.Repositories;

public class AbstractRepository<TRepository>
    where TRepository: IRepository
{
    protected IDictionary<Type, IDataProvider<TRepository>> RegisteredProviders { get; } =
        new ConcurrentDictionary<Type, IDataProvider<TRepository>>();

    public AbstractRepository(IEnumerable<IDataProvider<TRepository>> dataProviders)
    {
        ArgumentNullException.ThrowIfNull(dataProviders);

        foreach (var dataProvider in dataProviders)
        {
            var providerType = dataProvider.GetType();
            RegisteredProviders.TryAdd(providerType, dataProvider);
        }
    }
}
using System.Collections.Concurrent;
using Gamestore.DataProvider.Abstractions.Models;
using Gamestore.DataProvider.Abstractions.Properties;

namespace Gamestore.DataProvider.Abstractions.Services;

/// <summary>
/// TODO: Will be refactored
/// </summary>
public abstract class AbstractCompositeDataProvider : IDataProvider
{
    public void RegisterDataProvider(IDataProvider dataProvider)
    {
        ArgumentNullException.ThrowIfNull(dataProvider);

        var providerType = dataProvider.GetType();
        if (!RegisteredProviders.TryAdd(providerType, dataProvider))
        {
            throw new InvalidOperationException(
                Resources.CannotRegisterDataProviderSecondTime.Replace(":Name", providerType.FullName));
        }
    }

    protected IDictionary<Type, IDataProvider> RegisteredProviders { get; } =
        new ConcurrentDictionary<Type, IDataProvider>();

    public abstract IAsyncEnumerable<AvailableGame> GetAvailableGameListAsync(CancellationToken cancellationToken);

    public abstract Task<GameNews?> GetGameNewsAsync(string gameId, CancellationToken cancellationToken);
}
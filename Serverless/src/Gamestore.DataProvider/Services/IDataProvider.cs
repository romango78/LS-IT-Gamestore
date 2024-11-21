using Gamestore.DataProvider.Models;

namespace Gamestore.DataProvider.Services
{
    public interface IDataProvider
    {
        IAsyncEnumerable<AvailableGame> GetAvailableGameListAsync(CancellationToken cancellationToken);
    }
}

using Gamestore.DataProvider.Abstractions.Models;

namespace Gamestore.DataProvider.Abstractions.Services
{
    public interface IDataProvider
    {
        IAsyncEnumerable<AvailableGame> GetAvailableGameListAsync(CancellationToken cancellationToken);

        Task<GameNews?> GetGameNewsAsync(string gameId, CancellationToken cancellationToken);
    }
}

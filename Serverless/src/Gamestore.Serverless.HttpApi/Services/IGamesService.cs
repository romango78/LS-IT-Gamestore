using Gamestore.Serverless.HttpApi.Models;

namespace Gamestore.Serverless.HttpApi.Services;

public interface IGamesService
{
    Task<IEnumerable<Game>> GetGamesAsync(CancellationToken cancellationToken);
}
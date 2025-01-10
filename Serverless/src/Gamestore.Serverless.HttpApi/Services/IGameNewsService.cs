using Gamestore.DataProvider.Abstractions.Models;

namespace Gamestore.Serverless.HttpApi.Services;

public interface IGameNewsService
{
    Task<GameNews> GetNewsAsync(string gameId, CancellationToken cancellationToken);
}
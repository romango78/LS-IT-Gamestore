using System.Net;
using Gamestore.DataProvider.Abstractions.Services;
using Gamestore.Domain.Exceptions;
using Gamestore.Serverless.HttpApi.Models;
using Gamestore.Serverless.Resources;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gamestore.Serverless.HttpApi.Services;

internal class GamesService : IGamesService
{
    private readonly IDataProvider _dataProvider;
    private readonly ILogger<Functions> _logger;

    public GamesService([FromKeyedServices(nameof(CompositeDataProvider))] IDataProvider dataProvider,
        ILogger<Functions> logger)
    {
        ArgumentNullException.ThrowIfNull(dataProvider);
        ArgumentNullException.ThrowIfNull(logger);

        _dataProvider = dataProvider;
        _logger = logger;
    }

    public async Task<IEnumerable<Game>> GetGamesAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation(InfoRes.StartRequestMessage);

        try
        {
            var availableGameList = await _dataProvider.GetAvailableGameListAsync(cancellationToken)
                .Where(m => !string.IsNullOrWhiteSpace(m.Name))
                .OrderByDescending(m => m.GameId)
                .Distinct()
                .Take(30).ToListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

            _logger.LogInformation(InfoRes.EndRequestMessage);

            return availableGameList.Select(x => new Game { GameId = x.GameId, Name = x.Name });
        }
        catch (Exception e)
        {
            _logger.LogError(e, LoggerRes.GeneralError, e.Message);
            throw new HttpBusinessException(e.Message, e, HttpStatusCode.InternalServerError);
        }
    }
}
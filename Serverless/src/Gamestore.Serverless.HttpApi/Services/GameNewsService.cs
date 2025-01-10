﻿using System.Net;
using AWS.Lambda.Powertools.Logging;
using Gamestore.DataProvider.Abstractions.Models;
using Gamestore.DataProvider.Abstractions.Services;
using Gamestore.Serverless.HttpApi.Exceptions;
using Gamestore.Serverless.HttpApi.Properties;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gamestore.Serverless.HttpApi.Services;

internal class GameNewsService : IGameNewsService
{
    private readonly IDataProvider _dataProvider;
    private readonly ILogger<Functions> _logger;

    public GameNewsService([FromKeyedServices(nameof(CompositeDataProvider))] IDataProvider dataProvider,
        ILogger<Functions> logger)
    {
        ArgumentNullException.ThrowIfNull(dataProvider);
        ArgumentNullException.ThrowIfNull(logger);

        _dataProvider = dataProvider;
        _logger = logger;
    }

    public async Task<GameNews> GetNewsAsync(string gameId, CancellationToken cancellationToken)
    {
        _logger.LogInformation(InfoRes.StartRequestMessage);

        if (string.IsNullOrWhiteSpace(gameId))
        {
            _logger.LogWarning(ErrorsRes.InvalidGameIdParam);
            throw new ServiceException(ErrorsRes.InvalidGameIdParam, HttpStatusCode.BadRequest);
        }

        try
        {
            var gameNews = await _dataProvider.GetGameNewsAsync(gameId, cancellationToken).ConfigureAwait(false);

            if (gameNews == null)
            {
                _logger.LogWarning(ErrorsRes.GameNotFound, gameId);
                throw new ServiceException(ErrorsRes.GameNotFound.Replace("{gameId}", gameId), HttpStatusCode.NotFound);
            }

            _logger.LogInformation(InfoRes.EndRequestMessage);

            return gameNews;
        }
        catch (Exception e)
        {
            _logger.LogError(e, ErrorsRes.GeneralError, e.Message);
            throw new ServiceException(e.Message, e, HttpStatusCode.InternalServerError);
        }
    }
}
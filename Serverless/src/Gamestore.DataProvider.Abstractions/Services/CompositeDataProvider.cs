﻿using System.Runtime.CompilerServices;
using Gamestore.DataProvider.Abstractions.Models;

namespace Gamestore.DataProvider.Abstractions.Services
{
    /// <summary>
    /// TODO: Will be refactored
    /// </summary>
    public class CompositeDataProvider : AbstractCompositeDataProvider
    {
        private static readonly Random GetRandom = new ();

        public override async IAsyncEnumerable<AvailableGame> GetAvailableGameListAsync(
            [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            if (RegisteredProviders.Count == 0)
            {
                yield break;
            }

            await foreach (var game in GetRandomDataProvider().GetAvailableGameListAsync(cancellationToken)
                               .ConfigureAwait(false))
            {
                yield return game;
            }
        }

        public override async Task<GameNews?> GetGameNewsAsync(string gameId, CancellationToken cancellationToken)
        {
            foreach (var dataProvider in RegisteredProviders.Values)
            {
                var result = await dataProvider.GetGameNewsAsync(gameId, cancellationToken).ConfigureAwait(false);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private IDataProvider GetRandomDataProvider()
        {
            var index = GetRandom.Next(0, RegisteredProviders.Count - 1);
            return RegisteredProviders.ElementAt(index).Value;
        }
    }
}

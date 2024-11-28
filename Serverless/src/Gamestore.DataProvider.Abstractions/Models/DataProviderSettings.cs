namespace Gamestore.DataProvider.Abstractions.Models
{
    public record DataProviderSettings
    {
        public string AvailableGamesUrl { get; set; } = string.Empty;

        public string? HttpClientName { get; set; }

        public int HandlerLifetimeInSeconds { get; set; }

        public int TimeoutInSeconds { get; set; }

        public int PooledConnectionLifetimeInMinutes { get; set; } = 5;

        public int PooledConnectionIdleTimeoutInMinutes { get; set; } = 2;

        public int MaxConnectionsPerServer { get; set; } = 10;
    }
}

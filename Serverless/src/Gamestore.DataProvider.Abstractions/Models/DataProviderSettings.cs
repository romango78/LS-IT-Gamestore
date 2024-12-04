namespace Gamestore.DataProvider.Abstractions.Models
{
    public record DataProviderSettings
    {
        public UrlSettings AvailableGamesUrl { get; set; } = new ();

        public UrlSettings GameNewsUrl { get; set; } = new ();

        public string? HttpClientName { get; set; }

        public int HandlerLifetimeInSeconds { get; set; }

        public int MaxConnectionsPerServer { get; set; } = 10;

        public int PooledConnectionLifetimeInMinutes { get; set; } = 5;

        public int PooledConnectionIdleTimeoutInMinutes { get; set; } = 2;

        public int TimeoutInSeconds { get; set; }
    }
}

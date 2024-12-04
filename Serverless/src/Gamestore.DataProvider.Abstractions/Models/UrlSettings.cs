namespace Gamestore.DataProvider.Abstractions.Models;

public record UrlSettings
{
    public string BaseUrl { get; set; } = string.Empty;

    public IEnumerable<QueryUrlSettings> QueryParameters { get; set; } = Enumerable.Empty<QueryUrlSettings>();
}

public record QueryUrlSettings
{
    public string? Mapping { get; set; } = null;

    public string Name { get; set; } = String.Empty;

    public string Value {get; set; } = String.Empty;
}

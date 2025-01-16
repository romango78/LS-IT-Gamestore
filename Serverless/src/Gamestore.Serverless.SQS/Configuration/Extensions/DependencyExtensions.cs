using Microsoft.Extensions.DependencyInjection;
using Gamestore.Application.Configuration.Extensions;
using Gamestore.DataProvider.Origin.Configuration.Extensions;
using Gamestore.Infrastructure.Abstractions.Configuration.Extensions;

namespace Gamestore.Serverless.SQS.Configuration.Extensions;

internal static class DependencyExtensions
{
    public static IServiceCollection AddLocalDependencies(this IServiceCollection services)
    {
        services.AddApplication();
        services.AddMiddleware();

        services.AddOriginDataProviders();

        return services;
    }
}
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Gamestore.Infrastructure.Abstractions.Middleware;

namespace Gamestore.Infrastructure.Abstractions.Configuration.Extensions;

public static class DependencyExtensions
{
    public static IServiceCollection AddMiddleware(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(LoggingMiddleware<,>), ServiceLifetime.Scoped);
        });
        return services;
    }
}
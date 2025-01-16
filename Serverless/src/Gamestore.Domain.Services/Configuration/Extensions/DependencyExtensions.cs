using Gamestore.Domain.Configurations.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Gamestore.Domain.Services.Configuration.Extensions;

public static class DependencyExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddDomain();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}
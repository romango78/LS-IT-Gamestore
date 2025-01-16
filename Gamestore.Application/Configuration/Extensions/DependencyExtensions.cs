using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Gamestore.Domain.Services.Configuration.Extensions;

namespace Gamestore.Application.Configuration.Extensions;

public static class DependencyExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddDomainServices();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}
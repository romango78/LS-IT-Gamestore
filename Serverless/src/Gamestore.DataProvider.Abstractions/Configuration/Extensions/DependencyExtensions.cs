using Gamestore.DataProvider.Abstractions.Repositories;
using Gamestore.Domain.Services.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Gamestore.DataProvider.Abstractions.Configuration.Extensions;

public static class DependencyExtensions
{
    public static IServiceCollection AddDataProviders(this IServiceCollection services)
    {
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}
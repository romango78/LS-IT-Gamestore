using Gamestore.DataProvider.Abstractions.Configuration.Extensions;
using Gamestore.DataProvider.Abstractions.Providers;
using Gamestore.DataProvider.Origin.Providers;
using Gamestore.Domain.Services.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Gamestore.DataProvider.Origin.Configuration.Extensions;

public static class DependencyExtensions
{
    public static IServiceCollection AddOriginDataProviders(this IServiceCollection services)
    {
        services.AddScoped<ICustomerDataProvider, CustomerDataProvider>();

        services.AddScoped<IDataProvider<ICustomerRepository>, CustomerDataProvider>();
        services.AddDataProviders();

        return services;
    }
}
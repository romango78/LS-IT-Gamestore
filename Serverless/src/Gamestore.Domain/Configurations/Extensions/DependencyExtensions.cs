using FluentValidation;
using Gamestore.Domain.Events;
using Gamestore.Domain.Events.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Gamestore.Domain.Configurations.Extensions;

public static class DependencyExtensions
{
    public static IServiceCollection AddDomainDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IValidator<CartSubmitEvent>, CartSubmitEventValidator>();

        return services;
    }
}
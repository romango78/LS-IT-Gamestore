using FluentValidation;
using Gamestore.Domain.Events;
using Gamestore.Domain.Events.Validators;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Gamestore.Domain.Configurations.Extensions;

public static class DependencyExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddSingleton<IValidator<CartSubmitEvent>, CartSubmitEventValidator>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}
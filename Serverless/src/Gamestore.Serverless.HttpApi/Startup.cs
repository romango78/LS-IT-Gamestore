using FluentValidation;
using Gamestore.DataProvider.Abstractions.Models;
using Gamestore.DataProvider.Abstractions.Services;
using Gamestore.DataProvider.Steam.Extensions;
using Gamestore.DataProvider.Steam.Services;
using Gamestore.Domain.Configurations.Extensions;
using Gamestore.Serverless.HttpApi.Configurations.Extensions;
using Gamestore.Serverless.HttpApi.Models;
using Gamestore.Serverless.HttpApi.Services;
using Gamestore.Serverless.HttpApi.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gamestore.Serverless.HttpApi;

[Amazon.Lambda.Annotations.LambdaStartup]
public class Startup
{
    /// <summary>
    /// Services for Lambda functions can be registered in the services dependency injection container in this method. 
    ///
    /// The services can be injected into the Lambda function through the containing type's constructor or as a
    /// parameter in the Lambda function using the FromService attribute. Services injected for the constructor have
    /// the lifetime of the Lambda compute container. Services injected as parameters are created within the scope
    /// of the function invocation.
    /// </summary>
    public void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        services.Configure<DataProviderSettings>(configuration.GetSection(nameof(DataProviderSettings)));

        services.AddDomain();
        services.AddSteamDependencies(configuration);

        services.AddSingleton<IValidator<Cart>, CartValidator>();
        services.AddSingleton<IValidator<CartItem>, CartItemValidator>();

        services.AddScoped<IDataProvider, SteamDataProvider>();
        services.AddScoped<IGamesService, GamesService>();
        services.AddScoped<IGameNewsService, GameNewsService>();
        services.AddScoped<ICartService, CartService>();
        services.AddKeyedScoped<IDataProvider>(nameof(CompositeDataProvider), (serviceProvider, _) =>
        {
            var compositeDataProvider = new CompositeDataProvider();

            foreach (var dataProvider in serviceProvider.GetServices<IDataProvider>())
            {
                compositeDataProvider.RegisterDataProvider(dataProvider);
            }
            return compositeDataProvider;
        });

        services.AddLogging(builder =>
        {
            builder.AddConfiguration(configuration.GetSection("Logging"))
                .AddJsonConsole();
        });

        services.UseAmazonSqs(configuration);
    }
}

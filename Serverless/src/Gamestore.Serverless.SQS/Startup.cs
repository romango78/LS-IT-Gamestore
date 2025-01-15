using Gamestore.Domain.Configurations.Extensions;
using Gamestore.Serverless.SQS.Configuration.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gamestore.Serverless.SQS;

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

        services.AddOptions();
        //services.Configure<DataProviderSettings>(configuration.GetSection(nameof(DataProviderSettings)));

        services.AddDomainDependencies();
        services.AddApplicationDependencies(configuration);

        //services.AddSteamDependencies(configuration);

        //services.AddSingleton<IValidator<Cart>, CartValidator>();
        //services.AddSingleton<IValidator<CartItem>, CartItemValidator>();

        //services.AddScoped<IDataProvider, SteamDataProvider>();
        //services.AddScoped<IGamesService, GamesService>();
        //services.AddScoped<IGameNewsService, GameNewsService>();
        //services.AddScoped<ICartService, CartService>();
        //services.AddKeyedScoped<IDataProvider>(nameof(CompositeDataProvider), (serviceProvider, _) =>
        //{
        //    var compositeDataProvider = new CompositeDataProvider();

        //    foreach (var dataProvider in serviceProvider.GetServices<IDataProvider>())
        //    {
        //        compositeDataProvider.RegisterDataProvider(dataProvider);
        //    }
        //    return compositeDataProvider;
        //});

        services.AddLogging(builder =>
        {
            builder.AddConfiguration(configuration.GetSection("Logging"))
                .AddJsonConsole();
        });

        //services.UseAmazonSqs(configuration);
    }
}
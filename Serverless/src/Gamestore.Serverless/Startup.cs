using Gamestore.DataProvider.Models;
using Gamestore.DataProvider.Services;
using Gamestore.DataProvider.Steam.Extensions;
using Gamestore.DataProvider.Steam.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gamestore.Serverless;

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
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        services.AddOptions();
        services.Configure<DataProviderSettings>(configuration.GetSection(nameof(DataProviderSettings)));
        services.AddSteamDependencies(configuration);

        services.AddScoped<IDataProvider, SteamDataProvider>();
    }
}

using Amazon.Runtime;
using Amazon.SQS;
using Gamestore.SQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gamestore.Serverless.HttpApi.Configurations.Extensions;

internal static class AmazonSqsDependenciesExtensions
{
    public static IServiceCollection UseAmazonSqs(this IServiceCollection services, IConfiguration configuration)
    {
        var amazonQueueSettings = GetAmazonQueueSettings(configuration);
        services.Configure<AmazonQueueSettings>(options => amazonQueueSettings.CopyTo(options));

        services.AddScoped<IAmazonSQS>(_ => new AmazonSQSClient(GetCredentials(amazonQueueSettings)));
        return services;
    }

    private static AmazonQueueSettings GetAmazonQueueSettings(IConfiguration configuration)
    {
        var settings = new AmazonQueueSettings
        {
            QueueName = "",
            AccessKey = "",
            Secret = ""
        };
        configuration.GetSection(nameof(AmazonQueueSettings)).Bind(settings);
        settings.AccessKey = configuration[settings.AccessKey] ?? "";
        settings.Secret = configuration[settings.Secret] ?? "";

        return settings;
    }

    private static AWSCredentials GetCredentials(AmazonQueueSettings settings)
    {
        return new BasicAWSCredentials(settings.AccessKey, settings.Secret);
    }
}
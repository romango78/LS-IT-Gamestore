using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.SQS;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Gamestore.Domain.Exceptions;
using Gamestore.Serverless.Extensions;
using Gamestore.Serverless.Resources;
using Gamestore.Serverless.SQS.Properties;
using Gamestore.SQS.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Gamestore.Serverless.SQS;

/// <summary>
/// A collection of sample Lambda functions that provide a SQS handlers. 
/// </summary>
public class Functions
{
    private readonly IMediator _mediator;
    private readonly ILogger<Functions> _logger;

    /// <summary>
    /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
    /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
    /// region the Lambda function is executed in.
    /// </summary>
    public Functions(IMediator mediator, ILogger<Functions> logger)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        ArgumentNullException.ThrowIfNull(logger);

        _mediator = mediator;
        _logger = logger;
    }

    [LambdaFunction(ResourceName = "GamestoreServerlessDomainEventHandler", MemorySize = 128, Timeout = 60)]
    [SQSEvent("@GamestoreQueue", BatchSize = 10, MaximumConcurrency = 1000)]
    public Task<SQSBatchResponse> DomainEventHandlerAsync(SQSEvent sqsEvent, ILambdaContext context)
    {
        return PerformActionAsync(async (message, cancellationToken) =>
        {
            _logger.LogInformation(InfoRes.StartRequestMessage);
            _logger.LogInformation(InfoRes.QueueItemProcessingInfoMessage, message.MessageId, message.GetGroupId(),
                message.GetDeduplicationId());

            var domainEvent = message.ToDomainEvent();
            if (domainEvent == null)
            {
                throw new BusinessException(ErrorsRes.DomainEventParsingFailed);
            }

            await _mediator.Publish(domainEvent, cancellationToken).ConfigureAwait(false); ;

            _logger.LogInformation(InfoRes.EndRequestMessage);
        }, sqsEvent, context);
    }

    private async Task<SQSBatchResponse> PerformActionAsync(
        Func<SQSEvent.SQSMessage, CancellationToken, Task> performer, SQSEvent sqsEvent, ILambdaContext context)
    {
        using var logScope = _logger.BeginScope(new Dictionary<string, object>
        {
            [LoggerRes.ScopeCorrelationId] = context.AwsRequestId,
            [LoggerRes.ScopeFunctionName] = context.FunctionName,
            [LoggerRes.ScopeFunctionVersion] = context.FunctionVersion
        });
        using var cts = CancellationTokenExtensions.CreateCancellationTokenSource(context);

        var batchItemFailures = new List<SQSBatchResponse.BatchItemFailure>();
        foreach (var message in sqsEvent.Records)
        {
            try
            {
                await performer(message, cts.Token).ConfigureAwait(false);
            }
            catch (BusinessException e)
            {
                _logger.LogWarning(e.Message);
                //Add failed message identifier to the batchItemFailures list
                batchItemFailures.Add(new SQSBatchResponse.BatchItemFailure { ItemIdentifier = message!.MessageId });
            }
            catch (Exception e)
            {
                _logger.LogError(e, LoggerRes.GeneralError, e.Message);
                //Add failed message identifier to the batchItemFailures list
                batchItemFailures.Add(new SQSBatchResponse.BatchItemFailure { ItemIdentifier = message!.MessageId });
            }
        }

        return new SQSBatchResponse(batchItemFailures);
    }
}
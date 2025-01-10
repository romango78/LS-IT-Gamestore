using System.Net;
using Amazon.SQS;
using Amazon.SQS.Model;
using FluentValidation;
using Gamestore.Domain.Extensions;
using Gamestore.Serverless.HttpApi.Exceptions;
using Gamestore.Serverless.HttpApi.Extensions;
using Gamestore.Serverless.HttpApi.Models;
using Gamestore.Serverless.HttpApi.Properties;
using Gamestore.SQS;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Gamestore.Serverless.HttpApi.Services;

internal class CartService : ICartService
{
    private readonly IAmazonSQS _queueClient;
    private readonly IValidator<Cart> _validator;
    private readonly ILogger<Functions> _logger;
    private readonly AmazonQueueSettings _queueSettings;

    public CartService(IAmazonSQS queueClient, IValidator<Cart> validator, ILogger<Functions> logger,
        IOptions<AmazonQueueSettings> options)
    {
        ArgumentNullException.ThrowIfNull(queueClient);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(validator);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(options.Value);

        _queueClient = queueClient;
        _validator = validator;
        _logger = logger;
        _queueSettings = options.Value;
    }

    public async Task SubmitAsync(Cart cart, CancellationToken cancellationToken)
    {
        _logger.LogInformation(InfoRes.StartRequestMessage);

        var result = await _validator.ValidateAsync(cart, cancellationToken);

        if (!result.IsValid)
        {
            _logger.LogWarning(result.ToString("~"));
            throw new ServiceException(result.ToString(), HttpStatusCode.BadRequest);
        }

        try
        {
            var queueUrlResponse = await _queueClient.GetQueueUrlAsync(_queueSettings.QueueName, cancellationToken);

            if (queueUrlResponse.HttpStatusCode == HttpStatusCode.OK)
            {
                var domainEvent = cart.ToDomainEvent();
                var sendMessageRequest = new SendMessageRequest
                {
                    MessageDeduplicationId = Guid.NewGuid().ToString(),
                    MessageGroupId = MessageGroups.SubmittedCart,
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>(
                    [
                        MessageAttributes.SubmittedAt,
                        MessageAttributes.Source(domainEvent.GetType())
                    ]),
                    MessageBody = domainEvent.ToJson(),
                    QueueUrl = queueUrlResponse.QueueUrl
                };

                var sendMessageResponse = await _queueClient.SendMessageAsync(sendMessageRequest, cancellationToken);
                if (sendMessageResponse.HttpStatusCode != HttpStatusCode.OK)
                {
                    throw new ServiceException(
                        ErrorsRes.SendMessageRequestFailed
                            .Replace("{requestId}", sendMessageResponse.ResponseMetadata.RequestId)
                            .Replace("{messageId}", sendMessageResponse.MessageId)
                            .Replace("{statusCode}", sendMessageResponse.HttpStatusCode.ToString()),
                        HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                throw new ServiceException(
                    ErrorsRes.QueueUrlRequestFailed
                        .Replace("{requestId}", queueUrlResponse.ResponseMetadata.RequestId)
                        .Replace("{statusCode}", queueUrlResponse.HttpStatusCode.ToString()),
                    HttpStatusCode.InternalServerError);
            }
            
        }
        catch (Exception e)
        {
            _logger.LogError(e, ErrorsRes.GeneralError, e.Message);
            throw new ServiceException(e.Message, e, HttpStatusCode.InternalServerError);
        }

        _logger.LogInformation(InfoRes.EndRequestMessage);
    }
}
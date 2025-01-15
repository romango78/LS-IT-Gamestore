using System.Text.Json;
using Amazon.Lambda.SQSEvents;
using Gamestore.Domain.Extensions;
using MediatR;

namespace Gamestore.SQS.Extensions;

public static class SQSMessageExtensions
{
    public static INotification? ToDomainEvent(this SQSEvent.SQSMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (!string.IsNullOrWhiteSpace(message.Body) &&
            message.MessageAttributes.TryGetValue(MessageAttributes.SourceTypeAttrKey, out var messageAttribute))
        {
            var typeName = messageAttribute.StringValue;
            var domainEventType = typeName.GetDomainEventType();

            if (domainEventType != null)
            {
                var domainEvent = JsonSerializer.Deserialize(message.Body, domainEventType);
                return domainEvent as INotification;
            }
        }

        return null;
    }

    public static string GetDeduplicationId(this SQSEvent.SQSMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (message.Attributes.TryGetValue(MessageAttributes.MessageDeduplicationIdAttrKey, out var dedupId))
        {
            return dedupId;
        }

        return string.Empty;
    }

    public static string GetGroupId(this SQSEvent.SQSMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);

        if (message.Attributes.TryGetValue(MessageAttributes.MessageGroupIdAttrKey, out var groupId))
        {
            return groupId;
        }

        return string.Empty;
    }
}
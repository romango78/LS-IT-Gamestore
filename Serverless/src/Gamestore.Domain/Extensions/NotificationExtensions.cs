using System.Text.Json;
using MediatR;

namespace Gamestore.Domain.Extensions;

public static class NotificationExtensions
{
    public static string ToJson(this INotification notification)
    {
        var json = JsonSerializer.Serialize(notification, notification.GetType(), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = false
        });
        return json;
    }

    public static Type? GetDomainEventType(this string typeName)
    {
        return Type.GetType(typeName, throwOnError: false, ignoreCase: true);
    }
}
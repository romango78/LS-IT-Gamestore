using System.Text.Json;
using MediatR;

namespace Gamestore.Domain.Extensions;

public static class NotificationExtensions
{
    public static string ToJson(this INotification notification)
    {
        return JsonSerializer.Serialize(notification);
    }
}
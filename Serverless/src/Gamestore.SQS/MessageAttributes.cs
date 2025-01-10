using Amazon.SQS.Model;

namespace Gamestore.SQS;

public static class MessageAttributes
{
    public static readonly KeyValuePair<string, MessageAttributeValue> SubmittedAt =
        new("SubmittedAt",
            new MessageAttributeValue
                { DataType = "String", StringValue = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss") });

    public static KeyValuePair<string, MessageAttributeValue> Source(Type type)
    {
        return new("SourceType",
            new MessageAttributeValue { DataType = "String", StringValue = type.FullName });
    }
}
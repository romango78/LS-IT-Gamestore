using Amazon.SQS.Model;

namespace Gamestore.SQS;

public static class MessageAttributes
{
    public static readonly string MessageGroupIdAttrKey = "MessageGroupId";
    public static readonly string MessageDeduplicationIdAttrKey = "MessageDeduplicationId";

    public static readonly string SubmittedAtAttrKey = "SubmittedAt";
    public static readonly string SourceTypeAttrKey = "SourceType";
    

    public static readonly KeyValuePair<string, MessageAttributeValue> SubmittedAt =
        new(SubmittedAtAttrKey,
            new MessageAttributeValue
                { DataType = "String", StringValue = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss") });

    public static KeyValuePair<string, MessageAttributeValue> Source(Type type)
    {
        return new(SourceTypeAttrKey,
            new MessageAttributeValue { DataType = "String", StringValue = type.FullName });
    }
}
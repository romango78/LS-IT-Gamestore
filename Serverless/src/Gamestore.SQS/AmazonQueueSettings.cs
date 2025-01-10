namespace Gamestore.SQS;

public record AmazonQueueSettings
{
    public required string QueueName { get; set; } = "";

    public required string AccessKey { get; set; } = "";

    public required string Secret { get; set; } = "";

    public void CopyTo(AmazonQueueSettings instance)
    {
        instance.QueueName = this.QueueName;
        instance.AccessKey = this.AccessKey;
        instance.Secret = this.Secret;
    }
}
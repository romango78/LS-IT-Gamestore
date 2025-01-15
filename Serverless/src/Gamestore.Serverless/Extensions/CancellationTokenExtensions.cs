using Amazon.Lambda.Core;

namespace Gamestore.Serverless.Extensions;

public static class CancellationTokenExtensions
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(120);
    private static readonly TimeSpan CancellingReservedTime = TimeSpan.FromSeconds(5);

    public static CancellationTokenSource CreateCancellationTokenSource(ILambdaContext context)
    {   
        var timeout = context.RemainingTime.Subtract(CancellingReservedTime);
        if (timeout.TotalSeconds - CancellingReservedTime.TotalSeconds <= double.Epsilon)
        {
            timeout = DefaultTimeout;
        }
        return new CancellationTokenSource(timeout);
    }
}
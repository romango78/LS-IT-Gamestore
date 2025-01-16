using MediatR;
using Microsoft.Extensions.Logging;

namespace Gamestore.Infrastructure.Abstractions.Middleware;

public class LoggingMiddleware<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{

    private readonly ILogger<LoggingMiddleware<TRequest, TResponse>> _logger;

    public LoggingMiddleware(ILogger<LoggingMiddleware<TRequest, TResponse>> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);

        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var correlationId = Guid.NewGuid().ToString();
        _logger.LogInformation("[START] {RequestName} (CorrelationId: {CorrelationId}).", request.GetType().Name,
            correlationId);

        TResponse response;
        try
        {
            response = await next();
        }
        finally
        {
            _logger.LogInformation("[END] {RequestName} (CorrelationId: {CorrelationId}).", request.GetType().Name,
                correlationId);
        }

        return response;
    }
}
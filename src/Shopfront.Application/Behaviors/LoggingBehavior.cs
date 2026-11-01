using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Shopfront.Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var startedAt = Stopwatch.GetTimestamp();

        try
        {
            return await next();
        }
        finally
        {
            logger.LogInformation(
                "Handled {RequestName} in {ElapsedMilliseconds} ms",
                requestName,
                Stopwatch.GetElapsedTime(startedAt).TotalMilliseconds);
        }
    }
}

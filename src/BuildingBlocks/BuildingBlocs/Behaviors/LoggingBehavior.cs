using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse> 
    (ILogger<LoggingBehavior<TRequest, TResponse>> _logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("[START] Handling {RequestName} with content: {@Request}; Response {ResponseName}", typeof(TRequest).Name, request, typeof(TResponse).Name);

        var timer = Stopwatch.StartNew();
        timer.Start();

        TResponse? response = default;

        try
        {
            response = await next(cancellationToken);
        }
        finally
        {
            timer.Stop();
            var elapsedTime = timer.Elapsed;

            if (elapsedTime > TimeSpan.FromSeconds(3))
            {
                _logger.LogWarning("[PERFORMANCE] Handling {RequestName} took {ElapsedTime} ms", typeof(TRequest).Name, elapsedTime.TotalMilliseconds);
            }
            else
            {
                _logger.LogInformation("[END] Handling {RequestName} completed in {ElapsedTime} ms with response: {@Response}", typeof(TRequest).Name, elapsedTime.TotalMilliseconds, response);
            }
        }

        return response;
    }
}

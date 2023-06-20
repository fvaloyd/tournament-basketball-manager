using Domain.Common;
using MediatR;

namespace Application.Common.Behaviors;
public class LogginBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public readonly ILoggerManager _logger;

    public LogginBehavior(ILoggerManager logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInfo($"Handling::{typeof(TRequest).Name}");
        var response = await next();
        _logger.LogInfo($"Handled::{typeof(TResponse).Name}");
        return response;
    }
}

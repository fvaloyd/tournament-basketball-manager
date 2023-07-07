using Domain.Common;
using MediatR;

namespace Application.Common.Behaviors;
public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
	private readonly ILoggerManager _logger;

    public UnhandledExceptionBehavior(ILoggerManager logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
		try
		{
			return await next();
		}
		catch (Exception)
		{
			_logger.LogError($"Request: Unhandled Exception for Request {typeof(TRequest).Name}");
            throw;
		}
    }
}

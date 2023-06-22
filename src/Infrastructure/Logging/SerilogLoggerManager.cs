using Serilog;
using Domain.Common;

namespace Infrastructure;

public class SerilogLoggerManager : ILoggerManager
{
    private readonly ILogger _logger;

    public SerilogLoggerManager(ILogger logger) => _logger = logger;

    public void LogDebug(string message) => _logger.Debug(message);

    public void LogError(string message) => _logger.Error(message);

    public void LogInfo(string message) => _logger.Information(message);

    public void LogWarn(string message) => _logger.Warning(message);
}

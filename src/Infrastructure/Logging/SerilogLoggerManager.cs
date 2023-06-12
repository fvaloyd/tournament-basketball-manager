using Serilog;
using Domain.Common;

namespace Infrastructure;

public class SerilogLoggerManager : ILoggerManager
{
    public void LogDebug(string message) => Log.Debug(message);

    public void LogError(string message) => Log.Error(message);

    public void LogInfo(string message) => Log.Information(message);

    public void LogWarn(string message) => Log.Warning(message);
}

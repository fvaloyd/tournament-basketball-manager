namespace Domain.Common;
public interface ILoggerManager
{
    void LogInfo(string message);
    void LogWarn(string message);
    void LogError(string message);
    void LogDebug(string message);
}
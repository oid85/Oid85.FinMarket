namespace Oid85.FinMarket.Logging.Services;

/// <summary>
/// Сервис логирования
/// </summary>
public interface ILogService
{
    public Task LogTrace(string message);
    public Task LogInfo(string message);
    public Task LogError(string message);
    public Task LogException(Exception exception);
}
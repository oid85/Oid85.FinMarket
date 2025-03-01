namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис задач по расписанию
/// </summary>
public interface IJobService
{
    /// <summary>
    /// В начале дня
    /// </summary>
    /// <returns></returns>
    Task EarlyInTheDay();
    
    /// <summary>
    /// Каждые 15 минут
    /// </summary>
    /// <returns></returns>
    Task Every15Minutes();
}
namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис задач по расписанию
/// </summary>
public interface IJobService
{
    /// <summary>
    /// Загрузка дневных свечей
    /// </summary>
    /// <returns></returns>
    Task LoadDailyCandles();
    
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
    
    /// <summary>
    /// Каждые 10 минут
    /// </summary>
    /// <returns></returns>
    Task Every10Minutes();
    
    /// <summary>
    /// Каждые 5 минут
    /// </summary>
    /// <returns></returns>
    Task Every5Minutes();
}
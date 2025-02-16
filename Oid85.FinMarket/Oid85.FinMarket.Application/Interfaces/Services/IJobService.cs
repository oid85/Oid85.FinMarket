namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис задач по расписанию
/// </summary>
public interface IJobService
{
    /// <summary>
    /// Загрузка данных
    /// </summary>
    Task<bool> LoadAsync();
    
    /// <summary>
    /// Расчеты
    /// </summary>
    Task<bool> CalculateAsync();
}
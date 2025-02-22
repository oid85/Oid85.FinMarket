namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис задач по расписанию
/// </summary>
public interface IJobService
{
    /// <summary>
    /// Загрузка данных и расчет
    /// </summary>
    Task<bool> LoadAndCalculate();
}
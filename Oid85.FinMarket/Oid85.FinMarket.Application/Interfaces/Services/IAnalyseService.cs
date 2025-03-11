namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис анализа
/// </summary>
public interface IAnalyseService
{
    /// <summary>
    /// Анализ акций
    /// </summary>
    Task<bool> DailyAnalyseSharesAsync();
    
    /// <summary>
    /// Анализ облигаций
    /// </summary>
    Task<bool> DailyAnalyseBondsAsync();
    
    /// <summary>
    /// Анализ валют
    /// </summary>
    Task<bool> DailyAnalyseCurrenciesAsync();
    
    /// <summary>
    /// Анализ фьючерсов
    /// </summary>
    Task<bool> DailyAnalyseFuturesAsync();
    
    /// <summary>
    /// Анализ индексов
    /// </summary>
    Task<bool> DailyAnalyseIndexesAsync();
}
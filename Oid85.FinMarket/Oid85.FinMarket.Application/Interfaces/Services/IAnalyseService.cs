using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис анализа
/// </summary>
public interface IAnalyseService
{
    /// <summary>
    /// Анализ акций
    /// </summary>
    Task<bool> AnalyseSharesAsync();

    /// <summary>
    /// Анализ облигаций
    /// </summary>
    Task<bool> AnalyseBondsAsync();
    
    /// <summary>
    /// Анализ валют
    /// </summary>
    Task<bool> AnalyseCurrenciesAsync();
    
    /// <summary>
    /// Анализ фьючерсов
    /// </summary>
    Task<bool> AnalyseFuturesAsync();
    
    /// <summary>
    /// Анализ индексов
    /// </summary>
    Task<bool> AnalyseIndexesAsync();
}
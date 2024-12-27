using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис анализа
/// </summary>
public interface IAnalyseService
{
    /// <summary>
    /// Анализ всех акций
    /// </summary>
    Task<bool> AnalyseStocksAsync();

    /// <summary>
    /// Анализ индексов
    /// </summary>
    Task<bool> AnalyseIndexesAsync();
    
    /// <summary>
    /// Анализ с индикатором Супертренд
    /// </summary>
    Task<List<AnalyseResult>> SupertrendAnalyseAsync(Guid instrumentId);

    /// <summary>
    /// Анализ последовательности подряд идущих свечей
    /// </summary>
    Task<List<AnalyseResult>> CandleSequenceAnalyseAsync(Guid instrumentId);

    /// <summary>
    /// Анализ растущего объема
    /// </summary>
    Task<List<AnalyseResult>> CandleVolumeAnalyseAsync(Guid instrumentId);

    /// <summary>
    /// Анализ RSI
    /// </summary>
    Task<List<AnalyseResult>> RsiAnalyseAsync(Guid instrumentId);
    
    /// <summary>
    /// Анализ доходности LTM
    /// </summary>
    Task<List<AnalyseResult>> YieldLtmAnalyseAsync(Guid instrumentId);
}
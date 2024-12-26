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
    public Task<bool> AnalyseStocksAsync();

    /// <summary>
    /// Анализ с индикатором Супертренд
    /// </summary>
    public Task<List<AnalyseResult>> SupertrendAnalyseAsync(Guid instrumentId);

    /// <summary>
    /// Анализ последовательности подряд идущих свечей
    /// </summary>
    public Task<List<AnalyseResult>> CandleSequenceAnalyseAsync(Guid instrumentId);

    /// <summary>
    /// Анализ растущего объема
    /// </summary>
    public Task<List<AnalyseResult>> CandleVolumeAnalyseAsync(Guid instrumentId);

    /// <summary>
    /// Анализ RSI
    /// </summary>
    public Task<List<AnalyseResult>> RsiAnalyseAsync(Guid instrumentId);
}
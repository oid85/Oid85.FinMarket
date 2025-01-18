namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис событий рынка
/// </summary>
public interface IMarketEventService
{
    /// <summary>
    /// Расчет рыночного события Супертренд
    /// </summary>
    Task CheckSupertrendMarketEventAsync();

    /// <summary>
    /// Расчет рыночного события Растущий объем
    /// </summary>
    Task CheckCandleVolumeMarketEventAsync();

    /// <summary>
    /// Расчет рыночного события Свечи одного цвета
    /// </summary>
    Task CheckCandleSequenceMarketEventAsync();
}
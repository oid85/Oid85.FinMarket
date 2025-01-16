namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис событий рынка
/// </summary>
public interface IMarketEventService
{
    /// <summary>
    /// Расчет рыночного события Супертренд
    /// </summary>
    Task CheckSupertrendMarketEventAsync(List<Guid> instrumentIds);
}
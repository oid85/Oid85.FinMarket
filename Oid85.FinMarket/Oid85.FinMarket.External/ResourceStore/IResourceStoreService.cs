namespace Oid85.FinMarket.External.ResourceStore;

/// <summary>
/// Сервис ресурсов
/// </summary>
public interface IResourceStoreService
{
    /// <summary>
    /// Получить тикеры акций из списка наблюдения
    /// </summary>
    Task<List<string>> GetSharesWatchlistAsync();
    
    /// <summary>
    /// Получить тикеры облигаций из списка наблюдения
    /// </summary>
    Task<List<string>> GetBondsWatchlistAsync();
    
    /// <summary>
    /// Получить тикеры фьючерсов из списка наблюдения
    /// </summary>
    Task<List<string>> GetFuturesWatchlistAsync();
    
    /// <summary>
    /// Получить тикеры валют из списка наблюдения
    /// </summary>
    Task<List<string>> GetCurrenciesWatchlistAsync();
    
    /// <summary>
    /// Получить тикеры индексов из списка наблюдения
    /// </summary>
    Task<List<string>> GetIndexesWatchlistAsync();
}
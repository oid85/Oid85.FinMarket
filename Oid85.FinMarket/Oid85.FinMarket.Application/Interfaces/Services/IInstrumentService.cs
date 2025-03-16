using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис инструментов
/// </summary>
public interface IInstrumentService
{
    /// <summary>
    /// Получить Id инструментов из списка наблюдения
    /// </summary>
    Task<List<Guid>> GetInstrumentIdsInWatchlist();
    
    /// <summary>
    /// Получить акции из списка наблюдения
    /// </summary>
    Task<List<Share>> GetSharesInWatchlist();

    /// <summary>
    /// Получить акции из индекса Московской биржи
    /// </summary>
    Task<List<Share>> GetSharesInIndexMoex();
    
    /// <summary>
    /// Получить облигации из списка наблюдения
    /// </summary>
    Task<List<Bond>> GetBondsInWatchlist();
    
    /// <summary>
    /// Получить облигации, подходящие под фильтр
    /// </summary>
    Task<List<Bond>> GetBondsByFilter();
    
    /// <summary>
    /// Получить фьючерсы из списка наблюдения
    /// </summary>
    Task<List<Future>> GetFuturesInWatchlist();
    
    /// <summary>
    /// Получить валюты из списка наблюдения
    /// </summary>
    Task<List<Currency>> GetCurrenciesInWatchlist();
    
    /// <summary>
    /// Получить индексы из списка наблюдения
    /// </summary>
    Task<List<FinIndex>> GetFinIndexesInWatchlist();
}
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис работы со списками тикеров
/// </summary>
public interface ITickerListUtilService
{
    /// <summary>
    /// Получить акции из указанного тикерлиста
    /// </summary>
    /// <param name="tickerListName">Наименование тикерлиста</param>
    Task<List<Share>> GetSharesByTickerListAsync(string tickerListName);
    
    /// <summary>
    /// Получить облигации из указанного тикерлиста
    /// </summary>
    /// <param name="tickerListName">Наименование тикерлиста</param>
    Task<List<Bond>> GetBondsByTickerListAsync(string tickerListName);
    
    /// <summary>
    /// Получить фьючерсы из указанного тикерлиста
    /// </summary>
    /// <param name="tickerListName">Наименование тикерлиста</param>
    Task<List<Future>> GetFuturesByTickerListAsync(string tickerListName);
    
    /// <summary>
    /// Получить валюты из указанного тикерлиста
    /// </summary>
    /// <param name="tickerListName">Наименование тикерлиста</param>
    Task<List<Currency>> GetCurrenciesByTickerListAsync(string tickerListName);
    
    /// <summary>
    /// Получить индексы из указанного тикерлиста
    /// </summary>
    /// <param name="tickerListName">Наименование тикерлиста</param>
    Task<List<FinIndex>> GetFinIndexesByTickerListAsync(string tickerListName);
    
    /// <summary>
    /// Получить Id инструментов из списка наблюдения
    /// </summary>
    Task<List<Guid>> GetInstrumentIdsInWatchlist();
    
    /// <summary>
    /// Получить облигации, подходящие под фильтр
    /// </summary>
    Task<List<Bond>> GetBondsByFilter();
}
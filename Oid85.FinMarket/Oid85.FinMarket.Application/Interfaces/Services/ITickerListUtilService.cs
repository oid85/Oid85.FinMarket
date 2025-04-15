using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис работы со списками тикеров
/// </summary>
public interface ITickerListUtilService
{
    Task<List<Share>> GetSharesByTickerListAsync(string tickerList);
    Task<List<Bond>> GetBondsByTickerListAsync(string tickerList);
    Task<List<Future>> GeFuturesByTickerListAsync(string tickerList);
    Task<List<Currency>> GetCurrenciesByTickerListAsync(string tickerList);
    Task<List<FinIndex>> GetFinIndexesByTickerListAsync(string tickerList);
    
    /// <summary>
    /// Получить Id инструментов из списка наблюдения
    /// </summary>
    Task<List<Guid>> GetInstrumentIdsInWatchlist();
    
    /// <summary>
    /// Получить облигации, подходящие под фильтр
    /// </summary>
    Task<List<Bond>> GetBondsByFilter();
}
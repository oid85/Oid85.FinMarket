using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.ResourceStore;
using Oid85.FinMarket.External.ResourceStore.Models;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class TickerListUtilService(
    IResourceStoreService resourceStoreService,
    IShareRepository shareRepository,
    IBondRepository bondRepository,
    IFutureRepository futureRepository,
    ICurrencyRepository currencyRepository,
    IIndexRepository indexRepository
    ) : ITickerListUtilService
{
    /// <inheritdoc />
    public async Task<List<Share>> GetSharesByTickerListAsync(string tickerListName) => 
        await shareRepository.GetAsync(
            (await resourceStoreService.GetTickerListAsync(tickerListName)).Tickers);

    /// <inheritdoc />
    public async Task<List<Bond>> GetBondsByTickerListAsync(string tickerListName) => 
        await bondRepository.GetAsync(
            (await resourceStoreService.GetTickerListAsync(tickerListName)).Tickers);

    /// <inheritdoc />
    public async Task<List<Future>> GetFuturesByTickerListAsync(string tickerListName)
    {
        var tickersFromResource = (await resourceStoreService.GetTickerListAsync(tickerListName)).Tickers;
        var tickersFromDatabase = (await futureRepository.GetAllAsync())
            .Where(x => x.ExpirationDate >= DateOnly.FromDateTime(DateTime.Today))
            .Select(x => x.Ticker).ToList();

        var tickers = new List<string>();

        foreach (var tickerFromDatabase in tickersFromDatabase)
            foreach (var tickerFromResource in tickersFromResource)
                if (tickerFromResource.Length == 2) // Если тикер фьючерса задан как маска
                {
                    if (tickerFromDatabase[0] == tickerFromResource[0] &&
                        tickerFromDatabase[1] == tickerFromResource[1])
                        if (!tickers.Contains(tickerFromDatabase))
                            tickers.Add(tickerFromDatabase);
                }
                
                else
                {
                    if (tickerFromDatabase == tickerFromResource)
                        if (!tickers.Contains(tickerFromDatabase))
                            tickers.Add(tickerFromDatabase);
                }
        
        return await futureRepository.GetAsync(tickers);
    }

    /// <inheritdoc />
    public async Task<List<Currency>> GetCurrenciesByTickerListAsync(string tickerListName) => 
        await currencyRepository.GetAsync(
            (await resourceStoreService.GetTickerListAsync(tickerListName)).Tickers);

    /// <inheritdoc />
    public async Task<List<FinIndex>> GetFinIndexesByTickerListAsync(string tickerListName) => 
        await indexRepository.GetAsync(
            (await resourceStoreService.GetTickerListAsync(tickerListName)).Tickers);

    /// <inheritdoc />
    public async Task<List<Guid>> GetInstrumentIdsInWatchlist()
    {
        var shares = await GetSharesByTickerListAsync(KnownTickerLists.SharesWatchlist);
        var bonds = await GetBondsByTickerListAsync(KnownTickerLists.BondsWatchlist);
        var futures = await GetFuturesByTickerListAsync(KnownTickerLists.FuturesWatchlist);
        var currencies = await GetCurrenciesByTickerListAsync(KnownTickerLists.CurrenciesWatchlist);
        var indexes = await GetFinIndexesByTickerListAsync(KnownTickerLists.IndexesWatchlist);
     
        var instrumentIds = new List<Guid>();
        
        instrumentIds.AddRange(shares.Select(x => x.InstrumentId));
        instrumentIds.AddRange(bonds.Select(x => x.InstrumentId));
        instrumentIds.AddRange(futures.Select(x => x.InstrumentId));
        instrumentIds.AddRange(currencies.Select(x => x.InstrumentId));
        instrumentIds.AddRange(indexes.Select(x => x.InstrumentId));
        
        return instrumentIds;
    }

    /// <inheritdoc />
    public async Task<List<Bond>> GetBondsByFilter()
    {
        var filter = await resourceStoreService.GetFilterBondsResourceAsync();

        var from = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddYears(filter!.YearsToMaturity.Min));
        var to = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddYears(filter.YearsToMaturity.Max));
        
        var items = (await bondRepository.GetAllAsync())
            .Where(x => 
                filter.Currencies.Contains("all") || 
                filter.Currencies.Contains(x.Currency))
            .Where(x => 
                filter.Sectors.Contains("all") || 
                filter.Sectors.Contains(x.Sector))
            .Where(x => 
                x.MaturityDate >= from && 
                x.MaturityDate <= to)
            .Where(x => 
                (x.RiskLevel == 0 && filter.RiskLevels.Low) || 
                (x.RiskLevel == 1 && filter.RiskLevels.Middle) || 
                (x.RiskLevel == 2 && filter.RiskLevels.High) || 
                (x.RiskLevel == 3 && filter.RiskLevels.VeryHigh))
            .Where(x => 
                x.LastPrice >= filter.Price.Min && 
                x.LastPrice <= filter.Price.Max)
            .ToList();
        
        return items;
    }
}
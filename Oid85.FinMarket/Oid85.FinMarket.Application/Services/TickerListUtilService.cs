using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.ResourceStore;

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
    public async Task<List<Share>> GetSharesByTickerListAsync(string tickerListName)
    {
        var tickerListResource = await resourceStoreService.GetTickerListAsync(tickerListName);
        var shares = await shareRepository.GetAsync(tickerListResource.Tickers);
        return shares;
    }

    /// <inheritdoc />
    public async Task<List<Bond>> GetBondsByTickerListAsync(string tickerListName)
    {
        var tickerListResource = await resourceStoreService.GetTickerListAsync(tickerListName);
        var bonds = await bondRepository.GetAsync(tickerListResource.Tickers);
        return bonds;
    }

    /// <inheritdoc />
    public async Task<List<Future>> GetFuturesByTickerListAsync(string tickerListName)
    {
        var tickerListResource = await resourceStoreService.GetTickerListAsync(tickerListName);
        var futures = await futureRepository.GetAsync(tickerListResource.Tickers);
        return futures;
    }

    /// <inheritdoc />
    public async Task<List<Currency>> GetCurrenciesByTickerListAsync(string tickerListName)
    {
        var tickerListResource = await resourceStoreService.GetTickerListAsync(tickerListName);
        var currencies = await currencyRepository.GetAsync(tickerListResource.Tickers);
        return currencies;
    }

    /// <inheritdoc />
    public async Task<List<FinIndex>> GetFinIndexesByTickerListAsync(string tickerListName)
    {
        var tickerListResource = await resourceStoreService.GetTickerListAsync(tickerListName);
        var finIndexes = await indexRepository.GetAsync(tickerListResource.Tickers);
        return finIndexes;
    }

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
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class InstrumentService(
    IResourceStoreService resourceStoreService,
    IShareRepository shareRepository,
    IBondRepository bondRepository,
    IFutureRepository futureRepository,
    ICurrencyRepository currencyRepository,
    IIndexRepository indexRepository
    ) : IInstrumentService
{
    /// <inheritdoc />
    public async Task<List<Guid>> GetInstrumentIdsInWatchlist()
    {
        var instrumentIds = new List<Guid>();
        
        instrumentIds.AddRange((await GetSharesInWatchlist()).Select(x => x.InstrumentId));
        instrumentIds.AddRange((await GetBondsInWatchlist()).Select(x => x.InstrumentId));
        instrumentIds.AddRange((await GetFuturesInWatchlist()).Select(x => x.InstrumentId));
        instrumentIds.AddRange((await GetCurrenciesInWatchlist()).Select(x => x.InstrumentId));
        instrumentIds.AddRange((await GetFinIndexesInWatchlist()).Select(x => x.InstrumentId));
        
        return instrumentIds;
    }

    /// <inheritdoc />
    public async Task<List<Share>> GetSharesInWatchlist()
    {
        var tickers = await resourceStoreService.GetSharesWatchlistAsync();
        var items = await shareRepository.GetByTickersAsync(tickers);
        return items;
    }

    /// <inheritdoc />
    public async Task<List<Share>> GetSharesInIndexMoex()
    {
        var tickers = await resourceStoreService.GetIndexMoexTickersAsync();
        var items = await shareRepository.GetByTickersAsync(tickers);
        return items;
    }
    
    /// <inheritdoc />
    public async Task<List<Bond>> GetBondsInWatchlist()
    {
        var tickers = await resourceStoreService.GetBondsWatchlistAsync();
        var items = await bondRepository.GetByTickersAsync(tickers);
        return items;
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

    /// <inheritdoc />
    public async Task<List<Future>> GetFuturesInWatchlist()
    {
        var tickers = await resourceStoreService.GetFuturesWatchlistAsync();
        var items = await futureRepository.GetByTickersAsync(tickers);
        return items;
    }

    /// <inheritdoc />
    public async Task<List<Currency>> GetCurrenciesInWatchlist()
    {
        var tickers = await resourceStoreService.GetCurrenciesWatchlistAsync();
        var items = await currencyRepository.GetByTickersAsync(tickers);
        return items;
    }

    /// <inheritdoc />
    public async Task<List<FinIndex>> GetFinIndexesInWatchlist()
    {
        var tickers = await resourceStoreService.GetIndexesWatchlistAsync();
        var items = await indexRepository.GetByTickersAsync(tickers);
        return items;
    }
}
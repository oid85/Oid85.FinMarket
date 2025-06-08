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
    public async Task<List<Share>> GetAllSharesInTickerListsAsync()
    {
        List<string> tickers = 
        [
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesExporters)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesImoex)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesMaterials)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesPortfolio)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesRosseti)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesWatchlist)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesEchelonsEchelon1)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesEchelonsEchelon2)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesEchelonsEchelon3)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesSectorsBanks)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesSectorsEnerg)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesSectorsFinance)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesSectorsHousingAndUtilities)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesSectorsIronAndSteelIndustry)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesSectorsIt)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesSectorsMining)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesSectorsNonFerrousMetallurgy)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesSectorsOilAndGas)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesSectorsrRetail)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.SharesSectorsTelecom)).Tickers
        ];

        return await shareRepository.GetAsync(tickers.Distinct().ToList());
    }

    /// <inheritdoc />
    public async Task<List<Bond>> GetAllBondsInTickerListsAsync()
    {
        List<string> tickers = 
            [
                ..(await GetBondsByFilter()).Select(x => x.Ticker).ToList(), 
                ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.BondsCorp)).Tickers,
                ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.BondsCouponEveryMonth)).Tickers,
                ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.BondsMunicipals)).Tickers,
                ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.BondsOfz)).Tickers,
                ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.BondsLongOfz)).Tickers,
                ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.BondsPortfolio)).Tickers,
                ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.BondsReplacement)).Tickers,
                ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.BondsWatchlist)).Tickers
            ];

        return await bondRepository.GetAsync(tickers.Distinct().ToList());
    }

    /// <inheritdoc />
    public async Task<List<Future>> GetAllFuturesInTickerListsAsync()
    {
        List<string> tickers = 
        [
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.FuturesCny)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.FuturesEur)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.FuturesGld)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.FuturesMoex)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.FuturesNg)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.FuturesPortfolio)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.FuturesRi)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.FuturesUsd)).Tickers,
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.FuturesWatchlist)).Tickers
        ];

        return await futureRepository.GetAsync(tickers.Distinct().ToList());
    }

    /// <inheritdoc />
    public async Task<List<Currency>> GetAllCurrenciesInTickerListsAsync()
    {
        List<string> tickers = 
        [
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.CurrenciesWatchlist)).Tickers
        ];

        return await currencyRepository.GetAsync(tickers.Distinct().ToList());
    }
    
    /// <inheritdoc />
    public async Task<List<FinIndex>> GetAllIndexesInTickerListsAsync()
    {
        List<string> tickers = 
        [
            ..(await resourceStoreService.GetTickerListAsync(KnownTickerLists.IndexesWatchlist)).Tickers
        ];

        return await indexRepository.GetAsync(tickers.Distinct().ToList());
    }

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
                (x.RiskLevel == 0 && filter.RiskLevelsResource.Low) || 
                (x.RiskLevel == 1 && filter.RiskLevelsResource.Middle) || 
                (x.RiskLevel == 2 && filter.RiskLevelsResource.High) || 
                (x.RiskLevel == 3 && filter.RiskLevelsResource.VeryHigh))
            .Where(x => 
                x.LastPrice >= filter.Price.Min && 
                x.LastPrice <= filter.Price.Max)
            .ToList();
        
        return items;
    }
}
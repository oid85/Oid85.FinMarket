using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
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
    public async Task<List<Share>> GetSharesByTickerListAsync(string tickerList)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Bond>> GetBondsByTickerListAsync(string tickerList)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Future>> GeFuturesByTickerListAsync(string tickerList)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Currency>> GetCurrenciesByTickerListAsync(string tickerList)
    {
        throw new NotImplementedException();
    }

    public async Task<List<FinIndex>> GetFinIndexesByTickerListAsync(string tickerList)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task<List<Guid>> GetInstrumentIdsInWatchlist()
    {
        var instrumentIds = new List<Guid>();
        
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
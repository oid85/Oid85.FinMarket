using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class MarketEventService(
    IMarketEventRepository marketEventRepository,
    IAnalyseResultRepository analyseResultRepository,
    IInstrumentRepository instrumentRepository,
    IShareRepository shareRepository,
    IFutureRepository futureRepository,
    IBondRepository bondRepository,
    ICurrencyRepository currencyRepository,
    IIndexRepository indexRepository) 
    : IMarketEventService
{
    /// <inheritdoc />
    public async Task CheckSupertrendUpMarketEventAsync()
    {
        var instrumentIds = await GetInstrumentIds();
        
        foreach (var instrumentId in instrumentIds)
        {
            var analyseResults = await analyseResultRepository
                .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.Supertrend);
            
            var marketEvent = await CreateMarketEvent(
                instrumentId, KnownMarketEventTypes.SupertrendUp);

            string previous = analyseResults[0].ResultString;
            string current = analyseResults[1].ResultString;
            
            marketEvent.IsActive = previous == KnownTrendDirections.Down && current == KnownTrendDirections.Up;
            marketEvent.IsActive |= previous == string.Empty && current == KnownTrendDirections.Up;
            
            await SaveMarketEventAsync(marketEvent);
        }
    }
    
    /// <inheritdoc />
    public async Task CheckSupertrendDownMarketEventAsync()
    {
        var instrumentIds = await GetInstrumentIds();
        
        foreach (var instrumentId in instrumentIds)
        {
            var analyseResults = await analyseResultRepository
                .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.Supertrend);

            var marketEvent = await CreateMarketEvent(
                instrumentId, KnownMarketEventTypes.SupertrendDown);

            string previous = analyseResults[0].ResultString;
            string current = analyseResults[1].ResultString;
            
            marketEvent.IsActive = previous == KnownTrendDirections.Up && current == KnownTrendDirections.Down;
            marketEvent.IsActive |= previous == string.Empty && current == KnownTrendDirections.Down;
            
            await SaveMarketEventAsync(marketEvent);
        }
    }

    public Task CheckCandleVolumeUpMarketEventAsync()
    {
        throw new NotImplementedException();
    }

    public Task CheckCandleSequenceWhiteMarketEventAsync()
    {
        throw new NotImplementedException();
    }

    public Task CheckCandleSequenceBlackMarketEventAsync()
    {
        throw new NotImplementedException();
    }

    public Task CheckRsiOverBoughtInputMarketEventAsync()
    {
        throw new NotImplementedException();
    }

    public Task CheckRsiOverBoughtOutputMarketEventAsync()
    {
        throw new NotImplementedException();
    }

    public Task CheckRsiOverOverSoldInputMarketEventAsync()
    {
        throw new NotImplementedException();
    }

    public Task CheckRsiOverOverSoldOutputMarketEventAsync()
    {
        throw new NotImplementedException();
    }

    private async Task<List<Guid>> GetInstrumentIds()
    {
        var instrumentIds = new List<Guid>();
        
        instrumentIds.AddRange((await shareRepository.GetWatchListAsync()).Select(s => s.InstrumentId));
        instrumentIds.AddRange((await futureRepository.GetWatchListAsync()).Select(s => s.InstrumentId));
        instrumentIds.AddRange((await bondRepository.GetWatchListAsync()).Select(s => s.InstrumentId));
        instrumentIds.AddRange((await currencyRepository.GetWatchListAsync()).Select(s => s.InstrumentId));
        instrumentIds.AddRange((await indexRepository.GetWatchListAsync()).Select(s => s.InstrumentId));
        
        return instrumentIds;
    }

    private async Task<MarketEvent> CreateMarketEvent(
        Guid instrumentId,
        string analyseType)
    {
        var instrument = await instrumentRepository.GetByInstrumentIdAsync(instrumentId);
        
        return new MarketEvent
        {
            InstrumentId = instrumentId,
            Ticker = instrument?.Ticker ?? string.Empty,
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            Time = TimeOnly.FromDateTime(DateTime.UtcNow),
            MarketEventType = analyseType
        };
    }
    
    private async Task SaveMarketEventAsync(MarketEvent marketEvent)
    {
        if (!marketEvent.IsActive)    
            await marketEventRepository.DeactivateAsync(marketEvent);
            
        if (marketEvent.IsActive)    
            await marketEventRepository.ActivateAsync(marketEvent);
    }
}
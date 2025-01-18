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
            
            marketEvent.IsActive = previous == KnownTrendDirections.Down && 
                                   current == KnownTrendDirections.Up;
            
            marketEvent.IsActive |= previous == string.Empty && 
                                    current == KnownTrendDirections.Up;
            
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
            
            marketEvent.IsActive = previous == KnownTrendDirections.Up && 
                                   current == KnownTrendDirections.Down;
            
            marketEvent.IsActive |= previous == string.Empty && 
                                    current == KnownTrendDirections.Down;
            
            await SaveMarketEventAsync(marketEvent);
        }
    }
    
    /// <inheritdoc />
    public async Task CheckCandleVolumeUpMarketEventAsync()
    {
        var instrumentIds = await GetInstrumentIds();
        
        foreach (var instrumentId in instrumentIds)
        {
            var analyseResults = await analyseResultRepository
                .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.CandleVolume);

            var marketEvent = await CreateMarketEvent(
                instrumentId, KnownMarketEventTypes.CandleVolumeUp);

            string previous = analyseResults[0].ResultString;
            string current = analyseResults[1].ResultString;
            
            marketEvent.IsActive = previous == string.Empty && 
                                   current == KnownVolumeDirections.Up;
            
            await SaveMarketEventAsync(marketEvent);
        }
    }

    /// <inheritdoc />
    public async Task CheckCandleSequenceWhiteMarketEventAsync()
    {
        var instrumentIds = await GetInstrumentIds();
        
        foreach (var instrumentId in instrumentIds)
        {
            var analyseResults = await analyseResultRepository
                .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.CandleSequence);

            var marketEvent = await CreateMarketEvent(
                instrumentId, KnownMarketEventTypes.CandleSequenceWhite);

            string previous = analyseResults[0].ResultString;
            string current = analyseResults[1].ResultString;
            
            marketEvent.IsActive = previous == string.Empty && 
                                   current == KnownCandleSequences.White;
            
            await SaveMarketEventAsync(marketEvent);
        }
    }

    /// <inheritdoc />
    public async Task CheckCandleSequenceBlackMarketEventAsync()
    {
        var instrumentIds = await GetInstrumentIds();
        
        foreach (var instrumentId in instrumentIds)
        {
            var analyseResults = await analyseResultRepository
                .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.CandleSequence);

            var marketEvent = await CreateMarketEvent(
                instrumentId, KnownMarketEventTypes.CandleSequenceBlack);

            string previous = analyseResults[0].ResultString;
            string current = analyseResults[1].ResultString;
            
            marketEvent.IsActive = previous == string.Empty && 
                                   current == KnownCandleSequences.Black;
            
            await SaveMarketEventAsync(marketEvent);
        }
    }

    /// <inheritdoc />
    public async Task CheckRsiOverBoughtInputMarketEventAsync()
    {
        var instrumentIds = await GetInstrumentIds();
        
        foreach (var instrumentId in instrumentIds)
        {
            var analyseResults = await analyseResultRepository
                .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.Rsi);

            var marketEvent = await CreateMarketEvent(
                instrumentId, KnownMarketEventTypes.RsiOverBoughtInput);

            string previous = analyseResults[0].ResultString;
            string current = analyseResults[1].ResultString;
            
            marketEvent.IsActive = previous == string.Empty && 
                                   current == KnownRsiInterpretations.OverBought;
            
            await SaveMarketEventAsync(marketEvent);
        }
    }
    
    /// <inheritdoc />
    public async Task CheckRsiOverBoughtOutputMarketEventAsync()
    {
        var instrumentIds = await GetInstrumentIds();
        
        foreach (var instrumentId in instrumentIds)
        {
            var analyseResults = await analyseResultRepository
                .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.Rsi);

            var marketEvent = await CreateMarketEvent(
                instrumentId, KnownMarketEventTypes.RsiOverBoughtOutput);

            string previous = analyseResults[0].ResultString;
            string current = analyseResults[1].ResultString;
            
            marketEvent.IsActive = previous == KnownRsiInterpretations.OverBought && 
                                   current == string.Empty;
            
            await SaveMarketEventAsync(marketEvent);
        }
    }
    
    /// <inheritdoc />
    public async Task CheckRsiOverOverSoldInputMarketEventAsync()
    {
        var instrumentIds = await GetInstrumentIds();
        
        foreach (var instrumentId in instrumentIds)
        {
            var analyseResults = await analyseResultRepository
                .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.Rsi);

            var marketEvent = await CreateMarketEvent(
                instrumentId, KnownMarketEventTypes.RsiOverSoldInput);

            string previous = analyseResults[0].ResultString;
            string current = analyseResults[1].ResultString;
            
            marketEvent.IsActive = previous == string.Empty && 
                                   current == KnownRsiInterpretations.OverSold;
            
            await SaveMarketEventAsync(marketEvent);
        }
    }
    
    /// <inheritdoc />
    public async Task CheckRsiOverOverSoldOutputMarketEventAsync()
    {
        var instrumentIds = await GetInstrumentIds();
        
        foreach (var instrumentId in instrumentIds)
        {
            var analyseResults = await analyseResultRepository
                .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.Rsi);

            var marketEvent = await CreateMarketEvent(
                instrumentId, KnownMarketEventTypes.RsiOverSoldOutput);

            string previous = analyseResults[0].ResultString;
            string current = analyseResults[1].ResultString;
            
            marketEvent.IsActive = previous == KnownRsiInterpretations.OverSold && 
                                   current == string.Empty;
            
            await SaveMarketEventAsync(marketEvent);
        }
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
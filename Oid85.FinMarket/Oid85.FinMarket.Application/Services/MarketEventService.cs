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
    IShareRepository shareRepository) 
    : IMarketEventService
{
    /// <inheritdoc />
    public async Task CheckSupertrendMarketEventAsync()
    {
        var instrumentIds = await GetInstrumentIds();
        
        foreach (var instrumentId in instrumentIds)
        {
            var (marketEvent, marketEventReverse) = await CheckMarketEventAsync(
                instrumentId,
                KnownAnalyseTypes.Supertrend,
                (KnownMarketEventTypes.SupertrendUpMarketEvent, KnownMarketEventTypes.SupertrendDownMarketEvent),
                (KnownTrendDirections.Down, KnownTrendDirections.Up));
            
            await SaveMarketEventAsync(marketEvent, marketEventReverse);
        }
    }

    /// <inheritdoc />
    public async Task CheckCandleVolumeMarketEventAsync()
    {
        var instrumentIds = await GetInstrumentIds();
        
        foreach (var instrumentId in instrumentIds)
        {
            var (marketEvent, marketEventReverse) = await CheckMarketEventAsync(
                instrumentId,
                KnownAnalyseTypes.CandleVolume,
                (
                    KnownMarketEventTypes.CandleVolumeUpMarketEvent, 
                    KnownMarketEventTypes.CandleVolumeDownMarketEvent),
                (
                    KnownVolumeDirections.Down, 
                    KnownVolumeDirections.Up));
            
            await SaveMarketEventAsync(marketEvent, marketEventReverse);
        }
    }

    /// <inheritdoc />
    public async Task CheckCandleSequenceMarketEventAsync()
    {
        var instrumentIds = await GetInstrumentIds();
        
        foreach (var instrumentId in instrumentIds)
        {
            var (marketEvent, marketEventReverse) = await CheckMarketEventAsync(
                instrumentId,
                KnownAnalyseTypes.CandleSequence,
                (
                    KnownMarketEventTypes.CandleSequenceWhiteMarketEvent, 
                    KnownMarketEventTypes.CandleSequenceBlackMarketEvent),
                (
                    KnownCandleSequences.Black, 
                    KnownCandleSequences.White));
            
            await SaveMarketEventAsync(marketEvent, marketEventReverse);
        }
    }

    private async Task<List<Guid>> GetInstrumentIds()
    {
        var instrumentIds = new List<Guid>();
        
        instrumentIds.AddRange(
            (await shareRepository.GetWatchListAsync())
            .Select(s => s.InstrumentId));

        return instrumentIds;
    }    
    
    private async Task SaveMarketEventAsync(MarketEvent marketEvent, MarketEvent marketEventReverse)
    {
        if (!marketEvent.IsActive)    
            await marketEventRepository.DeactivateAsync(marketEvent);
            
        if (!marketEventReverse.IsActive)    
            await marketEventRepository.DeactivateAsync(marketEventReverse);
            
        if (marketEvent.IsActive)    
            await marketEventRepository.ActivateAsync(marketEvent);
            
        if (marketEventReverse.IsActive)    
            await marketEventRepository.ActivateAsync(marketEventReverse);
    }
    
    private async Task<(MarketEvent marketEvent, MarketEvent marketEventReverse)> CheckMarketEventAsync(
        Guid instrumentId,
        string analyseType,
        (string MarketEventType1, string MarketEventType2) marketEventTypes,
        (string MarketEventCondition1, string MarketEventCondition2) marketEventConditions)
    {
        var analyseResults = await analyseResultRepository
            .GetTwoLastAsync(instrumentId, analyseType);

        string ticker = (await instrumentRepository.GetByInstrumentIdAsync(instrumentId))!.Ticker;
            
        var marketEvent = new MarketEvent
        {
            InstrumentId = instrumentId,
            Ticker = ticker,
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            Time = TimeOnly.FromDateTime(DateTime.UtcNow),
            MarketEventType = marketEventTypes.MarketEventType1
        };
            
        var marketEventReverse = new MarketEvent
        {
            InstrumentId = instrumentId,
            Ticker = ticker,
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            Time = TimeOnly.FromDateTime(DateTime.UtcNow),
            MarketEventType = marketEventTypes.MarketEventType2
        };
            
        if ((analyseResults[0].ResultString == marketEventConditions.MarketEventCondition1 && 
             analyseResults[1].ResultString == marketEventConditions.MarketEventCondition2) ||
            (analyseResults[0].ResultString == string.Empty && 
             analyseResults[1].ResultString == marketEventConditions.MarketEventCondition2))
        {
            marketEvent.IsActive = true;
            marketEventReverse.IsActive = false;
        }
            
        else if ((analyseResults[0].ResultString == marketEventConditions.MarketEventCondition2 && 
                  analyseResults[1].ResultString == marketEventConditions.MarketEventCondition1) ||
                 (analyseResults[0].ResultString == string.Empty &&
                  analyseResults[1].ResultString == marketEventConditions.MarketEventCondition1))
        {
            marketEvent.IsActive = false;
            marketEventReverse.IsActive = true;
        }

        else
        {
            marketEvent.IsActive = false;
            marketEventReverse.IsActive = false;
        }

        return (marketEvent, marketEventReverse);
    }
}
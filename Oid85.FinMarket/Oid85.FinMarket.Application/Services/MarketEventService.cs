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
        var instrumentIds = (await shareRepository.GetWatchListAsync())
            .Select(s => s.InstrumentId)
            .ToList();
        
        foreach (var instrumentId in instrumentIds)
        {
            var analyseResults = await analyseResultRepository
                .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.Supertrend);

            string ticker = (await instrumentRepository.GetByInstrumentIdAsync(instrumentId))!.Ticker;
            
            var marketEvent = new MarketEvent
            {
                InstrumentId = instrumentId,
                Ticker = ticker,
                Date = DateOnly.FromDateTime(DateTime.UtcNow),
                Time = TimeOnly.FromDateTime(DateTime.UtcNow),
                MarketEventType = KnownMarketEvents.SupertrendTrendUpMarketEvent
            };
            
            var marketEventReverse = new MarketEvent
            {
                InstrumentId = instrumentId,
                Ticker = ticker,
                Date = DateOnly.FromDateTime(DateTime.UtcNow),
                Time = TimeOnly.FromDateTime(DateTime.UtcNow),
                MarketEventType = KnownMarketEvents.SupertrendTrendDownMarketEvent
            };
            
            if (analyseResults[0].ResultString == KnownTrendDirections.Down && 
                analyseResults[1].ResultString == KnownTrendDirections.Up)
            {
                marketEvent.IsActive = true;
                marketEventReverse.IsActive = false;
            }
            
            else if (analyseResults[0].ResultString == KnownTrendDirections.Up && 
                analyseResults[1].ResultString == KnownTrendDirections.Down)
            {
                marketEvent.IsActive = false;
                marketEventReverse.IsActive = true;
            }

            else
            {
                marketEvent.IsActive = false;
                marketEventReverse.IsActive = false;
            }
            
            await SaveMarketEventAsync(marketEvent, marketEventReverse);
        }
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
}
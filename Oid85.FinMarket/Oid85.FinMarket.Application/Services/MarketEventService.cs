using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class MarketEventService(
    ILogger logger,
    IInstrumentService instrumentService,
    IMarketEventRepository marketEventRepository,
    IAnalyseResultRepository analyseResultRepository,
    ICandleRepository candleRepository,
    IInstrumentRepository instrumentRepository,
    ISpreadRepository spreadRepository,
    IResourceStoreService resourceStoreService) 
    : IMarketEventService
{
    /// <inheritdoc />
    public async Task CheckSupertrendUpMarketEventAsync()
    {
        try
        {
            var instrumentIds = await instrumentService.GetInstrumentIdsInWatchlist();
        
            foreach (var instrumentId in instrumentIds)
            {
                var analyseResults = await analyseResultRepository
                    .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.Supertrend);
            
                var marketEvent = await CreateMarketEvent(
                    instrumentId, KnownMarketEventTypes.SupertrendUp);

                marketEvent.MarketEventText = "Тренд вверх";
            
                string previous = analyseResults[0].ResultString;
                string current = analyseResults[1].ResultString;
            
                marketEvent.IsActive = previous == KnownTrendDirections.Down && 
                                       current == KnownTrendDirections.Up;
            
                marketEvent.IsActive |= previous == string.Empty && 
                                        current == KnownTrendDirections.Up;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Trace(exception.Message);
        }
    }
    
    /// <inheritdoc />
    public async Task CheckSupertrendDownMarketEventAsync()
    {
        try
        {
            var instrumentIds = await instrumentService.GetInstrumentIdsInWatchlist();
        
            foreach (var instrumentId in instrumentIds)
            {
                var analyseResults = await analyseResultRepository
                    .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.Supertrend);

                var marketEvent = await CreateMarketEvent(
                    instrumentId, KnownMarketEventTypes.SupertrendDown);

                marketEvent.MarketEventText = "Тренд вниз";
            
                string previous = analyseResults[0].ResultString;
                string current = analyseResults[1].ResultString;
            
                marketEvent.IsActive = previous == KnownTrendDirections.Up && 
                                       current == KnownTrendDirections.Down;
            
                marketEvent.IsActive |= previous == string.Empty && 
                                        current == KnownTrendDirections.Down;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Trace(exception.Message);
        }
    }
    
    /// <inheritdoc />
    public async Task CheckCandleVolumeUpMarketEventAsync()
    {
        try
        {
            var instrumentIds = await instrumentService.GetInstrumentIdsInWatchlist();
        
            foreach (var instrumentId in instrumentIds)
            {
                var analyseResults = await analyseResultRepository
                    .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.CandleVolume);

                var marketEvent = await CreateMarketEvent(
                    instrumentId, KnownMarketEventTypes.CandleVolumeUp);

                marketEvent.MarketEventText = "Рост днев. объемов";
            
                string previous = analyseResults[0].ResultString;
                string current = analyseResults[1].ResultString;
            
                marketEvent.IsActive = previous == string.Empty && 
                                       current == KnownVolumeDirections.Up;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Trace(exception.Message);
        }
    }

    /// <inheritdoc />
    public async Task CheckCandleSequenceWhiteMarketEventAsync()
    {
        try
        {
            var instrumentIds = await instrumentService.GetInstrumentIdsInWatchlist();
        
            foreach (var instrumentId in instrumentIds)
            {
                var analyseResults = await analyseResultRepository
                    .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.CandleSequence);

                var marketEvent = await CreateMarketEvent(
                    instrumentId, KnownMarketEventTypes.CandleSequenceWhite);

                marketEvent.MarketEventText = "3 белых свечи";
            
                string previous = analyseResults[0].ResultString;
                string current = analyseResults[1].ResultString;
            
                marketEvent.IsActive = previous == string.Empty && 
                                       current == KnownCandleSequences.White;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Trace(exception.Message);
        }
    }

    /// <inheritdoc />
    public async Task CheckCandleSequenceBlackMarketEventAsync()
    {
        try
        {
            var instrumentIds = await instrumentService.GetInstrumentIdsInWatchlist();
        
            foreach (var instrumentId in instrumentIds)
            {
                var analyseResults = await analyseResultRepository
                    .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.CandleSequence);

                var marketEvent = await CreateMarketEvent(
                    instrumentId, KnownMarketEventTypes.CandleSequenceBlack);

                marketEvent.MarketEventText = "3 черных свечи";
            
                string previous = analyseResults[0].ResultString;
                string current = analyseResults[1].ResultString;
            
                marketEvent.IsActive = previous == string.Empty && 
                                       current == KnownCandleSequences.Black;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Trace(exception.Message);
        }
    }

    /// <inheritdoc />
    public async Task CheckRsiOverBoughtInputMarketEventAsync()
    {
        try
        {
            var instrumentIds = await instrumentService.GetInstrumentIdsInWatchlist();
        
            foreach (var instrumentId in instrumentIds)
            {
                var analyseResults = await analyseResultRepository
                    .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.Rsi);

                var marketEvent = await CreateMarketEvent(
                    instrumentId, KnownMarketEventTypes.RsiOverBoughtInput);

                marketEvent.MarketEventText = "RSI - вход в перекуп.";
            
                string previous = analyseResults[0].ResultString;
                string current = analyseResults[1].ResultString;
            
                marketEvent.IsActive = previous == string.Empty && 
                                       current == KnownRsiInterpretations.OverBought;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Trace(exception.Message);
        }
    }
    
    /// <inheritdoc />
    public async Task CheckRsiOverBoughtOutputMarketEventAsync()
    {
        try
        {
            var instrumentIds = await instrumentService.GetInstrumentIdsInWatchlist();
        
            foreach (var instrumentId in instrumentIds)
            {
                var analyseResults = await analyseResultRepository
                    .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.Rsi);

                var marketEvent = await CreateMarketEvent(
                    instrumentId, KnownMarketEventTypes.RsiOverBoughtOutput);

                marketEvent.MarketEventText = "RSI - выход из перекуп.";
            
                string previous = analyseResults[0].ResultString;
                string current = analyseResults[1].ResultString;
            
                marketEvent.IsActive = previous == KnownRsiInterpretations.OverBought && 
                                       current == string.Empty;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Trace(exception.Message);
        }
    }
    
    /// <inheritdoc />
    public async Task CheckRsiOverOverSoldInputMarketEventAsync()
    {
        try
        {
            var instrumentIds = await instrumentService.GetInstrumentIdsInWatchlist();
        
            foreach (var instrumentId in instrumentIds)
            {
                var analyseResults = await analyseResultRepository
                    .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.Rsi);

                var marketEvent = await CreateMarketEvent(
                    instrumentId, KnownMarketEventTypes.RsiOverSoldInput);

                marketEvent.MarketEventText = "RSI - вход в перепрод.";
            
                string previous = analyseResults[0].ResultString;
                string current = analyseResults[1].ResultString;
            
                marketEvent.IsActive = previous == string.Empty && 
                                       current == KnownRsiInterpretations.OverSold;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Trace(exception.Message);
        }
    }
    
    /// <inheritdoc />
    public async Task CheckRsiOverOverSoldOutputMarketEventAsync()
    {
        try
        {
            var instrumentIds = await instrumentService.GetInstrumentIdsInWatchlist();
        
            foreach (var instrumentId in instrumentIds)
            {
                var analyseResults = await analyseResultRepository
                    .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.Rsi);

                var marketEvent = await CreateMarketEvent(
                    instrumentId, KnownMarketEventTypes.RsiOverSoldOutput);

                marketEvent.MarketEventText = "RSI - выход из перепрод.";
            
                string previous = analyseResults[0].ResultString;
                string current = analyseResults[1].ResultString;
            
                marketEvent.IsActive = previous == KnownRsiInterpretations.OverSold && 
                                       current == string.Empty;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Trace(exception.Message);
        }
    }
    
    /// <inheritdoc />
    public async Task CheckCrossPriceLevelMarketEventAsync()
    {
        try
        {
            var instrumentIds = await instrumentService.GetInstrumentIdsInWatchlist();
        
            foreach (var instrumentId in instrumentIds)
            {
                string ticker = (await instrumentRepository.GetByInstrumentIdAsync(instrumentId))!.Ticker;
                var priceLevels = await resourceStoreService.GetPriceLevelsAsync(ticker);
            
                foreach (var priceLevel in priceLevels)
                {
                    var marketEvent = await CreateMarketEvent(
                        instrumentId, KnownMarketEventTypes.CrossPriceLevel);
                
                    marketEvent.MarketEventText = $"Достигнут уровнь '{priceLevel}'";

                    var lastCandle = await candleRepository.GetLastAsync(instrumentId);
                
                    marketEvent.IsActive =
                        lastCandle is not null &&
                        priceLevel.Enable &&
                        lastCandle.Low <= priceLevel.Value && 
                        lastCandle.High >= priceLevel.Value;
            
                    await SaveMarketEventAsync(marketEvent);
                }
            }
        }
        
        catch (Exception exception)
        {
            logger.Trace(exception.Message);
        }
    }

    /// <inheritdoc />
    public async Task CheckSpreadGreaterPercent1MarketEventAsync()
    {
        try
        {
            var spreads = (await spreadRepository.GetAllAsync()).ToList();
        
            foreach (var spread in spreads)
            {
                var marketEvent = await CreateMarketEvent(
                    spread.FirstInstrumentId, KnownMarketEventTypes.SpreadGreaterPercent1);
            
                marketEvent.MarketEventText = $"Спред '{spread.FirstInstrumentTicker}/{spread.SecondInstrumentTicker}' превышает 1 %";
            
                if (spread is
                    {
                        FirstInstrumentPrice: > 0, 
                        SecondInstrumentPrice: > 0, 
                        PriceDifference: > 0
                    })
                    marketEvent.IsActive = (spread.PriceDifference / spread.FirstInstrumentPrice) > 0.01;
            
                else
                    marketEvent.IsActive = false;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Trace(exception.Message);
        }
    }

    /// <inheritdoc />
    public async Task CheckSpreadGreaterPercent2MarketEventAsync()
    {
        try
        {
            var spreads = (await spreadRepository.GetAllAsync()).ToList();
        
            foreach (var spread in spreads)
            {
                var marketEvent = await CreateMarketEvent(
                    spread.FirstInstrumentId, KnownMarketEventTypes.SpreadGreaterPercent2);
            
                marketEvent.MarketEventText = $"Спред '{spread.FirstInstrumentTicker}/{spread.SecondInstrumentTicker}' превышает 2 %";
            
                if (spread is
                    {
                        FirstInstrumentPrice: > 0, 
                        SecondInstrumentPrice: > 0, 
                        PriceDifference: > 0
                    })
                    marketEvent.IsActive = (spread.PriceDifference / spread.FirstInstrumentPrice) > 0.02;
            
                else
                    marketEvent.IsActive = false;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Trace(exception.Message);
        }
    }

    /// <inheritdoc />
    public async Task CheckSpreadGreaterPercent3MarketEventAsync()
    {
        try
        {
            var spreads = (await spreadRepository.GetAllAsync()).ToList();
        
            foreach (var spread in spreads)
            {
                var marketEvent = await CreateMarketEvent(
                    spread.FirstInstrumentId, KnownMarketEventTypes.SpreadGreaterPercent3);
            
                marketEvent.MarketEventText = $"Спред '{spread.FirstInstrumentTicker}/{spread.SecondInstrumentTicker}' превышает 3 %";
            
                if (spread is
                    {
                        FirstInstrumentPrice: > 0, 
                        SecondInstrumentPrice: > 0, 
                        PriceDifference: > 0
                    })
                    marketEvent.IsActive = (spread.PriceDifference / spread.FirstInstrumentPrice) > 0.03;
            
                else
                    marketEvent.IsActive = false;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Trace(exception.Message);
        }
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
        switch (marketEvent.IsActive)
        {
            case true:
                await marketEventRepository.ActivateAsync(marketEvent);
                break;
            
            case false:
                await marketEventRepository.DeactivateAsync(marketEvent);
                break;
        }
    }
}
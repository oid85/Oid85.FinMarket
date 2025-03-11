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
    IFiveMinuteCandleRepository fiveMinuteCandleRepository,
    IInstrumentRepository instrumentRepository,
    ISpreadRepository spreadRepository,
    IResourceStoreService resourceStoreService,
    IForecastConsensusRepository forecastConsensusRepository) 
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
            
                if (analyseResults is [])
                    continue;
                
                var marketEvent = await CreateMarketEvent(
                    instrumentId, 
                    KnownMarketEventTypes.SupertrendUp,
                    "Тренд вверх");
            
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
            logger.Error(exception);
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

                if (analyseResults is [])
                    continue;
                
                var marketEvent = await CreateMarketEvent(
                    instrumentId, 
                    KnownMarketEventTypes.SupertrendDown, 
                    "Тренд вниз");
            
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
            logger.Error(exception);
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
                
                if (analyseResults is [])
                    continue;
                
                var marketEvent = await CreateMarketEvent(
                    instrumentId, 
                    KnownMarketEventTypes.CandleVolumeUp, 
                    "Рост днев. объемов");
            
                string previous = analyseResults[0].ResultString;
                string current = analyseResults[1].ResultString;
            
                marketEvent.IsActive = previous == string.Empty && 
                                       current == KnownVolumeDirections.Up;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
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

                if (analyseResults is [])
                    continue;
                
                var marketEvent = await CreateMarketEvent(
                    instrumentId, 
                    KnownMarketEventTypes.CandleSequenceWhite, 
                    "3 белых свечи");
                
                string previous = analyseResults[0].ResultString;
                string current = analyseResults[1].ResultString;
            
                marketEvent.IsActive = previous == string.Empty && 
                                       current == KnownCandleSequences.White;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
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

                if (analyseResults is [])
                    continue;
                
                var marketEvent = await CreateMarketEvent(
                    instrumentId, 
                    KnownMarketEventTypes.CandleSequenceBlack, 
                    "3 черных свечи");
                
                string previous = analyseResults[0].ResultString;
                string current = analyseResults[1].ResultString;
            
                marketEvent.IsActive = previous == string.Empty && 
                                       current == KnownCandleSequences.Black;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
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

                if (analyseResults is [])
                    continue;
                
                var marketEvent = await CreateMarketEvent(
                    instrumentId, 
                    KnownMarketEventTypes.RsiOverBoughtInput,
                    "RSI - вход в перекуп.");
                
                string previous = analyseResults[0].ResultString;
                string current = analyseResults[1].ResultString;
            
                marketEvent.IsActive = previous == string.Empty && 
                                       current == KnownRsiInterpretations.OverBought;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
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

                if (analyseResults is [])
                    continue;
                
                var marketEvent = await CreateMarketEvent(
                    instrumentId, 
                    KnownMarketEventTypes.RsiOverBoughtOutput,
                    "RSI - выход из перекуп.");
                
                string previous = analyseResults[0].ResultString;
                string current = analyseResults[1].ResultString;
            
                marketEvent.IsActive = previous == KnownRsiInterpretations.OverBought && 
                                       current == string.Empty;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
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

                if (analyseResults is [])
                    continue;
                
                var marketEvent = await CreateMarketEvent(
                    instrumentId, 
                    KnownMarketEventTypes.RsiOverSoldInput,
                    "RSI - вход в перепрод.");
                
                string previous = analyseResults[0].ResultString;
                string current = analyseResults[1].ResultString;
            
                marketEvent.IsActive = previous == string.Empty && 
                                       current == KnownRsiInterpretations.OverSold;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
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

                if (analyseResults is [])
                    continue;
                
                var marketEvent = await CreateMarketEvent(
                    instrumentId, 
                    KnownMarketEventTypes.RsiOverSoldOutput,
                    "RSI - выход из перепрод.");

                string previous = analyseResults[0].ResultString;
                string current = analyseResults[1].ResultString;
            
                marketEvent.IsActive = previous == KnownRsiInterpretations.OverSold && 
                                       current == string.Empty;
            
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
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
                        instrumentId, 
                        KnownMarketEventTypes.CrossPriceLevel,
                        $"Достигнут уровень '{priceLevel.Value}'");
                    
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
            logger.Error(exception);
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
                    spread.FirstInstrumentId, 
                    KnownMarketEventTypes.SpreadGreaterPercent1,
                    $"Спред '{spread.FirstInstrumentTicker}' / '{spread.SecondInstrumentTicker}' превышает 1 %");
            
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
            logger.Error(exception);
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
                    spread.FirstInstrumentId, 
                    KnownMarketEventTypes.SpreadGreaterPercent2,
                    $"Спред '{spread.FirstInstrumentTicker}' / '{spread.SecondInstrumentTicker}' превышает 2 %");
            
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
            logger.Error(exception);
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
                    spread.FirstInstrumentId, 
                    KnownMarketEventTypes.SpreadGreaterPercent3,
                    $"Спред '{spread.FirstInstrumentTicker}' / '{spread.SecondInstrumentTicker}' превышает 3 %");
                
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
            logger.Error(exception);
        }
    }

    /// <inheritdoc />
    public async Task CheckForecastReleasedMarketEventAsync()
    {
        try
        {
            var shares = await instrumentService.GetSharesInWatchlist();
            var forecasts = await forecastConsensusRepository.GetAllAsync();

            foreach (var share in shares)
            {
                var forecast = forecasts.FirstOrDefault(x => x.InstrumentId == share.InstrumentId);

                if (forecast is not null)
                {
                    var marketEvent = await CreateMarketEvent(
                        share.InstrumentId, 
                        KnownMarketEventTypes.ForecastReleased,
                        $"Прогноз по '{share.Ticker}'. Уровень цены {share.LastPrice} в прогнозном диапазоне {forecast.MinTarget} - {forecast.MaxTarget}");

                    marketEvent.IsActive = share.LastPrice >= forecast.MinTarget && share.LastPrice <= forecast.MaxTarget;
                    await SaveMarketEventAsync(marketEvent);
                }
            }
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
        }
    }

    /// <inheritdoc />
    public async Task CheckStrikeDayMarketEventAsync()
    {
        try
        {
            var shares = await instrumentService.GetSharesInWatchlist();

            foreach (var share in shares)
            {
                var candles = await fiveMinuteCandleRepository.GetLastDayAsync(share.InstrumentId);

                // Объем последней свечи больше, чем объем 90% свечей
                long volume = candles[^1].Volume;
                int countCandlesLessVolume = candles.Count(x => x.Volume < volume);
                bool result = (double) countCandlesLessVolume / (double) candles.Count >= 0.9;

                // Объем растет
                result &= candles[^1].Volume > candles[^2].Volume;
                
                // Последние 2 свечи белые
                result &= candles[^1].Close > candles[^1].Open;
                result &= candles[^2].Close > candles[^2].Open;
                
                var marketEvent = await CreateMarketEvent(
                    share.InstrumentId, 
                    KnownMarketEventTypes.ForecastReleased,
                    $"(!) Ударный день '{share.Ticker}'");

                marketEvent.IsActive = result;
                await SaveMarketEventAsync(marketEvent);
            }
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
        }
    }

    private async Task<MarketEvent> CreateMarketEvent(
        Guid instrumentId,
        string marketEventType,
        string marketEventText)
    {
        var instrument = await instrumentRepository.GetByInstrumentIdAsync(instrumentId);
        
        var marketEvent =  new MarketEvent
        {
            InstrumentId = instrumentId,
            Ticker = instrument?.Ticker ?? string.Empty,
            InstrumentName = instrument?.Name ?? string.Empty,
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            Time = TimeOnly.FromDateTime(DateTime.UtcNow),
            MarketEventType = marketEventType,
            MarketEventText = marketEventText,
            IsActive = false
        };

        await marketEventRepository.AddIfNotExistsAsync(marketEvent);
        
        return marketEvent;
    }

    private async Task SaveMarketEventAsync(MarketEvent marketEvent)
    {
        if (marketEvent.IsActive)
            await marketEventRepository.ActivateAsync(marketEvent);

        else
        {
            await marketEventRepository.DeactivateAsync(marketEvent);
            await marketEventRepository.MarkAsNoSentAsync(marketEvent);
        }
    }
}
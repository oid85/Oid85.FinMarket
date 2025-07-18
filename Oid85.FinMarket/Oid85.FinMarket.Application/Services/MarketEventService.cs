﻿using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class MarketEventService(
    ILogger logger,
    ITickerListUtilService tickerListUtilService,
    IMarketEventRepository marketEventRepository,
    IAnalyseResultRepository analyseResultRepository,
    IDailyCandleRepository dailyCandleRepository,
    IInstrumentRepository instrumentRepository,
    IResourceStoreService resourceStoreService,
    IForecastConsensusRepository forecastConsensusRepository) 
    : IMarketEventService
{
    /// <inheritdoc />
    public async Task CheckSupertrendUpMarketEventAsync()
    {
        try
        {
            var instrumentIds = await tickerListUtilService.GetInstrumentIdsInWatchlist();
        
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
            var instrumentIds = await tickerListUtilService.GetInstrumentIdsInWatchlist();
        
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
            var instrumentIds = await tickerListUtilService.GetInstrumentIdsInWatchlist();
        
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
            var instrumentIds = await tickerListUtilService.GetInstrumentIdsInWatchlist();
        
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
            var instrumentIds = await tickerListUtilService.GetInstrumentIdsInWatchlist();
        
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
            var instrumentIds = await tickerListUtilService.GetInstrumentIdsInWatchlist();
        
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
            var instrumentIds = await tickerListUtilService.GetInstrumentIdsInWatchlist();
        
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
            var instrumentIds = await tickerListUtilService.GetInstrumentIdsInWatchlist();
        
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
            var instrumentIds = await tickerListUtilService.GetInstrumentIdsInWatchlist();
        
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
            var instrumentIds = await tickerListUtilService.GetInstrumentIdsInWatchlist();
        
            foreach (var instrumentId in instrumentIds)
            {
                string ticker = (await instrumentRepository.GetAsync(instrumentId))!.Ticker;
                var priceLevels = await resourceStoreService.GetPriceLevelsAsync(ticker);
            
                foreach (var priceLevel in priceLevels)
                {
                    var marketEvent = await CreateMarketEvent(
                        instrumentId, 
                        KnownMarketEventTypes.CrossPriceLevel,
                        $"Достигнут уровень '{priceLevel.Value}'");
                    
                    var lastCandle = await dailyCandleRepository.GetLastAsync(instrumentId);
                
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
    public async Task CheckForecastReleasedMarketEventAsync()
    {
        try
        {
            var shares = await tickerListUtilService.GetSharesByTickerListAsync(KnownTickerLists.SharesWatchlist);
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
    
    private async Task<MarketEvent> CreateMarketEvent(
        Guid instrumentId,
        string marketEventType,
        string marketEventText)
    {
        var instrument = await instrumentRepository.GetAsync(instrumentId);
        
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
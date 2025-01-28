﻿using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Tinkoff;
using Oid85.FinMarket.Logging.Services;

namespace Oid85.FinMarket.Application.Services;

public class LoadService(
    ILogService logService,
    ITinkoffService tinkoffService,
    IShareRepository shareRepository,
    IFutureRepository futureRepository,
    IBondRepository bondRepository,
    IIndexRepository indexRepository,
    ICurrencyRepository currencyRepository,
    IInstrumentService instrumentService,
    ICandleRepository candleRepository,
    IFiveMinuteCandleRepository fiveMinuteCandleRepository,
    IDividendInfoRepository dividendInfoRepository,
    IBondCouponRepository bondCouponRepository,
    IAssetFundamentalRepository assetFundamentalRepository,
    IInstrumentRepository instrumentRepository,
    IForecastTargetRepository forecastTargetRepository,
    IForecastConsensusRepository forecastConsensusRepository)
    : ILoadService
{
    public async Task LoadSharesAsync()
    {
        var shares = await tinkoffService.GetSharesAsync();
        await shareRepository.AddAsync(shares);

        var tickers = shares
            .Select(x => new Instrument()
            {
                InstrumentId = x.InstrumentId,
                Ticker = x.Ticker,
                Name = x.Name,
                Type = KnownInstrumentTypes.Share
            })
            .ToList();
        
        await instrumentRepository.AddOrUpdateAsync(tickers);
        
        await logService.LogTrace($"Загружены акции. {shares.Count} шт.");
    }

    public async Task LoadShareLastPricesAsync()
    {
        var shares = await instrumentService.GetSharesInWatchlist();
            
        var instrumentIds = shares.Select(x => x.InstrumentId).ToList();
            
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await shareRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
            
        await logService.LogTrace($"Загружены последние цены по акциям. {shares.Count} шт.");
    }

    public async Task LoadShareDailyCandlesAsync()
    {
        var instruments = await instrumentService.GetSharesInWatchlist();

        foreach (var instrument in instruments)
        {
            var lastCandle = await candleRepository.GetLastAsync(instrument.InstrumentId);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int historyInYears = 3;

                for (int year = currentYear - historyInYears; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetCandlesAsync(
                        instrument.InstrumentId, year);
                    
                    await candleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetCandlesAsync(
                    instrument.InstrumentId,
                    lastCandle.Date,
                    DateOnly.FromDateTime(DateTime.Today));
                    
                await candleRepository.AddOrUpdateAsync(candles);
            }
            
            await logService.LogTrace($"Загружены дневные свечи по '{instrument.Ticker}' ({instrument.Name})");
        }
    }

    public async Task LoadShareFiveMinuteCandlesAsync()
    {
        var instruments = await instrumentService.GetSharesInWatchlist();

        foreach (var instrument in instruments)
        {
            var lastCandle = await fiveMinuteCandleRepository.GetLastAsync(instrument.InstrumentId);

            if (lastCandle is null || 
                (lastCandle.Date.ToDateTime(TimeOnly.MinValue) - DateTime.Today).TotalDays > 5)
            {
                var candles = await tinkoffService.GetFiveMinuteCandlesAsync(
                    instrument.InstrumentId,
                    DateTime.UtcNow.AddDays(-5),
                    DateTime.UtcNow);
                    
                await fiveMinuteCandleRepository.AddOrUpdateAsync(candles);
            }

            else
            {
                var candles = await tinkoffService.GetFiveMinuteCandlesAsync(
                    instrument.InstrumentId,
                    lastCandle.Date.ToDateTime(lastCandle.Time),
                    DateTime.UtcNow);
                    
                await fiveMinuteCandleRepository.AddOrUpdateAsync(candles);
            }
            
            await logService.LogTrace($"Загружены 5-минутные свечи по '{instrument.Ticker}' ({instrument.Name})");
        }
    }

    public async Task LoadForecastsAsync()
    {
        var instruments = await instrumentService.GetSharesInWatchlist();

        foreach (var instrument in instruments)
        {
            var (targets, consensus) = await tinkoffService.GetForecastAsync(instrument.InstrumentId);
            
            await forecastTargetRepository.AddAsync(targets);
            await forecastConsensusRepository.AddAsync([consensus]);
            
            await logService.LogTrace($"Загружен прогноз по '{instrument.Ticker}' ({instrument.Name})");
        }
    }


    public async Task LoadFuturesAsync()
    {
        var futures = await tinkoffService.GetFuturesAsync();
        await futureRepository.AddAsync(futures);
            
        var tickers = futures
            .Select(x => new Instrument()
            {
                InstrumentId = x.InstrumentId,
                Ticker = x.Ticker,
                Name = x.Name,
                Type = KnownInstrumentTypes.Future
            })
            .ToList();
        
        await instrumentRepository.AddOrUpdateAsync(tickers);
        
        await logService.LogTrace($"Загружены фьючерсы. {futures.Count} шт.");
    }

    public async Task LoadFutureLastPricesAsync()
    {
        var futures = await instrumentService.GetFuturesInWatchlist();
            
        var instrumentIds = futures
            .Select(x => x.InstrumentId)
            .ToList();
            
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await futureRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
            
        await logService.LogTrace($"Загружены последние цены по фьючерсам. {futures.Count} шт.");
    }

    public async Task LoadFutureDailyCandlesAsync()
    {
        var instruments = await instrumentService.GetFuturesInWatchlist();

        foreach (var instrument in instruments)
        {
            var lastCandle = await candleRepository.GetLastAsync(instrument.InstrumentId);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int historyInYears = 3;

                for (int year = currentYear - historyInYears; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetCandlesAsync(
                        instrument.InstrumentId, year);
                    
                    await candleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetCandlesAsync(
                    instrument.InstrumentId,
                    lastCandle.Date,
                    DateOnly.FromDateTime(DateTime.Today));
                    
                await candleRepository.AddOrUpdateAsync(candles);
            }
            
            await logService.LogTrace($"Загружены свечи по '{instrument.Ticker}' ({instrument.Name})");
        }
    }

    
    public async Task LoadIndexesAsync()
    {
        var indicatives = await tinkoffService.GetIndexesAsync();
        await indexRepository.AddAsync(indicatives);
            
        var tickers = indicatives
            .Select(x => new Instrument()
            {
                InstrumentId = x.InstrumentId,
                Ticker = x.Ticker,
                Name = x.Name,
                Type = KnownInstrumentTypes.Index
            })
            .ToList();
        
        await instrumentRepository.AddOrUpdateAsync(tickers);
        
        await logService.LogTrace($"Загружены индикативные инструменты. {indicatives.Count} шт.");
    }
    
    public async Task LoadIndexLastPricesAsync()
    {
        var indicatives = await instrumentService.GetFinIndexesInWatchlist();
            
        var instrumentIds = indicatives
            .Select(x => x.InstrumentId)
            .ToList();
            
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await indexRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
            
        await logService.LogTrace($"Загружены последние цены по индикативам. {indicatives.Count} шт.");
    }

    public async Task LoadIndexDailyCandlesAsync()
    {
        var instruments = await instrumentService.GetFinIndexesInWatchlist();

        foreach (var instrument in instruments)
        {
            var lastCandle = await candleRepository.GetLastAsync(
                instrument.InstrumentId);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int historyInYears = 3;

                for (int year = currentYear - historyInYears; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetCandlesAsync(
                        instrument.InstrumentId, year);
                    
                    await candleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetCandlesAsync(
                    instrument.InstrumentId,
                    lastCandle.Date,
                    DateOnly.FromDateTime(DateTime.Today));
                    
                await candleRepository.AddOrUpdateAsync(candles);
            }
            
            await logService.LogTrace($"Загружены свечи по '{instrument.Ticker}' ({instrument.Name})");
        }
    }

    
    public async Task LoadCurrenciesAsync()
    {
        var currencies = await tinkoffService.GetCurrenciesAsync();
        await currencyRepository.AddAsync(currencies);
            
        var tickers = currencies
            .Select(x => new Instrument()
            {
                InstrumentId = x.InstrumentId,
                Ticker = x.Ticker,
                Name = x.Name,
                Type = KnownInstrumentTypes.Currency
            })
            .ToList();
        
        await instrumentRepository.AddOrUpdateAsync(tickers);
        
        await logService.LogTrace($"Загружены валюты. {currencies.Count} шт.");
    }

    public async Task LoadCurrencyLastPricesAsync()
    {
        var currencies = await instrumentService.GetCurrenciesInWatchlist();
            
        var instrumentIds = currencies
            .Select(x => x.InstrumentId)
            .ToList();
            
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await currencyRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
            
        await logService.LogTrace($"Загружены последние цены по валютам. {currencies.Count} шт.");
    }

    public async Task LoadCurrencyDailyCandlesAsync()
    {
        var instruments = await instrumentService.GetCurrenciesInWatchlist();

        foreach (var instrument in instruments)
        {
            var lastCandle = await candleRepository.GetLastAsync(
                instrument.InstrumentId);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int historyInYears = 3;

                for (int year = currentYear - historyInYears; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetCandlesAsync(
                        instrument.InstrumentId, year);
                    
                    await candleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetCandlesAsync(
                    instrument.InstrumentId,
                    lastCandle.Date,
                    DateOnly.FromDateTime(DateTime.Today));
                    
                await candleRepository.AddOrUpdateAsync(candles);
            }
            
            await logService.LogTrace($"Загружены свечи по '{instrument.Ticker}' ({instrument.Name})");
        }
    }

    
    public async Task LoadAssetFundamentalsAsync()
    {
        var shares = await instrumentService.GetSharesInWatchlist();

        var instrumentIds = new List<Guid>();

        // Загружаем данные, которых нет
        foreach (var share in shares)
        {
            var assetFundamental = await assetFundamentalRepository
                .GetLastAsync(share.InstrumentId);
            
            if (assetFundamental is null)
                instrumentIds.Add(share.InstrumentId);
            
            else
                if (assetFundamental.Date < DateOnly.FromDateTime(DateTime.Today))
                    instrumentIds.Add(share.InstrumentId);
        }

        var assetFundamentals = await tinkoffService
            .GetAssetFundamentalsAsync(instrumentIds);
        
        await assetFundamentalRepository.AddAsync(assetFundamentals);
            
        await logService.LogTrace($"Загружены фундаментальные данные. {assetFundamentals.Count} шт.");
    }

    
    public async Task LoadDividendInfosAsync()
    {
        var shares = await instrumentService.GetSharesInWatchlist();
        var dividendInfos = await tinkoffService.GetDividendInfoAsync(shares);
        await dividendInfoRepository.AddOrUpdateAsync(dividendInfos);
            
        await logService.LogTrace($"Загружена информация по дивидендам. {dividendInfos.Count} шт.");
    }
    
    
    public async Task LoadBondsAsync()
    {
        var bonds = await tinkoffService.GetBondsAsync();
        await bondRepository.AddAsync(bonds);
            
        var tickers = bonds
            .Select(x => new Instrument()
            {
                InstrumentId = x.InstrumentId,
                Ticker = x.Ticker,
                Name = x.Name,
                Type = KnownInstrumentTypes.Bond
            })
            .ToList();
        
        await instrumentRepository.AddOrUpdateAsync(tickers);
        
        await logService.LogTrace($"Загружены облигации. {bonds.Count} шт.");
    }
    
    public async Task LoadBondLastPricesAsync()
    {
        var bonds = await instrumentService.GetBondsInWatchlist();
            
        var instrumentIds = bonds
            .Select(x => x.InstrumentId)
            .ToList();
            
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await bondRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
            
        await logService.LogTrace($"Загружены последние цены по облигациям. {bonds.Count} шт.");
    }

    public async Task LoadBondDailyCandlesAsync()
    {
        var instruments = await instrumentService.GetBondsInWatchlist();

        foreach (var instrument in instruments)
        {
            var lastCandle = await candleRepository.GetLastAsync(
                instrument.InstrumentId);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int historyInYears = 3;

                for (int year = currentYear - historyInYears; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetCandlesAsync(
                        instrument.InstrumentId, year);
                    
                    await candleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetCandlesAsync(
                    instrument.InstrumentId,
                    lastCandle.Date,
                    DateOnly.FromDateTime(DateTime.Today));
                    
                await candleRepository.AddOrUpdateAsync(candles);
            }
            
            await logService.LogTrace($"Загружены свечи по '{instrument.Ticker}' ({instrument.Name})");
        }
    }

    public async Task LoadBondCouponsAsync()
    {
        var bonds = await instrumentService.GetBondsInWatchlist();
        var bondCoupons = await tinkoffService.GetBondCouponsAsync(bonds);
        await bondCouponRepository.AddAsync(bondCoupons);
            
        await logService.LogTrace($"Загружена информация по купонам облигаций. {bondCoupons.Count} шт.");
    }
}
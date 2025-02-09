﻿using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Tinkoff;

namespace Oid85.FinMarket.Application.Services;

public class LoadService(
    ILogger logger,
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
    public async Task<bool> LoadSharesAsync()
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
        
        logger.Trace($"Загружены акции. {shares.Count} шт.");
        
        return true;
    }

    public async Task<bool> LoadShareLastPricesAsync()
    {
        var shares = await instrumentService.GetSharesInWatchlist();
            
        var instrumentIds = shares.Select(x => x.InstrumentId).ToList();
            
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await shareRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
            
        logger.Trace($"Загружены последние цены по акциям. {shares.Count} шт.");
        
        return true;
    }

    public async Task<bool> LoadShareDailyCandlesAsync()
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
            
            logger.Trace($"Загружены дневные свечи по '{instrument.Ticker}' ({instrument.Name})");
        }
        
        return true;
    }

    public async Task<bool> LoadShareFiveMinuteCandlesAsync()
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
            
            logger.Trace($"Загружены 5-минутные свечи по '{instrument.Ticker}' ({instrument.Name})");
        }
        
        return true;
    }

    public async Task<bool> LoadForecastsAsync()
    {
        var instruments = await instrumentService.GetSharesInWatchlist();

        foreach (var instrument in instruments)
        {
            var (targets, consensus) = await tinkoffService.GetForecastAsync(instrument.InstrumentId);
            
            await forecastTargetRepository.AddAsync(targets);
            await forecastConsensusRepository.AddAsync([consensus]);
            
            logger.Trace($"Загружен прогноз по '{instrument.Ticker}' ({instrument.Name})");
        }
        
        return true;
    }


    public async Task<bool> LoadFuturesAsync()
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
        
        logger.Trace($"Загружены фьючерсы. {futures.Count} шт.");
        
        return true;
    }

    public async Task<bool> LoadFutureLastPricesAsync()
    {
        var futures = await instrumentService.GetFuturesInWatchlist();
            
        var instrumentIds = futures
            .Select(x => x.InstrumentId)
            .ToList();
            
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await futureRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
            
        logger.Trace($"Загружены последние цены по фьючерсам. {futures.Count} шт.");
        
        return true;
    }

    public async Task<bool> LoadFutureDailyCandlesAsync()
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
            
            logger.Trace($"Загружены свечи по '{instrument.Ticker}' ({instrument.Name})");
        }
        
        return true;
    }

    
    public async Task<bool> LoadIndexesAsync()
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
        
        logger.Trace($"Загружены индикативные инструменты. {indicatives.Count} шт.");
        
        return true;
    }
    
    public async Task<bool> LoadIndexLastPricesAsync()
    {
        var indicatives = await instrumentService.GetFinIndexesInWatchlist();
            
        var instrumentIds = indicatives
            .Select(x => x.InstrumentId)
            .ToList();
            
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await indexRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
            
        logger.Trace($"Загружены последние цены по индикативам. {indicatives.Count} шт.");
        
        return true;
    }

    public async Task<bool> LoadIndexDailyCandlesAsync()
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
            
            logger.Trace($"Загружены свечи по '{instrument.Ticker}' ({instrument.Name})");
        }
        
        return true;
    }

    
    public async Task<bool> LoadCurrenciesAsync()
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
        
        logger.Trace($"Загружены валюты. {currencies.Count} шт.");
        
        return true;
    }

    public async Task<bool> LoadCurrencyLastPricesAsync()
    {
        var currencies = await instrumentService.GetCurrenciesInWatchlist();
            
        var instrumentIds = currencies
            .Select(x => x.InstrumentId)
            .ToList();
            
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await currencyRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
            
        logger.Trace($"Загружены последние цены по валютам. {currencies.Count} шт.");
        
        return true;
    }

    public async Task<bool> LoadCurrencyDailyCandlesAsync()
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
            
            logger.Trace($"Загружены свечи по '{instrument.Ticker}' ({instrument.Name})");
        }
        
        return true;
    }

    
    public async Task<bool> LoadAssetFundamentalsAsync()
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
            
        logger.Trace($"Загружены фундаментальные данные. {assetFundamentals.Count} шт.");
        
        return true;
    }

    
    public async Task<bool> LoadDividendInfosAsync()
    {
        var shares = await instrumentService.GetSharesInWatchlist();
        var dividendInfos = await tinkoffService.GetDividendInfoAsync(shares);
        await dividendInfoRepository.AddOrUpdateAsync(dividendInfos);
            
        logger.Trace($"Загружена информация по дивидендам. {dividendInfos.Count} шт.");
        
        return true;
    }
    
    public async Task<bool> LoadBondsAsync()
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
        
        logger.Trace($"Загружены облигации. {bonds.Count} шт.");
        
        return true;
    }
    
    public async Task<bool> LoadBondLastPricesAsync()
    {
        var bonds = await instrumentService.GetBondsInWatchlist();
            
        var instrumentIds = bonds
            .Select(x => x.InstrumentId)
            .ToList();
            
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await bondRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
            
        logger.Trace($"Загружены последние цены по облигациям. {bonds.Count} шт.");
        
        return true;
    }

    public async Task<bool> LoadBondDailyCandlesAsync()
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
            
            logger.Trace($"Загружены свечи по '{instrument.Ticker}' ({instrument.Name})");
        }
        
        return true;
    }

    public async Task<bool> LoadBondCouponsAsync()
    {
        var bonds = await instrumentService.GetBondsByFilter();
        var bondCoupons = await tinkoffService.GetBondCouponsAsync(bonds);
        await bondCouponRepository.AddAsync(bondCoupons);
            
        logger.Trace($"Загружена информация по купонам облигаций. {bondCoupons.Count} шт.");

        return true;
    }
}
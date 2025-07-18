﻿using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Domain.Mapping;
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
    ITickerListUtilService tickerListUtilService,
    IDailyCandleRepository dailyCandleRepository,
    IHourlyCandleRepository hourlyCandleRepository,
    IDividendInfoRepository dividendInfoRepository,
    IBondCouponRepository bondCouponRepository,
    IInstrumentRepository instrumentRepository,
    IForecastTargetRepository forecastTargetRepository,
    IForecastConsensusRepository forecastConsensusRepository,
    IAssetReportEventRepository assetReportEventRepository)
    : ILoadService
{
    public async Task LoadSharesAsync()
    {
        var shares = await tinkoffService.GetSharesAsync();
        await shareRepository.AddAsync(shares);
        var instruments = shares.Select(DomainMapper.Map).ToList();
        await instrumentRepository.AddOrUpdateAsync(instruments);
    }

    public async Task LoadShareLastPricesAsync()
    {
        var shares = await tickerListUtilService.GetAllSharesInTickerListsAsync();
        var instrumentIds = shares.Select(x => x.InstrumentId).ToList();
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await shareRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
    }

    public async Task LoadShareDailyCandlesAsync()
    {
        var instruments = await tickerListUtilService.GetAllSharesInTickerListsAsync();

        foreach (var instrument in instruments)
        {
            var lastCandle = await dailyCandleRepository.GetLastAsync(instrument.InstrumentId);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int years = 5;

                for (int year = currentYear - years; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetDailyCandlesAsync(instrument.InstrumentId, year);
                    await dailyCandleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetDailyCandlesAsync(
                    instrument.InstrumentId, lastCandle.Date, DateOnly.FromDateTime(DateTime.Today).AddDays(1));
                await dailyCandleRepository.AddOrUpdateAsync(candles);
            }
        }
    }

    public async Task LoadShareHourlyCandlesAsync()
    {
        var instruments = await tickerListUtilService.GetAllSharesInTickerListsAsync();

        foreach (var instrument in instruments)
        {
            var lastCandle = await hourlyCandleRepository.GetLastAsync(instrument.InstrumentId);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int years = 5;
                    
                for (int year = currentYear - years; year <= currentYear; year++)
                {
                    for (int month = 1; month <= 12; month++)
                    {
                        var from = new DateOnly(year, month, 1);
                        var to = from.AddDays(31);
                    
                        var candles = await tinkoffService.GetHourlyCandlesAsync(instrument.InstrumentId, from, to);
                        await hourlyCandleRepository.AddOrUpdateAsync(candles);
                    }
                }
            }

            else
            {
                var candles = await tinkoffService.GetHourlyCandlesAsync(
                    instrument.InstrumentId, lastCandle.Date, DateOnly.FromDateTime(DateTime.Today).AddDays(1));
                await hourlyCandleRepository.AddOrUpdateAsync(candles);
            }
        }
    }

    public async Task LoadForecastsAsync()
    {
        var instruments = await tickerListUtilService.GetAllSharesInTickerListsAsync();

        foreach (var instrument in instruments)
        {
            var (targets, consensus) = await tinkoffService.GetForecastAsync(instrument.InstrumentId);
            await forecastTargetRepository.AddAsync(targets);
            await forecastConsensusRepository.AddAsync([consensus]);
        }
    }

    public async Task LoadAssetReportEventsAsync()
    {
        var shares = await tickerListUtilService.GetAllSharesInTickerListsAsync();
        var instrumentIds = shares.Select(x => x.InstrumentId).ToList();
        var assetReportEvents = await tinkoffService.GetAssetReportEventsAsync(instrumentIds);
        await assetReportEventRepository.AddAsync(assetReportEvents);
    }
    
    public async Task LoadFuturesAsync()
    {
        var futures = await tinkoffService.GetFuturesAsync();
        await futureRepository.AddAsync(futures);
        var instruments = futures.Select(DomainMapper.Map).ToList();
        await instrumentRepository.AddOrUpdateAsync(instruments);
    }

    public async Task LoadFutureLastPricesAsync()
    {
        var futures = await tickerListUtilService.GetAllFuturesInTickerListsAsync();
        var instrumentIds = futures.Select(x => x.InstrumentId).ToList();
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await futureRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
    }

    public async Task LoadFutureDailyCandlesAsync()
    {
        var instruments = await tickerListUtilService.GetAllFuturesInTickerListsAsync();

        foreach (var instrument in instruments)
        {
            var lastCandle = await dailyCandleRepository.GetLastAsync(instrument.InstrumentId);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int years = 3;

                for (int year = currentYear - years; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetDailyCandlesAsync(instrument.InstrumentId, year);
                    await dailyCandleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetDailyCandlesAsync(
                    instrument.InstrumentId, lastCandle.Date, DateOnly.FromDateTime(DateTime.Today));
                await dailyCandleRepository.AddOrUpdateAsync(candles);
            }
        }
    }

    public async Task LoadIndexesAsync()
    {
        var indexes = await tinkoffService.GetIndexesAsync();
        await indexRepository.AddAsync(indexes);
        var instruments = indexes.Select(DomainMapper.Map).ToList();
        await instrumentRepository.AddOrUpdateAsync(instruments);
    }
    
    public async Task LoadIndexLastPricesAsync()
    {
        var indexes = await tickerListUtilService.GetAllIndexesInTickerListsAsync();
        var instrumentIds = indexes.Select(x => x.InstrumentId).ToList();
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await indexRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
    }

    public async Task LoadIndexDailyCandlesAsync()
    {
        var instruments = await tickerListUtilService.GetAllIndexesInTickerListsAsync();

        foreach (var instrument in instruments)
        {
            var lastCandle = await dailyCandleRepository.GetLastAsync(instrument.InstrumentId);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int years = 3;

                for (int year = currentYear - years; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetDailyCandlesAsync(instrument.InstrumentId, year);
                    await dailyCandleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetDailyCandlesAsync(
                    instrument.InstrumentId, lastCandle.Date, DateOnly.FromDateTime(DateTime.Today));
                await dailyCandleRepository.AddOrUpdateAsync(candles);
            }
        }
    }

    
    public async Task LoadCurrenciesAsync()
    {
        var currencies = await tinkoffService.GetCurrenciesAsync();
        await currencyRepository.AddAsync(currencies);
        var instruments = currencies.Select(DomainMapper.Map).ToList();
        await instrumentRepository.AddOrUpdateAsync(instruments);
    }

    public async Task LoadCurrencyLastPricesAsync()
    {
        var currencies = await tickerListUtilService.GetAllCurrenciesInTickerListsAsync();
        var instrumentIds = currencies.Select(x => x.InstrumentId).ToList();
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await currencyRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
    }

    public async Task LoadCurrencyDailyCandlesAsync()
    {
        var instruments = await tickerListUtilService.GetAllCurrenciesInTickerListsAsync();

        foreach (var instrument in instruments)
        {
            var lastCandle = await dailyCandleRepository.GetLastAsync(instrument.InstrumentId);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int years = 5;

                for (int year = currentYear - years; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetDailyCandlesAsync(instrument.InstrumentId, year);
                    await dailyCandleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetDailyCandlesAsync(
                    instrument.InstrumentId, lastCandle.Date, DateOnly.FromDateTime(DateTime.Today));
                await dailyCandleRepository.AddOrUpdateAsync(candles);
            }
        }
    }

    public async Task<bool> LoadHistoryShareDailyCandlesAsync()
    {
        int currentYear = DateTime.Now.Year;
        const int years = 5;
        
        var instruments = await tickerListUtilService.GetAllSharesInTickerListsAsync();

        int count = 0;
        
        foreach (var instrument in instruments)
        {
            count++;
            
            for (int year = currentYear - years; year <= currentYear; year++)
            {
                var candles = await tinkoffService.GetDailyCandlesAsync(instrument.InstrumentId, year);
                await dailyCandleRepository.AddOrUpdateAsync(candles);
            }
            
            logger.Info($"Загружены свечи по инструменту '{instrument.Ticker}'. {count} из {instruments.Count}");
        }

        return true;
    }

    public async Task<bool> LoadHistoryShareHourlyCandlesAsync()
    {
        int currentYear = DateTime.Now.Year;
        const int years = 5;
        
        var instruments = await tickerListUtilService.GetAllSharesInTickerListsAsync();
        
        int count = 0;
        
        foreach (var instrument in instruments)
        {
            count++;
            
            for (int year = currentYear - years; year <= currentYear; year++)
            {
                for (int month = 1; month <= 12; month++)
                {
                    var from = new DateOnly(year, month, 1);
                    var to = from.AddDays(31);
                    
                    var candles = await tinkoffService.GetHourlyCandlesAsync(instrument.InstrumentId, from, to);
                    await hourlyCandleRepository.AddOrUpdateAsync(candles);
                }
            }
            
            logger.Info($"Загружены свечи по инструменту '{instrument.Ticker}'. {count} из {instruments.Count}");
        }

        return true;
    }
    
    public async Task LoadDividendInfosAsync()
    {
        var shares = await tickerListUtilService.GetAllSharesInTickerListsAsync();
        var dividendInfos = await tinkoffService.GetDividendInfoAsync(shares);
        await dividendInfoRepository.AddOrUpdateAsync(dividendInfos);
    }
    
    public async Task LoadBondsAsync()
    {
        var bonds = await tinkoffService.GetBondsAsync();
        await bondRepository.AddAsync(bonds);
        var instruments = bonds.Select(DomainMapper.Map).ToList();
        await instrumentRepository.AddOrUpdateAsync(instruments);
    }
    
    public async Task LoadBondLastPricesAsync()
    {
        var bonds = await tickerListUtilService.GetAllBondsInTickerListsAsync();
        var instrumentIds = bonds.Select(x => x.InstrumentId).ToList();
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await bondRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
    }

    public async Task LoadBondDailyCandlesAsync()
    {
        var instruments = await tickerListUtilService.GetAllBondsInTickerListsAsync();

        foreach (var instrument in instruments)
        {
            var lastCandle = await dailyCandleRepository.GetLastAsync(instrument.InstrumentId);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int years = 3;

                for (int year = currentYear - years; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetDailyCandlesAsync(instrument.InstrumentId, year);
                    await dailyCandleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetDailyCandlesAsync(
                    instrument.InstrumentId, lastCandle.Date, DateOnly.FromDateTime(DateTime.Today));
                await dailyCandleRepository.AddOrUpdateAsync(candles);
            }
        }
    }

    public async Task LoadBondCouponsAsync()
    {
        var bonds = await tickerListUtilService.GetAllBondsInTickerListsAsync();
        var bondCoupons = await tinkoffService.GetBondCouponsAsync(bonds);
        await bondCouponRepository.AddAsync(bondCoupons);
    }
}
using Oid85.FinMarket.Application.Interfaces.Repositories;
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
    IIndicativeRepository indicativeRepository,
    ICurrencyRepository currencyRepository,
    ICandleRepository candleRepository,
    IDividendInfoRepository dividendInfoRepository,
    IBondCouponRepository bondCouponRepository)
    : ILoadService
{
    public async Task LoadStocksAsync()
    {
        var shares = await tinkoffService.GetSharesAsync();
        await shareRepository.AddOrUpdateAsync(shares);
            
        await logService.LogTrace($"Загружены акции. {shares.Count} шт.");
    }

    public async Task LoadStockPricesAsync()
    {
        var shares = await shareRepository.GetWatchListAsync();
            
        var instrumentIds = shares.Select(x => x.Uid).ToList();
            
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < shares.Count; i++) 
            shares[i].Price = lastPrices[i];
            
        await shareRepository.AddOrUpdateAsync(shares);
            
        await logService.LogTrace($"Загружены последние цены по акциям. {shares.Count} шт.");
    }

    public async Task LoadStockDailyCandlesAsync()
    {
        var instruments = await shareRepository.GetWatchListAsync();

        foreach (var instrument in instruments)
        {
            var lastCandle = await candleRepository.GetLastAsync(
                instrument.Ticker, KnownTimeframes.Daily);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int historyInYears = 3;

                for (int year = currentYear - historyInYears; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetCandlesAsync(
                        instrument.Uid, instrument.Ticker, KnownTimeframes.Daily, year);
                    
                    await candleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetCandlesAsync(
                    instrument.Uid, instrument.Ticker, KnownTimeframes.Daily);
                    
                await candleRepository.AddOrUpdateAsync(candles);
            }
        }
    }
    
    public async Task LoadFuturesAsync()
    {
        var futures = await tinkoffService.GetFuturesAsync();
        await futureRepository.AddOrUpdateAsync(futures);
            
        await logService.LogTrace($"Загружены фьючерсы. {futures.Count} шт.");
    }

    public async Task LoadFuturePricesAsync()
    {
        var futures = await futureRepository.GetWatchListAsync();
            
        var instrumentIds = futures.Select(x => x.Uid).ToList();
            
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < futures.Count; i++) 
            futures[i].Price = lastPrices[i];
            
        await futureRepository.AddOrUpdateAsync(futures);
            
        await logService.LogTrace($"Загружены последние цены по фьючерсам. {futures.Count} шт.");
    }

    public async Task LoadFutureDailyCandlesAsync()
    {
        var instruments = await futureRepository.GetWatchListAsync();

        foreach (var instrument in instruments)
        {
            var lastCandle = await candleRepository.GetLastAsync(
                instrument.Ticker, KnownTimeframes.Daily);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int historyInYears = 3;

                for (int year = currentYear - historyInYears; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetCandlesAsync(
                        instrument.Uid, instrument.Ticker, KnownTimeframes.Daily, year);
                    
                    await candleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetCandlesAsync(
                    instrument.Uid, instrument.Ticker, KnownTimeframes.Daily);
                    
                await candleRepository.AddOrUpdateAsync(candles);
            }
        }
    }

    public async Task LoadIndicativePricesAsync()
    {
        var indicatives = await indicativeRepository.GetWatchListAsync();
            
        var instrumentIds = indicatives.Select(x => x.Uid).ToList();
            
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < indicatives.Count; i++) 
            indicatives[i].Price = lastPrices[i];
            
        await indicativeRepository.AddOrUpdateAsync(indicatives);
            
        await logService.LogTrace($"Загружены последние цены по индикативам. {indicatives.Count} шт.");
    }

    public async Task LoadIndicativeDailyCandlesAsync()
    {
        var instruments = await indicativeRepository.GetWatchListAsync();

        foreach (var instrument in instruments)
        {
            var lastCandle = await candleRepository.GetLastAsync(
                instrument.Ticker, KnownTimeframes.Daily);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int historyInYears = 3;

                for (int year = currentYear - historyInYears; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetCandlesAsync(
                        instrument.Uid, instrument.Ticker, KnownTimeframes.Daily, year);
                    
                    await candleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetCandlesAsync(
                    instrument.Uid, instrument.Ticker, KnownTimeframes.Daily);
                    
                await candleRepository.AddOrUpdateAsync(candles);
            }
        }
    }

    public async Task LoadCurrenciesAsync()
    {
        var currencies = await tinkoffService.GetCurrenciesAsync();
        await currencyRepository.AddOrUpdateAsync(currencies);
            
        await logService.LogTrace($"Загружены валюты. {currencies.Count} шт.");
    }

    public async Task LoadCurrencyPricesAsync()
    {
        var currencies = await currencyRepository.GetWatchListAsync();
            
        var instrumentIds = currencies.Select(x => x.Uid).ToList();
            
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < currencies.Count; i++) 
            currencies[i].Price = lastPrices[i];
            
        await currencyRepository.AddOrUpdateAsync(currencies);
            
        await logService.LogTrace($"Загружены последние цены по валютам. {currencies.Count} шт.");
    }

    public async Task LoadBondsAsync()
    {
        var bonds = await tinkoffService.GetBondsAsync();
        await bondRepository.AddOrUpdateAsync(bonds);
            
        await logService.LogTrace($"Загружены облигации. {bonds.Count} шт.");
    }

    public async Task LoadIndicativesAsync()
    {
        var indicatives = await tinkoffService.GetIndicativesAsync();
        await indicativeRepository.AddOrUpdateAsync(indicatives);
            
        await logService.LogTrace($"Загружены индикативные инструменты. {indicatives.Count} шт.");
    }
    
    public async Task LoadBondPricesAsync()
    {
        var bonds = await bondRepository.GetWatchListAsync();
            
        var instrumentIds = bonds.Select(x => x.Uid).ToList();
            
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < bonds.Count; i++) 
            bonds[i].Price = lastPrices[i];
            
        await bondRepository.AddOrUpdateAsync(bonds);
            
        await logService.LogTrace($"Загружены последние цены по облигациям. {bonds.Count} шт.");
    }

    public async Task LoadBondDailyCandlesAsync()
    {
        var instruments = await bondRepository.GetWatchListAsync();

        foreach (var instrument in instruments)
        {
            var lastCandle = await candleRepository.GetLastAsync(
                instrument.Ticker, KnownTimeframes.Daily);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int historyInYears = 3;

                for (int year = currentYear - historyInYears; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetCandlesAsync(
                        instrument.Uid, instrument.Ticker, KnownTimeframes.Daily, year);
                    
                    await candleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetCandlesAsync(
                    instrument.Uid, instrument.Ticker, KnownTimeframes.Daily);
                    
                await candleRepository.AddOrUpdateAsync(candles);
            }
        }
    }

    public async Task LoadDividendInfosAsync()
    {
        var shares = await shareRepository.GetWatchListAsync();
        var dividendInfos = await tinkoffService.GetDividendInfoAsync(shares);
        await dividendInfoRepository.AddOrUpdateAsync(dividendInfos);
            
        await logService.LogTrace($"Загружена информация по дивидендам. {dividendInfos.Count} шт.");
    }

    public async Task LoadBondCouponsAsync()
    {
        var bonds = await bondRepository.GetWatchListAsync();
        var bondCoupons = await tinkoffService.GetBondCouponsAsync(bonds);
        await bondCouponRepository.AddOrUpdateAsync(bondCoupons);
            
        await logService.LogTrace($"Загружена информация по купонам облигаций. {bondCoupons.Count} шт.");
    }
}
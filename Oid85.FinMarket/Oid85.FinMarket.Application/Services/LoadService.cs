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
    IIndexRepository indexRepository,
    ICurrencyRepository currencyRepository,
    ICandleRepository candleRepository,
    IDividendInfoRepository dividendInfoRepository,
    IBondCouponRepository bondCouponRepository,
    IAssetFundamentalRepository assetFundamentalRepository,
    IInstrumentRepository instrumentRepository)
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
        var shares = await shareRepository.GetWatchListAsync();
            
        var instrumentIds = shares.Select(x => x.InstrumentId).ToList();
            
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await shareRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
            
        await logService.LogTrace($"Загружены последние цены по акциям. {shares.Count} шт.");
    }

    public async Task LoadShareDailyCandlesAsync()
    {
        var instruments = await shareRepository.GetWatchListAsync();

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
                    instrument.InstrumentId);
                    
                await candleRepository.AddOrUpdateAsync(candles);
            }
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
        var futures = await futureRepository.GetWatchListAsync();
            
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
        var instruments = await futureRepository.GetWatchListAsync();

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
                    instrument.InstrumentId);
                    
                await candleRepository.AddOrUpdateAsync(candles);
            }
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
        var indicatives = await indexRepository.GetWatchListAsync();
            
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
        var instruments = await indexRepository.GetWatchListAsync();

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
                    instrument.InstrumentId);
                    
                await candleRepository.AddOrUpdateAsync(candles);
            }
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
        var currencies = await currencyRepository.GetWatchListAsync();
            
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
        var instruments = await currencyRepository.GetWatchListAsync();

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
                    instrument.InstrumentId);
                    
                await candleRepository.AddOrUpdateAsync(candles);
            }
        }
    }

    
    public async Task LoadAssetFundamentalsAsync()
    {
        var shares = await shareRepository.GetWatchListAsync();

        var instrumentIds = new List<Guid>();

        // Загружаем данные, которых нет
        foreach (var share in shares)
        {
            var assetFundamental = await assetFundamentalRepository
                .GetLastAsync(share.InstrumentId);
            
            if (assetFundamental is not null)
                if (assetFundamental.Date < DateOnly.FromDateTime(DateTime.Today))
                    instrumentIds.Add(share.InstrumentId);
        }

        var assetFundamentals = await tinkoffService
            .GetAssetFundamentalsAsync(instrumentIds);
        
        await assetFundamentalRepository.AddOrUpdateAsync(assetFundamentals);
            
        await logService.LogTrace($"Загружены фундаментальные данные. {assetFundamentals.Count} шт.");
    }

    
    public async Task LoadDividendInfosAsync()
    {
        var shares = await shareRepository.GetWatchListAsync();
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
        var bonds = await bondRepository.GetWatchListAsync();
            
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
        var instruments = await bondRepository.GetWatchListAsync();

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
                    instrument.InstrumentId);
                    
                await candleRepository.AddOrUpdateAsync(candles);
            }
        }
    }

    public async Task LoadBondCouponsAsync()
    {
        var bonds = await bondRepository.GetWatchListAsync();
        var bondCoupons = await tinkoffService.GetBondCouponsAsync(bonds);
        await bondCouponRepository.AddOrUpdateAsync(bondCoupons);
            
        await logService.LogTrace($"Загружена информация по купонам облигаций. {bondCoupons.Count} шт.");
    }
}
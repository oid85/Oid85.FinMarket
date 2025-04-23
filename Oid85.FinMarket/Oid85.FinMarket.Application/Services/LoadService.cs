using NLog;
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
    ISpreadRepository spreadRepository,
    IBondRepository bondRepository,
    IIndexRepository indexRepository,
    ICurrencyRepository currencyRepository,
    ITickerListUtilService tickerListUtilService,
    ICandleRepository candleRepository,
    IFiveMinuteCandleRepository fiveMinuteCandleRepository,
    IDividendInfoRepository dividendInfoRepository,
    IBondCouponRepository bondCouponRepository,
    IInstrumentRepository instrumentRepository,
    IForecastTargetRepository forecastTargetRepository,
    IForecastConsensusRepository forecastConsensusRepository,
    IAssetReportEventRepository assetReportEventRepository)
    : ILoadService
{
    public async Task<bool> LoadSharesAsync()
    {
        var shares = await tinkoffService.GetSharesAsync();
        await shareRepository.AddAsync(shares);
        var instruments = shares.Select(DomainMapper.Map).ToList();
        await instrumentRepository.AddOrUpdateAsync(instruments);

        return true;
    }

    public async Task<bool> LoadShareLastPricesAsync()
    {
        var shares = await tickerListUtilService.GetAllSharesInTickerListsAsync();
        var instrumentIds = shares.Select(x => x.InstrumentId).ToList();
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await shareRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
        
        return true;
    }

    public async Task<bool> LoadShareDailyCandlesAsync()
    {
        var instruments = await tickerListUtilService.GetAllSharesInTickerListsAsync();

        foreach (var instrument in instruments)
        {
            var lastCandle = await candleRepository.GetLastAsync(instrument.InstrumentId);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int years = 3;

                for (int year = currentYear - years; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetDailyCandlesAsync(instrument.InstrumentId, year);
                    await candleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetDailyCandlesAsync(
                    instrument.InstrumentId, lastCandle.Date, DateOnly.FromDateTime(DateTime.Today));
                await candleRepository.AddOrUpdateAsync(candles);
            }
        }
        
        return true;
    }

    public async Task<bool> LoadShareFiveMinuteCandlesAsync()
    {
        var instruments = await tickerListUtilService.GetAllSharesInTickerListsAsync();

        foreach (var instrument in instruments)
        {
            var candles = await tinkoffService.GetFiveMinuteCandlesAsync(
                instrument.InstrumentId, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
            
            logger.Trace($"Загрузка {candles.Count} свечей 5min по инструменту '{instrument.Name}' ({instrument.InstrumentId})");
            
            await fiveMinuteCandleRepository.AddOrUpdateAsync(candles);
        }
        
        return true;
    }

    public async Task<bool> LoadForecastsAsync()
    {
        var instruments = await tickerListUtilService.GetAllSharesInTickerListsAsync();

        foreach (var instrument in instruments)
        {
            var (targets, consensus) = await tinkoffService.GetForecastAsync(instrument.InstrumentId);
            await forecastTargetRepository.AddAsync(targets);
            await forecastConsensusRepository.AddAsync([consensus]);
        }
        
        return true;
    }

    public async Task<bool> LoadAssetReportEventsAsync()
    {
        var shares = await tickerListUtilService.GetAllSharesInTickerListsAsync();
        var instrumentIds = shares.Select(x => x.InstrumentId).ToList();
        var assetReportEvents = await tinkoffService.GetAssetReportEventsAsync(instrumentIds);
        await assetReportEventRepository.AddAsync(assetReportEvents);
        
        return true;
    }
    
    public async Task<bool> LoadFuturesAsync()
    {
        var futures = await tinkoffService.GetFuturesAsync();
        await futureRepository.AddAsync(futures);
        var instruments = futures.Select(DomainMapper.Map).ToList();
        await instrumentRepository.AddOrUpdateAsync(instruments);
        
        return true;
    }

    public async Task<bool> LoadFutureLastPricesAsync()
    {
        var futures = await tickerListUtilService.GetAllFuturesInTickerListsAsync();
        var instrumentIds = futures.Select(x => x.InstrumentId).ToList();
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await futureRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
        
        return true;
    }

    public async Task<bool> LoadFutureDailyCandlesAsync()
    {
        var instruments = await tickerListUtilService.GetAllFuturesInTickerListsAsync();

        foreach (var instrument in instruments)
        {
            var lastCandle = await candleRepository.GetLastAsync(instrument.InstrumentId);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int years = 3;

                for (int year = currentYear - years; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetDailyCandlesAsync(instrument.InstrumentId, year);
                    await candleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetDailyCandlesAsync(
                    instrument.InstrumentId, lastCandle.Date, DateOnly.FromDateTime(DateTime.Today));
                await candleRepository.AddOrUpdateAsync(candles);
            }
        }
        
        return true;
    }

    public async Task<bool> LoadSpreadLastPricesAsync()
    {
        var spreads = await spreadRepository.GetAllAsync();
        var firstInstrumentIds = spreads.Select(x => x.FirstInstrumentId).Distinct().ToList();
        var secondInstrumentIds = spreads.Select(x => x.SecondInstrumentId).Distinct().ToList();
        var firstLastPrices = await tinkoffService.GetPricesAsync(firstInstrumentIds);
        var secondLastPrices = await tinkoffService.GetPricesAsync(secondInstrumentIds);

        for (int i = 0; i < firstLastPrices.Count; i++) 
            await spreadRepository.UpdateLastPricesAsync(firstInstrumentIds[i], firstLastPrices[i]);
            
        for (int i = 0; i < secondLastPrices.Count; i++) 
            await spreadRepository.UpdateLastPricesAsync(secondInstrumentIds[i], secondLastPrices[i]);            
            
        return true;
    }


    public async Task<bool> LoadIndexesAsync()
    {
        var indexes = await tinkoffService.GetIndexesAsync();
        await indexRepository.AddAsync(indexes);
        var instruments = indexes.Select(DomainMapper.Map).ToList();
        await instrumentRepository.AddOrUpdateAsync(instruments);
            
        return true;
    }
    
    public async Task<bool> LoadIndexLastPricesAsync()
    {
        var indexes = await tickerListUtilService.GetAllIndexesInTickerListsAsync();
        var instrumentIds = indexes.Select(x => x.InstrumentId).ToList();
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await indexRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);

        return true;
    }

    public async Task<bool> LoadIndexDailyCandlesAsync()
    {
        var instruments = await tickerListUtilService.GetAllIndexesInTickerListsAsync();

        foreach (var instrument in instruments)
        {
            var lastCandle = await candleRepository.GetLastAsync(instrument.InstrumentId);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int years = 3;

                for (int year = currentYear - years; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetDailyCandlesAsync(instrument.InstrumentId, year);
                    await candleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetDailyCandlesAsync(
                    instrument.InstrumentId, lastCandle.Date, DateOnly.FromDateTime(DateTime.Today));
                await candleRepository.AddOrUpdateAsync(candles);
            }
        }
        
        return true;
    }

    
    public async Task<bool> LoadCurrenciesAsync()
    {
        var currencies = await tinkoffService.GetCurrenciesAsync();
        await currencyRepository.AddAsync(currencies);
        var instruments = currencies.Select(DomainMapper.Map).ToList();
        await instrumentRepository.AddOrUpdateAsync(instruments);
            
        return true;
    }

    public async Task<bool> LoadCurrencyLastPricesAsync()
    {
        var currencies = await tickerListUtilService.GetAllCurrenciesInTickerListsAsync();
        var instrumentIds = currencies.Select(x => x.InstrumentId).ToList();
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await currencyRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);

        return true;
    }

    public async Task<bool> LoadCurrencyDailyCandlesAsync()
    {
        var instruments = await tickerListUtilService.GetAllCurrenciesInTickerListsAsync();

        foreach (var instrument in instruments)
        {
            var lastCandle = await candleRepository.GetLastAsync(instrument.InstrumentId);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int years = 3;

                for (int year = currentYear - years; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetDailyCandlesAsync(instrument.InstrumentId, year);
                    await candleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetDailyCandlesAsync(
                    instrument.InstrumentId, lastCandle.Date, DateOnly.FromDateTime(DateTime.Today));
                await candleRepository.AddOrUpdateAsync(candles);
            }
        }
        
        return true;
    }

    public async Task<bool> LoadDividendInfosAsync()
    {
        var shares = await tickerListUtilService.GetAllSharesInTickerListsAsync();
        var dividendInfos = await tinkoffService.GetDividendInfoAsync(shares);
        await dividendInfoRepository.AddOrUpdateAsync(dividendInfos);
            
        return true;
    }
    
    public async Task<bool> LoadBondsAsync()
    {
        var bonds = await tinkoffService.GetBondsAsync();
        await bondRepository.AddAsync(bonds);
        var instruments = bonds.Select(DomainMapper.Map).ToList();
        await instrumentRepository.AddOrUpdateAsync(instruments);
            
        return true;
    }
    
    public async Task<bool> LoadBondLastPricesAsync()
    {
        var bonds = await tickerListUtilService.GetAllBondsInTickerListsAsync();
        var instrumentIds = bonds.Select(x => x.InstrumentId).ToList();
        var lastPrices = await tinkoffService.GetPricesAsync(instrumentIds);

        for (int i = 0; i < instrumentIds.Count; i++) 
            await bondRepository.UpdateLastPricesAsync(instrumentIds[i], lastPrices[i]);
            
        return true;
    }

    public async Task<bool> LoadBondDailyCandlesAsync()
    {
        var instruments = await tickerListUtilService.GetAllBondsInTickerListsAsync();

        foreach (var instrument in instruments)
        {
            var lastCandle = await candleRepository.GetLastAsync(instrument.InstrumentId);

            if (lastCandle is null)
            {
                int currentYear = DateTime.Now.Year;
                const int years = 3;

                for (int year = currentYear - years; year <= currentYear; year++)
                {
                    var candles = await tinkoffService.GetDailyCandlesAsync(instrument.InstrumentId, year);
                    await candleRepository.AddOrUpdateAsync(candles);
                }
            }

            else
            {
                var candles = await tinkoffService.GetDailyCandlesAsync(
                    instrument.InstrumentId, lastCandle.Date, DateOnly.FromDateTime(DateTime.Today));
                await candleRepository.AddOrUpdateAsync(candles);
            }
        }
        
        return true;
    }

    public async Task<bool> LoadBondCouponsAsync()
    {
        var bonds = await tickerListUtilService.GetAllBondsInTickerListsAsync();
        var bondCoupons = await tinkoffService.GetBondCouponsAsync(bonds);
        await bondCouponRepository.AddAsync(bondCoupons);
            
        return true;
    }
}
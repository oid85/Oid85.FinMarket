using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.ResourceStore;
using Oid85.FinMarket.Logging.Services;

namespace Oid85.FinMarket.Application.Services;

public class SpreadService(
    ILogService logService,
    IInstrumentRepository instrumentRepository,
    ISpreadRepository spreadRepository,
    IShareRepository shareRepository,
    IFutureRepository futureRepository,
    ICurrencyRepository currencyRepository,
    IResourceStoreService resourceStoreService) 
    : ISpreadService
{
    public async Task<List<Spread>> FillingSpreadPairsAsync()
    {
        await DeleteExpiratedFuturesAsync();

        var spreadResources = await resourceStoreService.GetSpreadsAsync();
        
        var spreads = new List<Spread>();

        foreach (var resource in spreadResources)
            spreads.AddRange(await GetSpreadsAsync(
                resource.BaseAssetTicker, 
                resource.Multiplier, 
                resource.ForeverFutureTicker, 
                resource.FutureTickerPrefix));
        
        await spreadRepository.AddAsync(spreads);
        
        return spreads;
    }
    
    public async Task<List<Spread>> CalculateSpreadsAsync()
    {
        var spreads = await spreadRepository.GetAllAsync();

        foreach (var spread in spreads)
        {
            spread.FirstInstrumentPrice = await GetInstrumentPriceAsync(spread.FirstInstrumentId);
            spread.SecondInstrumentPrice = await GetInstrumentPriceAsync(spread.SecondInstrumentId);

            if (spread is not
                {
                    FirstInstrumentPrice: > 0.0,
                    SecondInstrumentPrice: > 0.0
                }) 
                continue;

            // В зависимости от порядка цен, домножаем на коэффициент
            spread.PriceDifference = spread.SecondInstrumentPrice / spread.FirstInstrumentPrice > 2.0
                ? spread.SecondInstrumentPrice - spread.FirstInstrumentPrice * spread.Multiplier
                : spread.SecondInstrumentPrice - spread.FirstInstrumentPrice;
            
            spread.PriceDifferencePrc = spread.PriceDifference / spread.SecondInstrumentPrice * 100.0; 
            
            spread.DateTime = DateTime.UtcNow;
            
            /*
             Контанго
                Цена контракта > цены базового актива
                Цена дальнего контракта > цены ближнего контракта
            */
            
            spread.SpreadPricePosition = 
                spread.PriceDifference > 0.0 
                    ? KnownSpreadPricePositions.Contango
                    : KnownSpreadPricePositions.Backwardation;
            
            await spreadRepository.UpdateSpreadAsync(spread);
        }
        
        return spreads;
    }
    
    private async Task DeleteExpiratedFuturesAsync()
    {
        var spreads = await spreadRepository.GetAllAsync();

        if (spreads is [])
            return;
        
        foreach (var spread in spreads)
        {
            var firstFuture = await futureRepository.GetByTickerAsync(spread.FirstInstrumentTicker);
            var secondFuture = await futureRepository.GetByTickerAsync(spread.SecondInstrumentTicker);

            if ((firstFuture is not null && 
                 firstFuture.ExpirationDate <= DateOnly.FromDateTime(DateTime.UtcNow.Date)) ||
                (secondFuture is not null && 
                 secondFuture.ExpirationDate <= DateOnly.FromDateTime(DateTime.UtcNow.Date))) 
                await spreadRepository.SetAsDeletedAsync(spread);
        }
    }
    
    private async Task<List<Spread>> GetSpreadsAsync(
        string baseAssetTicker,
        double multiplier,
        string foreverFutureTicker,
        string futureTickerPrefix)
    {
        try
        {
            var futures = (await futureRepository.GetAllAsync())
                .Where(x => x.ExpirationDate > DateOnly.FromDateTime(DateTime.UtcNow.Date))
                .Where(x => x.BasicAsset == baseAssetTicker)
                .Where(x => x.Ticker != foreverFutureTicker)
                .Where(x =>
                    x.Ticker[0] == futureTickerPrefix[0] &&
                    x.Ticker[1] == futureTickerPrefix[1])
                .OrderBy(x => x.ExpirationDate)
                .ToList();

            var baseAssetInstrumentId = GetBaseActiveInstrumentId(baseAssetTicker);

            Guid foreverFutureInstrumentId = Guid.Empty;

            if (!string.IsNullOrEmpty(foreverFutureTicker))
                foreverFutureInstrumentId =
                    (await instrumentRepository.GetByTickerAsync(foreverFutureTicker))!.InstrumentId;

            var spreads = new List<Spread>();

            // Базовый актив - вечный фьючерс
            if (!string.IsNullOrEmpty(foreverFutureTicker))
                spreads.Add(new Spread
                {
                    FirstInstrumentId = baseAssetInstrumentId,
                    FirstInstrumentTicker = baseAssetTicker,
                    FirstInstrumentRole = KnownSpreadRoles.BaseActive,
                    SecondInstrumentId = foreverFutureInstrumentId,
                    SecondInstrumentTicker = foreverFutureTicker,
                    SecondInstrumentRole = KnownSpreadRoles.ForeverFuture,
                    Multiplier = multiplier
                });

            // Вечный фьючерс - фьючерс
            if (!string.IsNullOrEmpty(foreverFutureTicker))
                foreach (var future in futures)
                {
                    spreads.Add(new Spread
                    {
                        FirstInstrumentId = foreverFutureInstrumentId,
                        FirstInstrumentTicker = foreverFutureTicker,
                        FirstInstrumentRole = KnownSpreadRoles.ForeverFuture,
                        SecondInstrumentId = future.InstrumentId,
                        SecondInstrumentTicker = future.Ticker,
                        SecondInstrumentRole = KnownSpreadRoles.Future,
                        Multiplier = multiplier
                    });
                }

            // Базовый актив - фьючерс
            foreach (var future in futures)
            {
                spreads.Add(new Spread
                {
                    FirstInstrumentId = baseAssetInstrumentId,
                    FirstInstrumentTicker = baseAssetTicker,
                    FirstInstrumentRole = KnownSpreadRoles.BaseActive,
                    SecondInstrumentId = future.InstrumentId,
                    SecondInstrumentTicker = future.Ticker,
                    SecondInstrumentRole = KnownSpreadRoles.Future,
                    Multiplier = multiplier
                });
            }

            // Ближний фьючерс - дальний фьючерс
            for (int i = 1; i < futures.Count; i++)
            {
                spreads.Add(new Spread
                {
                    FirstInstrumentId = futures[0].InstrumentId,
                    FirstInstrumentTicker = futures[0].Ticker,
                    FirstInstrumentRole = KnownSpreadRoles.NearFuture,
                    SecondInstrumentId = futures[i].InstrumentId,
                    SecondInstrumentTicker = futures[i].Ticker,
                    SecondInstrumentRole = KnownSpreadRoles.FarFuture,
                    Multiplier = 1
                });
            }

            await logService.LogTrace($"Добавление спредов по базовому активу '{baseAssetTicker}'");
            
            return spreads;
        }
        
        catch (Exception exception)
        {
            await logService.LogException(exception);
            return [];
        }
    }

    private static Guid GetBaseActiveInstrumentId(string ticker) =>
        ticker switch
        {
            "IMOEX" => KnownInstrumentIds.IMoex,
            "CNY/RUB" => KnownInstrumentIds.CnyRub,
            "EUR/USD" => KnownInstrumentIds.EurUsd,
            "USD/RUB" => KnownInstrumentIds.UsdRub,
            _ => Guid.Empty
        };

    private async ValueTask<double> GetInstrumentPriceAsync(Guid instrumentId)
    {
        var share = await shareRepository.GetByInstrumentIdAsync(instrumentId);

        if (share is not null)
            return share.LastPrice;
        
        var future = await futureRepository.GetByInstrumentIdAsync(instrumentId);

        if (future is not null)
            return future.LastPrice;
        
        var currency = await currencyRepository.GetByInstrumentIdAsync(instrumentId);

        if (currency is not null)
            return currency.LastPrice;

        return 0.0;
    }
}
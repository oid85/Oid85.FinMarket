using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Logging.Services;

namespace Oid85.FinMarket.Application.Services;

public class SpreadService(
    ILogService logService,
    IInstrumentRepository instrumentRepository,
    ISpreadRepository spreadRepository,
    IShareRepository shareRepository,
    IFutureRepository futureRepository,
    ICurrencyRepository currencyRepository) 
    : ISpreadService
{
    public async Task<List<Spread>> FillingSpreadPairsAsync()
    {
        await DeleteExpiratedFuturesAsync();
        
        var spreads = new List<Spread>();
        
        spreads.AddRange(await GetSpreadsAsync(
            "IMOEX", "IMOEXF", "MX"));
        spreads.AddRange(await GetSpreadsAsync(
            "CNY/RUB", "CNYRUBF", "CR"));
        spreads.AddRange(await GetSpreadsAsync(
            "EUR/USD", "", "ED"));
        spreads.AddRange(await GetSpreadsAsync(
            "USD/RUB", "USDRUBF", "Si"));
        
        await spreadRepository.AddAsync(spreads);
        
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
                    SecondInstrumentRole = KnownSpreadRoles.ForeverFuture
                });

            // Вечный фьючерс - фьючерс
            if (!string.IsNullOrEmpty(foreverFutureTicker))
                for (int i = 0; i < futures.Count; i++)
                {
                    spreads.Add(new Spread
                    {
                        FirstInstrumentId = foreverFutureInstrumentId,
                        FirstInstrumentTicker = foreverFutureTicker,
                        FirstInstrumentRole = KnownSpreadRoles.ForeverFuture,
                        SecondInstrumentId = futures[i].InstrumentId,
                        SecondInstrumentTicker = futures[i].Ticker,
                        SecondInstrumentRole = KnownSpreadRoles.Future
                    });
                }

            // Базовый актив - фьючерс
            for (int i = 0; i < futures.Count; i++)
            {
                spreads.Add(new Spread
                {
                    FirstInstrumentId = baseAssetInstrumentId,
                    FirstInstrumentTicker = baseAssetTicker,
                    FirstInstrumentRole = KnownSpreadRoles.BaseActive,
                    SecondInstrumentId = futures[i].InstrumentId,
                    SecondInstrumentTicker = futures[i].Ticker,
                    SecondInstrumentRole = KnownSpreadRoles.Future
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
                    SecondInstrumentRole = KnownSpreadRoles.FarFuture
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

    private static Guid GetBaseActiveInstrumentId(string ticker)
    {
        if (ticker == "CNY/RUB")
            return Guid.Parse("4587ab1d-a9c9-4910-a0d6-86c7b9c42510");
        
        if (ticker == "EUR/USD")
            return Guid.Parse("980156e9-4f81-4ee2-af21-895b57a2b1bf");
        
        if (ticker == "USD/RUB")
            return Guid.Parse("a22a1263-8e1b-4546-a1aa-416463f104d3");
        
        return Guid.Empty;
    }
    
    public async Task<List<Spread>> CalculateSpreadsAsync()
    {
        var spreads = await spreadRepository.GetWatchListAsync();

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
            
            spread.PriceDifference = spread.FirstInstrumentPrice - spread.SecondInstrumentPrice;
            spread.PriceDifferencePrc = spread.PriceDifference / spread.FirstInstrumentPrice * 100.0;
            
            spread.DateTime = DateTime.UtcNow;
            
            FillSpreadPricePosition(spread);

            spread.PriceDifferenceAverage = (await spreadRepository.GetAllAsync())
                .Average(x => x.PriceDifference);
            
            spread.PriceDifferenceAveragePrc = (await spreadRepository.GetAllAsync())
                .Average(x => x.PriceDifferencePrc);
            
            await spreadRepository.UpdateSpreadAsync(spread);
        }
        
        return spreads;
    }
    
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

    private void FillSpreadPricePosition(Spread spread)
    {
        /*
         Контанго
            Цена контракта > цены базового актива
            Цена дальнего контракта > цены ближнего контракта
        */

        // Цена контракта выше цены базового актива
        if (spread is
            {
                FirstInstrumentRole: KnownSpreadRoles.BaseActive, 
                SecondInstrumentRole: KnownSpreadRoles.ForeverFuture
            })
        {
            spread.SpreadPricePosition = 
                spread.SecondInstrumentPrice > spread.FirstInstrumentPrice 
                    ? KnownSpreadPricePositions.Contango 
                    : KnownSpreadPricePositions.Backwardation;
        }
        
        // Цена контракта выше цены базового актива
        else if (spread is
                 {
                     FirstInstrumentRole: KnownSpreadRoles.ForeverFuture, 
                     SecondInstrumentRole: KnownSpreadRoles.BaseActive
                 })
        {
            spread.SpreadPricePosition = 
                spread.SecondInstrumentPrice > spread.FirstInstrumentPrice 
                    ? KnownSpreadPricePositions.Contango 
                    : KnownSpreadPricePositions.Backwardation;
        }
        
        // Цена дальнего контракта больше цены ближнего
        else if (spread is
            {
                FirstInstrumentRole: KnownSpreadRoles.NearFuture, 
                SecondInstrumentRole: KnownSpreadRoles.FarFuture
            })
        {
            spread.SpreadPricePosition = 
                spread.SecondInstrumentPrice > spread.FirstInstrumentPrice 
                    ? KnownSpreadPricePositions.Contango 
                    : KnownSpreadPricePositions.Backwardation;
        }
        
        // Цена дальнего контракта больше цены ближнего
        else if (spread is
                 {
                     FirstInstrumentRole: KnownSpreadRoles.FarFuture, 
                     SecondInstrumentRole: KnownSpreadRoles.NearFuture
                 })
        {
            spread.SpreadPricePosition = 
                spread.SecondInstrumentPrice > spread.FirstInstrumentPrice 
                    ? KnownSpreadPricePositions.Contango 
                    : KnownSpreadPricePositions.Backwardation;
        }        
    }
}
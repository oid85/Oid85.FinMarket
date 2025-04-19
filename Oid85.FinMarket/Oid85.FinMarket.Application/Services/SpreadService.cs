using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Services;

public class SpreadService(
    IInstrumentRepository instrumentRepository,
    ISpreadRepository spreadRepository,
    IFutureRepository futureRepository,
    IResourceStoreService resourceStoreService) 
    : ISpreadService
{
    public async Task<List<Spread>> ProcessSpreadPairsAsync()
    {
        await DeleteExpiratedFuturesAsync();
        await FillingSpreadPairsAsync();
        return await CalculateSpreadsAsync();
    }
    
    private async Task DeleteExpiratedFuturesAsync()
    {
        var spreads = await spreadRepository.GetAllAsync();

        if (spreads is [])
            return;
        
        foreach (var spread in spreads)
        {
            var firstFuture = await futureRepository.GetAsync(spread.FirstInstrumentTicker);
            var secondFuture = await futureRepository.GetAsync(spread.SecondInstrumentTicker);

            if ((firstFuture is not null && 
                 firstFuture.ExpirationDate <= DateOnly.FromDateTime(DateTime.UtcNow.Date)) ||
                (secondFuture is not null && 
                 secondFuture.ExpirationDate <= DateOnly.FromDateTime(DateTime.UtcNow.Date))) 
                await spreadRepository.SetAsDeletedAsync(spread);
        }
    }    
    
    private async Task FillingSpreadPairsAsync()
    {
        var spreadResources = await resourceStoreService.GetSpreadsAsync();
        
        var spreads = new List<Spread>();

        foreach (var resource in spreadResources)
            spreads.AddRange(await GetSpreadsAsync(
                resource.BaseAssetTicker, 
                resource.ForeverFutureTicker, 
                resource.FutureTickerPrefix));
        
        await spreadRepository.AddAsync(spreads);
    }
    
    private async Task<List<Spread>> CalculateSpreadsAsync()
    {
        var spreads = await spreadRepository.GetAllAsync();

        foreach (var spread in spreads)
        {
            if (spread is not
                {
                    FirstInstrumentPrice: > 0.0,
                    SecondInstrumentPrice: > 0.0
                }) 
                continue;

            double multiplier = GetMultiplier(spread.FirstInstrumentPrice, spread.SecondInstrumentPrice);
            
            // Рассчитываем спред
            spread.PriceDifference = spread.SecondInstrumentPrice - spread.FirstInstrumentPrice * multiplier;
            spread.PriceDifferencePrc = spread.PriceDifference / spread.SecondInstrumentPrice * 100.0;
            spread.Multiplier = multiplier;
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

    private double GetMultiplier(double firstInstrumentPrice, double secondInstrumentPrice)
    {
        if (secondInstrumentPrice / firstInstrumentPrice > 1500.0)
            return 1000.0;
        
        if (secondInstrumentPrice / firstInstrumentPrice > 150.0)
            return 100.0;
        
        if (secondInstrumentPrice / firstInstrumentPrice > 15.0)
            return 10.0;
        
        if (secondInstrumentPrice / firstInstrumentPrice > 1.5)
            return 1.0;
        
        if (firstInstrumentPrice / secondInstrumentPrice > 1500.0)
            return 1.0 / 1000.0;
        
        if (firstInstrumentPrice / secondInstrumentPrice > 150.0)
            return 1.0 / 100.0;
        
        if (firstInstrumentPrice / secondInstrumentPrice > 15.0)
            return 1.0 / 10.0;
        
        if (firstInstrumentPrice / secondInstrumentPrice > 1.5)
            return 1.0 / 1.0;
        
        return 1.0;
    }

    private async Task<List<Spread>> GetSpreadsAsync(
        string baseAssetTicker,
        string foreverFutureTicker,
        string futureTickerPrefix)
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
                (await instrumentRepository.GetAsync(foreverFutureTicker))!.InstrumentId;

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
            foreach (var future in futures)
            {
                spreads.Add(new Spread
                {
                    FirstInstrumentId = foreverFutureInstrumentId,
                    FirstInstrumentTicker = foreverFutureTicker,
                    FirstInstrumentRole = KnownSpreadRoles.ForeverFuture,
                    SecondInstrumentId = future.InstrumentId,
                    SecondInstrumentTicker = future.Ticker,
                    SecondInstrumentRole = KnownSpreadRoles.Future
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
        
        return spreads;
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
}
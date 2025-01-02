using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Services;

public class SpreadService(
    ISpreadRepository spreadRepository,
    IShareRepository shareRepository,
    IFutureRepository futureRepository,
    ICurrencyRepository currencyRepository) 
    : ISpreadService
{
    public async Task<List<Spread>> CalculateSpreadsAsync()
    {
        var spreads = await spreadRepository.GetWatchListAsync();

        foreach (var spread in spreads)
        {
            spread.FirstInstrumentPrice = await GetInstrumentPrice(spread.FirstInstrumentId);
            spread.SecondInstrumentPrice = await GetInstrumentPrice(spread.SecondInstrumentId);

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
            
            await spreadRepository.UpdateSpreadAsync(spread);
        }
        
        return spreads;
    }

    private async ValueTask<double> GetInstrumentPrice(Guid instrumentId)
    {
        var share = await shareRepository.GetByInstrumentIdAsync(instrumentId);

        if (share is not null)
            return share.Price;
        
        var future = await futureRepository.GetByInstrumentIdAsync(instrumentId);

        if (future is not null)
            return future.Price;
        
        var currency = await currencyRepository.GetByInstrumentIdAsync(instrumentId);

        if (currency is not null)
            return currency.Price;

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
                SecondInstrumentRole: KnownSpreadRoles.Contract
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
                     FirstInstrumentRole: KnownSpreadRoles.Contract, 
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
                FirstInstrumentRole: KnownSpreadRoles.NearContract, 
                SecondInstrumentRole: KnownSpreadRoles.FarContract
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
                     FirstInstrumentRole: KnownSpreadRoles.FarContract, 
                     SecondInstrumentRole: KnownSpreadRoles.NearContract
                 })
        {
            spread.SpreadPricePosition = 
                spread.SecondInstrumentPrice > spread.FirstInstrumentPrice 
                    ? KnownSpreadPricePositions.Contango 
                    : KnownSpreadPricePositions.Backwardation;
        }        
    }
}
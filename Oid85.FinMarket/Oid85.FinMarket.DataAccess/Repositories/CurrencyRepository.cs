using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class CurrencyRepository(
    FinMarketContext context,
    IMapper mapper) : ICurrencyRepository
{
    public async Task AddOrUpdateAsync(List<Currency> currencies)
    {
        if (!currencies.Any())
            return;
        
        foreach (var currency in currencies)
        {
            var entity = context.CurrencyEntities
                .FirstOrDefault(x => 
                    x.Ticker == currency.Ticker);

            if (entity is null)
            {
                entity = mapper.Map<CurrencyEntity>(currency);
                await context.CurrencyEntities.AddAsync(entity);
            }

            else
            {
                entity.Ticker = currency.Ticker;
                entity.Price = currency.Price;
                entity.Isin = currency.Isin;
                entity.Figi = currency.Figi;
                entity.ClassCode = currency.ClassCode;
                entity.Name = currency.Name;
                entity.IsoCurrencyName = currency.IsoCurrencyName;
                entity.Uid = currency.Uid;
                entity.InWatchList = currency.InWatchList;
            }
        }

        await context.SaveChangesAsync();
    }

    public Task<List<Currency>> GetAllAsync() =>
        context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .Select(x => mapper.Map<Currency>(x))
            .OrderBy(x => x.Ticker)
            .ToListAsync();

    public Task<List<Currency>> GetWatchListAsync() =>
        context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .Select(x => mapper.Map<Currency>(x))
            .OrderBy(x => x.Ticker)
            .ToListAsync();

    public async Task<Currency?> GetByTickerAsync(string ticker)
    {
        var entity = await context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity is null 
            ? null 
            : mapper.Map<Currency>(entity);
    }
}
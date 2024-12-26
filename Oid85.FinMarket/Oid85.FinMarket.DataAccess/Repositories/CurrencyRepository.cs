using Mapster;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class CurrencyRepository(
    FinMarketContext context) : ICurrencyRepository
{
    public async Task AddOrUpdateAsync(List<Currency> currencies)
    {
        if (currencies.Count == 0)
            return;
        
        foreach (var currency in currencies)
        {
            var entity = context.CurrencyEntities
                .FirstOrDefault(x => 
                    x.Ticker == currency.Ticker);

            if (entity is null)
            {
                entity = currency.Adapt<CurrencyEntity>();
                await context.CurrencyEntities.AddAsync(entity);
            }

            else
            {
                entity.Adapt(currency);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<Currency>> GetAllAsync() =>
        (await context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .ToListAsync())
        .Select(x => x.Adapt<Currency>())
        .ToList();

    public async Task<List<Currency>> GetWatchListAsync() =>
        (await context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .OrderBy(x => x.Ticker)
            .ToListAsync())
        .Select(x => x.Adapt<Currency>())
        .ToList();

    public async Task<Currency?> GetByTickerAsync(string ticker)
    {
        var entity = await context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Ticker == ticker);

        return entity?.Adapt<Currency>();
    }
}
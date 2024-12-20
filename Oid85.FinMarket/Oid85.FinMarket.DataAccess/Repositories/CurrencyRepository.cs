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
        if (!currencies.Any())
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

    public Task<List<Currency>> GetAllAsync() =>
        context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .Select(x => x.Adapt<Currency>())
            .OrderBy(x => x.Ticker)
            .ToListAsync();

    public Task<List<Currency>> GetWatchListAsync() =>
        context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .Select(x => x.Adapt<Currency>())
            .OrderBy(x => x.Ticker)
            .ToListAsync();

    public async Task<Currency?> GetByTickerAsync(string ticker)
    {
        var entity = await context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Ticker == ticker);

        return entity?.Adapt<Currency>();
    }
}
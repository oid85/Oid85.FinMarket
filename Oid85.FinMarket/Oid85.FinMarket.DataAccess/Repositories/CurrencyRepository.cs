using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class CurrencyRepository(
    ILogger logger,
    FinMarketContext context) 
    : ICurrencyRepository
{
    public async Task AddAsync(List<Currency> currencies)
    {
        if (currencies is [])
            return;

        var entities = new List<CurrencyEntity>();
        
        foreach (var currency in currencies)
            if (!await context.CurrencyEntities
                    .AnyAsync(x => x.InstrumentId == currency.InstrumentId))
                entities.Add(DataAccessMapper.Map(currency));

        await context.CurrencyEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.CurrencyEntities
                .Where(x => x.InstrumentId == instrumentId)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(entity => entity.LastPrice, lastPrice));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }
    
    public async Task<List<Currency>> GetByTickersAsync(List<string> tickers) =>
        (await context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .Where(x => tickers.Contains(x.Ticker))
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();
}
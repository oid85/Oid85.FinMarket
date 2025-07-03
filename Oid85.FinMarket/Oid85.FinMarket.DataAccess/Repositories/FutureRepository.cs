using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class FutureRepository(
    ILogger logger,
    IDbContextFactory<FinMarketContext> contextFactory)
    : IFutureRepository
{
    public async Task AddAsync(List<Future> futures)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        if (futures is [])
            return;

        var entities = new List<FutureEntity>();
        
        foreach (var future in futures)
            if (!await context.FutureEntities
                    .AnyAsync(x => x.InstrumentId == future.InstrumentId))
                entities.Add(DataAccessMapper.Map(future));

        await context.FutureEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }
    
    public async Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.FutureEntities
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

    public async Task<List<Future>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        return (await context.FutureEntities
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Ticker)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }

    public async Task<List<Future>> GetAsync(List<string> tickers)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        return (await context.FutureEntities
                .Where(x => !x.IsDeleted)
                .Where(x => tickers.Contains(x.Ticker))
                .OrderBy(x => x.Ticker)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }

    public async Task<Future?> GetAsync(string ticker)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        var entity = await context.FutureEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ticker == ticker);

        return entity is null ? null : DataAccessMapper.Map(entity);
    }
}
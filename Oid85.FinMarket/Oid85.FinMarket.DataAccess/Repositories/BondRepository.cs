using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;
namespace Oid85.FinMarket.DataAccess.Repositories;

public class BondRepository(
    ILogger logger,
    IDbContextFactory<FinMarketContext> contextFactory)
    : IBondRepository
{
    public async Task AddAsync(List<Bond> bonds)
    {        
        await using var context = await contextFactory.CreateDbContextAsync();
        
        if (bonds is [])
            return;

        var entities = new List<BondEntity>();
        
        foreach (var bond in bonds)
            if (!await context.BondEntities
                    .AnyAsync(x => x.InstrumentId == bond.InstrumentId))
                entities.Add(DataAccessMapper.Map(bond));

        await context.BondEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.BondEntities
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

    public async Task<List<Bond>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        return (await context.BondEntities
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Ticker)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }

    public async Task<List<Bond>> GetAsync(List<Guid> instrumentIds)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        return (await context.BondEntities
                .Where(x => !x.IsDeleted)
                .Where(x => instrumentIds.Contains(x.InstrumentId))
                .OrderBy(x => x.Ticker)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }

    public async Task<List<Bond>> GetAsync(List<string> tickers)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return (await context.BondEntities
                .Where(x => !x.IsDeleted)
                .Where(x => tickers.Contains(x.Ticker))
                .OrderBy(x => x.Ticker)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }
}
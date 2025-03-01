using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;
namespace Oid85.FinMarket.DataAccess.Repositories;

public class BondRepository(
    ILogger logger,
    FinMarketContext context) 
    : IBondRepository
{
    public async Task AddAsync(List<Bond> bonds)
    {        
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

    public async Task<List<Bond>> GetAllAsync() =>
        (await context.BondEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();

    public async Task<List<Bond>> GetByTickersAsync(List<string> tickers) =>
        (await context.BondEntities
            .Where(x => !x.IsDeleted)
            .Where(x => tickers.Contains(x.Ticker))
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();

    public async Task<Bond?> GetByTickerAsync(string ticker)
    {
        var entity = await context.BondEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity is null ? null : DataAccessMapper.Map(entity);
    }

    public async Task<Bond?> GetByInstrumentIdAsync(Guid instrumentId)
    {
        var entity = await context.BondEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);
        
        return entity is null ? null : DataAccessMapper.Map(entity);
    }
}
using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class ShareRepository(
    ILogger logger,
    FinMarketContext context) 
    : IShareRepository
{
    public async Task AddAsync(List<Share> shares)
    {
        if (shares is [])
            return;

        var entities = new List<ShareEntity>();
        
        foreach (var share in shares)
            if (!await context.ShareEntities
                    .AnyAsync(x => x.InstrumentId == share.InstrumentId))
                entities.Add(DataAccessMapper.Map(share));

        await context.ShareEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.ShareEntities
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
    
    public async Task<List<Share>> GetAsync(List<string> tickers) =>
        (await context.ShareEntities
            .Where(x => !x.IsDeleted)
            .Where(x => tickers.Contains(x.Ticker))
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();

    public async Task<Share?> GetAsync(string ticker)
    {
        var entity = await context.ShareEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity is null ? null : DataAccessMapper.Map(entity);
    }

    public async Task<Share?> GetAsync(Guid instrumentId)
    {
        var entity = await context.ShareEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);
        
        return entity is null ? null : DataAccessMapper.Map(entity);
    }

    public async Task<List<Share>> GetAsync(List<Guid> instrumentIds) =>
        (await context.ShareEntities
            .Where(x => !x.IsDeleted)
            .Where(x => instrumentIds.Contains(x.InstrumentId))
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();
}
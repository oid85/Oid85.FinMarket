using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class ShareMultiplicatorRepository(
    ILogger logger,
    IDbContextFactory<FinMarketContext> contextFactory,
    IInstrumentRepository instrumentRepository) 
    : IShareMultiplicatorRepository
{
    public async Task AddOrUpdateAsync(List<ShareMultiplicator> multiplicators)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        if (multiplicators is [])
            return;

        var entities = new List<ShareMultiplicatorEntity>();
        
        foreach (var multiplicator in multiplicators)
            if (!await context.MultiplicatorEntities
                    .AnyAsync(x => x.Ticker == multiplicator.Ticker))
                entities.Add(DataAccessMapper.Map(multiplicator));
            else
                await UpdateFieldsAsync(multiplicator);

        await context.MultiplicatorEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateFieldsAsync(ShareMultiplicator shareMultiplicator)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.MultiplicatorEntities
                .Where(x => 
                    x.Ticker == shareMultiplicator.Ticker)
                .ExecuteUpdateAsync(x => x
                        .SetProperty(entity => entity.MarketCap, shareMultiplicator.MarketCap));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }
    
    public async Task<List<ShareMultiplicator>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        return (await context.MultiplicatorEntities
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Ticker)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }

    public async Task<ShareMultiplicator?> GetAsync(string ticker)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        var entity = await context.MultiplicatorEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity is null ? null : DataAccessMapper.Map(entity);
    }

    public async Task<List<ShareMultiplicator>> GetAsync(List<Guid> instrumentIds)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        var tickers = (await instrumentRepository.GetAsync(instrumentIds))
            .Select(x => x.Ticker).ToList();
        
        return (await context.MultiplicatorEntities
                .Where(x => !x.IsDeleted)
                .Where(x => tickers.Contains(x.Ticker))
                .OrderBy(x => x.Ticker)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
        
    }
}
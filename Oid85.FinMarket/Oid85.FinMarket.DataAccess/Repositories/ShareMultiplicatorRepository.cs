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
            if (!await context.ShareMultiplicatorEntities
                    .AnyAsync(x => x.Ticker == multiplicator.Ticker))
                entities.Add(DataAccessMapper.Map(multiplicator));
            else
                await UpdateFieldsAsync(multiplicator);

        await context.ShareMultiplicatorEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateFieldsAsync(ShareMultiplicator multiplicator)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.ShareMultiplicatorEntities
                .Where(x => 
                    x.Ticker == multiplicator.Ticker)
                .ExecuteUpdateAsync(x => x
                        .SetProperty(entity => entity.Name, multiplicator.Name)
                        .SetProperty(entity => entity.MarketCap, multiplicator.MarketCap)
                        .SetProperty(entity => entity.Ev, multiplicator.Ev)
                        .SetProperty(entity => entity.Revenue, multiplicator.Revenue)
                        .SetProperty(entity => entity.NetIncome, multiplicator.NetIncome)
                        .SetProperty(entity => entity.DdAo, multiplicator.DdAo)
                        .SetProperty(entity => entity.DdAp, multiplicator.DdAp)
                        .SetProperty(entity => entity.DdNetIncome, multiplicator.DdNetIncome)
                        .SetProperty(entity => entity.Pe, multiplicator.Pe)
                        .SetProperty(entity => entity.Ps, multiplicator.Ps)
                        .SetProperty(entity => entity.Pb, multiplicator.Pb)
                        .SetProperty(entity => entity.EvEbitda, multiplicator.EvEbitda)
                        .SetProperty(entity => entity.EbitdaMargin, multiplicator.EbitdaMargin)
                        .SetProperty(entity => entity.NetDebtEbitda, multiplicator.NetDebtEbitda));
            
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
        
        return (await context.ShareMultiplicatorEntities
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
        
        var entity = await context.ShareMultiplicatorEntities
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
        
        return (await context.ShareMultiplicatorEntities
                .Where(x => !x.IsDeleted)
                .Where(x => tickers.Contains(x.Ticker))
                .OrderBy(x => x.Ticker)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }
}
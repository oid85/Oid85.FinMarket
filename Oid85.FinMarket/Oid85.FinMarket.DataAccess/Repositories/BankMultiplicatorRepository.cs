using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class BankMultiplicatorRepository(
    ILogger logger,
    IDbContextFactory<FinMarketContext> contextFactory,
    IInstrumentRepository instrumentRepository) 
    : IBankMultiplicatorRepository
{
    public async Task AddOrUpdateAsync(List<BankMultiplicator> multiplicators)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        if (multiplicators is [])
            return;

        var entities = new List<BankMultiplicatorEntity>();
        
        foreach (var multiplicator in multiplicators)
            if (!await context.BankMultiplicatorEntities
                    .AnyAsync(x => x.Ticker == multiplicator.Ticker))
                entities.Add(DataAccessMapper.Map(multiplicator));
            else
                await UpdateFieldsAsync(multiplicator);

        await context.BankMultiplicatorEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateFieldsAsync(BankMultiplicator shareMultiplicator)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.BankMultiplicatorEntities
                .Where(x => 
                    x.Ticker == shareMultiplicator.Ticker)
                .ExecuteUpdateAsync(x => x
                        .SetProperty(entity => entity.Name, shareMultiplicator.Name)
                        .SetProperty(entity => entity.MarketCap, shareMultiplicator.MarketCap)
                        .SetProperty(entity => entity.NetOperatingIncome, shareMultiplicator.NetOperatingIncome)
                        .SetProperty(entity => entity.NetIncome, shareMultiplicator.NetIncome)
                        .SetProperty(entity => entity.DdAo, shareMultiplicator.DdAo)
                        .SetProperty(entity => entity.DdAp, shareMultiplicator.DdAp)
                        .SetProperty(entity => entity.DdNetIncome, shareMultiplicator.DdNetIncome)
                        .SetProperty(entity => entity.Pe, shareMultiplicator.Pe)
                        .SetProperty(entity => entity.Pb, shareMultiplicator.Pb)
                        .SetProperty(entity => entity.NetInterestMargin, shareMultiplicator.NetInterestMargin)
                        .SetProperty(entity => entity.Roe, shareMultiplicator.Roe)
                        .SetProperty(entity => entity.Roa, shareMultiplicator.Roa));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }
    
    public async Task<List<BankMultiplicator>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        return (await context.BankMultiplicatorEntities
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Ticker)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }

    public async Task<BankMultiplicator?> GetAsync(string ticker)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        var entity = await context.BankMultiplicatorEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity is null ? null : DataAccessMapper.Map(entity);
    }

    public async Task<List<BankMultiplicator>> GetAsync(List<Guid> instrumentIds)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        var tickers = (await instrumentRepository.GetAsync(instrumentIds))
            .Select(x => x.Ticker).ToList();
        
        return (await context.BankMultiplicatorEntities
                .Where(x => !x.IsDeleted)
                .Where(x => tickers.Contains(x.Ticker))
                .OrderBy(x => x.Ticker)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }
}
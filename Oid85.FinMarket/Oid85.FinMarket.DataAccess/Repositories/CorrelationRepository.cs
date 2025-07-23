using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models.StatisticalArbitration;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class CorrelationRepository(
    ILogger logger,
    IDbContextFactory<FinMarketContext> contextFactory)
    : ICorrelationRepository
{
    public async Task AddAsync(Correlation correlation)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        if (!await context.CorrelationEntities.AnyAsync(x =>
                x.Ticker1 == correlation.Ticker1 &&
                x.Ticker2 == correlation.Ticker2))
            await context.CorrelationEntities.AddAsync(DataAccessMapper.Map(correlation));

        else
            await UpdateAsync(correlation.Ticker1, correlation.Ticker2, correlation.Value); 
        
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(string ticker1, string ticker2, double value)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.CorrelationEntities
                .Where(x => 
                    x.Ticker1 == ticker1 && 
                    x.Ticker2 == ticker2)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(entity => entity.Value, value)
                    .SetProperty(entity => entity.UpdatedAt, DateTime.UtcNow));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }

    public async Task<List<Correlation>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return (await context.CorrelationEntities
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Ticker1)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }

    public async Task DeleteAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await context.CorrelationEntities.ExecuteDeleteAsync();
    }
}
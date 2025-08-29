using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models.Algo;

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
                x.TickerFirst == correlation.TickerFirst &&
                x.TickerSecond == correlation.TickerSecond))
            await context.CorrelationEntities.AddAsync(DataAccessMapper.Map(correlation));

        else
            await UpdateAsync(correlation.TickerFirst, correlation.TickerSecond, correlation.Value); 
        
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(string tickerFirst, string tickerSecond, double value)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.CorrelationEntities
                .Where(x => 
                    x.TickerFirst == tickerFirst && 
                    x.TickerSecond == tickerSecond)
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
                .OrderBy(x => x.TickerFirst)
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
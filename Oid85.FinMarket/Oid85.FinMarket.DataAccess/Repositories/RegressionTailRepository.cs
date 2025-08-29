using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class RegressionTailRepository(
    ILogger logger,
    IDbContextFactory<FinMarketContext> contextFactory)
    : IRegressionTailRepository
{
    public async Task AddAsync(RegressionTail regressionTail)
    {
        await using var context = await contextFactory.CreateDbContextAsync();

        if (!await context.RegressionTailEntities.AnyAsync(x =>
                x.TickerFirst == regressionTail.TickerFirst &&
                x.TickerSecond == regressionTail.TickerSecond))
            await context.RegressionTailEntities.AddAsync(DataAccessMapper.Map(regressionTail));

        else
            await UpdateAsync(regressionTail.TickerFirst, regressionTail.TickerSecond, regressionTail.Tails, regressionTail.IsStationary); 
        
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(string tickerFirst, string tickerSecond, List<RegressionTailItem> tails, bool isStationary)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            var json = JsonSerializer.Serialize(tails);
            
            await context.RegressionTailEntities
                .Where(x => 
                    x.TickerFirst == tickerFirst && 
                    x.TickerSecond == tickerSecond)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(entity => entity.Tails, json)
                    .SetProperty(entity => entity.IsStationary, isStationary)
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

    public async Task<List<RegressionTail>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return (await context.RegressionTailEntities
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.TickerFirst)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }

    public async Task<RegressionTail?> GetAsync(string tickerFirst, string tickerSecond)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        var entity = await context.RegressionTailEntities
            .FirstOrDefaultAsync(
                x => 
                    x.TickerFirst == tickerFirst &&
                    x.TickerSecond == tickerSecond);
        
        return entity is null ? null : DataAccessMapper.Map(entity);
    }

    public async Task DeleteAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await context.RegressionTailEntities.ExecuteDeleteAsync();
    }
}
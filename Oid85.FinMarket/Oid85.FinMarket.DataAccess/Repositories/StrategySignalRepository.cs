using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class StrategySignalRepository(
    ILogger logger,
    IDbContextFactory<FinMarketContext> contextFactory)
    : IStrategySignalRepository
{
    public async Task AddAsync(StrategySignal strategySignal)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        if (!await context.StrategySignalEntities.AnyAsync(x => x.Ticker == strategySignal.Ticker))
            await context.StrategySignalEntities.AddAsync(DataAccessMapper.Map(strategySignal));
        
        await context.SaveChangesAsync();
    }

    public async Task UpdatePositionAsync(string ticker, int countSignals, double positionCost, int positionSize, double lastPrice)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.StrategySignalEntities
                .Where(x => x.Ticker == ticker)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(entity => entity.CountSignals, countSignals)
                    .SetProperty(entity => entity.PositionCost, positionCost)
                    .SetProperty(entity => entity.PositionSize, positionSize)
                    .SetProperty(entity => entity.LastPrice, lastPrice)
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

    public async Task<List<StrategySignal>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return (await context.StrategySignalEntities
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Ticker)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }
}
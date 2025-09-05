using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class PairArbitrageStrategySignalRepository(
    ILogger logger,
    IDbContextFactory<FinMarketContext> contextFactory)
    : IPairArbitrageStrategySignalRepository
{
    public async Task AddAsync(PairArbitrageStrategySignal strategySignal)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        if (!await context.PairArbitrageStrategySignalEntities.AnyAsync(
                x => 
                    x.TickerFirst == strategySignal.TickerFirst && 
                    x.TickerSecond == strategySignal.TickerSecond))
            await context.PairArbitrageStrategySignalEntities.AddAsync(DataAccessMapper.Map(strategySignal));
        
        await context.SaveChangesAsync();
    }

    public async Task UpdatePositionAsync(PairArbitrageStrategySignal strategySignal)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.PairArbitrageStrategySignalEntities
                .Where(x => 
                    x.TickerFirst == strategySignal.TickerFirst && 
                    x.TickerSecond == strategySignal.TickerSecond)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(entity => entity.CountStrategies, strategySignal.CountStrategies)
                    .SetProperty(entity => entity.CountSignals, strategySignal.CountSignals)
                    .SetProperty(entity => entity.PercentSignals, strategySignal.PercentSignals)
                    .SetProperty(entity => entity.LastPriceFirst, strategySignal.LastPriceFirst)
                    .SetProperty(entity => entity.LastPriceSecond, strategySignal.LastPriceSecond)
                    .SetProperty(entity => entity.PositionCost, strategySignal.PositionCost)
                    .SetProperty(entity => entity.PositionSizeFirst, strategySignal.PositionSizeFirst)
                    .SetProperty(entity => entity.PositionSizeSecond, strategySignal.PositionSizeSecond)
                    .SetProperty(entity => entity.PositionPercentPortfolio, strategySignal.PositionPercentPortfolio)
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

    public async Task<List<PairArbitrageStrategySignal>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        return (await context.PairArbitrageStrategySignalEntities
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.TickerFirst)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }
}
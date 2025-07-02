using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class StrategySignalRepository(
    ILogger logger,
    FinMarketContext context) 
    : IStrategySignalRepository
{
    public async Task AddAsync(StrategySignal strategySignal)
    {
        if (!await context.StrategySignalEntities.AnyAsync(x => x.Ticker == strategySignal.Ticker))
            await context.StrategySignalEntities.AddAsync(DataAccessMapper.Map(strategySignal));
        
        await context.SaveChangesAsync();
    }

    public async Task UpdatePositionAsync(List<string> tickers, int countSignals, double positionCost)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.StrategySignalEntities
                .Where(x => tickers.Contains(x.Ticker))
                .ExecuteUpdateAsync(x => x
                    .SetProperty(entity => entity.CountSignals, countSignals)
                    .SetProperty(entity => entity.PositionCost, positionCost)
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

    public async Task<List<StrategySignal>> GetAllAsync() =>
        (await context.StrategySignalEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();
}
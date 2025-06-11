using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Mapping;
using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.External.ResourceStore.Models.Algo;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class BacktestResultRepository(
    FinMarketContext context)  
    : IBacktestResultRepository
{
    public async Task AddAsync(List<BacktestResult> backtestResults)
    {
        if (backtestResults is [])
            return;

        var entities = backtestResults.Select(DataAccessMapper.Map);
        
        await context.BacktestResultEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task<List<BacktestResult>> GetAsync(BacktestResultFilterResource filter) =>
        (await context.BacktestResultEntities
            .Where(x => 
                x.ProfitFactor >= filter.ProfitFactor &&
                x.RecoveryFactor >= filter.RecoveryFactor &&
                x.MaxDrawdownPercent <= filter.MaxDrawdownPercent)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();

    public Task DeleteAsync(Guid strategyId) => 
        context.BacktestResultEntities.Where(x => x.StrategyId == strategyId).ExecuteDeleteAsync();
}
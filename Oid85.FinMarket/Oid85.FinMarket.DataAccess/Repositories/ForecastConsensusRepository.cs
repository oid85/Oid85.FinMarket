using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class ForecastConsensusRepository(
    ILogger logger,
    FinMarketContext context) 
    : IForecastConsensusRepository
{
    public async Task AddAsync(List<ForecastConsensus> forecastConsensuses)
    {
        if (forecastConsensuses is [])
            return;

        var entities = new List<ForecastConsensusEntity>();
        
        foreach (var forecastConsensuse in forecastConsensuses)
            if (!await context.ForecastConsensusEntities
                    .AnyAsync(x => 
                        x.InstrumentId == forecastConsensuse.InstrumentId))
                entities.Add(DataAccessMapper.Map(forecastConsensuse));

        await context.ForecastConsensusEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateForecastTargetAsync(Guid instrumentId, ForecastConsensus forecastConsensus)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.ForecastConsensusEntities
                .Where(x => 
                    x.InstrumentId == forecastConsensus.InstrumentId)
                .ExecuteUpdateAsync(x => x
                        .SetProperty(entity => entity.RecommendationString, forecastConsensus.RecommendationString)
                        .SetProperty(entity => entity.RecommendationNumber, forecastConsensus.RecommendationNumber)
                        .SetProperty(entity => entity.CurrentPrice, forecastConsensus.CurrentPrice)
                        .SetProperty(entity => entity.ConsensusPrice, forecastConsensus.ConsensusPrice)
                        .SetProperty(entity => entity.MinTarget, forecastConsensus.MinTarget)
                        .SetProperty(entity => entity.MaxTarget, forecastConsensus.MaxTarget)
                        .SetProperty(entity => entity.PriceChange, forecastConsensus.PriceChange)
                        .SetProperty(entity => entity.PriceChangeRel, forecastConsensus.PriceChangeRel));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }

    public async Task<List<ForecastConsensus>> GetAllAsync() =>
        (await context.ForecastConsensusEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();

    public async Task<ForecastConsensus?> GetByTickerAsync(string ticker)
    {
        var entity = await context.ForecastConsensusEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity is null ? null : DataAccessMapper.Map(entity);
    }

    public async Task<ForecastConsensus?> GetByInstrumentIdAsync(Guid instrumentId)
    {
        var entity = await context.ForecastConsensusEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);
        
        return entity is null ? null : DataAccessMapper.Map(entity);
    }
}
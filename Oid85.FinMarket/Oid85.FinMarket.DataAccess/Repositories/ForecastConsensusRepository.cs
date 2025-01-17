using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Logging.Services;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class ForecastConsensusRepository(
    ILogService logService,
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
                entities.Add(GetEntity(forecastConsensuse));

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
                .ExecuteUpdateAsync(
                    s => s
                        .SetProperty(u => u.RecommendationString, forecastConsensus.RecommendationString)
                        .SetProperty(u => u.RecommendationNumber, forecastConsensus.RecommendationNumber)
                        .SetProperty(u => u.CurrentPrice, forecastConsensus.CurrentPrice)
                        .SetProperty(u => u.ConsensusPrice, forecastConsensus.ConsensusPrice)
                        .SetProperty(u => u.MinTarget, forecastConsensus.MinTarget)
                        .SetProperty(u => u.MaxTarget, forecastConsensus.MaxTarget)
                        .SetProperty(u => u.PriceChange, forecastConsensus.PriceChange)
                        .SetProperty(u => u.PriceChangeRel, forecastConsensus.PriceChangeRel));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            await logService.LogException(exception);
        }
    }

    public async Task<List<ForecastConsensus>> GetAllAsync() =>
        (await context.ForecastConsensusEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<ForecastConsensus?> GetByTickerAsync(string ticker)
    {
        var entity = await context.ForecastConsensusEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity is null 
            ? null 
            : GetModel(entity);
    }

    public async Task<ForecastConsensus?> GetByInstrumentIdAsync(Guid instrumentId)
    {
        var entity = await context.ForecastConsensusEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);
        
        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    private ForecastConsensusEntity GetEntity(ForecastConsensus model)
    {
        var entity = new ForecastConsensusEntity();

        entity.Ticker = model.Ticker;
        entity.InstrumentId = model.InstrumentId;
        entity.RecommendationString = model.RecommendationString;
        entity.RecommendationNumber = model.RecommendationNumber;
        entity.Currency = model.Currency;
        entity.CurrentPrice = model.CurrentPrice;
        entity.ConsensusPrice = model.ConsensusPrice;
        entity.MinTarget = model.MinTarget;
        entity.MaxTarget = model.MaxTarget;
        entity.PriceChange = model.PriceChange;
        entity.PriceChangeRel = model.PriceChangeRel;
        
        return entity;
    }
    
    private ForecastConsensus GetModel(ForecastConsensusEntity entity)
    {
        var model = new ForecastConsensus();
        
        model.Id = entity.Id;
        model.Ticker = entity.Ticker;
        model.InstrumentId = entity.InstrumentId;
        model.RecommendationString = entity.RecommendationString;
        model.RecommendationNumber = entity.RecommendationNumber;
        model.Currency = entity.Currency;
        model.CurrentPrice = entity.CurrentPrice;
        model.ConsensusPrice = entity.ConsensusPrice;
        model.MinTarget = entity.MinTarget;
        model.MaxTarget = entity.MaxTarget;
        model.PriceChange = entity.PriceChange;
        model.PriceChangeRel = entity.PriceChangeRel;

        return model;
    }
}
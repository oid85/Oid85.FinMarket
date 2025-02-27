﻿using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
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
        var entity = new ForecastConsensusEntity
        {
            Ticker = model.Ticker,
            InstrumentId = model.InstrumentId,
            RecommendationString = model.RecommendationString,
            RecommendationNumber = model.RecommendationNumber,
            Currency = model.Currency,
            CurrentPrice = model.CurrentPrice,
            ConsensusPrice = model.ConsensusPrice,
            MinTarget = model.MinTarget,
            MaxTarget = model.MaxTarget,
            PriceChange = model.PriceChange,
            PriceChangeRel = model.PriceChangeRel
        };

        return entity;
    }
    
    private ForecastConsensus GetModel(ForecastConsensusEntity entity)
    {
        var model = new ForecastConsensus
        {
            Id = entity.Id,
            Ticker = entity.Ticker,
            InstrumentId = entity.InstrumentId,
            RecommendationString = entity.RecommendationString,
            RecommendationNumber = entity.RecommendationNumber,
            Currency = entity.Currency,
            CurrentPrice = entity.CurrentPrice,
            ConsensusPrice = entity.ConsensusPrice,
            MinTarget = entity.MinTarget,
            MaxTarget = entity.MaxTarget,
            PriceChange = entity.PriceChange,
            PriceChangeRel = entity.PriceChangeRel
        };

        return model;
    }
}
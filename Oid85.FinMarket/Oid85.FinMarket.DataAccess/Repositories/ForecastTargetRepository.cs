using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class ForecastTargetRepository(
    ILogger logger,
    FinMarketContext context) 
    : IForecastTargetRepository
{
    public async Task AddAsync(List<ForecastTarget> forecastTargets)
    {
        if (forecastTargets is [])
            return;

        var entities = new List<ForecastTargetEntity>();
        
        foreach (var forecastTarget in forecastTargets)
            if (!await context.ForecastTargetEntities
                    .AnyAsync(x => 
                        x.InstrumentId == forecastTarget.InstrumentId && 
                        x.Company == forecastTarget.Company && 
                        x.RecommendationDate == forecastTarget.RecommendationDate))
                entities.Add(GetEntity(forecastTarget));

        await context.ForecastTargetEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateForecastTargetAsync(Guid instrumentId, ForecastTarget forecastTarget)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.ForecastTargetEntities
                .Where(x => 
                    x.InstrumentId == forecastTarget.InstrumentId && 
                    x.Company == forecastTarget.Company && 
                    x.RecommendationDate == forecastTarget.RecommendationDate)
                .ExecuteUpdateAsync(
                    s => s
                        .SetProperty(entity => entity.RecommendationString, forecastTarget.RecommendationString)
                        .SetProperty(entity => entity.RecommendationNumber, forecastTarget.RecommendationNumber)
                        .SetProperty(entity => entity.CurrentPrice, forecastTarget.CurrentPrice)
                        .SetProperty(entity => entity.TargetPrice, forecastTarget.TargetPrice)
                        .SetProperty(entity => entity.PriceChange, forecastTarget.PriceChange)
                        .SetProperty(entity => entity.PriceChangeRel, forecastTarget.PriceChangeRel));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }

    public async Task<List<ForecastTarget>> GetAllAsync() =>
        (await context.ForecastTargetEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<List<ForecastTarget>> GetByTickerAsync(string ticker)
    {
        var entities = await context.ForecastTargetEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.Ticker == ticker)
            .AsNoTracking()
            .ToListAsync();
        
        var models = entities
            .Select(GetModel)
            .ToList();
        
        return models;
    }

    public async Task<List<ForecastTarget>> GetByInstrumentIdAsync(Guid instrumentId)
    {
        var entities = await context.ForecastTargetEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InstrumentId == instrumentId)
            .AsNoTracking()
            .ToListAsync();
        
        var models = entities
            .Select(GetModel)
            .ToList();
        
        return models;
    }
    
    private ForecastTargetEntity GetEntity(ForecastTarget model)
    {
        var entity = new ForecastTargetEntity
        {
            Ticker = model.Ticker,
            InstrumentId = model.InstrumentId,
            Company = model.Company,
            RecommendationString = model.RecommendationString,
            RecommendationNumber = model.RecommendationNumber,
            RecommendationDate = model.RecommendationDate,
            Currency = model.Currency,
            CurrentPrice = model.CurrentPrice,
            TargetPrice = model.TargetPrice,
            PriceChange = model.PriceChange,
            PriceChangeRel = model.PriceChangeRel,
            ShowName = model.ShowName
        };

        return entity;
    }
    
    private ForecastTarget GetModel(ForecastTargetEntity entity)
    {
        var model = new ForecastTarget
        {
            Id = entity.Id,
            Ticker = entity.Ticker,
            InstrumentId = entity.InstrumentId,
            Company = entity.Company,
            RecommendationString = entity.RecommendationString,
            RecommendationNumber = entity.RecommendationNumber,
            RecommendationDate = entity.RecommendationDate,
            Currency = entity.Currency,
            CurrentPrice = entity.CurrentPrice,
            TargetPrice = entity.TargetPrice,
            PriceChange = entity.PriceChange,
            PriceChangeRel = entity.PriceChangeRel,
            ShowName = entity.ShowName
        };

        return model;
    }
}
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Logging.Services;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class ForecastTargetRepository(
    ILogService logService,
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
                        .SetProperty(u => u.RecommendationString, forecastTarget.RecommendationString)
                        .SetProperty(u => u.RecommendationNumber, forecastTarget.RecommendationNumber)
                        .SetProperty(u => u.CurrentPrice, forecastTarget.CurrentPrice)
                        .SetProperty(u => u.TargetPrice, forecastTarget.TargetPrice)
                        .SetProperty(u => u.PriceChange, forecastTarget.PriceChange)
                        .SetProperty(u => u.PriceChangeRel, forecastTarget.PriceChangeRel));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            await logService.LogException(exception);
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
        var entity = new ForecastTargetEntity();

        entity.Ticker = model.Ticker;
        entity.InstrumentId = model.InstrumentId;
        entity.Company = model.Company;
        entity.RecommendationString = model.RecommendationString;
        entity.RecommendationNumber = model.RecommendationNumber;
        entity.RecommendationDate = model.RecommendationDate;
        entity.Currency = model.Currency;
        entity.CurrentPrice = model.CurrentPrice;
        entity.TargetPrice = model.TargetPrice;
        entity.PriceChange = model.PriceChange;
        entity.PriceChangeRel = model.PriceChangeRel;
        entity.ShowName = model.ShowName;
        
        return entity;
    }
    
    private ForecastTarget GetModel(ForecastTargetEntity entity)
    {
        var model = new ForecastTarget();
        
        model.Id = entity.Id;
        model.Ticker = entity.Ticker;
        model.InstrumentId = entity.InstrumentId;
        model.Company = entity.Company;
        model.RecommendationString = entity.RecommendationString;
        model.RecommendationNumber = entity.RecommendationNumber;
        model.RecommendationDate = entity.RecommendationDate;
        model.Currency = entity.Currency;
        model.CurrentPrice = entity.CurrentPrice;
        model.TargetPrice = entity.TargetPrice;
        model.PriceChange = entity.PriceChange;
        model.PriceChangeRel = entity.PriceChangeRel;
        model.ShowName = entity.ShowName;

        return model;
    }
}
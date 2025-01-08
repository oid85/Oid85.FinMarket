using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class AnalyseResultRepository(
    FinMarketContext context) 
    : IAnalyseResultRepository
{
    public async Task AddAsync(List<AnalyseResult> results)
    {
        if (results is [])
            return;

        var entities = new List<AnalyseResultEntity>();
        
        foreach (var result in results)
            if (!await context.AnalyseResultEntities
                    .AnyAsync(x => 
                        x.InstrumentId == result.InstrumentId
                        && x.AnalyseType == result.AnalyseType
                        && x.Date == result.Date))
                entities.Add(GetEntity(result));

        await context.AnalyseResultEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task<List<AnalyseResult>> GetAsync(
        Guid instrumentId, DateOnly from, DateOnly to) =>
        (await context.AnalyseResultEntities
            .Where(x => x.InstrumentId == instrumentId)
            .Where(x => x.Date >= from && x.Date <= to)
            .OrderBy(x => x.Date)
            .ToListAsync())
        .Select(GetModel)
        .ToList();
    
    public async Task<List<AnalyseResult>> GetAsync(
        List<Guid> instrumentIds, DateOnly from, DateOnly to)
    {
        var entities = await context.AnalyseResultEntities
            .Where(x => instrumentIds.Contains(x.InstrumentId))
            .Where(x => x.Date >= from && x.Date <= to)
            .OrderBy(x => x.Date)
            .ToListAsync();
        
        var models = entities
            .Select(GetModel)
            .ToList();
        
        return models;
    }

    public async Task<AnalyseResult?> GetLastAsync(
        Guid instrumentId, string analyseType)
    {
        bool exists = await context.AnalyseResultEntities
            .Where(x => 
                x.InstrumentId == instrumentId
                && x.AnalyseType == analyseType)
            .AnyAsync();

        if (!exists)
            return null;
        
        var maxDate = await context.AnalyseResultEntities
            .Where(x => 
                x.InstrumentId == instrumentId
                && x.AnalyseType == analyseType)
            .MaxAsync(x => x.Date);

        var entity = await context.AnalyseResultEntities
            .Where(x => 
                x.InstrumentId == instrumentId
                && x.AnalyseType == analyseType)
            .FirstAsync(x => x.Date == maxDate);
        
        var analyseResult = GetModel(entity);
        
        return analyseResult;
    }

    private AnalyseResultEntity GetEntity(AnalyseResult model)
    {
        var entity = new AnalyseResultEntity();

        entity.Date = model.Date;
        entity.InstrumentId = model.InstrumentId;
        entity.AnalyseType = model.AnalyseType;
        entity.ResultString = model.ResultString;
        entity.ResultNumber = model.ResultNumber;
        
        return entity;
    }
    
    private AnalyseResult GetModel(AnalyseResultEntity entity)
    {
        var model = new AnalyseResult();
        
        model.Id = entity.Id;
        model.Date = entity.Date;
        model.InstrumentId = entity.InstrumentId;
        model.AnalyseType = entity.AnalyseType;
        model.ResultString = entity.ResultString;
        model.ResultNumber = entity.ResultNumber;

        return model;
    }
}
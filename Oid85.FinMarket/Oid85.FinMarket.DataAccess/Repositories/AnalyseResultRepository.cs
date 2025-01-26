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
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();
    
    public async Task<List<AnalyseResult>> GetAsync(
        List<Guid> instrumentIds, DateOnly from, DateOnly to) =>
        (await context.AnalyseResultEntities
            .Where(x => instrumentIds.Contains(x.InstrumentId))
            .Where(x => x.Date >= from && x.Date <= to)
            .OrderBy(x => x.Date)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<AnalyseResult?> GetLastAsync(
        Guid instrumentId, string analyseType)
    {
        var entity = await context.AnalyseResultEntities
            .Where(x => 
                x.InstrumentId == instrumentId
                && x.AnalyseType == analyseType)
            .OrderByDescending(x => x.Date)
            .Take(1)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (entity is null)
            return null;
        
        var model = GetModel(entity);
        
        return model;
    }

    public async Task<List<AnalyseResult>> GetTwoLastAsync(Guid instrumentId, string analyseType)
    {
        var entities = await context.AnalyseResultEntities
            .Where(x => 
                x.InstrumentId == instrumentId
                && x.AnalyseType == analyseType)
            .OrderByDescending(x => x.Date)
            .Take(2)
            .OrderBy(x => x.Date)
            .AsNoTracking()
            .ToListAsync();

        if (entities.Count < 2)
            return [];
        
        var models = entities
            .Select(GetModel)
            .ToList();
        
        return models;
    }

    private AnalyseResultEntity GetEntity(AnalyseResult model)
    {
        var entity = new AnalyseResultEntity
        {
            Date = model.Date,
            InstrumentId = model.InstrumentId,
            AnalyseType = model.AnalyseType,
            ResultString = model.ResultString,
            ResultNumber = model.ResultNumber
        };

        return entity;
    }
    
    private AnalyseResult GetModel(AnalyseResultEntity entity)
    {
        var model = new AnalyseResult
        {
            Id = entity.Id,
            Date = entity.Date,
            InstrumentId = entity.InstrumentId,
            AnalyseType = entity.AnalyseType,
            ResultString = entity.ResultString,
            ResultNumber = entity.ResultNumber
        };

        return model;
    }
}
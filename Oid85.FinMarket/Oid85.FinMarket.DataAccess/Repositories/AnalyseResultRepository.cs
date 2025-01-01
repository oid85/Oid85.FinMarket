using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class AnalyseResultRepository(
    FinMarketContext context) 
    : IAnalyseResultRepository
{
    public async Task AddOrUpdateAsync(List<AnalyseResult> results)
    {
        if (results.Count == 0)
            return;
        
        var lastAnalyseResult = await GetLastAsync(results.First().InstrumentId);

        if (lastAnalyseResult is null)
        {
            var entities = results
                .Select(GetEntity);
            
            await context.AnalyseResultEntities.AddRangeAsync(entities);
        }
        
        else
        {
            var entities = results
                .Select(GetEntity)
                .Where(x => x.Date > lastAnalyseResult.Date);
                
            await context.AnalyseResultEntities.AddRangeAsync(entities);    
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<AnalyseResult>> GetAsync(
        Guid instrumentId, DateTime from, DateTime to) =>
        (await context.AnalyseResultEntities
            .Where(x => x.InstrumentId == instrumentId)
            .Where(x => x.Date >= from && x.Date <= to)
            .OrderBy(x => x.Date)
            .ToListAsync())
        .Select(GetModel)
        .ToList();
    
    public async Task<List<AnalyseResult>> GetAsync(
        List<Guid> instrumentIds, DateTime from, DateTime to) =>
        (await context.AnalyseResultEntities
            .Where(x => instrumentIds.Contains(x.InstrumentId))
            .Where(x => x.Date >= from && x.Date <= to)
            .OrderBy(x => x.Date)
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<AnalyseResult?> GetLastAsync(Guid instrumentId)
    {
        bool exists = await context.AnalyseResultEntities
            .Where(x => x.InstrumentId == instrumentId)
            .AnyAsync();

        if (!exists)
            return null;
        
        var maxDate = await context.AnalyseResultEntities
            .Where(x => x.InstrumentId == instrumentId)
            .MaxAsync(x => x.Date);

        var entity = await context.AnalyseResultEntities
            .Where(x => x.InstrumentId == instrumentId)
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
        entity.Result = model.Result;
        
        return entity;
    }
    
    private AnalyseResult GetModel(AnalyseResultEntity entity)
    {
        var model = new AnalyseResult();
        
        model.Id = entity.Id;
        model.Date = entity.Date;
        model.InstrumentId = entity.InstrumentId;
        model.AnalyseType = entity.AnalyseType;
        model.Result = entity.Result;

        return model;
    }
}
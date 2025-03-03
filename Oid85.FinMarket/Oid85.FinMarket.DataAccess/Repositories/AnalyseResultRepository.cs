using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
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
                entities.Add(DataAccessMapper.Map(result));

        await context.AnalyseResultEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }
    
    public async Task<List<AnalyseResult>> GetAsync(
        List<Guid> instrumentIds, DateOnly from, DateOnly to) =>
        (await context.AnalyseResultEntities
            .Where(x => instrumentIds.Contains(x.InstrumentId))
            .Where(x => x.Date >= from && x.Date <= to)
            .OrderBy(x => x.Date)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();

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

        return entities.Count < 2 ? [] : entities.Select(DataAccessMapper.Map).ToList();
    }
}
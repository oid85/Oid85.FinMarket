using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class DividendInfoRepository(
    FinMarketContext context) 
    : IDividendInfoRepository
{
    public async Task AddOrUpdateAsync(List<DividendInfo> dividendInfos)
    {
        if (dividendInfos is [])
            return;

        var entities = new List<DividendInfoEntity>();
        
        foreach (var dividendInfo in dividendInfos)
            if (!await context.DividendInfoEntities
                    .AnyAsync(x => 
                        x.InstrumentId == dividendInfo.InstrumentId &&
                        x.RecordDate == dividendInfo.RecordDate &&
                        x.DeclaredDate == dividendInfo.DeclaredDate))
                entities.Add(GetEntity(dividendInfo));

        await context.DividendInfoEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task<List<DividendInfo>> GetAllAsync() =>
        (await context.DividendInfoEntities
            .OrderBy(x => x.DividendPrc)
            .ToListAsync())
        .Select(GetModel)
        .ToList();
    
    public async Task<List<DividendInfo>> GetAsync(
        List<Guid> instrumentIds, DateOnly from, DateOnly to) =>
        (await context.DividendInfoEntities
            .Where(x => instrumentIds.Contains(x.InstrumentId))
            .Where(x =>
                x.RecordDate >= from &&
                x.RecordDate <= to)
            .OrderBy(x => x.DividendPrc)
            .ToListAsync())
        .Select(GetModel)
        .ToList();
    
    private DividendInfoEntity GetEntity(DividendInfo model)
    {
        var entity = new DividendInfoEntity();
        
        entity.InstrumentId = model.InstrumentId;
        entity.Ticker = model.Ticker;
        entity.RecordDate = model.RecordDate;
        entity.DeclaredDate = model.DeclaredDate;
        entity.Dividend = model.Dividend;
        entity.DividendPrc = model.DividendPrc;

        return entity;
    }
    
    private DividendInfo GetModel(DividendInfoEntity entity)
    {
        var model = new DividendInfo();
        
        model.Id = entity.Id;
        model.InstrumentId = entity.InstrumentId;
        model.Ticker = entity.Ticker;
        model.RecordDate = entity.RecordDate;
        model.DeclaredDate = entity.DeclaredDate;
        model.Dividend = entity.Dividend;
        model.DividendPrc = entity.DividendPrc;

        return model;
    }
}

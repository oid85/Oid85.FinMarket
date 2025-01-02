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
        if (dividendInfos.Count == 0)
            return;
        
        foreach (var dividendInfo in dividendInfos)
        {
            var entity = context.DividendInfoEntities
                .FirstOrDefault(x => 
                    x.InstrumentId == dividendInfo.InstrumentId &&
                    x.RecordDate == dividendInfo.RecordDate &&
                    x.DeclaredDate == dividendInfo.DeclaredDate);

            if (entity is null)
            {
                SetEntity(ref entity, dividendInfo);
                
                if (entity is not null)
                    await context.DividendInfoEntities.AddAsync(entity);
            }

            else
            {
                SetEntity(ref entity, dividendInfo);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<DividendInfo>> GetAllAsync() =>
        (await context.DividendInfoEntities
            .OrderBy(x => x.DividendPrc)
            .ToListAsync())
        .Select(GetModel)
        .ToList();
    
    public async Task<List<DividendInfo>> GetAsync(
        List<Guid> instrumentIds, DateTime from, DateTime to) =>
        (await context.DividendInfoEntities
            .Where(x => instrumentIds.Contains(x.InstrumentId))
            .Where(x =>
                x.RecordDate >= DateOnly.FromDateTime(from) &&
                x.RecordDate <= DateOnly.FromDateTime(to))
            .OrderBy(x => x.DividendPrc)
            .ToListAsync())
        .Select(GetModel)
        .ToList();
    
    private void SetEntity(ref DividendInfoEntity? entity, DividendInfo model)
    {
        entity ??= new DividendInfoEntity();
        
        entity.InstrumentId = model.InstrumentId;
        entity.Ticker = model.Ticker;
        entity.RecordDate = model.RecordDate;
        entity.DeclaredDate = model.DeclaredDate;
        entity.Dividend = model.Dividend;
        entity.DividendPrc = model.DividendPrc;
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

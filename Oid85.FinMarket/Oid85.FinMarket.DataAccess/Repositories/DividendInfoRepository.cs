using Mapster;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class DividendInfoRepository(
    FinMarketContext context) : IDividendInfoRepository
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
                entity = dividendInfo.Adapt<DividendInfoEntity>();
                await context.DividendInfoEntities.AddAsync(entity);
            }

            else
            {
                entity.Adapt(dividendInfo);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<DividendInfo>> GetAllAsync() =>
        (await context.DividendInfoEntities
            .OrderBy(x => x.DividendPrc)
            .ToListAsync())
        .Select(x => x.Adapt<DividendInfo>())
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
        .Select(x => x.Adapt<DividendInfo>())
        .ToList();
}

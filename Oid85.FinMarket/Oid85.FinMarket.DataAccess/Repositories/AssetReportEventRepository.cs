using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class AssetReportEventRepository(
    FinMarketContext context)  : 
    IAssetReportEventRepository
{
    public async Task AddAsync(List<AssetReportEvent> assetReportEvents)
    {
        if (assetReportEvents is [])
            return;

        var entities = new List<AssetReportEventEntity>();
        
        foreach (var assetReportEvent in assetReportEvents)
            if (!await context.AssetReportEventEntities
                    .AnyAsync(x => 
                        x.InstrumentId == assetReportEvent.InstrumentId
                        && x.PeriodYear == assetReportEvent.PeriodYear
                        && x.PeriodNum == assetReportEvent.PeriodNum 
                        && x.Type == assetReportEvent.Type))
                entities.Add(DataAccessMapper.Map(assetReportEvent));

        await context.AssetReportEventEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task<List<AssetReportEvent>> GetAllAsync() =>
        (await context.AssetReportEventEntities
            .OrderBy(x => x.ReportDate)
            .AsNoTracking()
            .ToListAsync())
        .Select(DataAccessMapper.Map)
        .ToList();
}
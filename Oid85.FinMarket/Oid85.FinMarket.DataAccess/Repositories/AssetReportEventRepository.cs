using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class AssetReportEventRepository(
    IDbContextFactory<FinMarketContext> contextFactory)  
    : IAssetReportEventRepository
{
    public async Task AddAsync(List<AssetReportEvent> assetReportEvents)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
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

    public async Task<List<AssetReportEvent>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        return (await context.AssetReportEventEntities
                .OrderBy(x => x.ReportDate)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }

    public async Task<List<AssetReportEvent>> GetAsync(List<Guid> instrumentIds)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        return (await context.AssetReportEventEntities
                .Where(x => instrumentIds.Contains(x.InstrumentId))
                .OrderBy(x => x.ReportDate)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }
}
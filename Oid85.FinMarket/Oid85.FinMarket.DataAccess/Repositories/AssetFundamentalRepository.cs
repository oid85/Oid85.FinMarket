using Mapster;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class AssetFundamentalRepository(
    FinMarketContext context) 
    : IAssetFundamentalRepository
{
    public async Task AddOrUpdateAsync(List<AssetFundamental> assetFundamentals)
    {
        if (assetFundamentals.Count == 0)
            return;

        foreach (var asset in assetFundamentals)
        {
            var lastAsset = await GetLastAsync(asset.InstrumentId);

            if (lastAsset is null)
            {
                var entity = asset.Adapt<AssetFundamentalEntity>();
                context.AssetFundamentalEntities.Add(entity);
                continue;
            }
            
            if (asset.Date > lastAsset.Date)
            {
                var entity = asset.Adapt<AssetFundamentalEntity>();
                context.AssetFundamentalEntities.Add(entity);
            }
        }
    }

    public Task<List<AssetFundamental>> GetAsync(Guid instrumentId) =>
        context.AssetFundamentalEntities
            .Where(x => instrumentId == x.InstrumentId)
            .OrderBy(x => x.Date)
            .Select(x => x.Adapt<AssetFundamental>())
            .ToListAsync();

    public async Task<AssetFundamental?> GetLastAsync(Guid instrumentId)
    {
        bool exists = await context.AssetFundamentalEntities
            .Where(x => instrumentId == x.InstrumentId)
            .AnyAsync();

        if (!exists)
            return null;
        
        var maxDate = await context.AssetFundamentalEntities
            .Where(x => instrumentId == x.InstrumentId)
            .MaxAsync(x => x.Date);

        var entity = await context.AssetFundamentalEntities
            .Where(x => instrumentId == x.InstrumentId)
            .FirstAsync(x => x.Date == maxDate);

        var assetFundamental = entity.Adapt<AssetFundamental>();
        
        return assetFundamental;
    }
}

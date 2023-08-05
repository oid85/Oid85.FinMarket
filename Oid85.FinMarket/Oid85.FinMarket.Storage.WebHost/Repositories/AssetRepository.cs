using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.DAL;
using Oid85.FinMarket.DAL.Entities;
using Oid85.FinMarket.Models;

namespace Oid85.FinMarket.Storage.WebHost.Repositories;

public class AssetRepository
{
    private readonly IServiceScopeFactory _scopeFactory;
    
    public AssetRepository(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<List<Asset>> GetAllAssetsAsync()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var assets = new List<Asset>();
            
            var context = scope.ServiceProvider.GetRequiredService<StorageDataBaseContext>();
                
            var assetEntities = await context.AssetEntities.ToListAsync();

            for (int i = 0; i < assetEntities.Count; i++)
            {
                var asset = new Asset
                {
                    Id = assetEntities[i].Id,
                    Ticker = assetEntities[i].Ticker,
                    Name = assetEntities[i].Name,
                    Figi = assetEntities[i].Figi,
                    Sector = assetEntities[i].Sector
                };

                assets.Add(asset);
            }
            
            return assets;
        }
    }

    public async Task CreateOrUpdateAsync(Asset asset)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<StorageDataBaseContext>();
            
            var assetEntity = await context.AssetEntities.FirstOrDefaultAsync(entity => entity.Ticker == asset.Ticker);

            if (assetEntity == null)
            {
                var assetEntityForInsert = new AssetEntity()
                {
                    Ticker = asset.Ticker,
                    Name = asset.Name,
                    Figi = asset.Figi,
                    Sector = asset.Sector
                };

                await context.AssetEntities.AddAsync(assetEntityForInsert);
                await context.SaveChangesAsync();
            }

            else
            {
                assetEntity.Ticker = asset.Ticker;
                assetEntity.Name = asset.Name;
                assetEntity.Figi = asset.Figi;
                assetEntity.Sector = asset.Sector;
                
                await context.SaveChangesAsync();
            }
        }
    }
}
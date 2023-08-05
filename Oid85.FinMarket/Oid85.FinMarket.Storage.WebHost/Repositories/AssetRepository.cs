using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.DAL;
using Oid85.FinMarket.Models;

namespace Oid85.FinMarket.Storage.WebHost.Repositories;

public class AssetRepository
{
    private readonly StorageDataBaseContext _context;

    public AssetRepository(StorageDataBaseContext context)
    {
        _context = context;
    }

    public async Task<List<Asset>> GetAllAssetsAsync()
    {
        var assets = new List<Asset>();

        var assetEntities = await _context.AssetEntities.ToListAsync();

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
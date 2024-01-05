using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.DAL;
using Oid85.FinMarket.DAL.Entities;
using Oid85.FinMarket.Models;

namespace Oid85.FinMarket.Storage.WebHost.Repositories;

public class StockRepository
{
    private readonly IServiceScopeFactory _scopeFactory;
    
    public StockRepository(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task<List<Stock>> GetAllStocksAsync()
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var stocks = new List<Stock>();
            
            var context = scope.ServiceProvider.GetRequiredService<StorageDataBaseContext>();
                
            var stockEntities = await context.StockEntities.ToListAsync();

            for (int i = 0; i < stockEntities.Count; i++)
            {
                var asset = new Stock
                {
                    Id = stockEntities[i].Id,
                    Ticker = stockEntities[i].Ticker,
                    Name = stockEntities[i].Name,
                    Figi = stockEntities[i].Figi,
                    Sector = stockEntities[i].Sector,
                    InWatchList = stockEntities[i].InWatchList
                };

                stocks.Add(asset);
            }
            
            return stocks;
        }
    }

    public async Task CreateOrUpdateAsync(Stock stock)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StorageDataBaseContext>();
            
        var stockEntity = await context.StockEntities.FirstOrDefaultAsync(entity => entity.Ticker == stock.Ticker);

        if (stockEntity == null)
        {
            var entityForInsert = new StockEntity()
            {
                Ticker = stock.Ticker,
                Name = stock.Name,
                Figi = stock.Figi,
                Sector = stock.Sector,
                InWatchList = stock.InWatchList
            };

            await context.StockEntities.AddAsync(entityForInsert);
            await context.SaveChangesAsync();
        }

        else
        {
            stockEntity.Ticker = stock.Ticker;
            stockEntity.Name = stock.Name;
            stockEntity.Figi = stock.Figi;
            stockEntity.Sector = stock.Sector;
                
            await context.SaveChangesAsync();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class FeerGreedRepository(
    ILogger logger,
    FinMarketContext context) 
    : IFeerGreedRepository
{
    public async Task AddAsync(List<FearGreedIndex> indexes)
    {
        if (indexes is [])
            return;

        var entities = new List<FearGreedIndexEntity>();
        
        foreach (var index in indexes)
            if (!await context.FearGreedIndexEntities
                    .AnyAsync(x => x.Date == index.Date))
                entities.Add(DataAccessMapper.Map(index));
            else
                await UpdateFieldsAsync(index);

        await context.FearGreedIndexEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateFieldsAsync(FearGreedIndex index)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.FearGreedIndexEntities
                .Where(x => x.Date == index.Date)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(entity => entity.MarketMomentum, index.MarketMomentum)
                    .SetProperty(entity => entity.MarketVolatility, index.MarketVolatility)
                    .SetProperty(entity => entity.StockPriceBreadth, index.StockPriceBreadth)
                    .SetProperty(entity => entity.StockPriceStrength, index.StockPriceStrength)
                    .SetProperty(entity => entity.Value, index.Value));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }    
    
    public async Task<List<FearGreedIndex>> GetAsync(DateOnly from, DateOnly to)
    {
        var entities = await context.FearGreedIndexEntities
            .Where(x => 
                x.Date >= from &&
                x.Date <= to)
            .OrderBy(x => x.Date)
            .AsNoTracking()
            .ToListAsync();

        return entities.Count == 0 ? [] : entities.Select(DataAccessMapper.Map).ToList();
    }
}
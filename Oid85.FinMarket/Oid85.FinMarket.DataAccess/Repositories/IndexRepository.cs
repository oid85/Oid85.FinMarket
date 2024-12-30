using Mapster;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;
using Index = Oid85.FinMarket.Domain.Models.Index;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class IndexRepository(
    FinMarketContext context) : IIndexRepository
{
    public async Task AddOrUpdateAsync(List<Index> indicatives)
    {
        if (indicatives.Count == 0)
            return;
        
        foreach (var indicative in indicatives)
        {
            var entity = context.IndicativeEntities
                .FirstOrDefault(x => 
                    x.Ticker == indicative.Ticker);

            if (entity is null)
            {
                entity = indicative.Adapt<IndexEntity>();
                await context.IndicativeEntities.AddAsync(entity);
            }

            else
            {
                entity.Adapt(indicative);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<Index>> GetAllAsync() =>
        (await context.IndicativeEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .ToListAsync())
        .Select(x => x.Adapt<Index>())
        .ToList();

    public async Task<List<Index>> GetWatchListAsync() =>
        (await context.IndicativeEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .OrderBy(x => x.Ticker)
            .ToListAsync())
        .Select(x => x.Adapt<Index>())
        .ToList();

    public async Task<Index?> GetByTickerAsync(string ticker)
    {
        var entity = await context.IndicativeEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Ticker == ticker);

        return entity?.Adapt<Index>();
    }
}
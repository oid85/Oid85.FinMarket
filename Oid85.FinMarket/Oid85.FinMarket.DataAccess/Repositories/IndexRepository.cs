using Mapster;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class IndexRepository(
    FinMarketContext context) : IIndexRepository
{
    public async Task AddOrUpdateAsync(List<FinIndex> indicatives)
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
                entity = indicative.Adapt<FinIndexEntity>();
                await context.IndicativeEntities.AddAsync(entity);
            }

            else
            {
                entity.Adapt(indicative);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<FinIndex>> GetAllAsync() =>
        (await context.IndicativeEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .ToListAsync())
        .Select(x => x.Adapt<FinIndex>())
        .ToList();

    public async Task<List<FinIndex>> GetWatchListAsync() =>
        (await context.IndicativeEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .OrderBy(x => x.Ticker)
            .ToListAsync())
        .Select(x => x.Adapt<FinIndex>())
        .ToList();

    public async Task<FinIndex?> GetByTickerAsync(string ticker)
    {
        var entity = await context.IndicativeEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Ticker == ticker);

        return entity?.Adapt<FinIndex>();
    }
}
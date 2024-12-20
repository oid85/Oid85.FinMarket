using Mapster;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class IndicativeRepository(
    FinMarketContext context) : IIndicativeRepository
{
    public async Task AddOrUpdateAsync(List<Indicative> indicatives)
    {
        if (!indicatives.Any())
            return;
        
        foreach (var indicative in indicatives)
        {
            var entity = context.IndicativeEntities
                .FirstOrDefault(x => 
                    x.Ticker == indicative.Ticker);

            if (entity is null)
            {
                entity = indicative.Adapt<IndicativeEntity>();
                await context.IndicativeEntities.AddAsync(entity);
            }

            else
            {
                entity.Adapt(indicative);
            }
        }

        await context.SaveChangesAsync();
    }

    public Task<List<Indicative>> GetAllAsync() =>
        context.IndicativeEntities
            .Where(x => !x.IsDeleted)
            .Select(x => x.Adapt<Indicative>())
            .OrderBy(x => x.Ticker)
            .ToListAsync();

    public Task<List<Indicative>> GetWatchListAsync() =>
        context.IndicativeEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .Select(x => x.Adapt<Indicative>())
            .OrderBy(x => x.Ticker)
            .ToListAsync();

    public async Task<Indicative?> GetByTickerAsync(string ticker)
    {
        var entity = await context.IndicativeEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Ticker == ticker);

        return entity?.Adapt<Indicative>();
    }
}
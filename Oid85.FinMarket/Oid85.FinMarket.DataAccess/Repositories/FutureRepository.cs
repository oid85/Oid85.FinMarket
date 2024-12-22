using Mapster;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class FutureRepository(
    FinMarketContext context) : IFutureRepository
{
    public async Task AddOrUpdateAsync(List<Future> futures)
    {
        if (!futures.Any())
            return;
        
        foreach (var future in futures)
        {
            var entity = context.FutureEntities
                .FirstOrDefault(x => 
                    x.Ticker == future.Ticker);

            if (entity is null)
            {
                entity = future.Adapt<FutureEntity>();
                await context.FutureEntities.AddAsync(entity);
            }

            else
            {
                entity.Adapt(future);
            }
        }

        await context.SaveChangesAsync();
    }

    public Task<List<Future>> GetAllAsync() =>
        context.FutureEntities
            .Where(x => !x.IsDeleted)
            .Select(x => x.Adapt<Future>())
            .OrderBy(x => x.Ticker)
            .ToListAsync();

    public Task<List<Future>> GetWatchListAsync() =>
        context.FutureEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .Select(x => x.Adapt<Future>())
            .OrderBy(x => x.Ticker)
            .ToListAsync();

    public async Task<Future?> GetByTickerAsync(string ticker)
    {
        var entity = await context.FutureEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Ticker == ticker);

        return entity?.Adapt<Future>();
    }
}
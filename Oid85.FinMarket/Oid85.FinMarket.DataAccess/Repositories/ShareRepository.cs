using Mapster;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class ShareRepository(
    FinMarketContext context) : IShareRepository
{
    public async Task AddOrUpdateAsync(List<Share> shares)
    {
        if (!shares.Any())
            return;
        
        foreach (var share in shares)
        {
            var entity = context.ShareEntities
                .FirstOrDefault(x => 
                    x.Ticker == share.Ticker);

            if (entity is null)
            {
                entity = share.Adapt<ShareEntity>();
                await context.ShareEntities.AddAsync(entity);
            }

            else
            {
                entity.Adapt(share);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<Share>> GetAllAsync() =>
        (await context.ShareEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .ToListAsync())
        .Select(x => x.Adapt<Share>())
        .ToList();

    public async Task<List<Share>> GetWatchListAsync() =>
        (await context.ShareEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .OrderBy(x => x.Ticker)
            .ToListAsync())
        .Select(x => x.Adapt<Share>())
        .ToList();

    public async Task<Share?> GetByTickerAsync(string ticker)
    {
        var entity = await context.ShareEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity?.Adapt<Share>();
    }
}
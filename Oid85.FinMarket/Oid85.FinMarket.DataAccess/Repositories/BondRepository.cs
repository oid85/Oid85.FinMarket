using Mapster;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class BondRepository(
    FinMarketContext context) : IBondRepository
{
    public async Task AddOrUpdateAsync(List<Bond> bonds)
    {        
        if (!bonds.Any())
            return;
        
        foreach (var bond in bonds)
        {
            var entity = context.BondEntities
                .FirstOrDefault(x => 
                    x.Ticker == bond.Ticker);

            if (entity is null)
            {
                entity = bond.Adapt<BondEntity>();
                await context.BondEntities.AddAsync(entity);
            }

            else
            {
                entity.Adapt(bond);
            }
        }

        await context.SaveChangesAsync();
    }

    public Task<List<Bond>> GetAllAsync() =>
        context.BondEntities
            .Where(x => !x.IsDeleted)
            .Select(x => x.Adapt<Bond>())
            .OrderBy(x => x.Ticker)
            .ToListAsync();

    public Task<List<Bond>> GetWatchListAsync() =>
        context.BondEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .Select(x => x.Adapt<Bond>())
            .OrderBy(x => x.Ticker)
            .ToListAsync();

    public async Task<Bond?> GetByTickerAsync(string ticker)
    {
        var entity = await context.BondEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity?.Adapt<Bond>();
    }
}
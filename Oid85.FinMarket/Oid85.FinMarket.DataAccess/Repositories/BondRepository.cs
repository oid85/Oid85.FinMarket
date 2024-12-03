using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class BondRepository(
    FinMarketContext context,
    IMapper mapper) : IBondRepository
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
                entity = mapper.Map<BondEntity>(bond);
                await context.BondEntities.AddAsync(entity);
            }

            else
            {
                entity.Isin = bond.Isin;
                entity.Figi = bond.Figi;
                entity.Description = bond.Description;
                entity.Sector = bond.Sector;
            }
        }

        await context.SaveChangesAsync();
    }

    public Task<List<Bond>> GetBondsAsync() =>
        context.BondEntities
            .Where(x => x.IsActive)
            .Select(x => mapper.Map<Bond>(x))
            .ToListAsync();
}
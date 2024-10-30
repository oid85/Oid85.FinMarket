using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class BondRepository : IBondRepository
{
    private readonly FinMarketContext _context;
    private readonly IMapper _mapper;
    
    public BondRepository(
        FinMarketContext context, 
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task AddOrUpdateAsync(List<Bond> bonds)
    {        
        if (!bonds.Any())
            return;
        
        foreach (var bond in bonds)
        {
            var entity = _context.BondEntities
                .FirstOrDefault(x => 
                    x.Ticker == bond.Ticker);

            if (entity is null)
            {
                entity = _mapper.Map<BondEntity>(bond);
                await _context.BondEntities.AddAsync(entity);
            }

            else
            {
                entity.Isin = bond.Isin;
                entity.Figi = bond.Figi;
                entity.Description = bond.Description;
                entity.Sector = bond.Sector;
            }
        }

        await _context.SaveChangesAsync();
    }

    public Task<List<Bond>> GetBondsAsync() =>
        _context.BondEntities
            .Select(x => _mapper.Map<Bond>(x))
            .ToListAsync();
}
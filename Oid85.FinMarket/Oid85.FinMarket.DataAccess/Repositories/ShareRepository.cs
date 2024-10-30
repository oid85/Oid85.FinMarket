using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class ShareRepository : IShareRepository
{
    private readonly FinMarketContext _context;
    private readonly IMapper _mapper;
    
    public ShareRepository(
        FinMarketContext context, 
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddOrUpdateAsync(List<Share> shares)
    {
        if (!shares.Any())
            return;
        
        foreach (var share in shares)
        {
            var entity = _context.ShareEntities
                .FirstOrDefault(x => 
                    x.Ticker == share.Ticker);

            if (entity is null)
            {
                entity = _mapper.Map<ShareEntity>(share);
                await _context.ShareEntities.AddAsync(entity);
            }

            else
            {
                entity.Isin = share.Isin;
                entity.Figi = share.Figi;
                entity.Description = share.Description;
                entity.Sector = share.Sector;
            }
        }

        await _context.SaveChangesAsync();
    }

    public Task<List<Share>> GetSharesAsync() =>
        _context.ShareEntities
            .Select(x => _mapper.Map<Share>(x))
            .ToListAsync();

    public Task<List<Share>> GetMoexIndexSharesAsync() =>
        _context.ShareEntities
            .Where(x => x.InIrusIndex)
            .Select(x => _mapper.Map<Share>(x))
            .ToListAsync();

    public Task<List<Share>> GetPortfolioSharesAsync() =>
        _context.ShareEntities
            .Where(x => x.InPortfolio)
            .Select(x => _mapper.Map<Share>(x))
            .ToListAsync();

    public Task<List<Share>> GetWatchListSharesAsync() =>
        _context.ShareEntities
            .Where(x => x.InWatchList)
            .Select(x => _mapper.Map<Share>(x))
            .ToListAsync();

    public async Task<Share?> GetShareByTickerAsync(string ticker)
    {
        var entity = await _context.ShareEntities
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity is null 
            ? null 
            : _mapper.Map<Share>(entity);
    }
}
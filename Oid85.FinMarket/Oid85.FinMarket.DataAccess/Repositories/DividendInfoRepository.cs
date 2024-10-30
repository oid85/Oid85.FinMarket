using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class DividendInfoRepository : IDividendInfoRepository
{
    private readonly FinMarketContext _context;
    private readonly IMapper _mapper;
    
    public DividendInfoRepository(
        FinMarketContext context, 
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddOrUpdateAsync(List<DividendInfo> dividendInfos)
    {
        if (!dividendInfos.Any())
            return;
        
        foreach (var dividendInfo in dividendInfos)
        {
            var entity = _context.DividendInfoEntities
                .FirstOrDefault(x => 
                    x.Ticker == dividendInfo.Ticker &&
                    x.RecordDate == dividendInfo.RecordDate &&
                    x.DeclaredDate == dividendInfo.DeclaredDate);

            if (entity is null)
            {
                entity = _mapper.Map<DividendInfoEntity>(dividendInfo);
                await _context.DividendInfoEntities.AddAsync(entity);
            }

            else
            {
                entity.Dividend = dividendInfo.Dividend;
                entity.DividendPrc = dividendInfo.DividendPrc;
            }
        }

        await _context.SaveChangesAsync();
    }

    public Task<List<DividendInfo>> GetDividendInfosAsync() =>
        _context.DividendInfoEntities
            .Select(x => _mapper.Map<DividendInfo>(x))
            .ToListAsync();
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class DividendInfoRepository(
    FinMarketContext context,
    IMapper mapper) : IDividendInfoRepository
{
    public async Task AddOrUpdateAsync(List<DividendInfo> dividendInfos)
    {
        if (!dividendInfos.Any())
            return;
        
        foreach (var dividendInfo in dividendInfos)
        {
            var entity = context.DividendInfoEntities
                .FirstOrDefault(x => 
                    x.Ticker == dividendInfo.Ticker &&
                    x.RecordDate == dividendInfo.RecordDate &&
                    x.DeclaredDate == dividendInfo.DeclaredDate);

            if (entity is null)
            {
                entity = mapper.Map<DividendInfoEntity>(dividendInfo);
                await context.DividendInfoEntities.AddAsync(entity);
            }

            else
            {
                entity.Dividend = dividendInfo.Dividend;
                entity.DividendPrc = dividendInfo.DividendPrc;
            }
        }

        await context.SaveChangesAsync();
    }

    public Task<List<DividendInfo>> GetDividendInfosAsync() =>
        context.DividendInfoEntities
            .Select(x => mapper.Map<DividendInfo>(x))
            .ToListAsync();
}

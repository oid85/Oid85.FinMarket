using Mapster;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class DividendInfoRepository(
    FinMarketContext context) : IDividendInfoRepository
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
                entity = dividendInfo.Adapt<DividendInfoEntity>();
                await context.DividendInfoEntities.AddAsync(entity);
            }

            else
            {
                entity.Adapt(dividendInfo);
            }
        }

        await context.SaveChangesAsync();
    }

    public Task<List<DividendInfo>> GetAllAsync() =>
        context.DividendInfoEntities
            .Select(x => x.Adapt<DividendInfo>())
            .OrderBy(x => x.DividendPrc)
            .ToListAsync();
    
    public Task<List<DividendInfo>> GetAsync(
        List<string> tickers, DateTime from, DateTime to) =>
        context.DividendInfoEntities
            .Where(x => tickers.Contains(x.Ticker))
            .Where(x => 
                x.RecordDate >= DateOnly.FromDateTime(from) && 
                x.RecordDate <= DateOnly.FromDateTime(to))
            .Select(x => x.Adapt<DividendInfo>())
            .OrderBy(x => x.DividendPrc)
            .ToListAsync();
}

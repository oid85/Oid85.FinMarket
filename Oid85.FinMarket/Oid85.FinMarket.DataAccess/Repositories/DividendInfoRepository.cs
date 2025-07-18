﻿using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class DividendInfoRepository(
    IDbContextFactory<FinMarketContext> contextFactory)
    : IDividendInfoRepository
{
    public async Task AddOrUpdateAsync(List<DividendInfo> dividendInfos)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        if (dividendInfos is [])
            return;

        var entities = new List<DividendInfoEntity>();
        
        foreach (var dividendInfo in dividendInfos)
            if (!await context.DividendInfoEntities
                    .AnyAsync(x => 
                        x.InstrumentId == dividendInfo.InstrumentId &&
                        x.RecordDate == dividendInfo.RecordDate &&
                        x.DeclaredDate == dividendInfo.DeclaredDate))
                entities.Add(DataAccessMapper.Map(dividendInfo));

        await context.DividendInfoEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task<List<DividendInfo>> GetAllAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        return (await context.DividendInfoEntities
                .OrderBy(x => x.DividendPrc)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }

    public async Task<List<DividendInfo>> GetAsync(List<Guid> instrumentIds)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        return (await context.DividendInfoEntities
                .Where(x => instrumentIds.Contains(x.InstrumentId))
                .OrderBy(x => x.DividendPrc)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }
}

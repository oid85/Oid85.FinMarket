﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class ShareRepository(
    FinMarketContext context,
    IMapper mapper) : IShareRepository
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
                entity = mapper.Map<ShareEntity>(share);
                await context.ShareEntities.AddAsync(entity);
            }

            else
            {
                entity.Isin = share.Isin;
                entity.Figi = share.Figi;
                entity.Description = share.Description;
                entity.Sector = share.Sector;
            }
        }

        await context.SaveChangesAsync();
    }

    public Task<List<Share>> GetAllAsync() =>
        context.ShareEntities
            .Where(x => !x.IsDeleted)
            .Select(x => mapper.Map<Share>(x))
            .OrderBy(x => x.Ticker)
            .ToListAsync();

    public Task<List<Share>> GetMoexIndexAsync() =>
        context.ShareEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InIrusIndex)
            .Select(x => mapper.Map<Share>(x))
            .OrderBy(x => x.Ticker)
            .ToListAsync();

    public Task<List<Share>> GetPortfolioAsync() =>
        context.ShareEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InPortfolio)
            .Select(x => mapper.Map<Share>(x))
            .OrderBy(x => x.Ticker)
            .ToListAsync();

    public Task<List<Share>> GetWatchListAsync() =>
        context.ShareEntities
            .Where(x => !x.IsDeleted)
            .Where(x => x.InWatchList)
            .Select(x => mapper.Map<Share>(x))
            .OrderBy(x => x.Ticker)
            .ToListAsync();

    public async Task<Share?> GetByTickerAsync(string ticker)
    {
        var entity = await context.ShareEntities
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity is null 
            ? null 
            : mapper.Map<Share>(entity);
    }
}
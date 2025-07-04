﻿using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class IndexRepository(
    ILogger logger,
    IDbContextFactory<FinMarketContext> contextFactory)
    : IIndexRepository
{
    public async Task AddAsync(List<FinIndex> indexes)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        if (indexes is [])
            return;

        var entities = new List<FinIndexEntity>();
        
        foreach (var index in indexes)
            if (!await context.IndicativeEntities
                    .AnyAsync(x => 
                        x.InstrumentId == index.InstrumentId))
                entities.Add(DataAccessMapper.Map(index));

        await context.IndicativeEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.IndicativeEntities
                .Where(x => x.InstrumentId == instrumentId)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(entity => entity.LastPrice, lastPrice));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }
    
    public async Task<List<FinIndex>> GetAsync(List<string> tickers)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        
        return (await context.IndicativeEntities
                .Where(x => !x.IsDeleted)
                .Where(x => tickers.Contains(x.Ticker))
                .OrderBy(x => x.Ticker)
                .AsNoTracking()
                .ToListAsync())
            .Select(DataAccessMapper.Map)
            .ToList();
    }
}
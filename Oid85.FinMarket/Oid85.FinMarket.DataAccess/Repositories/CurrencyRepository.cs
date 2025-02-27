﻿using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class CurrencyRepository(
    ILogger logger,
    FinMarketContext context) 
    : ICurrencyRepository
{
    public async Task AddAsync(List<Currency> currencies)
    {
        if (currencies is [])
            return;

        var entities = new List<CurrencyEntity>();
        
        foreach (var currency in currencies)
            if (!await context.CurrencyEntities
                    .AnyAsync(x => x.InstrumentId == currency.InstrumentId))
                entities.Add(GetEntity(currency));

        await context.CurrencyEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.CurrencyEntities
                .Where(x => x.InstrumentId == instrumentId)
                .ExecuteUpdateAsync(
                    s => s.SetProperty(
                        entity => entity.LastPrice, lastPrice));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }
    
    public async Task<List<Currency>> GetAllAsync() =>
        (await context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<List<Currency>> GetByTickersAsync(List<string> tickers) =>
        (await context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .Where(x => tickers.Contains(x.Ticker))
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<Currency?> GetByTickerAsync(string ticker)
    {
        var entity = await context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ticker == ticker);

        return entity is null 
            ? null 
            : GetModel(entity);
    }

    public async Task<Currency?> GetByInstrumentIdAsync(Guid instrumentId)
    {
        var entity = await context.CurrencyEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);

        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    private CurrencyEntity GetEntity(Currency model)
    {
        var entity = new CurrencyEntity
        {
            Ticker = model.Ticker,
            LastPrice = model.LastPrice,
            Isin = model.Isin,
            Figi = model.Figi,
            ClassCode = model.ClassCode,
            Name = model.Name,
            IsoCurrencyName = model.IsoCurrencyName,
            InstrumentId = model.InstrumentId
        };

        return entity;
    }
    
    private Currency GetModel(CurrencyEntity entity)
    {
        var model = new Currency
        {
            Id = entity.Id,
            Ticker = entity.Ticker,
            LastPrice = entity.LastPrice,
            Isin = entity.Isin,
            Figi = entity.Figi,
            ClassCode = entity.ClassCode,
            Name = entity.Name,
            IsoCurrencyName = entity.IsoCurrencyName,
            InstrumentId = entity.InstrumentId
        };

        return model;
    }
}
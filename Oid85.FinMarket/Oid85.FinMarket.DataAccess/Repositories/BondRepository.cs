using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;
namespace Oid85.FinMarket.DataAccess.Repositories;

public class BondRepository(
    ILogger logger,
    FinMarketContext context) 
    : IBondRepository
{
    public async Task AddAsync(List<Bond> bonds)
    {        
        if (bonds is [])
            return;

        var entities = new List<BondEntity>();
        
        foreach (var bond in bonds)
            if (!await context.BondEntities
                    .AnyAsync(x => x.InstrumentId == bond.InstrumentId))
                entities.Add(GetEntity(bond));

        await context.BondEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateLastPricesAsync(Guid instrumentId, double lastPrice)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.BondEntities
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

    public async Task<List<Bond>> GetAllAsync() =>
        (await context.BondEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<List<Bond>> GetByTickersAsync(List<string> tickers) =>
        (await context.BondEntities
            .Where(x => !x.IsDeleted)
            .Where(x => tickers.Contains(x.Ticker))
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<Bond?> GetByTickerAsync(string ticker)
    {
        var entity = await context.BondEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity is null 
            ? null 
            : GetModel(entity);
    }

    public async Task<Bond?> GetByInstrumentIdAsync(Guid instrumentId)
    {
        var entity = await context.BondEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);
        
        return entity is null 
            ? null 
            : GetModel(entity);
    }

    private BondEntity GetEntity(Bond model)
    {
        var entity = new BondEntity
        {
            Ticker = model.Ticker,
            LastPrice = model.LastPrice,
            Isin = model.Isin,
            Figi = model.Figi,
            InstrumentId = model.InstrumentId,
            Name = model.Name,
            Sector = model.Sector,
            Currency = model.Currency,
            Nkd = model.Nkd,
            MaturityDate = model.MaturityDate,
            FloatingCouponFlag = model.FloatingCouponFlag,
            RiskLevel = model.RiskLevel
        };

        return entity;
    }
    
    private Bond GetModel(BondEntity entity)
    {
        var model = new Bond
        {
            Id = entity.Id,
            Ticker = entity.Ticker,
            LastPrice = entity.LastPrice,
            Isin = entity.Isin,
            Figi = entity.Figi,
            InstrumentId = entity.InstrumentId,
            Name = entity.Name,
            Sector = entity.Sector,
            Currency = entity.Currency,
            Nkd = entity.Nkd,
            MaturityDate = entity.MaturityDate,
            FloatingCouponFlag = entity.FloatingCouponFlag,
            RiskLevel = entity.RiskLevel
        };

        return model;
    }
}
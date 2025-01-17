using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Logging.Services;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class MultiplicatorRepository(
    ILogService logService,
    FinMarketContext context) 
    : IMultiplicatorRepository
{
    public async Task AddAsync(List<Multiplicator> multiplicators)
    {
        if (multiplicators is [])
            return;

        var entities = new List<MultiplicatorEntity>();
        
        foreach (var multiplicator in multiplicators)
            if (!await context.MultiplicatorEntities
                    .AnyAsync(x => 
                        x.InstrumentId == multiplicator.InstrumentId))
                entities.Add(GetEntity(multiplicator));

        await context.MultiplicatorEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateSpreadAsync(Multiplicator multiplicator)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.MultiplicatorEntities
                .Where(x => 
                    x.InstrumentId == multiplicator.InstrumentId)
                .ExecuteUpdateAsync(
                    s => s
                        .SetProperty(u => u.MarketCapitalization, multiplicator.MarketCapitalization)
                        .SetProperty(u => u.LowOfYear, multiplicator.LowOfYear)
                        .SetProperty(u => u.HighOfYear, multiplicator.HighOfYear)
                        .SetProperty(u => u.Beta, multiplicator.Beta)
                        .SetProperty(u => u.NetIncome, multiplicator.NetIncome)
                        .SetProperty(u => u.Ebitda, multiplicator.Ebitda)
                        .SetProperty(u => u.Eps, multiplicator.Eps)
                        .SetProperty(u => u.FreeCashFlow, multiplicator.FreeCashFlow)
                        .SetProperty(u => u.EvToEbitda, multiplicator.EvToEbitda)
                        .SetProperty(u => u.TotalDebtToEbitda, multiplicator.TotalDebtToEbitda)
                        .SetProperty(u => u.NetDebtToEbitda, multiplicator.NetDebtToEbitda));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            await logService.LogException(exception);
        }
    }
    
    public async Task<List<Multiplicator>> GetAllAsync() =>
        (await context.MultiplicatorEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Ticker)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<Multiplicator?> GetAsync(Guid instrumentId)
    {
        var entity = await context.MultiplicatorEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);
        
        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    private MultiplicatorEntity GetEntity(Multiplicator model)
    {
        var entity = new MultiplicatorEntity();
        
        entity.Ticker = model.Ticker;
        entity.InstrumentId = model.InstrumentId;
        entity.MarketCapitalization = model.MarketCapitalization;
        entity.LowOfYear = model.LowOfYear;
        entity.HighOfYear = model.HighOfYear;
        entity.Beta = model.Beta;
        entity.NetIncome = model.NetIncome;
        entity.Ebitda = model.Ebitda;
        entity.Eps = model.Eps;
        entity.FreeCashFlow = model.FreeCashFlow;
        entity.EvToEbitda = model.EvToEbitda;
        entity.TotalDebtToEbitda = model.TotalDebtToEbitda;
        entity.NetDebtToEbitda = model.NetDebtToEbitda;

        return entity;
    }
    
    private Multiplicator GetModel(MultiplicatorEntity entity)
    {
        var model = new Multiplicator();
        
        model.Id = entity.Id;
        model.Ticker = entity.Ticker;
        model.InstrumentId = entity.InstrumentId;
        model.MarketCapitalization = entity.MarketCapitalization;
        model.LowOfYear = entity.LowOfYear;
        model.HighOfYear = entity.HighOfYear;
        model.Beta = entity.Beta;
        model.NetIncome = entity.NetIncome;
        model.Ebitda = entity.Ebitda;
        model.Eps = entity.Eps;
        model.FreeCashFlow = entity.FreeCashFlow;
        model.EvToEbitda = entity.EvToEbitda;
        model.TotalDebtToEbitda = entity.TotalDebtToEbitda;
        model.NetDebtToEbitda = entity.NetDebtToEbitda;

        return model;
    }
}
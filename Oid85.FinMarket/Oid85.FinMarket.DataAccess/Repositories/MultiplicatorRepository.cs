using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class MultiplicatorRepository(
    ILogger logger,
    FinMarketContext context) 
    : IMultiplicatorRepository
{
    public async Task AddOrUpdateAsync(List<Multiplicator> multiplicators)
    {
        if (multiplicators is [])
            return;

        var entities = new List<MultiplicatorEntity>();
        
        foreach (var multiplicator in multiplicators)
            if (!await context.MultiplicatorEntities
                    .AnyAsync(x => 
                        x.TickerAo == multiplicator.TickerAo ||
                        x.TickerAp == multiplicator.TickerAp))
                entities.Add(GetEntity(multiplicator));
            else
                await UpdateStaticFieldsAsync(multiplicator);

        await context.MultiplicatorEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateStaticFieldsAsync(Multiplicator multiplicator)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.MultiplicatorEntities
                .Where(x => 
                    x.TickerAo == multiplicator.TickerAo ||
                    x.TickerAp == multiplicator.TickerAp)
                .ExecuteUpdateAsync(
                    s => s
                        .SetProperty(entity => entity.TotalSharesAo, multiplicator.TotalSharesAo)
                        .SetProperty(entity => entity.TotalSharesAp, multiplicator.TotalSharesAp)
                        .SetProperty(entity => entity.Beta, multiplicator.Beta)
                        .SetProperty(entity => entity.Revenue, multiplicator.Revenue)
                        .SetProperty(entity => entity.OperatingIncome, multiplicator.OperatingIncome)
                        .SetProperty(entity => entity.Pe, multiplicator.Pe)
                        .SetProperty(entity => entity.Pb, multiplicator.Pb)
                        .SetProperty(entity => entity.Pbv, multiplicator.Pbv)
                        .SetProperty(entity => entity.Ev, multiplicator.Ev)
                        .SetProperty(entity => entity.Roe, multiplicator.Roe)
                        .SetProperty(entity => entity.Roa, multiplicator.Roa)
                        .SetProperty(entity => entity.NetInterestMargin, multiplicator.NetInterestMargin)
                        .SetProperty(entity => entity.TotalDebt, multiplicator.TotalDebt)
                        .SetProperty(entity => entity.NetDebt, multiplicator.NetDebt)
                        .SetProperty(entity => entity.NetIncome, multiplicator.NetIncome)
                        .SetProperty(entity => entity.Ebitda, multiplicator.Ebitda)
                        .SetProperty(entity => entity.Eps, multiplicator.Eps)
                        .SetProperty(entity => entity.FreeCashFlow, multiplicator.FreeCashFlow));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }

    public async Task UpdateCalculateFieldsAsync(Multiplicator multiplicator)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.MultiplicatorEntities
                .Where(x => 
                    x.TickerAo == multiplicator.TickerAo ||
                    x.TickerAp == multiplicator.TickerAp)
                .ExecuteUpdateAsync(
                    s => s
                        .SetProperty(entity => entity.MarketCapitalization, multiplicator.MarketCapitalization)
                        .SetProperty(entity => entity.EvToEbitda, multiplicator.EvToEbitda)
                        .SetProperty(entity => entity.TotalDebtToEbitda, multiplicator.TotalDebtToEbitda)
                        .SetProperty(entity => entity.NetDebtToEbitda, multiplicator.NetDebtToEbitda));
            
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
            
        catch (Exception exception)
        {
            await transaction.RollbackAsync();
            logger.Error(exception);
        }
    }

    public async Task<List<Multiplicator>> GetAllAsync() =>
        (await context.MultiplicatorEntities
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.TickerAo)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<Multiplicator?> GetAsync(string ticker)
    {
        var entity = await context.MultiplicatorEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => 
                x.TickerAo == ticker ||
                x.TickerAp == ticker);
        
        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    private MultiplicatorEntity GetEntity(Multiplicator model)
    {
        var entity = new MultiplicatorEntity
        {
            TickerAo = model.TickerAo,
            TickerAp = model.TickerAp,
            TotalSharesAo = model.TotalSharesAo,
            TotalSharesAp = model.TotalSharesAp,
            Beta = model.Beta,
            Revenue = model.Revenue,
            OperatingIncome = model.OperatingIncome,
            Pe = model.Pe,
            Pb = model.Pb,
            Pbv = model.Pbv,
            Ev = model.Ev,
            Roe = model.Roe,
            Roa = model.Roa,
            NetInterestMargin = model.NetInterestMargin,
            TotalDebt = model.TotalDebt,
            NetDebt = model.NetDebt,
            MarketCapitalization = model.MarketCapitalization,
            NetIncome = model.NetIncome,
            Ebitda = model.Ebitda,
            Eps = model.Eps,
            FreeCashFlow = model.FreeCashFlow,
            EvToEbitda = model.EvToEbitda,
            TotalDebtToEbitda = model.TotalDebtToEbitda,
            NetDebtToEbitda = model.NetDebtToEbitda
        };

        return entity;
    }
    
    private Multiplicator GetModel(MultiplicatorEntity entity)
    {
        var model = new Multiplicator
        {
            Id = entity.Id,
            TickerAo = entity.TickerAo,
            TickerAp = entity.TickerAp,
            TotalSharesAo = entity.TotalSharesAo,
            TotalSharesAp = entity.TotalSharesAp,
            Beta = entity.Beta,
            Revenue = entity.Revenue,
            OperatingIncome = entity.OperatingIncome,
            Pe = entity.Pe,
            Pb = entity.Pb,
            Pbv = entity.Pbv,
            Ev = entity.Ev,
            Roe = entity.Roe,
            Roa = entity.Roa,
            NetInterestMargin = entity.NetInterestMargin,
            TotalDebt = entity.TotalDebt,
            NetDebt = entity.NetDebt,
            MarketCapitalization = entity.MarketCapitalization,
            NetIncome = entity.NetIncome,
            Ebitda = entity.Ebitda,
            Eps = entity.Eps,
            FreeCashFlow = entity.FreeCashFlow,
            EvToEbitda = entity.EvToEbitda,
            TotalDebtToEbitda = entity.TotalDebtToEbitda,
            NetDebtToEbitda = entity.NetDebtToEbitda
        };

        return model;
    }
}
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
                await UpdateSpreadAsync(multiplicator);

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
                    x.TickerAo == multiplicator.TickerAo ||
                    x.TickerAp == multiplicator.TickerAp)
                .ExecuteUpdateAsync(
                    s => s
                        .SetProperty(u => u.TotalSharesAo, multiplicator.TotalSharesAo)
                        .SetProperty(u => u.TotalSharesAp, multiplicator.TotalSharesAp)
                        .SetProperty(u => u.Beta, multiplicator.Beta)
                        .SetProperty(u => u.Revenue, multiplicator.Revenue)
                        .SetProperty(u => u.OperatingIncome, multiplicator.OperatingIncome)
                        .SetProperty(u => u.Pe, multiplicator.Pe)
                        .SetProperty(u => u.Pb, multiplicator.Pb)
                        .SetProperty(u => u.Pbv, multiplicator.Pbv)
                        .SetProperty(u => u.Ev, multiplicator.Ev)
                        .SetProperty(u => u.Roe, multiplicator.Roe)
                        .SetProperty(u => u.Roa, multiplicator.Roa)
                        .SetProperty(u => u.NetInterestMargin, multiplicator.NetInterestMargin)
                        .SetProperty(u => u.TotalDebt, multiplicator.TotalDebt)
                        .SetProperty(u => u.NetDebt, multiplicator.NetDebt)
                        .SetProperty(u => u.MarketCapitalization, multiplicator.MarketCapitalization)
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
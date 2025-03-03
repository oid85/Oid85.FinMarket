using Microsoft.EntityFrameworkCore;
using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Mapping;
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
                entities.Add(DataAccessMapper.Map(multiplicator));
            else
                await UpdateStaticFieldsAsync(multiplicator);

        await context.MultiplicatorEntities.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    private async Task UpdateStaticFieldsAsync(Multiplicator multiplicator)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            await context.MultiplicatorEntities
                .Where(x => 
                    x.TickerAo == multiplicator.TickerAo ||
                    x.TickerAp == multiplicator.TickerAp)
                .ExecuteUpdateAsync(x => x
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
                .ExecuteUpdateAsync(x => x
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
        .Select(DataAccessMapper.Map)
        .ToList();

    public async Task<Multiplicator?> GetAsync(string ticker)
    {
        var entity = await context.MultiplicatorEntities
            .Where(x => !x.IsDeleted)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => 
                x.TickerAo == ticker || 
                x.TickerAp == ticker);
        
        return entity is null ? null : DataAccessMapper.Map(entity);
    }
}
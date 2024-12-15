using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class FutureRepository(
    FinMarketContext context,
    IMapper mapper) : IFutureRepository
{
    public async Task AddOrUpdateAsync(List<Future> futures)
    {
        if (!futures.Any())
            return;
        
        foreach (var future in futures)
        {
            var entity = context.FutureEntities
                .FirstOrDefault(x => 
                    x.Ticker == future.Ticker);

            if (entity is null)
            {
                entity = mapper.Map<FutureEntity>(future);
                await context.FutureEntities.AddAsync(entity);
            }

            else
            {
                entity.Figi = future.Figi;
                entity.Description = future.Description;
                entity.ExpirationDate = future.ExpirationDate;
            }
        }

        await context.SaveChangesAsync();
    }

    public Task<List<Future>> GetFuturesAsync() =>
        context.FutureEntities
            .Where(x => x.IsActive)
            .Select(x => mapper.Map<Future>(x))
            .ToListAsync();

    public Task<List<Future>> GetPortfolioFuturesAsync() =>
        context.FutureEntities
            .Where(x => x.InPortfolio)
            .Select(x => mapper.Map<Future>(x))
            .ToListAsync();

    public Task<List<Future>> GetWatchListFuturesAsync() =>
        context.FutureEntities
            .Where(x => x.InWatchList)
            .Select(x => mapper.Map<Future>(x))
            .ToListAsync();

    public async Task<Future?> GetFutureByTickerAsync(string ticker)
    {
        var entity = await context.FutureEntities
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity is null 
            ? null 
            : mapper.Map<Future>(entity);
    }
}
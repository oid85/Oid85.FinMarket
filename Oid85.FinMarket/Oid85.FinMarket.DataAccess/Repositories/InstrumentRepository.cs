using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class InstrumentRepository(
    FinMarketContext context,
    IShareRepository shareRepository,
    IFutureRepository futureRepository,
    IBondRepository bondRepository,
    ICurrencyRepository currencyRepository,
    IIndexRepository indexRepository) 
    : IInstrumentRepository
{
    public async Task AddOrUpdateAsync(List<Instrument> tickers)
    {
        if (tickers is [])
            return;
        
        foreach (var ticker in tickers)
        {
            var entity = context.InstrumentEntities
                .FirstOrDefault(x => 
                    x.InstrumentId == ticker.InstrumentId);

            if (entity is null)
            {
                SetEntity(ref entity, ticker);
                
                if (entity is not null)
                    await context.InstrumentEntities.AddAsync(entity);
            }

            else
            {
                SetEntity(ref entity, ticker);
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task<List<Instrument>> GetAllAsync() =>
        (await context.InstrumentEntities
            .OrderBy(x => x.Name)
            .AsNoTracking()
            .ToListAsync())
        .Select(GetModel)
        .ToList();

    public async Task<List<Instrument>> GetWatchListAsync()
    {
        var shareInstrumentIds = (await shareRepository.GetWatchListAsync()).Select(x => x.InstrumentId).ToList();
        var futureInstrumentIds = (await futureRepository.GetWatchListAsync()).Select(x => x.InstrumentId).ToList();
        var bondInstrumentIds = (await bondRepository.GetWatchListAsync()).Select(x => x.InstrumentId).ToList();
        var currencyInstrumentIds = (await currencyRepository.GetWatchListAsync()).Select(x => x.InstrumentId).ToList();
        var indexInstrumentIds = (await indexRepository.GetWatchListAsync()).Select(x => x.InstrumentId).ToList();

        bool InstrumentIdInWatchList(Guid instrumentId)
        {
            bool result = shareInstrumentIds.Contains(instrumentId);
            result |= futureInstrumentIds.Contains(instrumentId);
            result |= bondInstrumentIds.Contains(instrumentId);
            result |= currencyInstrumentIds.Contains(instrumentId);
            result |= indexInstrumentIds.Contains(instrumentId);

            return result;
        }

        var instruments = (await GetAllAsync())
            .Where(x => InstrumentIdInWatchList(x.InstrumentId))
            .ToList();
        
        return instruments;
    }

    public async Task<Instrument?> GetByInstrumentIdAsync(Guid instrumentId)
    {
        var entity = await context.InstrumentEntities
            .FirstOrDefaultAsync(x => x.InstrumentId == instrumentId);
        
        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    public async Task<Instrument?> GetByNameAsync(string name)
    {
        var entity = await context.InstrumentEntities
            .FirstOrDefaultAsync(x => x.Name == name);
        
        return entity is null 
            ? null 
            : GetModel(entity);
    }
    
    public async Task<Instrument?> GetByTickerAsync(string ticker)
    {
        var entity = await context.InstrumentEntities
            .FirstOrDefaultAsync(x => x.Ticker == ticker);
        
        return entity is null 
            ? null 
            : GetModel(entity);
    }

    public async Task<(double LowTargetPrice, double HighTargetPrice)> GetTargetPricesAsync(Guid instrumentId)
    {
        var share = await shareRepository.GetByInstrumentIdAsync(instrumentId);

        if (share is not null)
            return (share.LowTargetPrice, share.HighTargetPrice);
        
        var future = await futureRepository.GetByInstrumentIdAsync(instrumentId);

        if (future is not null)
            return (future.LowTargetPrice, future.HighTargetPrice);
        
        var currency = await currencyRepository.GetByInstrumentIdAsync(instrumentId);

        if (currency is not null)
            return (currency.LowTargetPrice, currency.HighTargetPrice);
        
        var bond = await bondRepository.GetByInstrumentIdAsync(instrumentId);

        if (bond is not null)
            return (bond.LowTargetPrice, bond.HighTargetPrice);
        
        var index = await indexRepository.GetByInstrumentIdAsync(instrumentId);

        if (index is not null)
            return (index.LowTargetPrice, index.HighTargetPrice);
        
        return (0.0, 0.0);
    }

    private void SetEntity(ref InstrumentEntity? entity, Instrument model)
    {
        entity ??= new InstrumentEntity();
        
        entity.InstrumentId = model.InstrumentId;
        entity.Ticker = model.Ticker;
        entity.Name = model.Name;
        entity.Type = model.Type;
    }
    
    private Instrument GetModel(InstrumentEntity entity)
    {
        var model = new Instrument
        {
            Id = entity.Id,
            InstrumentId = entity.InstrumentId,
            Ticker = entity.Ticker,
            Name = entity.Name,
            Type = entity.Type
        };

        return model;
    }
}
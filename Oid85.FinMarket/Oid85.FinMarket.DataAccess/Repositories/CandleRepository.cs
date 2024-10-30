using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class CandleRepository : ICandleRepository
{
    private readonly FinMarketContext _context;
    private readonly IMapper _mapper;
    
    public CandleRepository(
        FinMarketContext context, 
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task AddOrUpdateAsync(List<Candle> candles)
    {
        if (!candles.Any())
            return;
        
        var lastEntity = await GetLastAsync(
            candles.First().Ticker, candles.First().Timeframe);

        if (lastEntity is null)
        {
            var entities = _mapper.Map<List<CandleEntity>>(candles);
            await _context.CandleEntities.AddRangeAsync(entities);
        }
        
        else
        {
            if (!lastEntity.IsComplete)
            {
                var candle = candles.First(x => x.Date == lastEntity.Date);
                
                lastEntity.Open = candle.Open;
                lastEntity.Close = candle.Close;
                lastEntity.High = candle.High;
                lastEntity.Low = candle.Low;
                lastEntity.Volume = candle.Volume;
                lastEntity.IsComplete = candle.IsComplete;
            }

            var entities = candles
                .Select(x => _mapper.Map<CandleEntity>(x))
                .Where(x => x.Date > lastEntity.Date);
                
            await _context.CandleEntities.AddRangeAsync(entities);  
        }

        await _context.SaveChangesAsync();
    }

    public Task<List<Candle>> GetCandlesAsync(string ticker, string timeframe) =>
        _context.CandleEntities
            .Where(x => ticker == x.Ticker)
            .Where(x => x.Timeframe == timeframe)
            .OrderBy(x => x.Date)
            .Select(x => _mapper.Map<Candle>(x))
            .ToListAsync();

    private async Task<CandleEntity?> GetLastAsync(string ticker, string timeframe)
    {
        bool exists = await _context.CandleEntities
            .Where(x => x.Timeframe == timeframe)
            .Where(x => x.Ticker == ticker)
            .AnyAsync();

        if (!exists)
            return null;
        
        var maxDate = await _context.CandleEntities
            .Where(x => x.Timeframe == timeframe)
            .Where(x => x.Ticker == ticker)
            .MaxAsync(x => x.Date);

        var entity = await _context.CandleEntities
            .Where(x => x.Timeframe == timeframe)
            .Where(x => x.Ticker == ticker)
            .FirstAsync(x => x.Date == maxDate);
        
        return entity;
    }
}

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
        foreach (var candle in candles)
        {
            var entity = _context.CandleEntities
                .Include(x => x.Timeframe)
                .FirstOrDefault(x => 
                    x.Ticker == candle.Ticker &&
                    x.Timeframe.Name == candle.Timeframe.Name &&
                    x.Date == candle.Date);

            if (entity is null)
            {
                entity = _mapper.Map<CandleEntity>(candle);
                await _context.AddAsync(entity);
            }

            else
            {
                if (!entity.IsComplete)
                {
                    entity.Open = candle.Open;
                    entity.Close = candle.Close;
                    entity.High = candle.High;
                    entity.Low = candle.Low;         
                    entity.Volume = candle.Volume;         
                }
            }
        }

        await _context.SaveChangesAsync();
    }

    public Task<List<Candle>> GetCandlesAsync(string ticker, Timeframe timeframe) =>
        _context.AnalyseResultEntities
            .Where(x => ticker == x.Ticker)
            .Where(x => x.Timeframe.Name == timeframe.Name)
            .OrderBy(x => x.Date)
            .Select(x => _mapper.Map<Candle>(x))
            .ToListAsync();
}
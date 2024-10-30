using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class AnalyseResultRepository : IAnalyseResultRepository
{
    private readonly FinMarketContext _context;
    private readonly IMapper _mapper;
    
    public AnalyseResultRepository(
        FinMarketContext context, 
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task AddOrUpdateAsync(List<AnalyseResult> results)
    {
        if (!results.Any())
            return;
        
        var lastEntity = await GetLastAsync(
            results.First().Ticker, results.First().Timeframe);

        if (lastEntity is null)
        {
            var entities = results
                .Select(x => _mapper.Map<AnalyseResultEntity>(x));
            
            await _context.AnalyseResultEntities.AddRangeAsync(entities);
        }
        
        else
        {
            var entities = results
                .Select(x => _mapper.Map<AnalyseResultEntity>(x))
                .Where(x => x.Date > lastEntity.Date);
                
            await _context.AnalyseResultEntities.AddRangeAsync(entities);    
        }

        await _context.SaveChangesAsync();
    }

    public Task<List<AnalyseResult>> GetAnalyseResultsAsync(
        string ticker, DateTime from, DateTime to) =>
        _context.AnalyseResultEntities
            .Where(x => ticker == x.Ticker)
            .Where(x => x.Date >= from && x.Date <= to)
            .OrderBy(x => x.Date)
            .Select(x => _mapper.Map<AnalyseResult>(x))
            .ToListAsync();

    private async Task<AnalyseResultEntity?> GetLastAsync(string ticker, string timeframe)
    {
        bool exists = await _context.AnalyseResultEntities
            .Where(x => x.Timeframe == timeframe)
            .Where(x => x.Ticker == ticker)
            .AnyAsync();

        if (!exists)
            return null;
        
        var maxDate = await _context.AnalyseResultEntities
            .Where(x => x.Timeframe == timeframe)
            .Where(x => x.Ticker == ticker)
            .MaxAsync(x => x.Date);

        var entity = await _context.AnalyseResultEntities
            .Where(x => x.Timeframe == timeframe)
            .Where(x => x.Ticker == ticker)
            .FirstAsync(x => x.Date == maxDate);
        
        return entity;
    }
}
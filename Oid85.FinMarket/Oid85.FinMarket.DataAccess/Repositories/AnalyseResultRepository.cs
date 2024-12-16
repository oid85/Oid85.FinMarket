using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Repositories;

public class AnalyseResultRepository(
    FinMarketContext context,
    IMapper mapper) : IAnalyseResultRepository
{
    public async Task AddOrUpdateAsync(List<AnalyseResult> results)
    {
        if (!results.Any())
            return;
        
        var lastEntity = await GetLastAsync(
            results.First().Ticker, results.First().Timeframe);

        if (lastEntity is null)
        {
            var entities = results
                .Select(x => mapper.Map<AnalyseResultEntity>(x));
            
            await context.AnalyseResultEntities.AddRangeAsync(entities);
        }
        
        else
        {
            var entities = results
                .Select(x => mapper.Map<AnalyseResultEntity>(x))
                .Where(x => x.Date > lastEntity.Date);
                
            await context.AnalyseResultEntities.AddRangeAsync(entities);    
        }

        await context.SaveChangesAsync();
    }

    public Task<List<AnalyseResult>> GetAsync(
        string ticker, DateTime from, DateTime to) =>
        context.AnalyseResultEntities
            .Where(x => x.Ticker == ticker)
            .Where(x => x.Date >= from && x.Date <= to)
            .OrderBy(x => x.Date)
            .Select(x => mapper.Map<AnalyseResult>(x))
            .ToListAsync();
    
    public Task<List<AnalyseResult>> GetAsync(
        List<string> tickers, DateTime from, DateTime to) =>
        context.AnalyseResultEntities
            .Where(x => tickers.Contains(x.Ticker))
            .Where(x => x.Date >= from && x.Date <= to)
            .OrderBy(x => x.Date)
            .Select(x => mapper.Map<AnalyseResult>(x))
            .ToListAsync();

    private async Task<AnalyseResultEntity?> GetLastAsync(string ticker, string timeframe)
    {
        bool exists = await context.AnalyseResultEntities
            .Where(x => x.Timeframe == timeframe)
            .Where(x => x.Ticker == ticker)
            .AnyAsync();

        if (!exists)
            return null;
        
        var maxDate = await context.AnalyseResultEntities
            .Where(x => x.Timeframe == timeframe)
            .Where(x => x.Ticker == ticker)
            .MaxAsync(x => x.Date);

        var entity = await context.AnalyseResultEntities
            .Where(x => x.Timeframe == timeframe)
            .Where(x => x.Ticker == ticker)
            .FirstAsync(x => x.Date == maxDate);
        
        return entity;
    }
}
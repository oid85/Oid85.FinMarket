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
        foreach (var result in results)
        {
            var entity = _context.AnalyseResultEntities
                .Include(x => x.Timeframe)
                .FirstOrDefault(x => 
                    x.Ticker == result.Ticker &&
                    x.Date == result.Date);

            if (entity is null)
            {
                entity = _mapper.Map<AnalyseResultEntity>(result);
                await _context.AddAsync(entity);
            }
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
}
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class MarketEventService(
    IMarketEventRepository marketEventRepository,
    IAnalyseResultRepository analyseResultRepository,
    IShareRepository shareRepository) 
    : IMarketEventService
{
    /// <inheritdoc />
    public async Task CheckSupertrendMarketEventAsync()
    {
        var instrumentIds = (await shareRepository.GetWatchListAsync())
            .Select(s => s.InstrumentId)
            .ToList();
        
        foreach (var instrumentId in instrumentIds)
        {
            var twoLastAnalyseResults = await analyseResultRepository
                .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.Supertrend);
        }
    }
}
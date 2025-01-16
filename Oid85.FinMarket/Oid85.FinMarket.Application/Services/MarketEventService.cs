using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class MarketEventService(
    IMarketEventRepository marketEventRepository,
    IAnalyseResultRepository analyseResultRepository) 
    : IMarketEventService
{
    /// <inheritdoc />
    public async Task CheckSupertrendMarketEventAsync(List<Guid> instrumentIds)
    {
        foreach (var instrumentId in instrumentIds)
        {
            var twoLastAnalyseResults = await analyseResultRepository
                .GetTwoLastAsync(instrumentId, KnownAnalyseTypes.Supertrend);
        }
    }
}
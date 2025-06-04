using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services.Algo;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Services.Algo;

public class BacktestService(
    ILogger logger,
    IDailyCandleRepository dailyCandleRepository,
    IHourlyCandleRepository hourlyCandleRepository,
    IResourceStoreService resourceStoreService,
    IServiceProvider serviceProvider) 
    : AlgoEngine(
        logger,
        dailyCandleRepository, 
        hourlyCandleRepository,
        resourceStoreService,
        serviceProvider), 
        IBacktestService
{
    public async Task<bool> BacktestAsync()
    {
        await InitBacktestAsync();
        
        return true;
    }
}
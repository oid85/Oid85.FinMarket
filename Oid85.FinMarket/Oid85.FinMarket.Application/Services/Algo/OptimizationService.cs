using NLog;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services.Algo;
using Oid85.FinMarket.External.ResourceStore;

namespace Oid85.FinMarket.Application.Services.Algo;

public class OptimizationService(
    ILogger logger,
    IDailyCandleRepository dailyCandleRepository,
    IHourlyCandleRepository hourlyCandleRepository,
    IResourceStoreService resourceStoreService) 
    : AlgoEngine(
        logger,
        dailyCandleRepository,
        hourlyCandleRepository,
        resourceStoreService), 
        IOptimizationService
{
    public async Task<bool> OptimizeAsync()
    {
        await InitOptimizationAsync();
        
        return true;
    }
}
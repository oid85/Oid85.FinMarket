using NLog;
using Oid85.FinMarket.Application.Interfaces.Services;

namespace Oid85.FinMarket.Application.Services;

public class OptimizationService(
    ILogger logger) 
    : IOptimizationService
{
    public async Task<bool> OptimizeAsync()
    {
        logger.Info("Запуск оптимизации");
        return true;
    }
}
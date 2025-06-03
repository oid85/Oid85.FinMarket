using NLog;
using Oid85.FinMarket.Application.Interfaces.Services;

namespace Oid85.FinMarket.Application.Services;

public class BacktestService(
    ILogger logger) 
    : IBacktestService
{
    public async Task<bool> BacktestAsync()
    {
        logger.Info("Запуск бэктеста");
        return true;
    }
}
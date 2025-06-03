namespace Oid85.FinMarket.Application.Interfaces.Services;

public interface IBacktestService
{
    Task<bool> BacktestAsync();
}
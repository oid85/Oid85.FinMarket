namespace Oid85.FinMarket.Application.Interfaces.Services.Algo;

public interface IBacktestService
{
    Task<bool> BacktestAsync();
}
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IBacktestResultRepository
{
    Task AddAsync(List<BacktestResult> backtestResults);
}
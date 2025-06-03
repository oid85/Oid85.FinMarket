namespace Oid85.FinMarket.Application.Interfaces.Services.Algo;

public interface IOptimizationService
{
    Task<bool> OptimizeAsync();
}
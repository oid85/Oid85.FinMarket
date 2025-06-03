namespace Oid85.FinMarket.Application.Interfaces.Services;

public interface IOptimizationService
{
    Task<bool> OptimizeAsync();
}
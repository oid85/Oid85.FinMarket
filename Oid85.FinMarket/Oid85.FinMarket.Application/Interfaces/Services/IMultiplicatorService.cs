using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Services;

public interface IMultiplicatorService
{
    Task<List<Multiplicator>> CalculateMultiplicatorsAsync();
    Task FillingMultiplicatorInstrumentsAsync();
}
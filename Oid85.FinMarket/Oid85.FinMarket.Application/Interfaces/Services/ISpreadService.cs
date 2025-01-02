using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис работы со спредами
/// </summary>
public interface ISpreadService
{
    Task<List<Spread>> CalculateSpreadsAsync();
}
using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Application.Models.Requests;

namespace Oid85.FinMarket.Application.Interfaces.Services.DiagramServices;

public interface ICurrenciesDiagramService
{
    Task<SimpleDiagramData> GetDailyClosePricesAsync(DateRangeRequest request);
}
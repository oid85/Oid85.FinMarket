using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Application.Models.Requests;

namespace Oid85.FinMarket.Application.Interfaces.Services.DiagramServices;

public interface ISharesDiagramService
{
    Task<SimpleDiagramData> GetDailyClosePricesAsync(DateRangeRequest request);
    Task<BubbleDiagramData> GetShareMultiplicatorsMCapPeNetDebtEbitdaAsync(TickerListRequest request);
    Task<BubbleDiagramData> GetBankMultiplicatorsMCapPePbAsync(TickerListRequest request);
}
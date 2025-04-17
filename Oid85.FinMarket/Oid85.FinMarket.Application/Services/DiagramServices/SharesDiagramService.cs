using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Interfaces.Services.DiagramServices;
using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Application.Models.Requests;
using Oid85.FinMarket.Common.KnownConstants;

namespace Oid85.FinMarket.Application.Services.DiagramServices;

public class SharesDiagramService(
    ITickerListUtilService tickerListUtilService,
    IDiagramDataFactory diagramDataFactory) 
    : ISharesDiagramService
{
    private async Task<List<Guid>> GetInstrumentIds() =>
        (await tickerListUtilService.GetSharesByTickerListAsync(KnownTickerLists.SharesWatchlist))
        .OrderBy(x => x.Sector).Select(x => x.InstrumentId).ToList();
    
    public async Task<SimpleDiagramData> GetDailyClosePricesAsync(DateRangeRequest request) =>
        await diagramDataFactory.CreateDailyClosePricesDiagramDataAsync(await GetInstrumentIds(), request.From, request.To);

    public async Task<SimpleDiagramData> GetFiveMinutesClosePricesAsync(DateTimeRangeRequest request) =>
        await diagramDataFactory.CreateFiveMinutesClosePricesDiagramDataAsync(
            await GetInstrumentIds(), 
            System.Convert.ToDateTime(request.From),
            System.Convert.ToDateTime(request.To));

    public async Task<BubbleDiagramData> GetMultiplicatorsMCapPeNetDebtEbitdaAsync() =>
        await diagramDataFactory.CreateMultiplicatorsMCapPeNetDebtEbitdaAsync(await GetInstrumentIds());
}
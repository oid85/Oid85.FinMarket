using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Interfaces.Services.DiagramServices;
using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Application.Models.Requests;

namespace Oid85.FinMarket.Application.Services.DiagramServices;

public class SharesDiagramService(
    IInstrumentService instrumentService,
    IDiagramDataFactory diagramDataFactory) 
    : ISharesDiagramService
{
    private async Task<List<Guid>> GetInstrumentIds() =>
        (await instrumentService.GetSharesInWatchlist())
        .OrderBy(x => x.Sector).Select(x => x.InstrumentId).ToList();
    
    public async Task<SimpleDiagramData> GetDailyClosePricesAsync(DateRangeRequest request) =>
        await diagramDataFactory.CreateDailyClosePricesDiagramDataAsync(await GetInstrumentIds(), request.From, request.To);

    public async Task<SimpleDiagramData> GetFiveMinutesClosePricesAsync(DateRangeRequest request) =>
        await diagramDataFactory.CreateFiveMinutesClosePricesDiagramDataAsync(await GetInstrumentIds(), request.From, request.To);

    public async Task<BubbleDiagramData> GetMultiplicatorsMCapPENetDebtEbitdaAsync() =>
        await diagramDataFactory.CreateMultiplicatorsMCapPENetDebtEbitdaAsync(await GetInstrumentIds());
}
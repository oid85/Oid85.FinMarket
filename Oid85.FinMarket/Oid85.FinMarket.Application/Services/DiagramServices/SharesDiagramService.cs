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
    
    public async Task<SimpleDiagramData> GetClosePricesAsync(DateRangeRequest request) =>
        await diagramDataFactory.CreateClosePricesDiagramDataAsync(await GetInstrumentIds(), request.From, request.To);
}
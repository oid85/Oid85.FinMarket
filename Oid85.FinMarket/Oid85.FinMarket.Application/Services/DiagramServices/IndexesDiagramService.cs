using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Application.Interfaces.Services.DiagramServices;
using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Application.Models.Requests;

namespace Oid85.FinMarket.Application.Services.DiagramServices;

public class IndexesDiagramService(
    ITickerListUtilService tickerListUtilService,
    IDiagramDataFactory diagramDataFactory) 
    : IIndexesDiagramService
{
    private async Task<List<Guid>> GetInstrumentIds(string tickerList) =>
        (await tickerListUtilService.GetFinIndexesByTickerListAsync(tickerList))
        .OrderBy(x => x.Ticker).Select(x => x.InstrumentId).ToList();
    
    public async Task<SimpleDiagramData> GetDailyClosePricesAsync(DateRangeRequest request) =>
        await diagramDataFactory.CreateDailyClosePricesDiagramDataAsync(
            await GetInstrumentIds(request.TickerList), 
            request.From, 
            request.To);
}
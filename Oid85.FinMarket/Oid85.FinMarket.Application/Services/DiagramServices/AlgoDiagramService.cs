using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Services.DiagramServices;
using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Application.Models.Requests;

namespace Oid85.FinMarket.Application.Services.DiagramServices;

public class AlgoDiagramService(
    IDiagramDataFactory diagramDataFactory) 
    : IAlgoDiagramService
{
    public Task<SimpleDiagramData> GetSpreadsAsync(DateRangeRequest request) =>
        diagramDataFactory.CreateSpreadsDiagramDataAsync(request.From, request.To);
}
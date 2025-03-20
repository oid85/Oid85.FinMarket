using Oid85.FinMarket.Application.Models.Diagrams;

namespace Oid85.FinMarket.Application.Interfaces.Factories;

public interface IDiagramDataFactory
{
    Task<DiagramData<DateOnly, double?>> CreateClosePricesDiagramDataAsync(
        List<Guid> instrumentIds, DateOnly from, DateOnly to);
}
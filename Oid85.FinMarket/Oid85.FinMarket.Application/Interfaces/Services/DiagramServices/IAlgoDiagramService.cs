using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Application.Models.Requests;

namespace Oid85.FinMarket.Application.Interfaces.Services.DiagramServices;

public interface IAlgoDiagramService
{
    Task<SimpleDiagramData> GetSpreadsAsync(DateRangeRequest request);
}
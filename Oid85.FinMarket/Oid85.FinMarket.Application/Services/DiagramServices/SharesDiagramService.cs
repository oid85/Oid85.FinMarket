using Oid85.FinMarket.Application.Interfaces.Services.DiagramServices;
using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Application.Models.Requests;

namespace Oid85.FinMarket.Application.Services.DiagramServices;

public class SharesDiagramService : ISharesDiagramService
{
    public Task<DiagramData<DateOnly, double>> GetClosePricesAsync(DateRangeRequest request)
    {
        throw new NotImplementedException();
    }
}
using Oid85.FinMarket.Domain.Models.Analyse;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IMarketEventRepository
{
    Task AddIfNotExistsAsync(MarketEvent marketEvent);
    Task DeactivateAsync(MarketEvent marketEvent);
    Task ActivateAsync(MarketEvent marketEvent);
    Task MarkAsSentAsync(MarketEvent marketEvent);
    Task MarkAsNoSentAsync(MarketEvent marketEvent);
    Task<List<MarketEvent>> GetActivatedAsync();
}
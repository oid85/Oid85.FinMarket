using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Repositories;

public interface IMarketEventRepository
{
    Task ActivateAsync(MarketEvent marketEvent);
    Task DeactivateAsync(MarketEvent marketEvent);
    Task<List<MarketEvent>> GetActivatedAsync();
    Task SetSentNotificationAsync(Guid marketEventId);
}
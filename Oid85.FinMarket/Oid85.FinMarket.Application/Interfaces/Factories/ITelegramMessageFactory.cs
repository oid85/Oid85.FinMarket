using Oid85.FinMarket.Domain.Models.Analyse;

namespace Oid85.FinMarket.Application.Interfaces.Factories;

public interface ITelegramMessageFactory
{
    string CreateTelegramMessage(IEnumerable<MarketEvent> marketEvents);
}
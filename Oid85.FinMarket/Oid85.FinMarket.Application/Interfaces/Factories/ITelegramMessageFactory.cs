using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Interfaces.Factories;

public interface ITelegramMessageFactory
{
    string CreateTelegramMessage(List<MarketEvent> marketEvents);
}
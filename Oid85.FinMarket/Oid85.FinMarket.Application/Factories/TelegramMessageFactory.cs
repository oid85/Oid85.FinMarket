using System.Text;
using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.Application.Factories;

public class TelegramMessageFactory 
    : ITelegramMessageFactory
{
    public string CreateTelegramMessage(IEnumerable<MarketEvent> marketEvents)
    {
        var message = new StringBuilder();

        foreach (var marketEvent in marketEvents)
            message.AppendLine($"{marketEvent.Ticker} {marketEvent.InstrumentName} {marketEvent.MarketEventText}");
        
        return message.ToString();
    }
}
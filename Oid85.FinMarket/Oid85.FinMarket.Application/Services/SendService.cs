using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.External.ResourceStore;
using Oid85.FinMarket.External.Telegram;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class SendService(
    IMarketEventRepository marketEventRepository,
    ITelegramService telegramService,
    ITelegramMessageFactory telegramMessageFactory,
    IResourceStoreService resourceStoreService)
    : ISendService
{
    /// <inheritdoc />
    public async Task<bool> SendMessageAsync(string message)
    {
        await telegramService.SendMessageAsync(message);
        return true;
    }

    /// <inheritdoc />
    public async Task<bool> SendNotificationsAsync()
    {
        var marketEventTypesForSend = (await resourceStoreService.GetSendFilterAsync())
            .Where(x => x.Enable)
            .Select(x => x.Name)
            .ToList();
        
        var marketEvents = (await marketEventRepository.GetActivatedAsync())
            .Where(x => !x.SentNotification)
            .Where(x => marketEventTypesForSend.Contains(x.MarketEventType))
            .ToList();

        if (marketEvents is [])
            return true;

        const int maxEventInMessage = 5;

        var chunks = marketEvents.Chunk(maxEventInMessage);

        foreach (var chunk in chunks)
        {
            foreach (var marketEvent in chunk)
                await marketEventRepository.MarkAsSentAsync(marketEvent);
                
            string message = telegramMessageFactory.CreateTelegramMessage(chunk);
            await telegramService.SendMessageAsync(message);
        }
            
        return true;
    }    
}
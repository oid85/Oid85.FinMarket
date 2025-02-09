using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.External.Telegram;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class SendService(
    IMarketEventRepository marketEventRepository,
    ITelegramService telegramService,
    ITelegramMessageFactory telegramMessageFactory)
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
        var marketEvents = (await marketEventRepository
            .GetActivatedAsync())
            .Where(x => !x.SentNotification)
            .ToList();

        foreach (var marketEvent in marketEvents) 
            await marketEventRepository.SetSentNotificationAsync(marketEvent.Id);

        string message = telegramMessageFactory.CreateTelegramMessage(marketEvents);
        
        await telegramService.SendMessageAsync(message);
        
        return true;
    }
}
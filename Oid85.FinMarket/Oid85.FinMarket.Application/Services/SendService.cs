using NLog;
using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Telegram;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class SendService(
    ILogger logger,
    IMarketEventRepository marketEventRepository,
    ITelegramService telegramService,
    ITelegramMessageFactory telegramMessageFactory)
    : ISendService
{
    /// <inheritdoc />
    public async Task<bool> SendMessageAsync(string message)
    {
        try
        {
            await telegramService.SendMessageAsync(message);
            return true;
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<bool> SendNotificationsAsync()
    {
        try
        {
            var marketEvents = (await marketEventRepository
                    .GetActivatedAsync())
                .Where(x => !x.SentNotification)
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
        
        catch (Exception exception)
        {
            logger.Error(exception);
            return false;
        }
    }    
}
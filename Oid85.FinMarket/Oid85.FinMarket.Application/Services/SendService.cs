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

            var marketEventsForSend = new List<MarketEvent>();
            
            foreach (var marketEvent in marketEvents)
            {
                marketEventsForSend.Add(marketEvent);
                await marketEventRepository.MarkAsSentAsync(marketEvent);
                
                // Отправляем по 5 событий за раз
                if (marketEventsForSend.Count >= 5)
                {
                    string message = telegramMessageFactory.CreateTelegramMessage(marketEventsForSend);
                    await telegramService.SendMessageAsync(message);
                    marketEventsForSend.Clear();
                }
            }
            
            return true;
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
            return false;
        }
    }
    
    private async Task SendActiveMarketEvents()
    {
        try
        {
            var marketEvents = (await marketEventRepository.GetActivatedAsync()).ToList();

            foreach (var marketEvent in marketEvents) 
                await marketEventRepository.MarkAsSentAsync(marketEvent);

            string message = telegramMessageFactory.CreateTelegramMessage(marketEvents);
        
            await telegramService.SendMessageAsync(message);
        }
        
        catch (Exception exception)
        {
            logger.Error(exception);
        }
    }
}
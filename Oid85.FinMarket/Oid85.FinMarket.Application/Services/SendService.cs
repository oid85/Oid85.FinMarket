using NLog;
using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.External.Telegram;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

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
            logger.Trace(exception.Message);
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
            
            foreach (var marketEvent in marketEvents) 
                await marketEventRepository.SetSentNotificationAsync(marketEvent.Id);

            string message = telegramMessageFactory.CreateTelegramMessage(marketEvents);
        
            await telegramService.SendMessageAsync(message);
        
            return true;
        }
        
        catch (Exception exception)
        {
            logger.Trace(exception.Message);
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<bool> MessageHandleAsync(Message message, UpdateType type)
    {
        switch (message.Text)
        {
            case KnownTelegramCommand.ActiveMarketEvents:
                await SendActiveMarketEvents();
                break;
        }
        
        return true;
    }
    
    private async Task SendActiveMarketEvents()
    {
        try
        {
            var marketEvents = (await marketEventRepository
                    .GetActivatedAsync())
                .ToList();

            foreach (var marketEvent in marketEvents) 
                await marketEventRepository.SetSentNotificationAsync(marketEvent.Id);

            string message = telegramMessageFactory.CreateTelegramMessage(marketEvents);
        
            await telegramService.SendMessageAsync(message);
        }
        
        catch (Exception exception)
        {
            logger.Trace(exception.Message);
        }
    }
}
using Oid85.FinMarket.Application.Interfaces.Repositories;
using Oid85.FinMarket.Application.Interfaces.Services;
using Oid85.FinMarket.External.Telegram;

namespace Oid85.FinMarket.Application.Services;

/// <inheritdoc />
public class SendService(
    IMarketEventRepository marketEventRepository,
    ITelegramService telegramService)
    : ISendService
{
    /// <inheritdoc />
    public async Task<bool> SendMessageAsync(string message)
    {
        await telegramService.SendMessageAsync(message);   
        
        return true;
    }
}
using Microsoft.Extensions.Configuration;
using NLog;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Common.KnownConstants;
using Telegram.Bot;

namespace Oid85.FinMarket.External.Telegram;

/// <inheritdoc />
public class TelegramService(
    ILogger logger,
    IConfiguration configuration,
    TelegramBotClient botClient) 
    : ITelegramService
{
    /// <inheritdoc />
    public async Task SendMessageAsync(string message)
    {
        try
        {
            string chatId = ConvertHelper.Base64Decode(configuration.GetValue<string>(KnownSettingsKeys.TelegramChatId)!);
            await botClient.SendMessage(chatId, message);
            await Task.Delay(TimeSpan.FromSeconds(3));
        }
        
        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка отправки сообщения. {message}", message);
        }
    }
}
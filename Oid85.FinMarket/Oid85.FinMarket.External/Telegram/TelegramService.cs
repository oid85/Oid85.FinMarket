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
            string chatIdBase64 = configuration.GetValue<string>(KnownSettingsKeys.TelegramChatId)!;
            string chatId = ConvertHelper.Base64Decode(chatIdBase64);
            await botClient.SendMessage(chatId, message);
            await Task.Delay(TimeSpan.FromSeconds(3));
        }
        
        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка отправки сообщения. {message}", message);
        }
    }
}
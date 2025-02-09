using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Oid85.FinMarket.External.Telegram;

/// <summary>
/// Сервис работы с мессенджером Телеграм
/// </summary>
public interface ITelegramService
{
    /// <summary>
    /// Отправить сообщение
    /// </summary>
    Task SendMessageAsync(string message);

    Task MessageHandleAsync(Message message, UpdateType type);
}
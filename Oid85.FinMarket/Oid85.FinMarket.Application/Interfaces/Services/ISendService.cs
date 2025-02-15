using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис рассылки
/// </summary>
public interface ISendService
{
    /// <summary>
    /// Отправить сообщение
    /// </summary>
    Task<bool> SendMessageAsync(string message);
    
    /// <summary>
    /// Отправить оповещения
    /// </summary>
    Task<bool> SendNotificationsAsync();

    /// <summary>
    /// Обработать команду телеграм-бота
    /// </summary>
    Task<bool> MessageHandleAsync(Message message, UpdateType type);
}
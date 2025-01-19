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
}
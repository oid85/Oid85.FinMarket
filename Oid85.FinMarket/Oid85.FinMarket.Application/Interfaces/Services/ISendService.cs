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
}
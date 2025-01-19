namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис рассылки
/// </summary>
public interface ISendService
{
    /// <summary>
    /// Отправить оповещения
    /// </summary>
    Task SendNotificationsAsync();
}
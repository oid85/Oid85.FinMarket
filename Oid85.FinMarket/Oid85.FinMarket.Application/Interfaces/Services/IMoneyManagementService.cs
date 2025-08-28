namespace Oid85.FinMarket.Application.Interfaces.Services;

/// <summary>
/// Сервис расчета позиций
/// </summary>
public interface IMoneyManagementService
{
    /// <summary>
    /// Рассчитать размер позиции
    /// </summary>
    /// <param name="ticker">Тикер</param>
    /// <param name="orderPrice">Цена заявки</param>
    /// <param name="money">Сумма входа в сделку</param>
    /// <returns>Количество в штуках</returns>
    Task<int> GetPositionSizeAsync(string ticker, double orderPrice, double money);
    
    /// <summary>
    /// Рассчитать размер позиции для арбитража
    /// </summary>
    /// <param name="ticker">Тикер</param>
    /// <param name="orderPrice">Цена заявки</param>
    /// <param name="money">Сумма входа в сделку</param>
    /// <returns>Количество в штуках</returns>
    Task<(int First, int Second)> GetPositionSizeAsync((string First, string Second) ticker, (double First, double Second) orderPrice, (double First, double Second) money);    
}
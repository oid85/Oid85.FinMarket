namespace Oid85.FinMarket.Domain.Models;

public class Bond
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Тикер
    /// </summary>
    public string Ticker { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор ISIN
    /// </summary>
    public string Isin { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор FIGI
    /// </summary>
    public string Figi { get; set; } = string.Empty;

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Сектор
    /// </summary>
    public string Sector { get; set; } = string.Empty;

    /// <summary>
    /// Флаг активности
    /// </summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>
    /// Находится в портфеле
    /// </summary>
    public bool InPortfolio { get; set; }
    
    /// <summary>
    /// Находится в списке наблюдения
    /// </summary>
    public bool InWatchList { get; set; }

    /// <summary>
    /// Значение НКД (накопленного купонного дохода) на дату
    /// </summary>
    public double NKD { get; set; }
    
    /// <summary>
    /// Дата погашения облигации по UTC
    /// </summary>
    public DateTime MaturityDate { get; set; }
    
    /// <summary>
    /// Признак облигации с плавающим купоном
    /// </summary>
    public bool FloatingCouponFlag { get; set; }
}
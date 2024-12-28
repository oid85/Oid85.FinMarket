using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class SpreadEntity : AuditableEntity
{
    /// <summary>
    /// Id первого инструмента в паре
    /// </summary>
    [Column("first_instrument_id")]
    public Guid FirstInstrumentId { get; set; }
    
    /// <summary>
    /// Тикер первого инструмента в паре
    /// </summary>
    [Column("first_instrument_ticker")]
    public string FirstInstrumentTicker { get; set; } = string.Empty;
    
    /// <summary>
    /// Роль первого инструмента в паре
    /// базовый/производный, дальний/ближний (для фьючерсов)
    /// </summary>
    [Column("first_instrument_role")]
    public string FirstInstrumentRole { get; set; } = string.Empty;
    
    /// <summary>
    /// Цена первого инструмента в паре
    /// </summary>
    [Column("first_instrument_price")]
    public double FirstInstrumentPrice { get; set; }
    
    /// <summary>
    /// Id второго инструмента в паре
    /// </summary>
    [Column("second_instrument_id")]
    public Guid SecondInstrumentId { get; set; }
    
    /// <summary>
    /// Тикер второго инструмента в паре
    /// </summary>
    [Column("second_instrument_ticker")]
    public string SecondInstrumentTicker { get; set; } = string.Empty;
    
    /// <summary>
    /// Роль второго инструмента в паре 
    /// базовый/производный, дальний/ближний (для фьючерсов)
    /// </summary>
    [Column("second_instrument_role")]
    public string SecondInstrumentRole { get; set; } = string.Empty;
    
    /// <summary>
    /// Цена второго инструмента в паре
    /// </summary>
    [Column("second_instrument_price")]
    public double SecondInstrumentPrice { get; set; }
    
    /// <summary>
    /// Разница цен инструментов
    /// </summary>
    [Column("price_difference")]
    public double PriceDifference { get; set; }
    
    /// <summary>
    /// Фандинг
    /// </summary>
    [Column("funding")]
    public double Funding { get; set; }
    
    /// <summary>
    /// Отношение цен инструменов, относительно друг друга
    /// 1 - континго, 2 - бэквордация
    /// </summary>
    [Column("contango_backwardation")]
    public int ContangoBackwardation { get; set; }
    
    /// <summary>
    /// Находится в списке наблюдения
    /// </summary>
    [Column("in_watch_list")]
    public bool InWatchList { get; set; } = false; 
}
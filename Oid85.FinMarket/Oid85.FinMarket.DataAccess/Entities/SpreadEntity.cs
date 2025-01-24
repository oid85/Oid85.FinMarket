using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oid85.FinMarket.DataAccess.Entities.Base;

namespace Oid85.FinMarket.DataAccess.Entities;

public class SpreadEntity : AuditableEntity
{
    /// <summary>
    /// Время расчета спреда
    /// </summary>
    [Column("datetime", TypeName = "timestamp with time zone")]
    public DateTime DateTime { get; set; }
    
    /// <summary>
    /// Id первого инструмента в паре
    /// </summary>
    [Column("first_instrument_id")]
    public Guid FirstInstrumentId { get; set; }
    
    /// <summary>
    /// Тикер первого инструмента в паре
    /// </summary>
    [Column("first_instrument_ticker"), MaxLength(20)]
    public string FirstInstrumentTicker { get; set; } = string.Empty;
    
    /// <summary>
    /// Роль первого инструмента в паре
    /// базовый/производный, дальний/ближний (для фьючерсов)
    /// </summary>
    [Column("first_instrument_role"), MaxLength(20)]
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
    [Column("second_instrument_ticker"), MaxLength(20)]
    public string SecondInstrumentTicker { get; set; } = string.Empty;
    
    /// <summary>
    /// Роль второго инструмента в паре 
    /// базовый/производный, дальний/ближний (для фьючерсов)
    /// </summary>
    [Column("second_instrument_role"), MaxLength(20)]
    public string SecondInstrumentRole { get; set; } = string.Empty;
    
    /// <summary>
    /// Цена второго инструмента в паре
    /// </summary>
    [Column("second_instrument_price")]
    public double SecondInstrumentPrice { get; set; }
    
    /// <summary>
    /// Множитель (кол-во базовых активов в контракте)
    /// </summary>
    [Column("multiplier")]
    public double Multiplier { get; set; }
    
    /// <summary>
    /// Разница цен инструментов
    /// </summary>
    [Column("price_difference")]
    public double PriceDifference { get; set; }
    
    /// <summary>
    /// Разница цен инструментов, %
    /// </summary>
    [Column("price_difference_prc")]
    public double PriceDifferencePrc { get; set; }
    
    /// <summary>
    /// Разница цен инструментов (средняя)
    /// </summary>
    [Column("price_difference_average")]
    public double PriceDifferenceAverage { get; set; }
    
    /// <summary>
    /// Разница цен инструментов (средняя), %
    /// </summary>
    [Column("price_difference_average_prc")]
    public double PriceDifferenceAveragePrc { get; set; }
    
    /// <summary>
    /// Фандинг
    /// </summary>
    [Column("funding")]
    public double Funding { get; set; }
    
    /// <summary>
    /// Отношение цен инструменов, относительно друг друга
    /// 1 - континго, 2 - бэквордация
    /// </summary>
    [Column("price_position")]
    public int SpreadPricePosition { get; set; }
}
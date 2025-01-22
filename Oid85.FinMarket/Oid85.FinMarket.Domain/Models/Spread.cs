namespace Oid85.FinMarket.Domain.Models;

public class Spread
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Время расчета спреда
    /// </summary>
    public DateTime DateTime { get; set; }
    
    /// <summary>
    /// Id первого инструмента в паре
    /// </summary>
    public Guid FirstInstrumentId { get; set; }
    
    /// <summary>
    /// Тикер первого инструмента в паре
    /// </summary>
    public string FirstInstrumentTicker { get; set; } = string.Empty;
    
    /// <summary>
    /// Роль первого инструмента в паре
    /// базовый/производный, дальний/ближний (для фьючерсов)
    /// </summary>
    public string FirstInstrumentRole { get; set; } = string.Empty;
    
    /// <summary>
    /// Цена первого инструмента в паре
    /// </summary>
    public double FirstInstrumentPrice { get; set; }
    
    /// <summary>
    /// Id второго инструмента в паре
    /// </summary>
    public Guid SecondInstrumentId { get; set; }
    
    /// <summary>
    /// Тикер второго инструмента в паре
    /// </summary>
    public string SecondInstrumentTicker { get; set; } = string.Empty;
    
    /// <summary>
    /// Роль второго инструмента в паре 
    /// базовый/производный, дальний/ближний (для фьючерсов)
    /// </summary>
    public string SecondInstrumentRole { get; set; } = string.Empty;
    
    /// <summary>
    /// Цена второго инструмента в паре
    /// </summary>
    public double SecondInstrumentPrice { get; set; }
    
    /// <summary>
    /// Множитель (кол-во базовых активов в контракте)
    /// </summary>
    public double Multiplier { get; set; }
    
    /// <summary>
    /// Разница цен инструментов
    /// </summary>
    public double PriceDifference { get; set; }
    
    /// <summary>
    /// Разница цен инструментов, %
    /// </summary>
    public double PriceDifferencePrc { get; set; }
    
    /// <summary>
    /// Разница цен инструментов (средняя)
    /// </summary>
    public double PriceDifferenceAverage { get; set; }
    
    /// <summary>
    /// Разница цен инструментов (средняя), %
    /// </summary>
    public double PriceDifferenceAveragePrc { get; set; }
    
    /// <summary>
    /// Фандинг
    /// </summary>
    public double Funding { get; set; }
    
    /// <summary>
    /// Отношение цен инструменов, относительно друг друга
    /// 1 - контанго, 2 - бэквордация
    /// </summary>
    public int SpreadPricePosition { get; set; }
}
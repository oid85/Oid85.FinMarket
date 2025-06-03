using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Strategies.Models;

namespace Oid85.FinMarket.Strategies.Mapping;

public static class StrategyMapper
{
    public static Candle Map(DailyCandle model) =>
        new()
        {
            Open = model.Open,
            Close = model.Close,
            High = model.High,
            Low = model.Low,
            Volume = model.Volume,
            Date = new DateTime(model.Date, TimeOnly.MinValue)
        };    
    
    public static Candle Map(HourlyCandle model) =>
        new()
        {
            Open = model.Open,
            Close = model.Close,
            High = model.High,
            Low = model.Low,
            Volume = model.Volume,
            Date = new DateTime(model.Date, model.Time)
        };     
}
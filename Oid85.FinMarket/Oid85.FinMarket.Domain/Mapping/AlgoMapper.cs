using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Domain.Mapping;

public static class AlgoMapper
{
    public static Candle Map(DailyCandle model) =>
        new()
        {
            Open = model.Open,
            Close = model.Close,
            High = model.High,
            Low = model.Low,
            Volume = model.Volume,
            DateTime = new DateTime(model.Date, TimeOnly.MinValue)
        };    
    
    public static Candle Map(HourlyCandle model) =>
        new()
        {
            Open = model.Open,
            Close = model.Close,
            High = model.High,
            Low = model.Low,
            Volume = model.Volume,
            DateTime = new DateTime(model.Date, model.Time)
        };     
}
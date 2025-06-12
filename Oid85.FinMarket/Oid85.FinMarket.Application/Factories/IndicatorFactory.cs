using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Domain.Mapping;
using Oid85.FinMarket.Domain.Models.Algo;
using Skender.Stock.Indicators;

namespace Oid85.FinMarket.Application.Factories;

public class IndicatorFactory : IIndicatorFactory
{
    public List<double> Highest(List<double> values, int period) => 
        values.Select((_, i) => i - period < 0 ? 0.0 : values.Skip(i - period + 1).Take(period).Max()).ToList();

    public List<double> Lowest(List<double> values, int period) => 
        values.Select((_, i) => i - period < 0 ? 0.0 : values.Skip(i - period + 1).Take(period).Min()).ToList();

    public List<double> Sma(List<double> values, int period) => 
        values.Select((_, i) => i - period < 0 ? 0.0 : values.Skip(i - period + 1).Take(period).Average()).ToList();

    public List<double> Supertrend(List<Candle> candles, int period, double multiplier) =>
        candles
            .Select(AlgoMapper.Map)
            .GetSuperTrend(period, multiplier)
            .Select(x => x.SuperTrend)
            .Select(x => x.HasValue ? Convert.ToDouble(x.Value) : 0.0)
            .ToList();

    public List<double> Atr(List<Candle> candles, int period) =>
        candles
            .Select(AlgoMapper.Map)
            .GetAtr(period)
            .Select(x => x.Atr)
            .Select(x => x.HasValue ? Convert.ToDouble(x.Value) : 0.0)
            .ToList();

    public List<double> Hma(List<Candle> candles, int period) =>
        candles
            .Select(AlgoMapper.Map)
            .GetHma(period)
            .Select(x => x.Hma)
            .Select(x => x.HasValue ? Convert.ToDouble(x.Value) : 0.0)
            .ToList();
    
    public List<double> Ema(List<Candle> candles, int period) =>
        candles
            .Select(AlgoMapper.Map)
            .GetEma(period)
            .Select(x => x.Ema)
            .Select(x => x.HasValue ? Convert.ToDouble(x.Value) : 0.0)
            .ToList();
    
    public List<double> Adx(List<Candle> candles, int period) =>
        candles
            .Select(AlgoMapper.Map)
            .GetAdx(period)
            .Select(x => x.Adx)
            .Select(x => x.HasValue ? Convert.ToDouble(x.Value) : 0.0)
            .ToList();
}
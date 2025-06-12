using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Interfaces.Factories;

public interface IIndicatorFactory
{
    List<double> Highest(List<double> values, int period);
    List<double> Lowest(List<double> values, int period);
    List<double> Sma(List<double> values, int period);
    List<double> Supertrend(List<Candle> candles, int period, double multiplier);
    List<double> Atr(List<Candle> candles, int period);
    List<double> Hma(List<Candle> candles, int period);
    List<double> Ema(List<Candle> candles, int period);
    List<double> Adx(List<Candle> candles, int period);
}
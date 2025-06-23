using Oid85.FinMarket.Application.Interfaces.Factories;
using Oid85.FinMarket.Common.MathExtensions;
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

    public List<double> UltimateSmoother(List<double> values, int period)
    {
        // Ultimate Smoother function based on John Ehlers' formula
        double coeff = Math.Sqrt(2.0);
        double step = 2.0 * Math.PI / period;
        double a1 = Math.Exp(-1.0 * coeff * Math.PI / period);
        double b1 = 2.0 * a1 * Math.Cos(coeff * step / period);
        double c2 = b1;
        double c3 = -1.0 * a1 * a1;
        double c1 = (1 + c2 - c3) / 4.0;
        
        var us = new List<double>();

        for (int i = 0; i < values.Count; i++)
            us.Add(i < 3
                ? values[i]
                : (1 - c1) * values[i] + 
                  (2 * c1 - c2) * values[i - 1] - 
                  (c1 + c3) * values[i - 2] + 
                  c2 * us[i - 1] + 
                  c3 * us[i - 2]);

        return us;
    }

    public (List<double> UpperBand, List<double> LowerBand) BollingerBands(List<Candle> candles, int period, double stdDev)
    {
        var quotes = candles.Select(AlgoMapper.Map);
        var bollingerBandsResults = quotes.GetBollingerBands(period, stdDev);
        
        var upperBand = new List<double>();
        var lowerBand = new List<double>();

        foreach (var bollingerBandsResult in bollingerBandsResults)
        {
            upperBand.Add(bollingerBandsResult.UpperBand ?? 0.0);
            lowerBand.Add(bollingerBandsResult.LowerBand ?? 0.0);
        }

        return (upperBand, lowerBand);
    }

    public (List<double> UpperBand, List<double> LowerBand) AdaptivePriceChannelAdx(List<Candle> candles, int periodAdx, int periodPc)
    {
        List<double> price = candles.Select(x => (x.High + x.Low + x.Close + x.Close) / 4.0).ToList();
        List<double> adx = Adx(candles, periodAdx);
        List<double> upperBand = new List<double>().InitValues(candles.Count);
        List<double> lowerBand = new List<double>().InitValues(candles.Count);

        int startIndex = new List<int> {periodAdx, periodPc}.Max() + 1;
        
        for (int i = startIndex; i < candles.Count; i++)
        {
            int n = (int) Math.Floor(periodPc * ((100.0 - adx[i]) / 100.0));
            
            upperBand[i] = price.Take(i + 1).TakeLast(n).Max();
            lowerBand[i] = price.Take(i + 1).TakeLast(n).Min();
        }
        
        return (upperBand, lowerBand);
    }

    public List<double> EhlersNonlinearFilter(List<Candle> candles)
    {
        List<double> price = candles.Select(x => (x.High + x.Low + x.Close + x.Close) / 4.0).ToList();
        List<double> coef = new List<double>().InitValues(candles.Count);
        List<double> dcef = new List<double>().InitValues(candles.Count);

        const int coefLookback = 5;
        for (int i = coefLookback; i < candles.Count; i++)
            coef[i] = Math.Pow(price[i] - price[i - 1], 2) +
                      Math.Pow(price[i] - price[i - 2], 2) +
                      Math.Pow(price[i] - price[i - 3], 2) +
                      Math.Pow(price[i] - price[i - 4], 2) +
                      Math.Pow(price[i] - price[i - 5], 2);
        
        double sumCoef = 0.0;
        double sumCoefPrice = 0.0;

        for (int i = coefLookback; i < candles.Count; i++)
        {
            for (int j = 0; j < coefLookback; j++)
            {
                sumCoef += coef[i - j];
                sumCoefPrice += coef[i - j] * price[i - j];
            }

            dcef[i] = sumCoefPrice / sumCoef;
        }
        
        return dcef;
    }
}
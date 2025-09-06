using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Factories.Builders;

public class PairArbitrageBacktestResultDiagramDataBuilder
{
    public static PairArbitrageBacktestResultDiagramData SetDates(PairArbitrageBacktestResultDiagramData diagramData, PairArbitrageStrategy strategy)
    {
        for (int i = 0; i < strategy.Spreads.Count; i++)
            diagramData.Data.Series.Add(new PairArbitrageBacktestResultDataPoint
            {
                Date = strategy.Spreads[i].Date.ToString(KnownDateTimeFormats.DateISO),
            });
        
        return diagramData;
    }
    
    public static PairArbitrageBacktestResultDiagramData SetDates(PairArbitrageBacktestResultDiagramData diagramData, List<PairArbitrageStrategy> strategies)
    {
        for (int i = 0; i < strategies[0].Spreads.Count; i++)
            diagramData.Data.Series.Add(new PairArbitrageBacktestResultDataPoint
            {
                Date = strategies[0].Spreads[i].Date.ToString(KnownDateTimeFormats.DateISO),
            });
        
        return diagramData;
    }
    
    public static PairArbitrageBacktestResultDiagramData SetFirstPrices(PairArbitrageBacktestResultDiagramData diagramData, PairArbitrageStrategy strategy)
    {
        for (int i = 0; i < strategy.Spreads.Count; i++)
            diagramData.Data.Series[i].PriceFirst = strategy.Candles.First[i].Close;
        
        return diagramData;
    }
    
    public static PairArbitrageBacktestResultDiagramData SetFirstPrices(PairArbitrageBacktestResultDiagramData diagramData, List<PairArbitrageStrategy> strategies)
    {
        for (int i = 0; i < strategies[0].Spreads.Count; i++)
            diagramData.Data.Series[i].PriceFirst = strategies[0].Candles.First[i].Close;
        
        return diagramData;
    }    
    
    public static PairArbitrageBacktestResultDiagramData SetSecondPrices(PairArbitrageBacktestResultDiagramData diagramData, PairArbitrageStrategy strategy)
    {
        for (int i = 0; i < strategy.Spreads.Count; i++)
            diagramData.Data.Series[i].PriceSecond = strategy.Candles.Second[i].Close;
        
        return diagramData;
    }
    
    public static PairArbitrageBacktestResultDiagramData SetSecondPrices(PairArbitrageBacktestResultDiagramData diagramData, List<PairArbitrageStrategy> strategies)
    {
        for (int i = 0; i < strategies[0].Spreads.Count; i++)
            diagramData.Data.Series[i].PriceSecond = strategies[0].Candles.Second[i].Close;
        
        return diagramData;
    }    
    
    public static PairArbitrageBacktestResultDiagramData SetSpreads(PairArbitrageBacktestResultDiagramData diagramData, PairArbitrageStrategy strategy)
    {
        for (int i = 0; i < strategy.Spreads.Count; i++)
            diagramData.Data.Series[i].Spread = strategy.Spreads[i].Value;
        
        return diagramData;
    }
    
    public static PairArbitrageBacktestResultDiagramData SetSpreads(PairArbitrageBacktestResultDiagramData diagramData, List<PairArbitrageStrategy> strategies)
    {
        for (int i = 0; i < strategies[0].Spreads.Count; i++)
            diagramData.Data.Series[i].Spread = strategies[0].Spreads[i].Value;
        
        return diagramData;
    }    
    
    public static PairArbitrageBacktestResultDiagramData SetLongShortPositions(PairArbitrageBacktestResultDiagramData diagramData, PairArbitrageStrategy strategy)
    {
        return diagramData;
    }    
    
    public static PairArbitrageBacktestResultDiagramData SetLongShortPositions(PairArbitrageBacktestResultDiagramData diagramData, List<PairArbitrageStrategy> strategies)
    {
        return diagramData;
    }    
    
    public static PairArbitrageBacktestResultDiagramData SetShortLongPositions(PairArbitrageBacktestResultDiagramData diagramData, PairArbitrageStrategy strategy)
    {
        return diagramData;
    }
    
    public static PairArbitrageBacktestResultDiagramData SetShortLongPositions(PairArbitrageBacktestResultDiagramData diagramData, List<PairArbitrageStrategy> strategies)
    {
        return diagramData;
    }    
    
    public static PairArbitrageBacktestResultDiagramData SetEquity(PairArbitrageBacktestResultDiagramData diagramData, PairArbitrageStrategy strategy)
    {
        var from = strategy.Spreads.First().Date.ToDateTime(TimeOnly.MinValue);
        var to = strategy.Spreads.Last().Date.ToDateTime(TimeOnly.MinValue);
        
        var equity = strategy.EqiutyCurve.Expand(from, to);
        
        for (int i = 0; i < diagramData.Data.Series.Count; i++)
        {
            var date = Convert.ToDateTime(diagramData.Data.Series[i].Date);
            diagramData.Data.Series[i].Equity = Math.Round(equity[date], 2);
        }
        
        return diagramData;
    }
    
    public static PairArbitrageBacktestResultDiagramData SetEquity(PairArbitrageBacktestResultDiagramData diagramData, List<PairArbitrageStrategy> strategies)
    {
        var from = strategies[0].Spreads.First().Date.ToDateTime(TimeOnly.MinValue);
        var to = strategies[0].Spreads.Last().Date.ToDateTime(TimeOnly.MinValue);

        for (int i = 0; i < strategies.Count; i++)
        {
            var equity = strategies[i].EqiutyCurve.Expand(from, to);

            for (int j = 0; j < diagramData.Data.Series.Count; j++)
            {
                var date = Convert.ToDateTime(diagramData.Data.Series[j].Date);
                diagramData.Data.Series[j].Equity += Math.Round(equity[date], 2);
            }
        }
        
        return diagramData;
    }
    
    public static PairArbitrageBacktestResultDiagramData SetDrawdown(PairArbitrageBacktestResultDiagramData diagramData, PairArbitrageStrategy strategy)
    {
        var from = strategy.Spreads.First().Date.ToDateTime(TimeOnly.MinValue);
        var to = strategy.Spreads.Last().Date.ToDateTime(TimeOnly.MinValue);
        
        var drawdown = strategy.DrawdownCurve.Expand(from, to);
        
        for (int i = 0; i < diagramData.Data.Series.Count; i++)
        {
            var date = Convert.ToDateTime(diagramData.Data.Series[i].Date);
            diagramData.Data.Series[i].Drawdown = Math.Round(-1 * drawdown[date], 2);
        }
        
        return diagramData;
    }
    
    public static PairArbitrageBacktestResultDiagramData SetDrawdown(PairArbitrageBacktestResultDiagramData diagramData, List<PairArbitrageStrategy> strategies)
    {
        var from = strategies[0].Spreads.First().Date.ToDateTime(TimeOnly.MinValue);
        var to = strategies[0].Spreads.Last().Date.ToDateTime(TimeOnly.MinValue);

        for (int i = 0; i < strategies.Count; i++)
        {
            var drawdown = strategies[i].DrawdownCurve.Expand(from, to);

            for (int j = 0; j < diagramData.Data.Series.Count; j++)
            {
                var date = Convert.ToDateTime(diagramData.Data.Series[j].Date);
                diagramData.Data.Series[j].Drawdown += Math.Round(-1 * drawdown[date], 2);
            }
        }
        
        return diagramData;
    }
}
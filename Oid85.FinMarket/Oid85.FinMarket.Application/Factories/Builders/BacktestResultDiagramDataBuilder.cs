using Oid85.FinMarket.Application.Models.Diagrams;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models.Algo;

namespace Oid85.FinMarket.Application.Factories.Builders;

public class BacktestResultDiagramDataBuilder
{
    public static BacktestResultDiagramData SetDates(BacktestResultDiagramData diagramData, Strategy strategy)
    {
        for (int i = 0; i < strategy.Candles.Count; i++)
            diagramData.Data.Series.Add(new BacktestResultDataPoint
            {
                Date = strategy.Candles[i].DateTime.ToString(KnownDateTimeFormats.DateISO),
            });
        
        return diagramData;
    }
    
    public static BacktestResultDiagramData SetDates(BacktestResultDiagramData diagramData, List<Strategy> strategies)
    {
        for (int i = 0; i < strategies[0].Candles.Count; i++)
            diagramData.Data.Series.Add(new BacktestResultDataPoint
            {
                Date = strategies[0].Candles[i].DateTime.ToString(KnownDateTimeFormats.DateISO),
            });
        
        return diagramData;
    }
    
    public static BacktestResultDiagramData SetPrices(BacktestResultDiagramData diagramData, Strategy strategy)
    {
        for (int i = 0; i < strategy.Candles.Count; i++)
            diagramData.Data.Series[i].Price = strategy.Candles[i].Close;
        
        return diagramData;
    }
    
    public static BacktestResultDiagramData SetPrices(BacktestResultDiagramData diagramData, List<Strategy> strategies)
    {
        for (int i = 0; i < strategies[0].Candles.Count; i++)
            diagramData.Data.Series[i].Price = strategies[0].Candles[i].Close;
        
        return diagramData;
    }    
    
    public static BacktestResultDiagramData SetFilters(BacktestResultDiagramData diagramData, Strategy strategy)
    {
        for (int i = 0; i < strategy.Candles.Count; i++)
            diagramData.Data.Series[i].Filter = strategy.GraphPoints[i].Filter;
        
        return diagramData;
    }    
    
    public static BacktestResultDiagramData SetIndicators(BacktestResultDiagramData diagramData, Strategy strategy)
    {
        for (int i = 0; i < strategy.Candles.Count; i++)
            diagramData.Data.Series[i].Indicator = strategy.GraphPoints[i].Indicator;
        
        return diagramData;
    }
    
    public static BacktestResultDiagramData SetChannelBands(BacktestResultDiagramData diagramData, Strategy strategy)
    {
        for (int i = 0; i < strategy.Candles.Count; i++)
            diagramData.Data.Series[i].ChannelBands = strategy.GraphPoints[i].ChannelBands;
        
        return diagramData;
    }
    
    public static BacktestResultDiagramData SetLongPositions(BacktestResultDiagramData diagramData, Strategy strategy)
    {
        for (int i = 0; i < strategy.Positions.Count; i++)
            if (strategy.Positions[i].IsLong)
            {
                diagramData.Data.Series[strategy.Positions[i].EntryCandleIndex].BuyPrice = strategy.Positions[i].EntryPrice;
                
                if (!strategy.Positions[i].IsActive)
                    diagramData.Data.Series[strategy.Positions[i].ExitCandleIndex].SellPrice = strategy.Positions[i].ExitPrice;
            }
        
        return diagramData;
    }    
    
    public static BacktestResultDiagramData SetLongPositions(BacktestResultDiagramData diagramData, List<Strategy> strategies)
    {
        for (int i = 0; i < strategies.Count; i++)
            for (int j = 0; j < strategies[i].Positions.Count; j++)
                if (strategies[i].Positions[j].IsLong)
                {
                    diagramData.Data.Series[strategies[i].Positions[j].EntryCandleIndex].BuyPrice = strategies[i].Positions[j].EntryPrice;
                    
                    if (!strategies[i].Positions[j].IsActive)
                        diagramData.Data.Series[strategies[i].Positions[j].ExitCandleIndex].SellPrice = strategies[i].Positions[j].ExitPrice;
                }        
        
        return diagramData;
    }    
    
    public static BacktestResultDiagramData SetShortPositions(BacktestResultDiagramData diagramData, Strategy strategy)
    {
        for (int i = 0; i < strategy.Positions.Count; i++)
            if (strategy.Positions[i].IsShort)
            {
                diagramData.Data.Series[strategy.Positions[i].EntryCandleIndex].SellPrice = strategy.Positions[i].EntryPrice;
                
                if (!strategy.Positions[i].IsActive)
                    diagramData.Data.Series[strategy.Positions[i].ExitCandleIndex].BuyPrice = strategy.Positions[i].ExitPrice;
            }
        
        return diagramData;
    }
    
    public static BacktestResultDiagramData SetShortPositions(BacktestResultDiagramData diagramData, List<Strategy> strategies)
    {
        for (int i = 0; i < strategies.Count; i++)
            for (int j = 0; j < strategies[i].Positions.Count; j++)
                if (strategies[i].Positions[j].IsShort)
                {
                    diagramData.Data.Series[strategies[i].Positions[j].EntryCandleIndex].SellPrice = strategies[i].Positions[j].EntryPrice;
                        
                    if (!strategies[i].Positions[j].IsActive)
                        diagramData.Data.Series[strategies[i].Positions[j].ExitCandleIndex].BuyPrice = strategies[i].Positions[j].ExitPrice;
                }        
        
        return diagramData;
    }    
    
    public static BacktestResultDiagramData SetEquity(BacktestResultDiagramData diagramData, Strategy strategy)
    {
        var from = strategy.Candles.First().DateTime;
        var to = strategy.Candles.Last().DateTime;
        
        var equity = strategy.EqiutyCurve.Expand(from, to);
        
        for (int i = 0; i < diagramData.Data.Series.Count; i++)
        {
            var date = Convert.ToDateTime(diagramData.Data.Series[i].Date);
            diagramData.Data.Series[i].Equity = Math.Round(equity[date], 2);
        }
        
        return diagramData;
    }
    
    public static BacktestResultDiagramData SetEquity(BacktestResultDiagramData diagramData, List<Strategy> strategies)
    {
        var from = strategies[0].Candles.First().DateTime;
        var to = strategies[0].Candles.Last().DateTime;

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
    
    public static BacktestResultDiagramData SetDrawdown(BacktestResultDiagramData diagramData, Strategy strategy)
    {
        var from = strategy.Candles.First().DateTime;
        var to = strategy.Candles.Last().DateTime;
        
        var drawdown = strategy.DrawdownCurve.Expand(from, to);
        
        for (int i = 0; i < diagramData.Data.Series.Count; i++)
        {
            var date = Convert.ToDateTime(diagramData.Data.Series[i].Date);
            diagramData.Data.Series[i].Drawdown = Math.Round(-1 * drawdown[date], 2);
        }
        
        return diagramData;
    }
    
    public static BacktestResultDiagramData SetDrawdown(BacktestResultDiagramData diagramData, List<Strategy> strategies)
    {
        var from = strategies[0].Candles.First().DateTime;
        var to = strategies[0].Candles.Last().DateTime;

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
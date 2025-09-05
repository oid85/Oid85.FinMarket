using System.Text.Json;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Domain.Models.Algo;
using Skender.Stock.Indicators;

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

    public static Quote Map(Candle model) =>
        new()
        {
            Open = Convert.ToDecimal(model.Open),
            Close = Convert.ToDecimal(model.Close),
            High = Convert.ToDecimal(model.High),
            Low = Convert.ToDecimal(model.Low),
            Date = model.DateTime
        };

    public static OptimizationResult MapToOptimizationResult(Strategy strategy)
    {
        var json = JsonSerializer.Serialize(strategy.Parameters);

        var result = new OptimizationResult
        {
            StrategyId = strategy.StrategyId,
            StartDate = strategy.StartDate,
            EndDate = strategy.EndDate,
            Timeframe = strategy.Timeframe,
            Ticker = strategy.Ticker,
            StrategyDescription = strategy.StrategyDescription,
            StrategyName = strategy.StrategyName,
            StrategyParams = json,
            StrategyParamsHash = ConvertHelper.Md5Encode(json),
            NumberPositions = strategy.NumberPositions,
            CurrentPosition = strategy.CurrentPosition,
            CurrentPositionCost = strategy.CurrentPositionCost,
            ProfitFactor = strategy.ProfitFactor,
            RecoveryFactor = strategy.RecoveryFactor,
            NetProfit = strategy.NetProfit,
            AverageProfit = strategy.AverageNetProfit,
            AverageProfitPercent = strategy.AverageNetProfitPercent,
            Drawdown = strategy.Drawdown,
            MaxDrawdown = strategy.MaxDrawdown,
            MaxDrawdownPercent = strategy.MaxDrawdownPercent,
            WinningPositions = strategy.WinningPositions,
            WinningTradesPercent = strategy.WinningTradesPercent,
            StartMoney = strategy.StartMoney,
            EndMoney = strategy.EndMoney,
            TotalReturn = strategy.TotalReturn,
            AnnualYieldReturn = strategy.AnnualYieldReturn
        };

        return result;
    }
    
    public static BacktestResult MapToBacktestResult(Strategy strategy)
    {
        var json = JsonSerializer.Serialize(strategy.Parameters);
        
        var result = new BacktestResult
        {
            StrategyId = strategy.StrategyId,
            StartDate = strategy.StartDate,
            EndDate = strategy.EndDate,
            Timeframe  = strategy.Timeframe,
            Ticker  = strategy.Ticker,
            StrategyDescription  = strategy.StrategyDescription,
            StrategyName  = strategy.StrategyName,
            StrategyParams  = json,
            StrategyParamsHash = ConvertHelper.Md5Encode(json),
            NumberPositions  = strategy.NumberPositions,
            CurrentPosition  = strategy.CurrentPosition,
            CurrentPositionCost  = strategy.CurrentPositionCost,
            ProfitFactor  = strategy.ProfitFactor,
            RecoveryFactor  = strategy.RecoveryFactor,
            NetProfit  = strategy.NetProfit,
            AverageProfit  = strategy.AverageNetProfit,
            AverageProfitPercent  = strategy.AverageNetProfitPercent,
            Drawdown  = strategy.Drawdown,
            MaxDrawdown  = strategy.MaxDrawdown,
            MaxDrawdownPercent  = strategy.MaxDrawdownPercent,
            WinningPositions  = strategy.WinningPositions,
            WinningTradesPercent  = strategy.WinningTradesPercent,
            StartMoney  = strategy.StartMoney,
            EndMoney  = strategy.EndMoney,
            TotalReturn  = strategy.TotalReturn,
            AnnualYieldReturn  = strategy.AnnualYieldReturn
        };
        
        return result;
    } 
    
    public static PairArbitrageOptimizationResult MapToOptimizationResult(PairArbitrageStrategy strategy)
    {
        var json = JsonSerializer.Serialize(strategy.Parameters);

        var result = new PairArbitrageOptimizationResult
        {
            StrategyId = strategy.StrategyId,
            StartDate = strategy.StartDate,
            EndDate = strategy.EndDate,
            Timeframe = strategy.Timeframe,
            TickerFirst = strategy.Ticker.First,
            TickerSecond = strategy.Ticker.Second,
            StrategyDescription = strategy.StrategyDescription,
            StrategyName = strategy.StrategyName,
            StrategyParams = json,
            StrategyParamsHash = ConvertHelper.Md5Encode(json),
            NumberPositions = strategy.NumberPositions,
            CurrentPositionFirst = strategy.CurrentPosition.First,
            CurrentPositionSecond = strategy.CurrentPosition.Second,
            CurrentPositionCost = strategy.CurrentPositionCost,
            ProfitFactor = strategy.ProfitFactor,
            RecoveryFactor = strategy.RecoveryFactor,
            NetProfit = strategy.NetProfit,
            AverageProfit = strategy.AverageNetProfit,
            AverageProfitPercent = strategy.AverageNetProfitPercent,
            Drawdown = strategy.Drawdown,
            MaxDrawdown = strategy.MaxDrawdown,
            MaxDrawdownPercent = strategy.MaxDrawdownPercent,
            WinningPositions = strategy.WinningPositions,
            WinningTradesPercent = strategy.WinningTradesPercent,
            StartMoney = strategy.StartMoney,
            EndMoney = strategy.EndMoney,
            TotalReturn = strategy.TotalReturn,
            AnnualYieldReturn = strategy.AnnualYieldReturn
        };

        return result;
    }

    public static PairArbitrageBacktestResult MapToBacktestResult(PairArbitrageStrategy strategy)
    {
        var json = JsonSerializer.Serialize(strategy.Parameters);

        var result = new PairArbitrageBacktestResult
        {
            StrategyId = strategy.StrategyId,
            StartDate = strategy.StartDate,
            EndDate = strategy.EndDate,
            Timeframe = strategy.Timeframe,
            TickerFirst = strategy.Ticker.First,
            TickerSecond = strategy.Ticker.Second,
            StrategyDescription = strategy.StrategyDescription,
            StrategyName = strategy.StrategyName,
            StrategyParams = json,
            StrategyParamsHash = ConvertHelper.Md5Encode(json),
            NumberPositions = strategy.NumberPositions,
            CurrentPositionFirst = strategy.CurrentPosition.First,
            CurrentPositionSecond = strategy.CurrentPosition.Second,
            CurrentPositionCost = strategy.CurrentPositionCost,
            ProfitFactor = strategy.ProfitFactor,
            RecoveryFactor = strategy.RecoveryFactor,
            NetProfit = strategy.NetProfit,
            AverageProfit = strategy.AverageNetProfit,
            AverageProfitPercent = strategy.AverageNetProfitPercent,
            Drawdown = strategy.Drawdown,
            MaxDrawdown = strategy.MaxDrawdown,
            MaxDrawdownPercent = strategy.MaxDrawdownPercent,
            WinningPositions = strategy.WinningPositions,
            WinningTradesPercent = strategy.WinningTradesPercent,
            StartMoney = strategy.StartMoney,
            EndMoney = strategy.EndMoney,
            TotalReturn = strategy.TotalReturn,
            AnnualYieldReturn = strategy.AnnualYieldReturn
        };

        return result;
    }
}
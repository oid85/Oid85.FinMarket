using System.Text.Json;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Domain.Models.Algo;
using Oid85.FinMarket.Domain.Models.StatisticalArbitration;

namespace Oid85.FinMarket.DataAccess.Mapping;

public static class DataAccessMapper
{
    public static StrategySignalEntity Map(StrategySignal model) =>
        new()
        {
            Ticker = model.Ticker,
            CountSignals = model.CountSignals,
            CountStrategies = model.CountStrategies,
            PercentSignals = model.PercentSignals,
            PositionCost = model.PositionCost,
            PositionSize = model.PositionSize,
            PositionPercentPortfolio = model.PositionPercentPortfolio,
            LastPrice = model.LastPrice
        };
    
    public static StrategySignal Map(StrategySignalEntity entity) =>
        new()
        {
            Ticker = entity.Ticker,
            CountSignals = entity.CountSignals,
            CountStrategies = entity.CountStrategies,
            PercentSignals = entity.PercentSignals,
            PositionCost = entity.PositionCost,
            PositionSize = entity.PositionSize,
            PositionPercentPortfolio = entity.PositionPercentPortfolio,
            LastPrice = entity.LastPrice
        }; 
    
    public static OptimizationResultEntity Map(OptimizationResult model) =>
        new()
        {
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Ticker = model.Ticker,
            Timeframe = model.Timeframe,
            StrategyId = model.StrategyId,
            StrategyDescription = model.StrategyDescription,
            StrategyName = model.StrategyName,
            StrategyParams = model.StrategyParams,
            StrategyParamsHash = model.StrategyParamsHash,
            NumberPositions = model.NumberPositions,
            CurrentPosition = model.CurrentPosition,
            CurrentPositionCost = model.CurrentPositionCost,
            ProfitFactor = model.ProfitFactor,
            RecoveryFactor = model.RecoveryFactor,
            NetProfit = model.NetProfit,
            AverageProfit = model.AverageProfit,
            AverageProfitPercent = model.AverageProfitPercent,
            Drawdown = model.Drawdown,
            MaxDrawdown = model.MaxDrawdown,
            MaxDrawdownPercent = model.MaxDrawdownPercent,
            WinningPositions = model.WinningPositions,
            WinningTradesPercent = model.WinningTradesPercent,
            StartMoney = model.StartMoney,
            EndMoney = model.EndMoney,
            TotalReturn = model.TotalReturn,
            AnnualYieldReturn = model.AnnualYieldReturn
        };
    
    public static OptimizationResult Map(OptimizationResultEntity entity) =>
        new()
        {
            Id = entity.Id,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Ticker = entity.Ticker,
            Timeframe = entity.Timeframe,
            StrategyId = entity.StrategyId,
            StrategyDescription = entity.StrategyDescription,
            StrategyName = entity.StrategyName,
            StrategyParams = entity.StrategyParams,
            StrategyParamsHash = entity.StrategyParamsHash,
            NumberPositions = entity.NumberPositions,
            CurrentPosition = entity.CurrentPosition,
            CurrentPositionCost = entity.CurrentPositionCost,
            ProfitFactor = entity.ProfitFactor,
            RecoveryFactor = entity.RecoveryFactor,
            NetProfit = entity.NetProfit,
            AverageProfit = entity.AverageProfit,
            AverageProfitPercent = entity.AverageProfitPercent,
            Drawdown = entity.Drawdown,
            MaxDrawdown = entity.MaxDrawdown,
            MaxDrawdownPercent = entity.MaxDrawdownPercent,
            WinningPositions = entity.WinningPositions,
            WinningTradesPercent = entity.WinningTradesPercent,
            StartMoney = entity.StartMoney,
            EndMoney = entity.EndMoney,
            TotalReturn = entity.TotalReturn,
            AnnualYieldReturn = entity.AnnualYieldReturn
        };    
    
    public static BacktestResultEntity Map(BacktestResult model) =>
        new()
        {
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Ticker = model.Ticker,
            Timeframe = model.Timeframe,
            StrategyId = model.StrategyId,
            StrategyDescription = model.StrategyDescription,
            StrategyName = model.StrategyName,
            StrategyParams = model.StrategyParams,
            StrategyParamsHash = model.StrategyParamsHash,
            NumberPositions = model.NumberPositions,
            CurrentPosition = model.CurrentPosition,
            CurrentPositionCost = model.CurrentPositionCost,
            ProfitFactor = model.ProfitFactor,
            RecoveryFactor = model.RecoveryFactor,
            NetProfit = model.NetProfit,
            AverageProfit = model.AverageProfit,
            AverageProfitPercent = model.AverageProfitPercent,
            Drawdown = model.Drawdown,
            MaxDrawdown = model.MaxDrawdown,
            MaxDrawdownPercent = model.MaxDrawdownPercent,
            WinningPositions = model.WinningPositions,
            WinningTradesPercent = model.WinningTradesPercent,
            StartMoney = model.StartMoney,
            EndMoney = model.EndMoney,
            TotalReturn = model.TotalReturn,
            AnnualYieldReturn = model.AnnualYieldReturn
        };
    
    public static BacktestResult Map(BacktestResultEntity entity) =>
        new()
        {
            Id = entity.Id,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Ticker = entity.Ticker,
            Timeframe = entity.Timeframe,
            StrategyId = entity.StrategyId,
            StrategyDescription = entity.StrategyDescription,
            StrategyName = entity.StrategyName,
            StrategyParams = entity.StrategyParams,
            StrategyParamsHash = entity.StrategyParamsHash,
            NumberPositions = entity.NumberPositions,
            CurrentPosition = entity.CurrentPosition,
            CurrentPositionCost = entity.CurrentPositionCost,
            ProfitFactor = entity.ProfitFactor,
            RecoveryFactor = entity.RecoveryFactor,
            NetProfit = entity.NetProfit,
            AverageProfit = entity.AverageProfit,
            AverageProfitPercent = entity.AverageProfitPercent,
            Drawdown = entity.Drawdown,
            MaxDrawdown = entity.MaxDrawdown,
            MaxDrawdownPercent = entity.MaxDrawdownPercent,
            WinningPositions = entity.WinningPositions,
            WinningTradesPercent = entity.WinningTradesPercent,
            StartMoney = entity.StartMoney,
            EndMoney = entity.EndMoney,
            TotalReturn = entity.TotalReturn,
            AnnualYieldReturn = entity.AnnualYieldReturn
        };    
    
    public static AnalyseResultEntity Map(AnalyseResult model) =>
        new()
        {
            Date = model.Date,
            InstrumentId = model.InstrumentId,
            AnalyseType = model.AnalyseType,
            ResultString = model.ResultString,
            ResultNumber = model.ResultNumber
        };
    
    public static AnalyseResult Map(AnalyseResultEntity entity) =>
        new()
        {
            Id = entity.Id,
            Date = entity.Date,
            InstrumentId = entity.InstrumentId,
            AnalyseType = entity.AnalyseType,
            ResultString = entity.ResultString,
            ResultNumber = entity.ResultNumber
        };
    
    public static BondCouponEntity Map(BondCoupon model) =>
        new()
        {
            InstrumentId = model.InstrumentId,
            Ticker = model.Ticker,
            CouponDate = model.CouponDate,
            CouponNumber = model.CouponNumber,
            CouponPeriod = model.CouponPeriod,
            CouponStartDate = model.CouponStartDate,
            CouponEndDate = model.CouponEndDate,
            PayOneBond = model.PayOneBond
        };
    
    public static BondCoupon Map(BondCouponEntity entity) =>
        new()
        {
            Id = entity.Id,
            InstrumentId = entity.InstrumentId,
            Ticker = entity.Ticker,
            CouponDate = entity.CouponDate,
            CouponNumber = entity.CouponNumber,
            CouponPeriod = entity.CouponPeriod,
            CouponStartDate = entity.CouponStartDate,
            CouponEndDate = entity.CouponEndDate,
            PayOneBond = entity.PayOneBond
        };
    
    public static BondEntity Map(Bond model) =>
        new()
        {
            Ticker = model.Ticker,
            LastPrice = model.LastPrice,
            Isin = model.Isin,
            Figi = model.Figi,
            InstrumentId = model.InstrumentId,
            Name = model.Name,
            Sector = model.Sector,
            Currency = model.Currency,
            Nkd = model.Nkd,
            MaturityDate = model.MaturityDate,
            FloatingCouponFlag = model.FloatingCouponFlag,
            RiskLevel = model.RiskLevel
        };
    
    public static Bond Map(BondEntity entity) =>
        new()
        {
            Id = entity.Id,
            Ticker = entity.Ticker,
            LastPrice = entity.LastPrice,
            Isin = entity.Isin,
            Figi = entity.Figi,
            InstrumentId = entity.InstrumentId,
            Name = entity.Name,
            Sector = entity.Sector,
            Currency = entity.Currency,
            Nkd = entity.Nkd,
            MaturityDate = entity.MaturityDate,
            FloatingCouponFlag = entity.FloatingCouponFlag,
            RiskLevel = entity.RiskLevel
        };
    
    public static DailyCandleEntity Map(DailyCandle model) =>
        new()
        {
            InstrumentId = model.InstrumentId,
            Open = model.Open,
            Close = model.Close,
            High = model.High,
            Low = model.Low,
            Volume = model.Volume,
            Date = model.Date,
            IsComplete = model.IsComplete
        };
    
    public static DailyCandle Map(DailyCandleEntity entity) =>
        new()
        {
            Id = entity.Id,
            InstrumentId = entity.InstrumentId,
            Open = entity.Open,
            Close = entity.Close,
            High = entity.High,
            Low = entity.Low,
            Volume = entity.Volume,
            Date = entity.Date,
            IsComplete = entity.IsComplete
        };
    
    public static void Map(ref DailyCandleEntity? entity, DailyCandle model)
    {
        entity ??= new DailyCandleEntity();
        
        entity.InstrumentId = model.InstrumentId;
        entity.Open = model.Open;
        entity.Close = model.Close;
        entity.High = model.High;
        entity.Low = model.Low;
        entity.Volume = model.Volume;
        entity.Date = model.Date;
        entity.IsComplete = model.IsComplete;
    }
    
    public static CurrencyEntity Map(Currency model) =>
        new()
        {
            Ticker = model.Ticker,
            LastPrice = model.LastPrice,
            Isin = model.Isin,
            Figi = model.Figi,
            ClassCode = model.ClassCode,
            Name = model.Name,
            IsoCurrencyName = model.IsoCurrencyName,
            InstrumentId = model.InstrumentId
        };
    
    public static Currency Map(CurrencyEntity entity) =>
        new()
        {
            Id = entity.Id,
            Ticker = entity.Ticker,
            LastPrice = entity.LastPrice,
            Isin = entity.Isin,
            Figi = entity.Figi,
            ClassCode = entity.ClassCode,
            Name = entity.Name,
            IsoCurrencyName = entity.IsoCurrencyName,
            InstrumentId = entity.InstrumentId
        };
    
    public static DividendInfoEntity Map(DividendInfo model)
    {
        var entity = new DividendInfoEntity
        {
            InstrumentId = model.InstrumentId,
            Ticker = model.Ticker,
            RecordDate = model.RecordDate,
            DeclaredDate = model.DeclaredDate,
            Dividend = model.Dividend,
            DividendPrc = model.DividendPrc
        };

        return entity;
    }
    
    public static DividendInfo Map(DividendInfoEntity entity) =>
        new()
        {
            Id = entity.Id,
            InstrumentId = entity.InstrumentId,
            Ticker = entity.Ticker,
            RecordDate = entity.RecordDate,
            DeclaredDate = entity.DeclaredDate,
            Dividend = entity.Dividend,
            DividendPrc = entity.DividendPrc
        };
    
    public static void Map(ref HourlyCandleEntity? entity, HourlyCandle model)
    {
        entity ??= new HourlyCandleEntity();
        
        entity.InstrumentId = model.InstrumentId;
        entity.Open = model.Open;
        entity.Close = model.Close;
        entity.High = model.High;
        entity.Low = model.Low;
        entity.Volume = model.Volume;
        entity.Date = model.Date;
        entity.Time = model.Time;
        entity.IsComplete = model.IsComplete;
    }
    
    public static HourlyCandleEntity Map(HourlyCandle model) =>
        new()
        {
            InstrumentId = model.InstrumentId,
            Open = model.Open,
            Close = model.Close,
            High = model.High,
            Low = model.Low,
            Volume = model.Volume,
            Date = model.Date,
            Time = model.Time,
            DateTimeTicks = model.DateTimeTicks,
            IsComplete = model.IsComplete
        };
    
    public static HourlyCandle Map(HourlyCandleEntity entity) =>
        new()
        {
            Id = entity.Id,
            InstrumentId = entity.InstrumentId,
            Open = entity.Open,
            Close = entity.Close,
            High = entity.High,
            Low = entity.Low,
            Volume = entity.Volume,
            Date = entity.Date,
            Time = entity.Time,
            DateTimeTicks = entity.DateTimeTicks,
            IsComplete = entity.IsComplete
        };    
    
    public static ForecastConsensusEntity Map(ForecastConsensus model) =>
        new()
        {
            Ticker = model.Ticker,
            InstrumentId = model.InstrumentId,
            RecommendationString = model.RecommendationString,
            RecommendationNumber = model.RecommendationNumber,
            Currency = model.Currency,
            CurrentPrice = model.CurrentPrice,
            ConsensusPrice = model.ConsensusPrice,
            MinTarget = model.MinTarget,
            MaxTarget = model.MaxTarget,
            PriceChange = model.PriceChange,
            PriceChangeRel = model.PriceChangeRel
        };
    
    public static ForecastConsensus Map(ForecastConsensusEntity entity) =>
        new()
        {
            Id = entity.Id,
            Ticker = entity.Ticker,
            InstrumentId = entity.InstrumentId,
            RecommendationString = entity.RecommendationString,
            RecommendationNumber = entity.RecommendationNumber,
            Currency = entity.Currency,
            CurrentPrice = entity.CurrentPrice,
            ConsensusPrice = entity.ConsensusPrice,
            MinTarget = entity.MinTarget,
            MaxTarget = entity.MaxTarget,
            PriceChange = entity.PriceChange,
            PriceChangeRel = entity.PriceChangeRel
        };
    
    public static ForecastTargetEntity Map(ForecastTarget model) =>
        new()
        {
            Ticker = model.Ticker,
            InstrumentId = model.InstrumentId,
            Company = model.Company,
            RecommendationString = model.RecommendationString,
            RecommendationNumber = model.RecommendationNumber,
            RecommendationDate = model.RecommendationDate,
            Currency = model.Currency,
            CurrentPrice = model.CurrentPrice,
            TargetPrice = model.TargetPrice,
            PriceChange = model.PriceChange,
            PriceChangeRel = model.PriceChangeRel,
            ShowName = model.ShowName
        };
    
    public static ForecastTarget Map(ForecastTargetEntity entity) =>
        new()
        {
            Id = entity.Id,
            Ticker = entity.Ticker,
            InstrumentId = entity.InstrumentId,
            Company = entity.Company,
            RecommendationString = entity.RecommendationString,
            RecommendationNumber = entity.RecommendationNumber,
            RecommendationDate = entity.RecommendationDate,
            Currency = entity.Currency,
            CurrentPrice = entity.CurrentPrice,
            TargetPrice = entity.TargetPrice,
            PriceChange = entity.PriceChange,
            PriceChangeRel = entity.PriceChangeRel,
            ShowName = entity.ShowName
        };
    
    public static FutureEntity Map(Future model) =>
        new()
        {
            Ticker = model.Ticker,
            LastPrice = model.LastPrice,
            Figi = model.Figi,
            InstrumentId = model.InstrumentId,
            Name = model.Name,
            ExpirationDate = model.ExpirationDate,
            Lot = model.Lot,
            FirstTradeDate = model.FirstTradeDate,
            LastTradeDate = model.LastTradeDate,
            FutureType = model.FutureType,
            AssetType = model.AssetType,
            BasicAsset = model.BasicAsset,
            BasicAssetSize = model.BasicAssetSize,
            InitialMarginOnBuy = model.InitialMarginOnBuy,
            InitialMarginOnSell = model.InitialMarginOnSell,
            MinPriceIncrementAmount = model.MinPriceIncrementAmount
        };
    
    public static Future Map(FutureEntity entity) =>
        new()
        {
            Id = entity.Id,
            Ticker = entity.Ticker,
            LastPrice = entity.LastPrice,
            Figi = entity.Figi,
            InstrumentId = entity.InstrumentId,
            Name = entity.Name,
            ExpirationDate = entity.ExpirationDate,
            Lot = entity.Lot,
            FirstTradeDate = entity.FirstTradeDate,
            LastTradeDate = entity.LastTradeDate,
            FutureType = entity.FutureType,
            AssetType = entity.AssetType,
            BasicAsset = entity.BasicAsset,
            BasicAssetSize = entity.BasicAssetSize,
            InitialMarginOnBuy = entity.InitialMarginOnBuy,
            InitialMarginOnSell = entity.InitialMarginOnSell,
            MinPriceIncrementAmount = entity.MinPriceIncrementAmount
        };
    
    public static FinIndexEntity Map(FinIndex model) =>
        new()
        {
            Figi = model.Figi,
            InstrumentId = model.InstrumentId,
            Ticker = model.Ticker,
            LastPrice = model.LastPrice,
            ClassCode = model.ClassCode,
            Currency = model.Currency,
            InstrumentKind = model.InstrumentKind,
            Name = model.Name,
            Exchange = model.Exchange
        };
    
    public static FinIndex Map(FinIndexEntity entity) =>
        new()
        {
            Id = entity.Id,
            Figi = entity.Figi,
            InstrumentId = entity.InstrumentId,
            Ticker = entity.Ticker,
            LastPrice = entity.LastPrice,
            ClassCode = entity.ClassCode,
            Currency = entity.Currency,
            InstrumentKind = entity.InstrumentKind,
            Name = entity.Name,
            Exchange = entity.Exchange
        };
    
    public static void Map(ref InstrumentEntity? entity, Instrument model)
    {
        entity ??= new InstrumentEntity();
        
        entity.InstrumentId = model.InstrumentId;
        entity.Ticker = model.Ticker;
        entity.Name = model.Name;
        entity.Sector = model.Sector;
        entity.Type = model.Type;
    }
    
    public static Instrument Map(InstrumentEntity entity) =>
        new()
        {
            Id = entity.Id,
            InstrumentId = entity.InstrumentId,
            Ticker = entity.Ticker,
            Name = entity.Name,
            Sector = entity.Sector,
            Type = entity.Type
        };
    
    public static MarketEventEntity Map(MarketEvent model) =>
        new()
        {
            Date = model.Date,
            Time = model.Time,
            InstrumentId = model.InstrumentId,
            Ticker = model.Ticker,
            InstrumentName = model.InstrumentName,
            MarketEventType = model.MarketEventType,
            MarketEventText = model.MarketEventText,
            IsActive = model.IsActive,
            SentNotification = model.SentNotification
        };
    
    public static MarketEvent Map(MarketEventEntity entity) =>
        new()
        {
            Id = entity.Id,
            Date = entity.Date,
            Time = entity.Time,
            InstrumentId = entity.InstrumentId,
            Ticker = entity.Ticker,
            InstrumentName = entity.InstrumentName,
            MarketEventType = entity.MarketEventType,
            MarketEventText = entity.MarketEventText,
            IsActive = entity.IsActive,
            SentNotification = entity.SentNotification
        };
    
    public static ShareMultiplicatorEntity Map(ShareMultiplicator model) =>
        new()
        {
            Name = model.Name,
            Ticker = model.Ticker,
            MarketCap = model.MarketCap,
            Ev = model.Ev,
            Revenue = model.Revenue,
            NetIncome = model.NetIncome,
            DdAo = model.DdAo,
            DdAp = model.DdAp,
            DdNetIncome = model.DdNetIncome,
            Pe = model.Pe,
            Ps = model.Ps,
            Pb = model.Pb,
            EvEbitda = model.EvEbitda,
            EbitdaMargin = model.EbitdaMargin,
            NetDebtEbitda = model.NetDebtEbitda
        };
    
    public static ShareMultiplicator Map(ShareMultiplicatorEntity entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Ticker = entity.Ticker,
            MarketCap = entity.MarketCap,
            Ev = entity.Ev,
            Revenue = entity.Revenue,
            NetIncome = entity.NetIncome,
            DdAo = entity.DdAo,
            DdAp = entity.DdAp,
            DdNetIncome = entity.DdNetIncome,
            Pe = entity.Pe,
            Ps = entity.Ps,
            Pb = entity.Pb,
            EvEbitda = entity.EvEbitda,
            EbitdaMargin = entity.EbitdaMargin,
            NetDebtEbitda = entity.NetDebtEbitda
        };
    
    public static BankMultiplicatorEntity Map(BankMultiplicator model) =>
        new()
        {
            Name = model.Name,
            Ticker = model.Ticker,
            MarketCap = model.MarketCap,
            NetOperatingIncome = model.NetOperatingIncome,
            NetIncome = model.NetIncome,
            DdAo = model.DdAo,
            DdAp = model.DdAp,
            DdNetIncome = model.DdNetIncome,
            Pe = model.Pe,
            Pb = model.Pb,
            NetInterestMargin = model.NetInterestMargin,
            Roe = model.Roe,
            Roa = model.Roa
        };
    
    public static BankMultiplicator Map(BankMultiplicatorEntity entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Ticker = entity.Ticker,
            MarketCap = entity.MarketCap,
            NetOperatingIncome = entity.NetOperatingIncome,
            NetIncome = entity.NetIncome,
            DdAo = entity.DdAo,
            DdAp = entity.DdAp,
            DdNetIncome = entity.DdNetIncome,
            Pe = entity.Pe,
            Pb = entity.Pb,
            NetInterestMargin = entity.NetInterestMargin,
            Roe = entity.Roe,
            Roa = entity.Roa
        };    
    
    public static ShareEntity Map(Share model) =>
        new()
        {
            Ticker = model.Ticker,
            LastPrice = model.LastPrice,
            Isin = model.Isin,
            Figi = model.Figi,
            InstrumentId = model.InstrumentId,
            Name = model.Name,
            Sector = model.Sector
        };
    
    public static Share Map(ShareEntity entity) =>
        new()
        {
            Id = entity.Id,
            Ticker = entity.Ticker,
            LastPrice = entity.LastPrice,
            Isin = entity.Isin,
            Figi = entity.Figi,
            InstrumentId = entity.InstrumentId,
            Name = entity.Name,
            Sector = entity.Sector
        };

    public static AssetReportEventEntity Map(AssetReportEvent model) =>
        new()
        {
            InstrumentId = model.InstrumentId,
            ReportDate = model.ReportDate,
            PeriodYear = model.PeriodYear,
            PeriodNum = model.PeriodNum,
            Type = model.Type
        };
    
    public static AssetReportEvent Map(AssetReportEventEntity entity) =>
        new()
        {
            Id = entity.Id,
            InstrumentId = entity.InstrumentId,
            ReportDate = entity.ReportDate,
            PeriodYear = entity.PeriodYear,
            PeriodNum = entity.PeriodNum,
            Type = entity.Type
        };
    
    public static FearGreedIndexEntity Map(FearGreedIndex model) =>
        new()
        {
            Date = model.Date,
            MarketMomentum = model.MarketMomentum,
            MarketVolatility = model.MarketVolatility,
            StockPriceBreadth = model.StockPriceBreadth,
            StockPriceStrength = model.StockPriceStrength,
            Value = model.Value
        };
    
    public static FearGreedIndex Map(FearGreedIndexEntity entity) =>
        new()
        {
            Id = entity.Id,
            Date = entity.Date,
            MarketMomentum = entity.MarketMomentum,
            MarketVolatility = entity.MarketVolatility,
            StockPriceBreadth = entity.StockPriceBreadth,
            StockPriceStrength = entity.StockPriceStrength,
            Value = entity.Value
        };
    
    public static CorrelationEntity Map(Correlation model) =>
        new()
        {
            Ticker1 = model.Ticker1,
            Ticker2 = model.Ticker2,
            Value = model.Value
        };
    
    public static Correlation Map(CorrelationEntity entity) =>
        new()
        {
            Id = entity.Id,
            Ticker1 = entity.Ticker1,
            Ticker2 = entity.Ticker2,
            Value = entity.Value
        };    
    
    public static RegressionTailEntity Map(RegressionTail model) =>
        new()
        {
            Ticker1 = model.Ticker1,
            Ticker2 = model.Ticker2,
            Tails = JsonSerializer.Serialize(model.Tails),
            Slope = model.Slope,
            Intercept = model.Intercept,
            IsStationary = model.IsStationary
        };
    
    public static RegressionTail Map(RegressionTailEntity entity) =>
        new()
        {
            Id = entity.Id,
            Ticker1 = entity.Ticker1,
            Ticker2 = entity.Ticker2,
            Tails = JsonSerializer.Deserialize<List<RegressionTailItem>>(entity.Tails) ?? [],
            Slope = entity.Slope,
            Intercept = entity.Intercept,
            IsStationary = entity.IsStationary
        };    
}
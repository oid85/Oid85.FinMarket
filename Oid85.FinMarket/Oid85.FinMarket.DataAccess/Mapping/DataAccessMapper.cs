using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.DataAccess.Mapping;

public static class DataAccessMapper
{
    public static AnalyseResultEntity Map(AnalyseResult model)
    {
        var entity = new AnalyseResultEntity
        {
            Date = model.Date,
            InstrumentId = model.InstrumentId,
            AnalyseType = model.AnalyseType,
            ResultString = model.ResultString,
            ResultNumber = model.ResultNumber
        };

        return entity;
    }
    
    public static AnalyseResult Map(AnalyseResultEntity entity)
    {
        var model = new AnalyseResult
        {
            Id = entity.Id,
            Date = entity.Date,
            InstrumentId = entity.InstrumentId,
            AnalyseType = entity.AnalyseType,
            ResultString = entity.ResultString,
            ResultNumber = entity.ResultNumber
        };

        return model;
    }
    
    public static BondCouponEntity Map(BondCoupon model)
    {
        var entity = new BondCouponEntity
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

        return entity;
    }
    
    public static BondCoupon Map(BondCouponEntity entity)
    {
        var model = new BondCoupon
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

        return model;
    }
    
    public static BondEntity Map(Bond model)
    {
        var entity = new BondEntity
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

        return entity;
    }
    
    public static Bond Map(BondEntity entity)
    {
        var model = new Bond
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

        return model;
    }
    
    public static  CandleEntity Map(Candle model)
    {
        var entity = new CandleEntity
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

        return entity;
    }
    
    public static  Candle Map(CandleEntity entity)
    {
        var model = new Candle
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

        return model;
    }
    
    public static void Map(ref CandleEntity? entity, Candle model)
    {
        entity ??= new CandleEntity();
        
        entity.InstrumentId = model.InstrumentId;
        entity.Open = model.Open;
        entity.Close = model.Close;
        entity.High = model.High;
        entity.Low = model.Low;
        entity.Volume = model.Volume;
        entity.Date = model.Date;
        entity.IsComplete = model.IsComplete;
    }
    
    public static  CurrencyEntity Map(Currency model)
    {
        var entity = new CurrencyEntity
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

        return entity;
    }
    
    public static  Currency Map(CurrencyEntity entity)
    {
        var model = new Currency
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

        return model;
    }
    
    public static  DividendInfoEntity Map(DividendInfo model)
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
    
    public static  DividendInfo Map(DividendInfoEntity entity)
    {
        var model = new DividendInfo
        {
            Id = entity.Id,
            InstrumentId = entity.InstrumentId,
            Ticker = entity.Ticker,
            RecordDate = entity.RecordDate,
            DeclaredDate = entity.DeclaredDate,
            Dividend = entity.Dividend,
            DividendPrc = entity.DividendPrc
        };

        return model;
    }
    
    public static void Map(ref FiveMinuteCandleEntity? entity, FiveMinuteCandle model)
    {
        entity ??= new FiveMinuteCandleEntity();
        
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
    
    public static FiveMinuteCandleEntity Map(FiveMinuteCandle model)
    {
        var entity = new FiveMinuteCandleEntity
        {
            InstrumentId = model.InstrumentId,
            Open = model.Open,
            Close = model.Close,
            High = model.High,
            Low = model.Low,
            Volume = model.Volume,
            Date = model.Date,
            Time = model.Time,
            IsComplete = model.IsComplete
        };

        return entity;
    }
    
    public static FiveMinuteCandle Map(FiveMinuteCandleEntity entity)
    {
        var model = new FiveMinuteCandle
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
            IsComplete = entity.IsComplete
        };

        return model;
    }
    
    public static ForecastConsensusEntity Map(ForecastConsensus model)
    {
        var entity = new ForecastConsensusEntity
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

        return entity;
    }
    
    public static ForecastConsensus Map(ForecastConsensusEntity entity)
    {
        var model = new ForecastConsensus
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

        return model;
    }
    
    public static ForecastTargetEntity Map(ForecastTarget model)
    {
        var entity = new ForecastTargetEntity
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

        return entity;
    }
    
    public static ForecastTarget Map(ForecastTargetEntity entity)
    {
        var model = new ForecastTarget
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

        return model;
    }
    
    public static FutureEntity Map(Future model)
    {
        var entity = new FutureEntity
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

        return entity;
    }
    
    public static Future Map(FutureEntity entity)
    {
        var model = new Future
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

        return model;
    }
    
    public static FinIndexEntity Map(FinIndex model)
    {
        var entity = new FinIndexEntity
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

        return entity;
    }
    
    public static FinIndex Map(FinIndexEntity entity)
    {
        var model = new FinIndex
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

        return model;
    }
    
    public static void Map(ref InstrumentEntity? entity, Instrument model)
    {
        entity ??= new InstrumentEntity();
        
        entity.InstrumentId = model.InstrumentId;
        entity.Ticker = model.Ticker;
        entity.Name = model.Name;
        entity.Type = model.Type;
    }
    
    public static Instrument Map(InstrumentEntity entity)
    {
        var model = new Instrument
        {
            Id = entity.Id,
            InstrumentId = entity.InstrumentId,
            Ticker = entity.Ticker,
            Name = entity.Name,
            Type = entity.Type
        };

        return model;
    }
    
    public static MarketEventEntity Map(MarketEvent model)
    {
        var entity = new MarketEventEntity
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

        return entity;
    }
    
    public static MarketEvent Map(MarketEventEntity entity)
    {
        var model = new MarketEvent
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

        return model;
    }
    
        public static MultiplicatorEntity Map(Multiplicator model)
    {
        var entity = new MultiplicatorEntity
        {
            TickerAo = model.TickerAo,
            TickerAp = model.TickerAp,
            TotalSharesAo = model.TotalSharesAo,
            TotalSharesAp = model.TotalSharesAp,
            Beta = model.Beta,
            Revenue = model.Revenue,
            OperatingIncome = model.OperatingIncome,
            Pe = model.Pe,
            Pb = model.Pb,
            Pbv = model.Pbv,
            Ev = model.Ev,
            Roe = model.Roe,
            Roa = model.Roa,
            NetInterestMargin = model.NetInterestMargin,
            TotalDebt = model.TotalDebt,
            NetDebt = model.NetDebt,
            MarketCapitalization = model.MarketCapitalization,
            NetIncome = model.NetIncome,
            Ebitda = model.Ebitda,
            Eps = model.Eps,
            FreeCashFlow = model.FreeCashFlow,
            EvToEbitda = model.EvToEbitda,
            TotalDebtToEbitda = model.TotalDebtToEbitda,
            NetDebtToEbitda = model.NetDebtToEbitda
        };

        return entity;
    }
    
    public static Multiplicator Map(MultiplicatorEntity entity)
    {
        var model = new Multiplicator
        {
            Id = entity.Id,
            TickerAo = entity.TickerAo,
            TickerAp = entity.TickerAp,
            TotalSharesAo = entity.TotalSharesAo,
            TotalSharesAp = entity.TotalSharesAp,
            Beta = entity.Beta,
            Revenue = entity.Revenue,
            OperatingIncome = entity.OperatingIncome,
            Pe = entity.Pe,
            Pb = entity.Pb,
            Pbv = entity.Pbv,
            Ev = entity.Ev,
            Roe = entity.Roe,
            Roa = entity.Roa,
            NetInterestMargin = entity.NetInterestMargin,
            TotalDebt = entity.TotalDebt,
            NetDebt = entity.NetDebt,
            MarketCapitalization = entity.MarketCapitalization,
            NetIncome = entity.NetIncome,
            Ebitda = entity.Ebitda,
            Eps = entity.Eps,
            FreeCashFlow = entity.FreeCashFlow,
            EvToEbitda = entity.EvToEbitda,
            TotalDebtToEbitda = entity.TotalDebtToEbitda,
            NetDebtToEbitda = entity.NetDebtToEbitda
        };

        return model;
    }
    
    public static ShareEntity Map(Share model)
    {
        var entity = new ShareEntity
        {
            Ticker = model.Ticker,
            LastPrice = model.LastPrice,
            Isin = model.Isin,
            Figi = model.Figi,
            InstrumentId = model.InstrumentId,
            Name = model.Name,
            Sector = model.Sector
        };

        return entity;
    }
    
    public static Share Map(ShareEntity entity)
    {
        var model = new Share
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

        return model;
    }
    
        public static SpreadEntity Map(Spread model)
    {
        var entity = new SpreadEntity
        {
            DateTime = model.DateTime,
            FirstInstrumentId = model.FirstInstrumentId,
            FirstInstrumentTicker = model.FirstInstrumentTicker,
            FirstInstrumentRole = model.FirstInstrumentRole,
            FirstInstrumentPrice = model.FirstInstrumentPrice,
            SecondInstrumentId = model.SecondInstrumentId,
            SecondInstrumentTicker = model.SecondInstrumentTicker,
            SecondInstrumentRole = model.SecondInstrumentRole,
            SecondInstrumentPrice = model.SecondInstrumentPrice,
            Multiplier = model.Multiplier,
            PriceDifference = model.PriceDifference,
            PriceDifferencePrc = model.PriceDifferencePrc,
            PriceDifferenceAverage = model.PriceDifferenceAverage,
            PriceDifferenceAveragePrc = model.PriceDifferenceAveragePrc,
            Funding = model.Funding,
            SpreadPricePosition = model.SpreadPricePosition
        };

        return entity;
    }
    
    public static Spread Map(SpreadEntity entity)
    {
        var model = new Spread
        {
            Id = entity.Id,
            DateTime = entity.DateTime,
            FirstInstrumentId = entity.FirstInstrumentId,
            FirstInstrumentTicker = entity.FirstInstrumentTicker,
            FirstInstrumentRole = entity.FirstInstrumentRole,
            FirstInstrumentPrice = entity.FirstInstrumentPrice,
            SecondInstrumentId = entity.SecondInstrumentId,
            SecondInstrumentTicker = entity.SecondInstrumentTicker,
            SecondInstrumentRole = entity.SecondInstrumentRole,
            SecondInstrumentPrice = entity.SecondInstrumentPrice,
            Multiplier = entity.Multiplier,
            PriceDifference = entity.PriceDifference,
            PriceDifferencePrc = entity.PriceDifferencePrc,
            PriceDifferenceAverage = entity.PriceDifferenceAverage,
            PriceDifferenceAveragePrc = entity.PriceDifferenceAveragePrc,
            Funding = entity.Funding,
            SpreadPricePosition = entity.SpreadPricePosition
        };

        return model;
    }
}
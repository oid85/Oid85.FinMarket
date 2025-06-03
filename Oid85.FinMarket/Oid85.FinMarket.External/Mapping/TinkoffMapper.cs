using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Tinkoff.InvestApi.V1;
using Bond = Oid85.FinMarket.Domain.Models.Bond;
using Currency = Oid85.FinMarket.Domain.Models.Currency;
using Future = Oid85.FinMarket.Domain.Models.Future;
using Share = Oid85.FinMarket.Domain.Models.Share;
using TinkoffShare = Tinkoff.InvestApi.V1.Share;
using TinkoffFuture = Tinkoff.InvestApi.V1.Future;
using TinkoffBond = Tinkoff.InvestApi.V1.Bond;
using TinkoffCurrency = Tinkoff.InvestApi.V1.Currency;

namespace Oid85.FinMarket.External.Mapping;

public static class TinkoffMapper
{
    private static readonly List<string> BadTickerSymbols = [ "@", "-" ]; 
    
    public static Share? Map(TinkoffShare tinkoffShare)
    {
        bool containsBadSymbol = BadTickerSymbols.Any(x => tinkoffShare.Ticker.Contains(x));
        
        if (containsBadSymbol)
            return null;

        return new Share
        {
            Ticker = tinkoffShare.Ticker,
            Figi = tinkoffShare.Figi,
            InstrumentId = Guid.Parse(tinkoffShare.Uid),
            Isin = tinkoffShare.Isin,
            Name = tinkoffShare.Name,
            Sector = tinkoffShare.Sector
        };
    }

    public static Future? Map(TinkoffFuture tinkoffFuture)
    {
        bool containsBadSymbol = BadTickerSymbols.Any(x => tinkoffFuture.Ticker.Contains(x));
        
        if (containsBadSymbol)
            return null;

        return new Future
        {
            Ticker = tinkoffFuture.Ticker,
            Figi = tinkoffFuture.Figi,
            Name = tinkoffFuture.Name,
            InstrumentId = Guid.Parse(tinkoffFuture.Uid),
            ExpirationDate = ConvertHelper.TimestampToDateOnly(tinkoffFuture.ExpirationDate),
            Lot = tinkoffFuture.Lot,
            FirstTradeDate = ConvertHelper.TimestampToDateOnly(tinkoffFuture.FirstTradeDate),
            LastTradeDate = ConvertHelper.TimestampToDateOnly(tinkoffFuture.LastTradeDate),
            FutureType = tinkoffFuture.FuturesType,
            AssetType = tinkoffFuture.AssetType,
            BasicAsset = tinkoffFuture.BasicAsset,
            BasicAssetSize = ConvertHelper.QuotationToDouble(tinkoffFuture.BasicAssetSize),
            InitialMarginOnBuy = ConvertHelper.MoneyValueToDouble(tinkoffFuture.InitialMarginOnBuy),
            InitialMarginOnSell = ConvertHelper.MoneyValueToDouble(tinkoffFuture.InitialMarginOnSell),
            MinPriceIncrementAmount = ConvertHelper.QuotationToDouble(tinkoffFuture.MinPriceIncrementAmount)
        };
    }

    public static Bond? Map(TinkoffBond tinkoffBond)
    {
        bool containsBadSymbol = BadTickerSymbols.Any(x => tinkoffBond.Ticker.Contains(x));
        
        if (containsBadSymbol)
            return null;

        return new Bond
        {
            Ticker = tinkoffBond.Ticker,
            Figi = tinkoffBond.Figi,
            Isin = tinkoffBond.Isin,
            Name = tinkoffBond.Name,
            InstrumentId = Guid.Parse(tinkoffBond.Uid),
            Sector = tinkoffBond.Sector,
            Currency = tinkoffBond.Currency,
            Nkd = ConvertHelper.MoneyValueToDouble(tinkoffBond.AciValue),
            MaturityDate = ConvertHelper.TimestampToDateOnly(tinkoffBond.MaturityDate),
            FloatingCouponFlag = tinkoffBond.FloatingCouponFlag,
            RiskLevel = tinkoffBond.RiskLevel switch
            {
                RiskLevel.Low => 1,
                RiskLevel.Moderate => 2,
                RiskLevel.High => 3,
                RiskLevel.Unspecified => 0,
                _ => 0
            }
        };
    }

    public static FinIndex? Map(IndicativeResponse indicativeResponse)
    {
        bool containsBadSymbol = BadTickerSymbols.Any(x => indicativeResponse.Ticker.Contains(x));
        
        if (containsBadSymbol)
            return null;

        return new FinIndex
        {
            Figi = indicativeResponse.Figi,
            Ticker = indicativeResponse.Ticker,
            ClassCode = indicativeResponse.ClassCode,
            Currency = indicativeResponse.Currency,
            InstrumentKind = indicativeResponse.InstrumentKind.ToString(),
            Name = indicativeResponse.Name,
            Exchange = indicativeResponse.Exchange,
            InstrumentId = Guid.Parse(indicativeResponse.Uid)
        };
    }

    public static Currency? Map(TinkoffCurrency tinkoffCurrency)
    {
        bool containsBadSymbol = BadTickerSymbols.Any(x => tinkoffCurrency.Ticker.Contains(x));

        if (containsBadSymbol)
            return null;

        return new Currency
        {
            Ticker = tinkoffCurrency.Ticker,
            Isin = tinkoffCurrency.Isin,
            Figi = tinkoffCurrency.Figi,
            ClassCode = tinkoffCurrency.ClassCode,
            Name = tinkoffCurrency.Name,
            IsoCurrencyName = tinkoffCurrency.IsoCurrencyName,
            InstrumentId = Guid.Parse(tinkoffCurrency.Uid)
        };
    }
    
    public static DividendInfo Map(Dividend dividend, Share share) =>
        new()
        {
            Ticker = share.Ticker,
            InstrumentId = share.InstrumentId,
            DeclaredDate = ConvertHelper.TimestampToDateOnly(dividend.DeclaredDate),
            RecordDate = ConvertHelper.TimestampToDateOnly(dividend.RecordDate),
            Dividend = Math.Round(ConvertHelper.MoneyValueToDouble(dividend.DividendNet), 2),
            DividendPrc = Math.Round(ConvertHelper.QuotationToDouble(dividend.YieldValue), 2)
        };
    
    public static BondCoupon Map(Coupon coupon, Bond bond) =>
        new()
        {
            InstrumentId = bond.InstrumentId,
            Ticker = bond.Ticker,
            CouponNumber = coupon.CouponNumber,
            CouponPeriod = coupon.CouponPeriod,
            CouponDate = ConvertHelper.TimestampToDateOnly(coupon.CouponDate),
            CouponStartDate = ConvertHelper.TimestampToDateOnly(coupon.CouponStartDate),
            CouponEndDate = ConvertHelper.TimestampToDateOnly(coupon.CouponEndDate),
            PayOneBond = ConvertHelper.MoneyValueToDouble(coupon.PayOneBond)
        };
    
    public static HourlyDailyCandle MapHourlyCandle(HistoricCandle historicCandle) =>
        new()
        {
            Open = ConvertHelper.QuotationToDouble(historicCandle.Open),
            Close = ConvertHelper.QuotationToDouble(historicCandle.Close),
            High = ConvertHelper.QuotationToDouble(historicCandle.High),
            Low = ConvertHelper.QuotationToDouble(historicCandle.Low),
            Volume = historicCandle.Volume,
            Date = ConvertHelper.TimestampToDateOnly(historicCandle.Time),
            Time = ConvertHelper.TimestampToTimeOnly(historicCandle.Time),
            DateTimeTicks = new DateTime(
                    ConvertHelper.TimestampToDateOnly(historicCandle.Time), 
                    ConvertHelper.TimestampToTimeOnly(historicCandle.Time))
                .Ticks,
            IsComplete = historicCandle.IsComplete
        };    
    
    public static DailyCandle MapCandle(HistoricCandle historicCandle) =>
        new()
        {
            Open = ConvertHelper.QuotationToDouble(historicCandle.Open),
            Close = ConvertHelper.QuotationToDouble(historicCandle.Close),
            High = ConvertHelper.QuotationToDouble(historicCandle.High),
            Low = ConvertHelper.QuotationToDouble(historicCandle.Low),
            Volume = historicCandle.Volume,
            Date = ConvertHelper.TimestampToDateOnly(historicCandle.Time),
            IsComplete = historicCandle.IsComplete
        };    
    
    public static ForecastTarget Map(GetForecastResponse.Types.TargetItem targetItem) =>
        new()
        {
            InstrumentId = Guid.Parse(targetItem.Uid),
            Ticker = targetItem.Ticker,
            Company = targetItem.Company,
            RecommendationDate = ConvertHelper.TimestampToDateOnly(targetItem.RecommendationDate),
            Currency = targetItem.Currency,
            CurrentPrice = ConvertHelper.QuotationToDouble(targetItem.CurrentPrice),
            TargetPrice = ConvertHelper.QuotationToDouble(targetItem.TargetPrice),
            PriceChange = ConvertHelper.QuotationToDouble(targetItem.PriceChange),
            PriceChangeRel = ConvertHelper.QuotationToDouble(targetItem.PriceChangeRel),
            ShowName = targetItem.ShowName,
            RecommendationNumber = targetItem.Recommendation switch
            {
                Recommendation.Buy => 1,
                Recommendation.Hold => 2,
                Recommendation.Sell => 3,
                _ => 0
            },
            RecommendationString = targetItem.Recommendation switch
            {
                Recommendation.Buy => KnownForecastRecommendations.Buy,
                Recommendation.Hold => KnownForecastRecommendations.Hold,
                Recommendation.Sell => KnownForecastRecommendations.Sell,
                _ => string.Empty
            }
        };
    
    public static ForecastConsensus Map(GetForecastResponse.Types.ConsensusItem consensusItem) =>
        new()
        {
            InstrumentId = Guid.Parse(consensusItem.Uid),
            Ticker = consensusItem.Ticker,
            Currency = consensusItem.Currency,
            CurrentPrice = ConvertHelper.QuotationToDouble(consensusItem.CurrentPrice),
            MinTarget = ConvertHelper.QuotationToDouble(consensusItem.MinTarget),
            MaxTarget = ConvertHelper.QuotationToDouble(consensusItem.MaxTarget),
            PriceChange = ConvertHelper.QuotationToDouble(consensusItem.PriceChange),
            PriceChangeRel = ConvertHelper.QuotationToDouble(consensusItem.PriceChangeRel),
            RecommendationNumber = consensusItem.Recommendation switch
            {
                Recommendation.Buy => 1,
                Recommendation.Hold => 2,
                Recommendation.Sell => 3,
                _ => 0
            },
            RecommendationString = consensusItem.Recommendation switch
            {
                Recommendation.Buy => KnownForecastRecommendations.Buy,
                Recommendation.Hold => KnownForecastRecommendations.Hold,
                Recommendation.Sell => KnownForecastRecommendations.Sell,
                _ => string.Empty
            }
        };
    
    public static AssetReportEvent Map(GetAssetReportsResponse.Types.GetAssetReportsEvent report) =>
        new()
        {
            InstrumentId = Guid.Parse(report.InstrumentId),
            ReportDate = ConvertHelper.TimestampToDateOnly(report.ReportDate),
            PeriodYear = report.PeriodYear,
            PeriodNum = report.PeriodNum,
            Type = report.PeriodType switch
            {
                GetAssetReportsResponse.Types.AssetReportPeriodType.PeriodTypeQuarter => KnownAssetReportPeriodTypes.PeriodTypeQuarter,
                GetAssetReportsResponse.Types.AssetReportPeriodType.PeriodTypeSemiannual => KnownAssetReportPeriodTypes.PeriodTypeSemiannual,
                GetAssetReportsResponse.Types.AssetReportPeriodType.PeriodTypeAnnual => KnownAssetReportPeriodTypes.PeriodTypeAnnual,
                _ => string.Empty
            }
        };   
}
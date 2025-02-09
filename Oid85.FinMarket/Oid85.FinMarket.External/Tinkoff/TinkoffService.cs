﻿using Google.Protobuf.WellKnownTypes;
using NLog;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Candle = Oid85.FinMarket.Domain.Models.Candle;
using Share = Oid85.FinMarket.Domain.Models.Share;
using Bond = Oid85.FinMarket.Domain.Models.Bond;
using Currency = Oid85.FinMarket.Domain.Models.Currency;
using Future = Oid85.FinMarket.Domain.Models.Future;
using TinkoffShare = Tinkoff.InvestApi.V1.Share;
using TinkoffFuture = Tinkoff.InvestApi.V1.Future;
using TinkoffBond = Tinkoff.InvestApi.V1.Bond;

namespace Oid85.FinMarket.External.Tinkoff;

/// <inheritdoc />
public class TinkoffService(
    ILogger logger,
    InvestApiClient client)
    : ITinkoffService
{
    // <inheritdoc />
    public async Task<List<Candle>> GetCandlesAsync(
        Guid instrumentId, DateOnly from, DateOnly to)
    {
        try
        {
            return await GetCandlesAsync(
                instrumentId, 
                Timestamp.FromDateTime(from.ToDateTime(TimeOnly.MinValue).ToUniversalTime()), 
                Timestamp.FromDateTime(to.ToDateTime(TimeOnly.MinValue).ToUniversalTime()));
        }

        catch (Exception exception)
        {
            logger.Error(exception.Message);
            return [];
        }
    }

    /// <inheritdoc />
    public async Task<List<Candle>> GetCandlesAsync(Guid instrumentId, int year)
    {
        try
        {
            return await GetCandlesAsync(
                instrumentId, 
                Timestamp.FromDateTime(new DateTime(year, 1, 1).ToUniversalTime()), 
                Timestamp.FromDateTime(new DateTime(year, 12, 31).ToUniversalTime()));
        }

        catch (Exception exception)
        {
            logger.Error(exception.Message);
            return [];
        }
    }

    /// <inheritdoc />
    public async Task<List<FiveMinuteCandle>> GetFiveMinuteCandlesAsync(
        Guid instrumentId, DateTime from, DateTime to)
    {
        try
        {
            var request = new GetCandlesRequest
            {
                InstrumentId = instrumentId.ToString(),
                From = Timestamp.FromDateTime(from.ToUniversalTime()),
                To = Timestamp.FromDateTime(to.ToUniversalTime()),
                Interval = CandleInterval._5Min
            };

            var response = await client.MarketData.GetCandlesAsync(request);

            return response.Candles.Select(historicCandle => new FiveMinuteCandle
                {
                    InstrumentId = instrumentId,
                    Open = ConvertHelper.QuotationToDouble(historicCandle.Open),
                    Close = ConvertHelper.QuotationToDouble(historicCandle.Close),
                    High = ConvertHelper.QuotationToDouble(historicCandle.High),
                    Low = ConvertHelper.QuotationToDouble(historicCandle.Low),
                    Volume = historicCandle.Volume,
                    Date = ConvertHelper.TimestampToDateOnly(historicCandle.Time),
                    Time = ConvertHelper.TimestampToTimeOnly(historicCandle.Time),
                    IsComplete = historicCandle.IsComplete
                })
                .ToList();
        }

        catch (Exception exception)
        {
            logger.Error(exception.Message);
            return [];
        }
    }

    /// <inheritdoc />
    public async Task<List<double>> GetPricesAsync(List<Guid> instrumentIds)
    {
        try
        {
            var request = new GetLastPricesRequest();

            foreach (var instrumentId in instrumentIds)
                request.InstrumentId.Add(instrumentId.ToString());

            request.LastPriceType = LastPriceType.LastPriceExchange;
                
            var response = await client.MarketData.GetLastPricesAsync(request);
                
            if (response is null)
                return [];
                
            var result = response.LastPrices
                .Select(x => ConvertHelper.QuotationToDouble(x.Price))
                .ToList();

            return result;
        }

        catch (Exception exception)
        {
            logger.Error(exception.Message);
            return [];
        }
    }

    private async Task<List<Candle>> GetCandlesAsync(
        Guid instrumentId, Timestamp from, Timestamp to)
    {
        var request = new GetCandlesRequest
        {
            InstrumentId = instrumentId.ToString(),
            From = from,
            To = to,
            Interval = CandleInterval.Day
        };

        var response = await client.MarketData.GetCandlesAsync(request);

        return response.Candles.Select(historicCandle => new Candle
            {
                InstrumentId = instrumentId,
                Open = ConvertHelper.QuotationToDouble(historicCandle.Open),
                Close = ConvertHelper.QuotationToDouble(historicCandle.Close),
                High = ConvertHelper.QuotationToDouble(historicCandle.High),
                Low = ConvertHelper.QuotationToDouble(historicCandle.Low),
                Volume = historicCandle.Volume,
                Date = ConvertHelper.TimestampToDateOnly(historicCandle.Time),
                IsComplete = historicCandle.IsComplete
            })
            .ToList();
    }

    /// <inheritdoc />
    public async Task<List<Share>> GetSharesAsync()
    {
        try
        {
            List<TinkoffShare> shares = (await client.Instruments
                    .SharesAsync()).Instruments
                .Where(x => x.CountryOfRisk.ToLower() == "ru")
                .ToList(); 

            var result = new List<Share>();

            foreach (var share in shares)
            {
                if (share.Ticker.Contains("@"))
                    continue;
                    
                if (share.Ticker.Contains("-"))
                    continue;
                    
                result.Add(new Share
                {
                    Ticker = share.Ticker,
                    Figi = share.Figi,
                    InstrumentId = Guid.Parse(share.Uid),
                    Isin = share.Isin,
                    Name = share.Name,
                    Sector = share.Sector
                });
            }

            return result;
        }

        catch (Exception exception)
        {
            logger.Error(exception.Message);
            return [];
        }
    }
    /// <inheritdoc />
    public async Task<List<Future>> GetFuturesAsync()
    {
        try
        {
            List<TinkoffFuture> futures = (await client.Instruments
                    .FuturesAsync()).Instruments
                .Where(x => x.CountryOfRisk.ToLower() == "ru")
                .ToList(); 

            var result = new List<Future>();

            foreach (var future in futures)
            {
                if (future.Ticker.Contains("@"))
                    continue;
                    
                if (future.Ticker.Contains("-"))
                    continue;
                    
                result.Add(new Future
                {
                    Ticker = future.Ticker,
                    Figi = future.Figi,
                    Name = future.Name,
                    InstrumentId = Guid.Parse(future.Uid),
                    ExpirationDate = ConvertHelper.TimestampToDateOnly(future.ExpirationDate),
                    Lot = future.Lot,
                    FirstTradeDate = ConvertHelper.TimestampToDateOnly(future.FirstTradeDate),
                    LastTradeDate = ConvertHelper.TimestampToDateOnly(future.LastTradeDate),
                    FutureType = future.FuturesType,
                    AssetType = future.AssetType,
                    BasicAsset = future.BasicAsset,
                    BasicAssetSize = ConvertHelper.QuotationToDouble(future.BasicAssetSize),
                    InitialMarginOnBuy = ConvertHelper.MoneyValueToDouble(future.InitialMarginOnBuy),
                    InitialMarginOnSell = ConvertHelper.MoneyValueToDouble(future.InitialMarginOnSell),
                    MinPriceIncrementAmount = ConvertHelper.QuotationToDouble(future.MinPriceIncrementAmount)
                });
            }

            return result;
        }

        catch (Exception exception)
        {
            logger.Error(exception.Message);
            return [];
        }
    }
        
    /// <inheritdoc />
    public async Task<List<Bond>> GetBondsAsync()
    {
        try
        {
            List<TinkoffBond> bonds = (await client.Instruments.BondsAsync())
                .Instruments
                //.Where(x => x.CountryOfRisk.ToLower() == "ru")
                .ToList();

            var result = new List<Bond>();

            foreach (var bond in bonds)
            {
                var instrument = new Bond
                {
                    Ticker = bond.Ticker,
                    Figi = bond.Figi,
                    Isin = bond.Isin,
                    Name = bond.Name,
                    InstrumentId = Guid.Parse(bond.Uid),
                    Sector = bond.Sector,
                    Currency = bond.Currency,
                    Nkd = ConvertHelper.MoneyValueToDouble(bond.AciValue),
                    MaturityDate = ConvertHelper.TimestampToDateOnly(bond.MaturityDate),
                    FloatingCouponFlag = bond.FloatingCouponFlag,
                    RiskLevel = bond.RiskLevel switch
                    {
                        RiskLevel.Low => 1,
                        RiskLevel.Moderate => 2,
                        RiskLevel.High => 3,
                        RiskLevel.Unspecified => 0,
                        _ => 0
                    }
                };

                result.Add(instrument);
            }

            return result;
        }

        catch (Exception exception)
        {
            logger.Error(exception.Message);
            return [];
        }
    }
        
    /// <inheritdoc />
    public async Task<List<FinIndex>> GetIndexesAsync()
    {
        try
        {
            var request = new IndicativesRequest();
                
            var indicatives = (await client.Instruments
                    .IndicativesAsync(request))
                .Instruments
                .ToList();

            var result = new List<FinIndex>();

            foreach (var indicative in indicatives)
            {
                var instrument = new FinIndex
                {
                    Figi = indicative.Figi,
                    Ticker = indicative.Ticker,
                    ClassCode = indicative.ClassCode,
                    Currency = indicative.Currency,
                    InstrumentKind = indicative.InstrumentKind.ToString(),
                    Name = indicative.Name,
                    Exchange = indicative.Exchange,
                    InstrumentId = Guid.Parse(indicative.Uid)
                };

                result.Add(instrument);
            }

            return result;
        }

        catch (Exception exception)
        {
            logger.Error(exception.Message);
            return [];
        }
    }

    /// <inheritdoc />
    public async Task<List<Currency>> GetCurrenciesAsync()
    {
        try
        {
            var request = new InstrumentsRequest();
                
            var currencies = (await client.Instruments
                    .CurrenciesAsync(request))
                .Instruments
                .ToList();

            var result = new List<Currency>();

            foreach (var currency in currencies)
            {
                var instrument = new Currency
                {
                    Ticker = currency.Ticker,
                    Isin = currency.Isin,
                    Figi = currency.Figi,
                    ClassCode = currency.ClassCode,
                    Name = currency.Name,
                    IsoCurrencyName = currency.IsoCurrencyName,
                    InstrumentId = Guid.Parse(currency.Uid)
                };

                result.Add(instrument);
            }

            return result;
        }

        catch (Exception exception)
        {
            logger.Error(exception.Message);
            return [];
        }
    }

    /// <inheritdoc />
    public async Task<List<DividendInfo>> GetDividendInfoAsync(
        List<Share> shares)
    {
        try
        {
            var dividendInfos = new List<DividendInfo>();
            
            var from = DateTime.SpecifyKind(
                new DateTime(DateTime.UtcNow.Year, 1, 1), 
                DateTimeKind.Utc);
            
            var to = from.AddYears(2);

            foreach (var share in shares)
            {
                var request = new GetDividendsRequest
                {
                    InstrumentId = share.InstrumentId.ToString(),
                    From = Timestamp.FromDateTime(from),
                    To = Timestamp.FromDateTime(to)
                };

                var response = await client
                    .Instruments
                    .GetDividendsAsync(request);

                if (response is null)
                    continue;

                var dividends = response.Dividends.ToList();

                if (dividends.Any())
                {
                    foreach (var dividend in dividends)
                    {
                        if (dividend is null)
                            continue;

                        var dividendInfo = new DividendInfo
                        {
                            Ticker = share.Ticker,
                            InstrumentId = share.InstrumentId,
                            DeclaredDate = ConvertHelper.TimestampToDateOnly(dividend.DeclaredDate),
                            RecordDate = ConvertHelper.TimestampToDateOnly(dividend.RecordDate),
                            Dividend = Math.Round(ConvertHelper.MoneyValueToDouble(dividend.DividendNet), 2),
                            DividendPrc = Math.Round(ConvertHelper.QuotationToDouble(dividend.YieldValue), 2)
                        };

                        dividendInfos.Add(dividendInfo);
                    }                    
                }
            }

            return dividendInfos;
        }
            
        catch (Exception exception)
        {
            logger.Error(exception.Message);
            return [];
        }
    }

    /// <inheritdoc />
    public async Task<List<BondCoupon>> GetBondCouponsAsync(List<Bond> bonds)
    {
        try
        {
            var bondCoupons = new List<BondCoupon>();
            
            var from = DateTime.SpecifyKind(
                new DateTime(DateTime.UtcNow.Year, 1, 1), 
                DateTimeKind.Utc);
            
            var to = from.AddYears(2);

            for (var i = 0; i < bonds.Count; i++)
            {
                var request = new GetBondCouponsRequest
                {
                    InstrumentId = bonds[i].InstrumentId.ToString(),
                    From = Timestamp.FromDateTime(from),
                    To = Timestamp.FromDateTime(to)
                };

                var response = await client.Instruments.GetBondCouponsAsync(request);

                if (response is null)
                    continue;

                var coupons = response.Events.ToList();

                if (coupons.Any())
                {
                    foreach (var coupon in coupons)
                    {
                        if (coupon is null)
                            continue;

                        var bondCoupon = new BondCoupon
                        {
                            InstrumentId = bonds[i].InstrumentId,
                            Ticker = bonds[i].Ticker,
                            CouponNumber = coupon.CouponNumber,
                            CouponPeriod = coupon.CouponPeriod,
                            CouponDate = ConvertHelper.TimestampToDateOnly(coupon.CouponDate),
                            CouponStartDate = ConvertHelper.TimestampToDateOnly(coupon.CouponStartDate),
                            CouponEndDate = ConvertHelper.TimestampToDateOnly(coupon.CouponEndDate),
                            PayOneBond = ConvertHelper.MoneyValueToDouble(coupon.PayOneBond)
                        };

                        bondCoupons.Add(bondCoupon);
                    }
                }

                double percent = ((i + 1) / (double) bonds.Count) * 100;
                logger.Trace($"Загружены купоны для облигации '{bonds[i].Ticker}'. {i + 1} из {bonds.Count}. {percent:N2} % загружено");
            }

            return bondCoupons;
        }
            
        catch (Exception exception)
        {
            logger.Error(exception.Message);
            return [];
        }
    }

    /// <inheritdoc />
    public async Task<List<AssetFundamental>> GetAssetFundamentalsAsync(List<Guid> instrumentIds)
    {
        try
        {
            if (instrumentIds is [])
                return [];
            
            var request = new GetAssetFundamentalsRequest();

            foreach (var instrumentId in instrumentIds) 
                request.Assets.Add(instrumentId.ToString());
            
            var response = await client
                .Instruments
                .GetAssetFundamentalsAsync(request);

            if (response is null)
                return [];
            
            var result = new List<AssetFundamental>();

            foreach (var item in response.Fundamentals)
            {
                var assetFundamental = new AssetFundamental
                {
                    Date = DateOnly.FromDateTime(DateTime.Today),
                    InstrumentId = Guid.Parse(item.AssetUid),
                    Currency = item.Currency,
                    MarketCapitalization = item.MarketCapitalization,
                    HighPriceLast52Weeks = item.HighPriceLast52Weeks,
                    LowPriceLast52Weeks = item.LowPriceLast52Weeks,
                    AverageDailyVolumeLast10Days = item.AverageDailyVolumeLast10Days,
                    AverageDailyVolumeLast4Weeks = item.AverageDailyVolumeLast4Weeks,
                    Beta = item.Beta,
                    FreeFloat = item.FreeFloat,
                    ForwardAnnualDividendYield = item.ForwardAnnualDividendYield,
                    SharesOutstanding = item.SharesOutstanding,
                    RevenueTtm = item.RevenueTtm,
                    EbitdaTtm = item.EbitdaTtm,
                    NetIncomeTtm = item.NetIncomeTtm,
                    EpsTtm = item.EpsTtm,
                    DilutedEpsTtm = item.DilutedEpsTtm,
                    FreeCashFlowTtm = item.FreeCashFlowTtm,
                    FiveYearAnnualRevenueGrowthRate = item.FiveYearAnnualRevenueGrowthRate,
                    ThreeYearAnnualRevenueGrowthRate = item.ThreeYearAnnualRevenueGrowthRate,
                    PeRatioTtm = item.PeRatioTtm,
                    PriceToSalesTtm = item.PriceToSalesTtm,
                    PriceToBookTtm = item.PriceToBookTtm,
                    PriceToFreeCashFlowTtm = item.PriceToFreeCashFlowTtm,
                    TotalEnterpriseValueMrq = item.TotalEnterpriseValueMrq,
                    EvToEbitdaMrq = item.EvToEbitdaMrq,
                    NetMarginMrq = item.NetMarginMrq,
                    NetInterestMarginMrq = item.NetInterestMarginMrq,
                    Roe = item.Roe,
                    Roa = item.Roa,
                    Roic = item.Roic,
                    TotalDebtMrq = item.TotalDebtMrq,
                    TotalDebtToEquityMrq = item.TotalDebtToEquityMrq,
                    TotalDebtToEbitdaMrq = item.TotalDebtToEbitdaMrq,
                    FreeCashFlowToPrice = item.FreeCashFlowToPrice,
                    NetDebtToEbitda = item.NetDebtToEbitda,
                    CurrentRatioMrq = item.CurrentRatioMrq,
                    FixedChargeCoverageRatioFy = item.FixedChargeCoverageRatioFy,
                    DividendYieldDailyTtm = item.DividendYieldDailyTtm,
                    DividendRateTtm = item.DividendRateTtm,
                    DividendsPerShare = item.DividendsPerShare,
                    FiveYearsAverageDividendYield = item.FiveYearsAverageDividendYield,
                    FiveYearAnnualDividendGrowthRate = item.FiveYearAnnualDividendGrowthRate,
                    DividendPayoutRatioFy = item.DividendPayoutRatioFy,
                    BuyBackTtm = item.BuyBackTtm,
                    OneYearAnnualRevenueGrowthRate = item.OneYearAnnualRevenueGrowthRate,
                    DomicileIndicatorCode = item.DomicileIndicatorCode,
                    AdrToCommonShareRatio = item.AdrToCommonShareRatio,
                    NumberOfEmployees = item.NumberOfEmployees,
                    ExDividendDate = ConvertHelper.TimestampToDateOnly(item.ExDividendDate),
                    FiscalPeriodStartDate = ConvertHelper.TimestampToDateOnly(item.FiscalPeriodStartDate),
                    FiscalPeriodEndDate = ConvertHelper.TimestampToDateOnly(item.FiscalPeriodEndDate),
                    RevenueChangeFiveYears = item.RevenueChangeFiveYears,
                    EpsChangeFiveYears = item.EpsChangeFiveYears,
                    EbitdaChangeFiveYears = item.EbitdaChangeFiveYears,
                    TotalDebtChangeFiveYears = item.TotalDebtChangeFiveYears,
                    EvToSales = item.EvToSales
                };

                result.Add(assetFundamental);
            }

            return result;
        }

        catch (Exception exception)
        {
            logger.Error(exception.Message);
            return [];
        }
    }

    /// <inheritdoc />
    public async Task<(List<ForecastTarget>, ForecastConsensus)> GetForecastAsync(Guid instrumentId)
    {
        try
        {
            var request = new GetForecastRequest
            {
                InstrumentId = instrumentId.ToString()
            };
            
            var response = await client.Instruments.GetForecastByAsync(request);

            if (response is null)
                return ([], new());

            var targets = new List<ForecastTarget>();
            
            foreach (var targetItem in response.Targets)
            {
                var target = new ForecastTarget
                {
                    InstrumentId = Guid.Parse(targetItem.Uid),
                    Ticker = targetItem.Ticker,
                    Company = targetItem.Company
                };

                switch (targetItem.Recommendation)
                {
                    case Recommendation.Unspecified:
                        target.RecommendationString = KnownForecastRecommendations.Unknown;
                        target.RecommendationNumber = 0;
                        break;
                    
                    case Recommendation.Buy:
                        target.RecommendationString = KnownForecastRecommendations.Buy;
                        target.RecommendationNumber = 1;
                        break;
                    
                    case Recommendation.Hold:
                        target.RecommendationString = KnownForecastRecommendations.Hold;
                        target.RecommendationNumber = 2;
                        break;
                    
                    case Recommendation.Sell:
                        target.RecommendationString = KnownForecastRecommendations.Sell;
                        target.RecommendationNumber = 3;
                        break;
                }
                
                target.RecommendationDate = ConvertHelper.TimestampToDateOnly(targetItem.RecommendationDate);
                target.Currency = targetItem.Currency;
                target.CurrentPrice = ConvertHelper.QuotationToDouble(targetItem.CurrentPrice);
                target.TargetPrice = ConvertHelper.QuotationToDouble(targetItem.TargetPrice);
                target.PriceChange = ConvertHelper.QuotationToDouble(targetItem.PriceChange);
                target.PriceChangeRel = ConvertHelper.QuotationToDouble(targetItem.PriceChangeRel);
                target.ShowName = targetItem.ShowName;
                
                targets.Add(target);
            }
            
            var consensus = new ForecastConsensus
            {
                InstrumentId = Guid.Parse(response.Consensus.Uid),
                Ticker = response.Consensus.Ticker
            };

            switch (response.Consensus.Recommendation)
            {
                case Recommendation.Unspecified:
                    consensus.RecommendationString = KnownForecastRecommendations.Unknown;
                    consensus.RecommendationNumber = 0;
                    break;
                    
                case Recommendation.Buy:
                    consensus.RecommendationString = KnownForecastRecommendations.Buy;
                    consensus.RecommendationNumber = 1;
                    break;
                    
                case Recommendation.Hold:
                    consensus.RecommendationString = KnownForecastRecommendations.Hold;
                    consensus.RecommendationNumber = 2;
                    break;
                    
                case Recommendation.Sell:
                    consensus.RecommendationString = KnownForecastRecommendations.Sell;
                    consensus.RecommendationNumber = 3;
                    break;
            }
            
            consensus.Currency = response.Consensus.Currency;
            consensus.CurrentPrice = ConvertHelper.QuotationToDouble(response.Consensus.CurrentPrice);
            consensus.ConsensusPrice = ConvertHelper.QuotationToDouble(response.Consensus.Consensus);
            consensus.MinTarget = ConvertHelper.QuotationToDouble(response.Consensus.MinTarget);
            consensus.MaxTarget = ConvertHelper.QuotationToDouble(response.Consensus.MaxTarget);
            consensus.PriceChange = ConvertHelper.QuotationToDouble(response.Consensus.PriceChange);
            consensus.PriceChangeRel = ConvertHelper.QuotationToDouble(response.Consensus.PriceChangeRel);
            
            return (targets, consensus);
        }
        
        catch (Exception exception)
        {
            logger.Error(exception.Message);
            return ([], new());
        }
    }
}
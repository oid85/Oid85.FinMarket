using Google.Protobuf.WellKnownTypes;
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
    InvestApiClient client,
    GetPricesService getPricesService,
    GetInstrumentsService getInstrumentsService)
    : ITinkoffService
{
    private const int DelayInMilliseconds = 50;
    
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
            logger.Error(exception);
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
            logger.Error(exception);
            return [];
        }
    }

    /// <inheritdoc />
    public async Task<List<FiveMinuteCandle>> GetFiveMinuteCandlesAsync(
        Guid instrumentId, DateTime from, DateTime to)
    {
        try
        {
            await Task.Delay(DelayInMilliseconds);
            
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
            logger.Error(exception, "Ошибка получения данных. {instrumentId}", instrumentId);
            return [];
        }
    }

    /// <inheritdoc />
    public Task<List<double>> GetPricesAsync(List<Guid> instrumentIds) =>
        getPricesService.GetPricesAsync(instrumentIds);

    private async Task<List<Candle>> GetCandlesAsync(
        Guid instrumentId, Timestamp from, Timestamp to)
    {
        try
        {
            await Task.Delay(DelayInMilliseconds);
        
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
        
        catch (Exception exception)
        {
            logger.Error(exception, "Ошибка получения данных. {instrumentId}", instrumentId);
            return [];
        }
    }

    /// <inheritdoc />
    public Task<List<Share>> GetSharesAsync() =>
        getInstrumentsService.GetSharesAsync();
    
    /// <inheritdoc />
    public Task<List<Future>> GetFuturesAsync() =>
        getInstrumentsService.GetFuturesAsync();
    
    /// <inheritdoc />
    public Task<List<Bond>> GetBondsAsync() =>
        getInstrumentsService.GetBondsAsync();

    /// <inheritdoc />
    public Task<List<FinIndex>> GetIndexesAsync() =>
        getInstrumentsService.GetIndexesAsync();

    /// <inheritdoc />
    public Task<List<Currency>> GetCurrenciesAsync() =>
        getInstrumentsService.GetCurrenciesAsync();

    /// <inheritdoc />
    public async Task<List<DividendInfo>> GetDividendInfoAsync(
        List<Share> shares)
    {
        try
        {
            await Task.Delay(DelayInMilliseconds);
            
            var dividendInfos = new List<DividendInfo>();
            
            var from = DateTime.SpecifyKind(
                new DateTime(DateTime.UtcNow.Year, 1, 1), 
                DateTimeKind.Utc);
            
            var to = from.AddYears(2);

            foreach (var share in shares)
            {
                await Task.Delay(DelayInMilliseconds);
                
                var request = new GetDividendsRequest
                {
                    InstrumentId = share.InstrumentId.ToString()
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
            logger.Error(exception);
            return [];
        }
    }

    /// <inheritdoc />
    public async Task<List<BondCoupon>> GetBondCouponsAsync(List<Bond> bonds)
    {
        try
        {
            await Task.Delay(DelayInMilliseconds);
            
            var bondCoupons = new List<BondCoupon>();
            
            var from = DateTime.SpecifyKind(
                new DateTime(DateTime.UtcNow.Year, 1, 1), 
                DateTimeKind.Utc);
            
            var to = from.AddYears(2);

            for (var i = 0; i < bonds.Count; i++)
            {
                await Task.Delay(DelayInMilliseconds);
                
                var request = new GetBondCouponsRequest
                {
                    InstrumentId = bonds[i].InstrumentId.ToString()
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
            logger.Error(exception);
            return [];
        }
    }

    /// <inheritdoc />
    public async Task<(List<ForecastTarget>, ForecastConsensus)> GetForecastAsync(Guid instrumentId)
    {
        try
        {
            await Task.Delay(DelayInMilliseconds);
            
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
            logger.Error(exception, "Ошибка получения данных. {instrumentId}", instrumentId);
            return ([], new());
        }
    }
}
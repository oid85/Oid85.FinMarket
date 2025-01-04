using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.Logging.Services;
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
    ILogService logService,
    InvestApiClient client,
    IConfiguration configuration)
    : ITinkoffService
{
    /// <inheritdoc />
    public async Task<List<Candle>> GetCandlesAsync(Guid instrumentId)
    {
        try
        {
            var (from, to) = await GetDataRange();
            return await GetCandlesAsync(instrumentId, from, to);
        }

        catch (Exception exception)
        {
            await logService.LogException(exception);
            return [];
        }
    }

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
            await logService.LogException(exception);
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
            await logService.LogException(exception);
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
            await logService.LogException(exception);
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
            To = to
        };

        request.Interval = CandleInterval.Day;

        var response = await client.MarketData.GetCandlesAsync(request);

        var candles = new List<Candle>();

        for (var i = 0; i < response.Candles.Count; i++)
        {
            var candle = new Candle
            {
                InstrumentId = instrumentId,
                Open = ConvertHelper.QuotationToDouble(response.Candles[i].Open),
                Close = ConvertHelper.QuotationToDouble(response.Candles[i].Close),
                High = ConvertHelper.QuotationToDouble(response.Candles[i].High),
                Low = ConvertHelper.QuotationToDouble(response.Candles[i].Low),
                Volume = response.Candles[i].Volume,
                Date = ConvertHelper.TimestampToDateOnly(response.Candles[i].Time),
                IsComplete = response.Candles[i].IsComplete
            };

            candles.Add(candle);
        }

        return candles;
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
            await logService.LogException(exception);
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
            await logService.LogException(exception);
            return [];
        }
    }
        
    /// <inheritdoc />
    public async Task<List<Bond>> GetBondsAsync()
    {
        try
        {
            List<TinkoffBond> bonds = (await client.Instruments
                    .BondsAsync()).Instruments
                .Where(x => x.CountryOfRisk.ToLower() == "ru")
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
                    Nkd = ConvertHelper.MoneyValueToDouble(bond.AciValue),
                    MaturityDate = ConvertHelper.TimestampToDateOnly(bond.MaturityDate),
                    FloatingCouponFlag = bond.FloatingCouponFlag
                };

                result.Add(instrument);
            }

            return result;
        }

        catch (Exception exception)
        {
            await logService.LogException(exception);
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
            await logService.LogException(exception);
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
            await logService.LogException(exception);
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
                            Dividend = Math.Round(ConvertHelper.MoneyValueToDouble(dividend.DividendNet), 1),
                            DividendPrc = Math.Round(ConvertHelper.QuotationToDouble(dividend.YieldValue), 1)
                        };

                        dividendInfos.Add(dividendInfo);
                    }                    
                }
            }

            return dividendInfos;
        }
            
        catch (Exception exception)
        {
            await logService.LogException(exception);
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
                await logService.LogTrace($"Загружены купоны для облигации '{bonds[i].Ticker}'. {i + 1} из {bonds.Count}. {percent:N2} % загружено");
            }

            return bondCoupons;
        }
            
        catch (Exception exception)
        {
            await logService.LogException(exception);
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
                
            request.Assets.AddRange(instrumentIds.Select(x => x.ToString()));
            
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
            await logService.LogException(exception);
            return [];
        }
    }

    private Task<(Timestamp from, Timestamp to)> GetDataRange()
    {           
        var buffer = configuration.GetValue<int>(KnownSettingsKeys.ApplicationSettingsBuffer);

        var startDate = DateTime.Now.AddDays(-1 * buffer);
        var endDate = DateTime.Now;

        return Task.FromResult((
            Timestamp.FromDateTime(startDate.ToUniversalTime()), 
            Timestamp.FromDateTime(endDate.ToUniversalTime())));
    }
}
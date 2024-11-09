using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using NLog;
using Oid85.FinMarket.Common.Helpers;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Candle = Oid85.FinMarket.Domain.Models.Candle;
using Share = Oid85.FinMarket.Domain.Models.Share;
using Bond = Oid85.FinMarket.Domain.Models.Bond;
using TinkoffShare = Tinkoff.InvestApi.V1.Share;
using TinkoffBond = Tinkoff.InvestApi.V1.Bond;

namespace Oid85.FinMarket.External.Tinkoff
{
    /// <inheritdoc />
    public class TinkoffService : ITinkoffService
    {
        private readonly ILogger _logger;
        private readonly InvestApiClient _client;
        private readonly IConfiguration _configuration;

        public TinkoffService(
            ILogger logger,
            InvestApiClient client, 
            IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc />
        public async Task<List<Candle>> GetCandlesAsync(
            Share share, string timeframe)
        {
            try
            {
                var (from, to) = await GetDataRange(timeframe);
                return await GetCandlesAsync(share, timeframe, from, to);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
                return [];
            }
        }

        /// <inheritdoc />
        public async Task<List<Candle>> GetCandlesAsync(
            Share share, string timeframe, int year)
        {
            try
            {
                var from = Timestamp.FromDateTime((new DateTime(year, 1, 1)).ToUniversalTime());
                var to = Timestamp.FromDateTime((new DateTime(year, 12, 31)).ToUniversalTime());
                return await GetCandlesAsync(share, timeframe, from, to);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
                return [];
            }
        }        
        
        private async Task<List<Candle>> GetCandlesAsync(
            Share share, string timeframe, Timestamp from, Timestamp to)
        {
            var request = new GetCandlesRequest
            {
                InstrumentId = share.Figi,
                From = from,
                To = to
            };

            var interval = GetCandleInterval(timeframe);

            if (interval == CandleInterval.Unspecified)
            {
                _logger.Error("Неизвестный интервал. interval = CandleInterval.Unspecified");
                return [];
            }

            request.Interval = interval;

            var response = await _client.MarketData.GetCandlesAsync(request);

            var candles = new List<Candle>() { };

            for (var i = 0; i < response.Candles.Count; i++)
            {
                var candle = new Candle
                {
                    Ticker = share.Ticker,
                    Timeframe = timeframe,
                    Open = ConvertHelper.QuotationToDouble(response.Candles[i].Open),
                    Close = ConvertHelper.QuotationToDouble(response.Candles[i].Close),
                    High = ConvertHelper.QuotationToDouble(response.Candles[i].High),
                    Low = ConvertHelper.QuotationToDouble(response.Candles[i].Low),
                    Volume = response.Candles[i].Volume,
                    Date = response.Candles[i].Time.ToDateTime().ToUniversalTime(),
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
                List<TinkoffShare> shares = (await _client.Instruments
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
                        Isin = share.Isin,
                        Description = share.Name,
                        Sector = share.Sector
                    });
                }

                return result;
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
                return [];
            }
        }

        /// <inheritdoc />
        public async Task<List<Bond>> GetBondsAsync()
        {
            try
            {
                List<TinkoffBond> bonds = (await _client.Instruments
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
                        Description = bond.Name,
                        Sector = bond.Sector,
                        NKD = ConvertHelper.MoneyValueToDouble(bond.AciValue),
                        MaturityDate = bond.MaturityDate.ToDateTime().ToUniversalTime(),
                        FloatingCouponFlag = bond.FloatingCouponFlag
                    };

                    result.Add(instrument);
                }

                return result;
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
                return [];
            }
        }
        
        /// <inheritdoc />
        public async Task<List<DividendInfo>> GetDividendInfoAsync(
            List<Share> shares)
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
                    InstrumentId = share.Figi,
                    From = Timestamp.FromDateTime(from),
                    To = Timestamp.FromDateTime(to)
                };

                var response = await _client.Instruments.GetDividendsAsync(request);

                if (response is null)
                    continue;

                var dividends = response.Dividends.ToList();

                if (dividends.Any())
                {
                    foreach (var dividend in dividends)
                    {
                        if (dividend is null)
                            continue;

                        var dividendInfo = new DividendInfo();

                        dividendInfo.Ticker = share.Ticker;
                        
                        if (dividend.DeclaredDate is not null)
                            dividendInfo.DeclaredDate = dividend.DeclaredDate.ToDateTime();

                        if (dividend.RecordDate is not null)
                            dividendInfo.RecordDate = dividend.RecordDate.ToDateTime();

                        if (dividend.DividendNet is not null)
                            dividendInfo.Dividend = Math.Round(
                                ConvertHelper.MoneyValueToDouble(dividend.DividendNet), 2);

                        if (dividend.YieldValue is not null)
                            dividendInfo.DividendPrc = Math.Round(
                                ConvertHelper.QuotationToDouble(dividend.YieldValue), 2);

                        dividendInfos.Add(dividendInfo);
                    }                    
                }
            }

            return dividendInfos;
        }

        public async Task<List<BondCoupon>> GetBondCouponsAsync(List<Bond> bonds)
        {
            var bondCoupons = new List<BondCoupon>();
            
            var from = DateTime.SpecifyKind(
                new DateTime(DateTime.UtcNow.Year, 1, 1), 
                DateTimeKind.Utc);
            
            var to = from.AddYears(2);
            
            foreach (var bond in bonds)
            {
                var request = new GetBondCouponsRequest
                {
                    InstrumentId = bond.Figi,
                    From = Timestamp.FromDateTime(from),
                    To = Timestamp.FromDateTime(to)
                };

                var response = await _client.Instruments.GetBondCouponsAsync(request);

                if (response is null)
                    continue;

                var coupons = response.Events.ToList();

                if (coupons.Any())
                {
                    foreach (var coupon in coupons)
                    {
                        if (coupon is null)
                            continue;

                        var bondCoupon = new BondCoupon();

                        bondCoupon.Ticker = bond.Ticker;
                        
                        if (coupon.CouponDate is not null)
                            bondCoupon.CouponDate = coupon.CouponDate.ToDateTime().ToUniversalTime();
                        
                        bondCoupon.CouponNumber = coupon.CouponNumber;
                        
                        bondCoupon.CouponPeriod = coupon.CouponPeriod;
                        
                        if (coupon.CouponStartDate is not null)
                            bondCoupon.CouponStartDate = coupon.CouponStartDate.ToDateTime().ToUniversalTime();
                        
                        if (coupon.CouponEndDate is not null)
                            bondCoupon.CouponEndDate = coupon.CouponEndDate.ToDateTime().ToUniversalTime();
                        
                        if (coupon.PayOneBond is not null)
                            bondCoupon.PayOneBond = ConvertHelper.MoneyValueToDouble(coupon.PayOneBond);

                        bondCoupons.Add(bondCoupon);
                    }                    
                }
            }            
            
            return bondCoupons;
        }

        private Task<(Timestamp from, Timestamp to)> GetDataRange(string timeframe)
        {           
            var buffer = _configuration.GetValue<int>(KnownSettingsKeys.ApplicationSettings_Buffer);

            var startDate = DateTime.Now;
            var endDate = DateTime.Now;

            if (timeframe == KnownTimeframes.Daily)
                startDate = DateTime.Now.AddDays(-1 * buffer);

            else if (timeframe == KnownTimeframes.Hourly)
                startDate = DateTime.Now.AddHours(-1 * buffer);

            else if (timeframe == KnownTimeframes.FiveMinutes)
                startDate = DateTime.Now.AddMinutes(-5 * buffer);

            else if (timeframe == KnownTimeframes.OneMinutes)
                startDate = DateTime.Now.AddMinutes(-1 * buffer);

            return Task.FromResult((
                Timestamp.FromDateTime(startDate.ToUniversalTime()), 
                Timestamp.FromDateTime(endDate.ToUniversalTime())));
        }

        private static CandleInterval GetCandleInterval(string timeframe) 
        { 
            if (timeframe == KnownTimeframes.Daily)
                return CandleInterval.Day;

            if (timeframe == KnownTimeframes.Hourly)
                return CandleInterval.Hour;

            if (timeframe == KnownTimeframes.FiveMinutes)
                return CandleInterval._5Min;

            if (timeframe == KnownTimeframes.OneMinutes)
                return CandleInterval._1Min;

            return CandleInterval.Unspecified;
        }
    }
}

using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using NLog;
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
            Share share, Timeframe timeframe)
        {
            try
            {
                var (from, to) = await GetDataRange(timeframe.Name);
                return await GetCandlesAsync(share, timeframe.Name, from, to);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
                return [];
            }
        }

        /// <inheritdoc />
        public async Task<List<Candle>> GetCandlesAsync(
            Share share, Timeframe timeframe, int year)
        {
            try
            {
                var from = Timestamp.FromDateTime((new DateTime(year, 1, 1)).ToUniversalTime());
                var to = Timestamp.FromDateTime((new DateTime(year, 12, 31)).ToUniversalTime());
                return await GetCandlesAsync(share, timeframe.Name, from, to);
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
                    Open = ConvertToDouble(response.Candles[i].Open),
                    Close = ConvertToDouble(response.Candles[i].Close),
                    High = ConvertToDouble(response.Candles[i].High),
                    Low = ConvertToDouble(response.Candles[i].Low),
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
                    result.Add(new Share
                    {
                        Ticker = share.Ticker,
                        Figi = share.Figi,
                        Isin = share.Isin,
                        Description = share.Name,
                        Sector = share.Sector,
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
                        Sector = bond.Sector
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
            
            var from = DateTime.SpecifyKind(new DateTime(DateTime.UtcNow.Year, 1, 1), DateTimeKind.Utc);
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
                            dividendInfo.Dividend = Math.Round(ConvertToDouble(new Quotation()
                            {
                                Units = dividend.DividendNet.Units,
                                Nano = dividend.DividendNet.Nano
                            }), 2);

                        if (dividend.YieldValue is not null)
                            dividendInfo.DividendPrc = Math.Round(ConvertToDouble(dividend.YieldValue), 2);

                        dividendInfos.Add(dividendInfo);
                    }                    
                }
            }

            return dividendInfos;
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

        private static double ConvertToDouble(Quotation quotation)
        {
            return quotation.Units + quotation.Nano / 1_000_000_000.0;
        }
    }
}

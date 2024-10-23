﻿using Google.Protobuf.WellKnownTypes;
using NLog;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Settings;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using Candle = Oid85.FinMarket.Domain.Models.Candle;

namespace Oid85.FinMarket.External.Tinkoff
{
    /// <inheritdoc />
    public class TinkoffService : ITinkoffService
    {
        private readonly ILogger _logger;
        private readonly InvestApiClient _client;
        private readonly ISettingsService _settingsService;

        public TinkoffService(
            ILogger logger,
            InvestApiClient client,
            ISettingsService settingsService
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        /// <inheritdoc />
        public async Task<List<Candle>> GetCandlesAsync(
            FinInstrument instrument, string timeframe)
        {
            try
            {
                var (from, to) = await GetDataRange(timeframe);
                return await GetCandlesAsync(instrument, timeframe, from, to);
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
                return [];
            }
        }

        /// <inheritdoc />
        public async Task<List<Candle>> GetCandlesAsync(
            FinInstrument instrument, string timeframe, int year)
        {
            try
            {
                return await GetCandlesAsync(
                    instrument, 
                    timeframe,
                    Timestamp.FromDateTime(new DateTime(year, 1, 1, 0, 0, 0).ToUniversalTime()),
                    Timestamp.FromDateTime(new DateTime(year, 12, 31, 23, 59, 59).ToUniversalTime()));
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
                return [];
            }
        }

        private async Task<List<Candle>> GetCandlesAsync(
                    FinInstrument instrument, string timeframe,
                    Timestamp from, Timestamp to)
        {
            var request = new GetCandlesRequest
            {
                InstrumentId = instrument.Figi,
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
                    IsComplete = response.Candles[i].IsComplete == true ? 1 : 0
                };

                candles.Add(candle);
            }

            return candles;
        }

        /// <inheritdoc />
        public async Task<List<FinInstrument>> GetStocksAsync()
        {
            try
            {
                var shares = (await _client.Instruments
                    .SharesAsync()).Instruments
                    .Where(x => x.CountryOfRisk.ToLower() == "ru")
                    .ToList(); 

                var instruments = new List<FinInstrument>() { };

                foreach (var share in shares)
                {                    
                    var instrument = new FinInstrument
                    {
                        Ticker = share.Ticker,
                        Figi = share.Figi,
                        Isin = share.Isin,
                        Description = share.Name,
                        Sector = share.Sector,
                        IsActive = 1
                    };

                    instruments.Add(instrument);
                }

                return instruments;
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
                return [];
            }
        }

        /// <inheritdoc />
        public async Task<List<FinInstrument>> GetBondsAsync()
        {
            try
            {
                var bonds = (await _client.Instruments
                    .BondsAsync()).Instruments
                    .Where(x => x.CountryOfRisk.ToLower() == "ru")
                    .ToList();

                var instruments = new List<FinInstrument>() { };

                foreach (var bond in bonds)
                {
                    var instrument = new FinInstrument
                    {
                        Ticker = bond.Ticker,
                        Figi = bond.Figi,
                        Isin = bond.Isin,
                        Description = bond.Name,
                        Sector = bond.Sector,
                        IsActive = 1
                    };

                    instruments.Add(instrument);
                }

                return instruments;
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
                return [];
            }
        }

        /// <inheritdoc />
        public async Task<List<FinInstrument>> GetFuturesAsync()
        {
            try
            {
                var futures = (await _client.Instruments
                    .FuturesAsync()).Instruments
                    .Where(x => x.CountryOfRisk.ToLower() == "ru")
                    .ToList();

                var instruments = new List<FinInstrument>() { };

                foreach (var future in futures)
                {
                    var instrument = new FinInstrument
                    {
                        Ticker = future.Ticker,
                        Figi = future.Figi,
                        Description = future.Name,
                        Sector = future.Sector,
                        IsActive = 1
                    };

                    instruments.Add(instrument);
                }

                return instruments;
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
                return [];
            }
        }

        /// <inheritdoc />
        public async Task<List<FinInstrument>> GetCurrenciesAsync()
        {
            try
            {
                var currencies = (await _client.Instruments
                    .CurrenciesAsync()).Instruments
                    .ToList();

                var instruments = new List<FinInstrument>() { };

                foreach (var currency in currencies)
                {
                    var instrument = new FinInstrument
                    {
                        Ticker = currency.Ticker,
                        Figi = currency.Figi,
                        Isin = currency.Isin,
                        Description = currency.Name,
                        IsActive = 1
                    };

                    instruments.Add(instrument);
                }

                return instruments;
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
                return [];
            }
        }

        /// <inheritdoc />
        public async Task<List<DividendInfo>> GetDividendInfoAsync(
            List<FinInstrument> instruments)
        {
            var dividendInfos = new List<DividendInfo>();
            
            var from = DateTime.SpecifyKind(new DateTime(DateTime.UtcNow.Year, 1, 1), DateTimeKind.Utc);
            var to = from.AddYears(2);

            foreach (var instrument in instruments)
            {
                var request = new GetDividendsRequest
                {
                    InstrumentId = instrument.Figi,
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

                        dividendInfo.Ticker = instrument.Ticker;

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

        private async Task<(Timestamp from, Timestamp to)> GetDataRange(string timeframe)
        {           
            var buffer = await _settingsService.GetIntValueAsync(KnownSettingsKeys.ApplicationSettings_Buffer);

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

            return (Timestamp.FromDateTime(startDate.ToUniversalTime()), Timestamp.FromDateTime(endDate.ToUniversalTime()));
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

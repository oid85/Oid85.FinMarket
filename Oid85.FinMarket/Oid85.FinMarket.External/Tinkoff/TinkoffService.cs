﻿using Google.Protobuf.WellKnownTypes;
using NLog;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Settings;
using System.Xml.Linq;
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
            FinancicalInstrument instrument, string timeframe)
        {
            try
            {
                var (start, end) = await GetDataRange(timeframe);

                var request = new GetCandlesRequest
                {
                    InstrumentId = instrument.Figi,
                    From = start,
                    To = end
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

                for ( var i = 0; i < response.Candles.Count; i++)
                {
                    var candle = new Candle
                    {
                        Open = ConvertToDouble(response.Candles[i].Open),
                        Close = ConvertToDouble(response.Candles[i].Close),
                        High = ConvertToDouble(response.Candles[i].High),
                        Low = ConvertToDouble(response.Candles[i].Low),
                        Volume = response.Candles[i].Volume,
                        Date = response.Candles[i].Time.ToDateTime(),
                        IsComplete = response.Candles[i].IsComplete == true ? 1 : 0
                    };

                    candles.Add(candle);
                }

                return candles;
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
                return [];
            }
        }

        /// <inheritdoc />
        public List<FinancicalInstrument> GetStocks()
        {
            try
            {
                var shares = _client.Instruments
                    .Shares().Instruments
                    .Where(x => x.CountryOfRisk.ToLower() == "ru")
                    .ToList(); 

                var instruments = new List<FinancicalInstrument>() { };

                foreach (var share in shares)
                {
                    
                    var instrument = new FinancicalInstrument
                    {
                        Ticker = share.Ticker,
                        Figi = share.Figi,
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
        public List<FinancicalInstrument> GetBonds()
        {
            try
            {
                var bonds = _client.Instruments
                    .Bonds().Instruments
                    .Where(x => x.CountryOfRisk.ToLower() == "ru")
                    .ToList();

                var instruments = new List<FinancicalInstrument>() { };

                foreach (var bond in bonds)
                {
                    var instrument = new FinancicalInstrument
                    {
                        Ticker = bond.Ticker,
                        Figi = bond.Figi,
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
        public List<FinancicalInstrument> GetFutures()
        {
            try
            {
                var futures = _client.Instruments
                    .Futures().Instruments
                    .Where(x => x.CountryOfRisk.ToLower() == "ru")
                    .ToList();

                var instruments = new List<FinancicalInstrument>() { };

                foreach (var future in futures)
                {
                    var instrument = new FinancicalInstrument
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
        public List<FinancicalInstrument> GetCurrencies()
        {
            try
            {
                var currencies = _client.Instruments
                    .Currencies().Instruments
                    .ToList();

                var instruments = new List<FinancicalInstrument>() { };

                foreach (var currencie in currencies)
                {
                    var instrument = new FinancicalInstrument
                    {
                        Ticker = currencie.Ticker,
                        Figi = currencie.Figi,
                        Description = currencie.Name,
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

        private async Task<(Timestamp start, Timestamp end)> GetDataRange(string timeframe)
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
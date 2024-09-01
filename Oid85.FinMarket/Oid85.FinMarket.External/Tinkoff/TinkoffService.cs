using Google.Protobuf.WellKnownTypes;
using NLog;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
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

        public TinkoffService(
            ILogger logger,
            InvestApiClient client
            )
        {
            _logger = logger;
            _client = client;            
        }

        /// <inheritdoc />
        public async Task<IList<Candle>> GetCandlesAsync(FinancicalInstrument instrument, string timeframe)
        {
            try
            {
                var (start, end) = GetDataRange(timeframe);

                var request = new GetCandlesRequest();

                request.InstrumentId = instrument.Figi;
                request.From = start;
                request.To = end;

                var interval = GetCandleInterval(timeframe);

                if (interval == CandleInterval.Unspecified)
                {
                    _logger.Error("Неизвестный интервал. interval = CandleInterval.Unspecified");
                    return new List<Candle>() { };
                }

                request.Interval = interval;

                var response = await _client.MarketData.GetCandlesAsync(request);

                var candles = new List<Candle>() { };

                for ( var i = 0; i < response.Candles.Count; i++)
                {
                    var candle = new Candle();

                    candle.Open = ConvertToDouble(response.Candles[i].Open);
                    candle.Close = ConvertToDouble(response.Candles[i].Close);
                    candle.High = ConvertToDouble(response.Candles[i].High);
                    candle.Low = ConvertToDouble(response.Candles[i].Low);
                    candle.Volume = response.Candles[i].Volume;
                    candle.Date = response.Candles[i].Time.ToDateTime();

                    candles.Add(candle);
                }

                return candles;
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
                return new List<Candle>() { };
            }
        }

        private (Timestamp start, Timestamp end) GetDataRange(string timeframe)
        {
            const int buffer = 300; // Минимум свечей

            var startDate = DateTime.Now;
            var endDate = DateTime.Now;

            if (timeframe == KnownTimeframes.Daily)
                startDate = DateTime.Now.AddDays(-1 * buffer); // 300 дневок

            if (timeframe == KnownTimeframes.Hourly)
                startDate = DateTime.Now.AddHours(-1 * buffer); // 300 часовок

            if (timeframe == KnownTimeframes.FiveMinutes)
                startDate = DateTime.Now.AddMinutes(-5 * buffer); // 300 пятиминуток

            if (timeframe == KnownTimeframes.OneMinutes)
                startDate = DateTime.Now.AddMinutes(-1 * buffer); // 300 минуток

            return (Timestamp.FromDateTime(startDate), Timestamp.FromDateTime(endDate));
        }

        private CandleInterval GetCandleInterval(string timeframe) 
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

        private double ConvertToDouble(Quotation quotation)
        {
            return quotation.Units + quotation.Nano / 1_000_000_000.0;
        }
    }
}

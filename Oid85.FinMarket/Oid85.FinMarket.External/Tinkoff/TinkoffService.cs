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
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        /// <inheritdoc />
        public async Task<IList<Candle>> GetCandlesAsync(FinancicalInstrument instrument, string timeframe)
        {
            try
            {
                var (start, end) = GetDataRange(timeframe);

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
                        Date = response.Candles[i].Time.ToDateTime()
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

        private static (Timestamp start, Timestamp end) GetDataRange(string timeframe)
        {
            const int buffer = 300; // Минимум свечей за один запрос

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

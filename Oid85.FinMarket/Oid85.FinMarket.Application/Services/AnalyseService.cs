﻿using Npgsql;
using Oid85.FinMarket.Application.Constants;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Settings;
using Oid85.FinMarket.External.Storage;
using Skender.Stock.Indicators;
using System.Text.Json;
using Candle = Oid85.FinMarket.Domain.Models.Candle;
using ILogger = NLog.ILogger;

namespace Oid85.FinMarket.Application.Services
{
    /// <inheritdoc />
    public class AnalyseService : IAnalyseService
    {
        private readonly ILogger _logger;
        private readonly ISettingsService _settingsService;
        private readonly IStorageService _storageService;

        public AnalyseService(
            ILogger logger,
            ISettingsService settingsService,
            IStorageService storageService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        /// <inheritdoc />
        public async Task<List<AnalyseResult>> SupertrendAnalyseAsync(
            FinInstrument stock, string timeframe)
        {
            string GetTrendDirection(SuperTrendResult result)
            {
                if (result.SuperTrend == null)
                    return string.Empty;

                if (result.UpperBand == null && result.LowerBand != null)
                    return KnownTrendDirections.Up;

                if (result.UpperBand != null && result.LowerBand == null)
                    return KnownTrendDirections.Down;

                return string.Empty;
            }

            try
            {
                await using var connection = await GetPostgresConnectionAsync();

                await connection.OpenAsync();                

                string tableName = $"{stock.Ticker}_{timeframe}".ToLower();

                var candles = await _storageService.GetCandlesAsync(tableName);

                await connection.CloseAsync();

                int lookbackPeriods = 50;
                double multiplier = 3.0;

                var quotes = candles
                    .Select(x => new Quote()
                    {
                        Open = Convert.ToDecimal(x.Open),
                        Close = Convert.ToDecimal(x.Close),
                        High = Convert.ToDecimal(x.High),
                        Low = Convert.ToDecimal(x.Low),
                        Date = x.Date.ToUniversalTime()
                    })
                    .ToList();

                var superTrendResults = quotes.GetSuperTrend(lookbackPeriods, multiplier);

                var results = superTrendResults
                    .Select(x => new AnalyseResult()
                    {
                        Date = x.Date.ToUniversalTime(),
                        Ticker = stock.Ticker,
                        Timeframe = timeframe,
                        TrendDirection = GetTrendDirection(x),
                        Data = JsonSerializer.Serialize(x)
                    })
                    .ToList();

                await _storageService.SaveAnalyseResultsAsync(
                    $"{stock.Ticker}_supertrend_{KnownTimeframes.Daily}".ToLower(), 
                    results);

                return results;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
                throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
            }
        }

        /// <inheritdoc />
        public async Task<List<AnalyseResult>> CandleSequenceAnalyseAsync(
            FinInstrument stock, string timeframe)
        {
            string GetTrendDirection(List<Candle> candles)
            {
                if (candles == null)
                    return string.Empty;

                // Свечи белые
                if (candles.All(x => x.Close > x.Open))
                    return KnownTrendDirections.Up;

                // Свечи черные
                if (candles.All(x => x.Close < x.Open))
                    return KnownTrendDirections.Down;

                return string.Empty;
            }

            try
            {
                await using var connection = await GetPostgresConnectionAsync();

                await connection.OpenAsync();

                string tableName = $"{stock.Ticker}_{timeframe}".ToLower();

                var candles = await _storageService.GetCandlesAsync(tableName);

                await connection.CloseAsync();

                var results = new List<AnalyseResult>();

                for (int i = 0; i < candles.Count; i++)
                {
                    var result = new AnalyseResult();

                    if (i < 2)
                    {
                        result.Date = candles[i].Date;
                        result.Ticker = stock.Ticker;
                        result.Timeframe = timeframe;
                        result.TrendDirection = string.Empty;
                        result.Data = JsonSerializer.Serialize(candles[i]);
                    }

                    else
                    {
                        var candlesForAnalyse = new List<Candle>()
                        {
                            candles[i],
                            candles[i - 1]
                        };

                        result.Date = candles[i].Date;
                        result.Ticker = stock.Ticker;
                        result.Timeframe = timeframe;
                        result.TrendDirection = GetTrendDirection(candlesForAnalyse);
                        result.Data = JsonSerializer.Serialize(candles[i]);
                    }                    

                    results.Add(result);
                }

                await _storageService.SaveAnalyseResultsAsync(
                    $"{stock.Ticker}_candle_sequence_{KnownTimeframes.Daily}".ToLower(),
                    results);

                return results;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД finmarket. {exception}");
                throw new Exception($"Не удалось прочитать данные из БД finmarket. {exception}");
            }
        }

        /// <summary>
        /// Получить соединение с БД
        /// </summary>
        private async Task<NpgsqlConnection> GetPostgresConnectionAsync()
        {
            try
            {
                var connectionString = await _settingsService
                    .GetStringValueAsync(KnownSettingsKeys.Postgres_ConnectionString);

                var connection = new NpgsqlConnection(connectionString);

                return connection;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось установить соединение с БД finmarket. {exception}");
                throw new Exception($"Не удалось установить соединение с БД finmarket. {exception}");
            }
        }
    }
}

﻿using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using NLog;
using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.External.Catalogs
{
    public class CatalogService : ICatalogService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public CatalogService(
            ILogger logger,
            IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc />
        public async Task<FinInstrument?> GetFinInstrumentAsync(
            string tableName, string ticker)
        {
            try
            {
                await using var connection = GetSqliteConnection();

                await connection.OpenAsync();

                var financicalInstrument = (await connection
                    .QueryAsync<FinInstrument>(
                        $"select id, ticker, figi, description, sector, is_active " +
                        $"from {tableName.ToLower()} " +
                        $"where ticker = '{ticker}'"))
                    .FirstOrDefault();

                await connection.CloseAsync();

                if (financicalInstrument == null)
                {
                    _logger.Error($"В таблице {tableName.ToLower()} нет инструмента '{ticker}'");
                    return null;
                }
                
                return financicalInstrument;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД data.db. {exception}");
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<List<FinInstrument>> GetActiveFinInstrumentsAsync(
            string tableName)
        {
            try
            {
                await using var connection = GetSqliteConnection();

                await connection.OpenAsync();

                var financicalInstruments = (await connection
                    .QueryAsync<FinInstrument>(
                        $"select id, ticker, figi, description, sector, is_active " +
                        $"from {tableName.ToLower()} " +
                        $"where is_active = 1"))
                        .OrderBy(x => x.Sector)
                        .ToList();

                if (financicalInstruments == null || !financicalInstruments.Any())
                {
                        _logger.Error($"В таблице {tableName.ToLower()} нет активных инструментов");
                        return [];
                }

                await connection.CloseAsync();

                return financicalInstruments;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД data.db. {exception}");
                return [];
            }
        }

        /// <inheritdoc />
        public async Task UpdateFinInstrumentsAsync(
            string tableName, List<FinInstrument> instruments)
        {
            try
            {
                await using var connection = GetSqliteConnection();

                await connection.OpenAsync();

                foreach (var instrument in instruments) 
                {
                    var normalizeInstrument = Normalize(instrument);

                    string figi = normalizeInstrument.Figi;

                    var exist = (await connection
                        .QueryAsync<FinInstrument>(
                            $"select id, ticker, figi, description, sector, is_active " +
                            $"from {tableName.ToLower()} " +
                            $"where figi = '{figi}'"))
                        .FirstOrDefault();

                    if (exist is null)
                        await connection.ExecuteAsync(
                            $"insert into {tableName.ToLower()} " +
                            $"(ticker, figi, description, sector, is_active) " +
                            $"values (" +
                            $"'{normalizeInstrument.Ticker}', '{normalizeInstrument.Figi}', " +
                            $"'{normalizeInstrument.Description}', '{normalizeInstrument.Sector}', " +
                            $"{normalizeInstrument.IsActive})");

                    // Обновляем пользовательские списки
                    await connection.ExecuteAsync(
                        $"update moex_index_stocks " +
                        $"set " +
                        $"sector = '{normalizeInstrument.Sector}' " +
                        $"where ticker = '{normalizeInstrument.Ticker}'");

                    await connection.ExecuteAsync(
                        $"update portfolio_stocks " +
                        $"set " +
                        $"sector = '{normalizeInstrument.Sector}' " +
                        $"where ticker = '{normalizeInstrument.Ticker}'");

                    await connection.ExecuteAsync(
                        $"update watch_list_stocks " +
                        $"set " +
                        $"sector = '{normalizeInstrument.Sector}' " +
                        $"where ticker = '{normalizeInstrument.Ticker}'");
                }

                await connection.CloseAsync();
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД data.db. {exception}");
                return;
            }
        }

        /// <inheritdoc />
        public async Task<List<MoexIndexStock>> GetMoexIndexStocksAsync()
        {
            try
            {
                string tableName = "moex_index_stocks";

                await using var connection = GetSqliteConnection();

                await connection.OpenAsync();

                var items = (await connection
                    .QueryAsync<MoexIndexStock>(
                        $"select id, ticker, sector, number_shares, price, prc " +
                        $"from {tableName}"))
                        .OrderBy(x => x.Sector)
                        .ToList();

                if (items == null || !items.Any())
                {
                    _logger.Error($"В таблице '{tableName}' нет тикеров");
                    return [];
                }

                await connection.CloseAsync();

                return items;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД data.db. {exception}");
                return [];
            }
        }

        /// <inheritdoc />
        public async Task<List<PortfolioStock>> GetPortfolioStocksAsync()
        {
            try
            {
                string tableName = "portfolio_stocks";

                await using var connection = GetSqliteConnection();

                await connection.OpenAsync();

                var items = (await connection
                    .QueryAsync<PortfolioStock>(
                        $"select id, ticker, sector, number_shares, price, prc " +
                        $"from {tableName}"))
                        .OrderBy(x => x.Sector)
                        .ToList();

                if (items == null || !items.Any())
                {
                    _logger.Error($"В таблице '{tableName}' нет тикеров");
                    return [];
                }

                await connection.CloseAsync();

                return items;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД data.db. {exception}");
                return [];
            }
        }

        /// <inheritdoc />
        public async Task<List<WatchListStock>> GetWatchListStocksAsync()
        {
            try
            {
                string tableName = "watch_list_stocks";

                await using var connection = GetSqliteConnection();

                await connection.OpenAsync();

                var items = (await connection
                    .QueryAsync<WatchListStock>(
                        $"select id, ticker, sector, price " +
                        $"from {tableName}"))
                        .OrderBy(x => x.Sector)
                        .ToList();

                if (items == null || !items.Any())
                {
                    _logger.Error($"В таблице '{tableName}' нет тикеров");
                    return [];
                }

                await connection.CloseAsync();

                return items;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось прочитать данные из БД data.db. {exception}");
                return [];
            }
        }

        /// <summary>
        /// Получить соединение с БД
        /// </summary>
        private SqliteConnection GetSqliteConnection()
        {
            try
            {
                var connectionString = _configuration.GetValue<string>("SQLite:Settings:ConnectionString");
                var connection = new SqliteConnection(connectionString);

                return connection;
            }

            catch (Exception exception)
            {
                _logger.Error($"Не удалось установить соединение с БД data.db. {exception}");
                throw new Exception($"Не удалось установить соединение с БД data.db. {exception}");
            }
        }

        /// <summary>
        /// Нормализировать поля
        /// </summary>
        private FinInstrument Normalize(FinInstrument instrument)
        {
            string description = instrument.Description
                .Replace("'", "");

            instrument.Description = description;

            string ticker = instrument.Ticker
                .Replace("-", "_")
                .Replace("@", "_");

            instrument.Ticker = ticker;

            return instrument;
        }
    }
}

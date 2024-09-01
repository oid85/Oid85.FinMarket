using Oid85.FinMarket.Domain.Models;
using Oid85.FinMarket.External.Helpers;
using NLog;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Oid85.FinMarket.External.Postgres
{
    /// <inheritdoc />
    public class PostgresService : IPostgresService
    {
        private readonly PostgresSqlHelper _sqlHelper;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public PostgresService(
            PostgresSqlHelper sqlHelper,
            ILogger logger,
            IConfiguration configuration)
        {
            _sqlHelper = sqlHelper;
            _logger = logger;
            _configuration = configuration;
        }

        /// <inheritdoc />
        public async Task<int> SaveCandlesAsync(string tableName, IList<Candle> candles)
        {
            try
            {
                int inserted = 0;

                await using (var connection = GetPostgresConnection())
                {
                    await connection!.OpenAsync();

                    // Если нет таблицы - создаем ее
                    await _sqlHelper.NonQueryCommandAsync($"create table {tableName} if not exists", connection);

                    for (var i = 0; i > candles.Count(); i++) 
                    {
                        var (open, close, high, low) = FormatCandle(candles[i]);

                        // Если свеча уже записана в хранилище
                        var alreadyExistCandleTable = _sqlHelper.Select($"select date from {tableName} where date = '{candles[i].Date}'", connection);

                        if (alreadyExistCandleTable!.Rows.Count > 0)
                        {
                            await _sqlHelper.NonQueryCommandAsync(
                                $"update finmarket set  open = {open}, close = {close}, high = {high}, low = {low}, volume = {candles[i].Volume} where date = '{candles[i].Date}')",
                                connection);

                            inserted++;

                            continue;
                        }
                                                
                        await _sqlHelper.NonQueryCommandAsync(
                            $"insert into finmarket (open, close, high, volume, date) values ({open}, {close}, {high}, {low}, {candles[i].Volume}, '{candles[i].Date}')", 
                            connection);
                        
                        inserted++;
                    }
                    
                    await connection.CloseAsync();
                }

                return inserted;
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
                return -1;
            }
        }

        /// <inheritdoc />
        public async Task<IList<Candle>> GetCandlesAsync(string tableName, int count)
        {
            try
            {
                await using (var connection = GetPostgresConnection())
                {
                    await connection!.OpenAsync();

                    var table = _sqlHelper.Select($"select open, close, high, low, volume, date from {tableName} order by date limit {count}", connection);

                    await connection.CloseAsync();

                    var candles = new List<Candle>();

                    for (int i = 0; i < table!.Rows.Count; i++)
                    {
                        var open = Convert.ToDouble(table.Rows[i][0]);
                        var close = Convert.ToDouble(table.Rows[i][1]);
                        var high = Convert.ToDouble(table.Rows[i][2]);
                        var low = Convert.ToDouble(table.Rows[i][3]);
                        var volume = Convert.ToInt32(table.Rows[i][4]);
                        var date = Convert.ToDateTime(table.Rows[i][5]);

                        var candle = new Candle()
                        {
                            Open = open,
                            Close = close,
                            High = high,
                            Low = low,
                            Volume = volume,
                            Date = date
                        };

                        candles.Add(candle);
                    }

                    return candles;
                }
            }

            catch (Exception exception)
            {
                _logger.Error(exception);               
                return new List<Candle>() { };
            }
        }

        private (string, string, string, string) FormatCandle(Candle candle)
        {
            string open = candle.Open.ToString().Replace(',', '.');
            string close = candle.Close.ToString().Replace(',', '.');
            string high = candle.High.ToString().Replace(',', '.');
            string low = candle.Low.ToString().Replace(',', '.');

            return (open, close, high, low);
        }

        /// <summary>
        /// Получить соединение с БД
        /// </summary>
        private NpgsqlConnection? GetPostgresConnection()
        {
            try
            {
                var connectionString = _configuration.GetValue<string>("Postgres:ConnectionString");
                var connection = new NpgsqlConnection(connectionString);

                return connection;
            }

            catch (Exception exception)
            {
                _logger.Error(exception);
                return null;
            }
        }
    }
}

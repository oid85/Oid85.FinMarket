using System.Data;
using NLog;
using Npgsql;

namespace Oid85.FinMarket.External.Helpers
{
    public class PostgresSqlHelper
    {
        private readonly ILogger _logger;

        public PostgresSqlHelper(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Получить данные через команду select
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable? Select(string commandText, NpgsqlConnection connection)
        {
            try
            {
                var selectCommand = new NpgsqlCommand(commandText, connection);
                var dataAdapter = new NpgsqlDataAdapter(selectCommand);
                var dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                selectCommand.Dispose();
                dataAdapter.Dispose();

                return dataTable;
            }

            catch (Exception exception)
            {
                _logger.Error($"PostgresSqlHelper.Select: exception - '{exception}'. commandText - '{commandText}'");
                return null;
            }
        }

        /// <summary>
        /// Выполнить команду sql (insert, update и т.д.)
        /// </summary>
        public async Task NonQueryCommandAsync(string commandText, NpgsqlConnection connection)
        {
            try
            {
                var command = new NpgsqlCommand(commandText, connection);
                await command.ExecuteNonQueryAsync();
                await command.DisposeAsync();

                _logger.Trace($"PostgresSqlHelper.NonQueryCommandAsync: commandText - '{commandText}'");
            }

            catch (Exception exception)
            {
                _logger.Error($"PostgresSqlHelper.NonQueryCommandAsync: exception - '{exception}'. commandText - '{commandText}'");
            }
        }

        /// <summary>
        /// Выполнить команду sql (scalar)
        /// </summary>
        public async Task<object?> ScalarCommandAsync(string commandText, NpgsqlConnection connection)
        {
            try
            {
                var command = new NpgsqlCommand(commandText, connection);
                var result = await command.ExecuteScalarAsync();
                await command.DisposeAsync();

                return result;
            }

            catch (Exception exception)
            {
                _logger.Error($"PostgresSqlHelper.ScalarCommandAsync: exception - '{exception}'. commandText - '{commandText}'");
                return null;
            }
        }
    }
}
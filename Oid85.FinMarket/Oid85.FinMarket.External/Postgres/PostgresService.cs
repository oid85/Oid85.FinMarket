using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.External.Postgres
{
    /// <inheritdoc />
    public class PostgresService : IPostgresService
    {
        /// <inheritdoc />
        public IEnumerable<Candle> GetCandles(string tableName, int count)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public int InsertCandles(string tableName, IEnumerable<Candle> candles)
        {
            throw new NotImplementedException();
        }
    }
}

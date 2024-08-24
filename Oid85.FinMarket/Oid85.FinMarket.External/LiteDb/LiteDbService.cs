using Oid85.FinMarket.Domain.Models;

namespace Oid85.FinMarket.External.LiteDb
{
    /// <inheritdoc />
    public class LiteDbService : ILiteDbService
    {
        /// <inheritdoc />
        public IEnumerable<FinancicalInstrument> GetFinancicalInstruments()
        {
            throw new NotImplementedException();
        }
    }
}

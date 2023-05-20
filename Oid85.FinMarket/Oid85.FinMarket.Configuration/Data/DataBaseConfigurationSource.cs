using Microsoft.Extensions.Configuration;

namespace Oid85.FinMarket.Configuration.Data
{

    public class DataBaseConfigurationSource : IConfigurationSource
    {
        private readonly string _connectionString;
        
        public DataBaseConfigurationSource(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder) =>
            new DataBaseConfigurationProvider(_connectionString);
    }
}
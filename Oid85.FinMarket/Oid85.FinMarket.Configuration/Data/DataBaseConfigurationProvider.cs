using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Oid85.FinMarket.Configuration.Data
{
    public class DataBaseConfigurationProvider : ConfigurationProvider
    {
        private readonly string _connectionString;
        
        public DataBaseConfigurationProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public override void Load()
        {
            using var dbContext = new DataBaseConfigurationContext(_connectionString);
            
            dbContext.Database.EnsureCreated();

            var settings = dbContext.Settings.ToList();
            var debugSettings = dbContext.DebugSettings.ToList();

            Data ??= new Dictionary<string, string>();

            foreach (var item in settings) 
                Data.Add(item.Key, item.Value);

            foreach (var item in debugSettings) 
                Data.Add(item.Key, item.Value);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Configuration.Common;
using Oid85.FinMarket.Configuration.Models;

namespace Oid85.FinMarket.Configuration.Data
{
    public class DataBaseConfigurationContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Settings> Settings { get; set; }
        public DbSet<DebugSettings> DebugSettings { get; set; }

        public DataBaseConfigurationContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlite(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Settings>(b =>
            {
                b.HasKey(a => a.Key);
                
                b.HasData(
                    new Settings()
                    {
                        Order = 1, 
                        Key = ConfigParameterNames.TinkoffApiToken, 
                        Value = "t.U1XUi__xUJkh_1wjKtvhUV-FdN7XWb6ahZzeoQqVMmliIg7i9NbiTGv5F7lrt4oFGXJfrBtCGnieUNUaveG87w",
                        Description = "Токен доступа к API"
                    },                    
                    
                    new Settings()
                    {
                        Order = 2, 
                        Key = ConfigParameterNames.ConnectionStringStorage, 
                        Value = "server=localhost;user id=postgres;password=postgres;database=finmarket_api;Keepalive=1;Pooling=True;Minimum Pool Size=10;Maximum Pool Size=100",
                        Description = "Строка связи с БД Storage"
                    },
                    
                    new Settings()
                    {
                        Order = 3, 
                        Key = ConfigParameterNames.RepeatedApiRequestPeriodInMilliseconds, 
                        Value = "5000",
                        Description = "Интервал для повторного запроса к API (по-умолчанию 5000 миллисекунд)"
                    },
                    
                    new Settings()
                    {
                        Order = 4, 
                        Key = ConfigParameterNames.Load_1M_CandlesCronExpression, 
                        Value = "0/5 * * * *",
                        Description = "CRON-строка для периода загрузки 1M свечей в хранилище (по-умолчанию '0/5 * * * *' - каждые 5 минут)"
                    },
                    
                    new Settings()
                    {
                        Order = 5, 
                        Key = ConfigParameterNames.Load_1H_CandlesCronExpression, 
                        Value = "0/5 * * * *",
                        Description = "CRON-строка для периода загрузки 1M свечей в хранилище (по-умолчанию '0/5 * * * *' - каждые 5 минут)"
                    },
                    
                    new Settings()
                    {
                        Order = 6, 
                        Key = ConfigParameterNames.Load_1D_CandlesCronExpression, 
                        Value = "0/5 * * * *",
                        Description = "CRON-строка для периода загрузки 1M свечей в хранилище (по-умолчанию '0/5 * * * *' - каждые 5 минут)"
                    }
                );
            });
            
            modelBuilder.Entity<DebugSettings>(b =>
            {
                b.HasKey(a => a.Key);
                
                b.HasData(
                    new DebugSettings()
                    {
                        Order = 1, 
                        Key = ConfigDebugParameterNames.DebugMode, 
                        Value = "true",
                        Description = "Режим отладки"
                    }
                );
            });
        }
    }
}

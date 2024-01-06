using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Oid85.FinMarket.Configuration.Common;
using Oid85.FinMarket.Configuration.Data;
using Oid85.FinMarket.DAL.Entities;

namespace Oid85.FinMarket.DAL
{
    public class StorageDataBaseContext : DbContext
    {
        public StorageDataBaseContext()
        {

        }

        public StorageDataBaseContext(DbContextOptions<StorageDataBaseContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .AddDataBase("c:\\temp\\finmarket_configuration.db")
                .Build();
            
            var connectionString = configuration.GetValue<string>(ConfigParameterNames.ConnectionStringStorage);
            optionsBuilder.UseNpgsql(connectionString);
        }

        public DbSet<StockEntity> StockEntities { get; set; } = null!;
        
        public DbSet<CatalogLiquidTickerEntity> CatalogLiquidTickers { get; set; } = null!;

        public DbSet<_1H_CandleEntity> _1H_CandleEntities { get; set; } = null!;
        
        public DbSet<_1D_CandleEntity> _1D_CandleEntities { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
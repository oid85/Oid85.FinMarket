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
                .AddDataBase("c:\\temp\\wpanalyst_configuration.db")
                .Build();
            
            var connectionString = configuration.GetValue<string>(ConfigParameterNames.ConnectionStringStorage);
            optionsBuilder.UseNpgsql(connectionString);
        }

        public DbSet<CandleOneDayEntity> CandleOneDayEntities { get; set; } = null!;

        public DbSet<CandleOneMinuteEntity> CandleOneMinuteEntities { get; set; } = null!;
        
        public DbSet<MarketEventTypeEntity> MarketEventTypeEntities { get; set; } = null!;
        
        public DbSet<MarketEventEntity> MarketEventEntities { get; set; } = null!;
        
        public DbSet<AssetEntity> AssetEntities { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssetEntity>(b =>
            {
                b.HasKey(a => a.Id);
                
                b.HasData(
                    new AssetEntity() { Id = 1, Ticker = "SBER", Figi = "BBG004730N97" }
                );
            });
        }
    }
}
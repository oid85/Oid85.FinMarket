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

        public DbSet<AssetEntity> AssetEntities { get; set; } = null!;
        
        public DbSet<_1M_CandleEntity> _1M_CandleEntities { get; set; } = null!;
        
        public DbSet<_1H_CandleEntity> _1H_CandleEntities { get; set; } = null!;
        
        public DbSet<_1D_CandleEntity> _1D_CandleEntities { get; set; } = null!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssetEntity>(b =>
            {
                b.HasKey(a => a.Id);
                
                b.HasData(
                    new AssetEntity() { Id = Guid.Parse("da48943a-eed9-42ce-b0d6-c7a6ccdf3fc2"), Ticker = "ABRD", Name = "Абрау-Дюрсо, акция обыкновенная", Figi = "BBG002W2FT78", Sector = "Агропром и Пищепром"},
                    new AssetEntity() { Id = Guid.Parse("84072df0-85d8-4135-be2d-f508cd5c01eb"), Ticker = "AFLT", Name = "Аэрофлот, акция обыкновенная", Figi = "BBG004S683X6", Sector = "Транспорт" },
                    new AssetEntity() { Id = Guid.Parse("007dad9d-9dd6-4f5c-8cba-ee634db20be4"), Ticker = "AKRN", Name = "Акрон, акция обыкновенная", Figi = "BBG004S688H3", Sector = "Химия, удобрения" },
                    new AssetEntity() { Id = Guid.Parse("a4df3b20-a222-43cf-851e-bd3b705c0bd4"), Ticker = "ALRS", Name = "Алроса, акция обыкновенная", Figi = "BBG004S68B40", Sector = "Горнодобывающие" },
                    new AssetEntity() { Id = Guid.Parse("bf5d3d04-bfaa-4623-9a65-7431d00b2a86"), Ticker = "SBER", Name = "Сбербанк, акция обыкновенная", Figi = "BBG004730N97", Sector = "Банки" }
                );
            });
        }
    }
}
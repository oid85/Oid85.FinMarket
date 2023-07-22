using Microsoft.EntityFrameworkCore;
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

        }
        
        public DbSet<FinanceInstrumentEntity> FinanceInstrumentEntities { get; set; } = null!;
        
        public DbSet<OneMinuteCandleEntity> OneMinuteCandleEntities { get; set; } = null!;
        
        public DbSet<OneDayCandleEntity> OneDayCandleEntities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
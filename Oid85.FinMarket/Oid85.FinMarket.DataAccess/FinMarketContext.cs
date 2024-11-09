using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Schemas;

namespace Oid85.FinMarket.DataAccess;

public class FinMarketContext : DbContext
{
    public FinMarketContext(DbContextOptions<FinMarketContext> options) : base(options)
    {

    }

    public DbSet<ShareEntity> ShareEntities { get; set; }
    public DbSet<BondEntity> BondEntities { get; set; }
    public DbSet<DividendInfoEntity> DividendInfoEntities { get; set; }
    public DbSet<BondCouponEntity> BondCouponEntities { get; set; }
    public DbSet<CandleEntity> CandleEntities { get; set; }
    public DbSet<AnalyseResultEntity> AnalyseResultEntities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
    {
        optionBuilder.UseSnakeCaseNamingConvention();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .HasDefaultSchema(KnownDatabaseSchemas.Default)
            .ApplyConfigurationsFromAssembly(
                typeof(FinMarketContext).Assembly,
                type => type
                    .GetInterface(typeof(IFinMarketSchema).ToString()) != null)
            .UseIdentityAlwaysColumns();
    }    
}
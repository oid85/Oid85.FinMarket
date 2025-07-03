using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;
using Oid85.FinMarket.DataAccess.Schemas;

namespace Oid85.FinMarket.DataAccess;

public class FinMarketContext(DbContextOptions<FinMarketContext> options) : DbContext(options)
{
    public DbSet<ShareEntity> ShareEntities { get; set; }
    public DbSet<FutureEntity> FutureEntities { get; set; }
    public DbSet<BondEntity> BondEntities { get; set; }
    public DbSet<FinIndexEntity> IndicativeEntities { get; set; }
    public DbSet<CurrencyEntity> CurrencyEntities { get; set; }
    public DbSet<DividendInfoEntity> DividendInfoEntities { get; set; }
    public DbSet<BondCouponEntity> BondCouponEntities { get; set; }
    public DbSet<MultiplicatorEntity> MultiplicatorEntities { get; set; }
    public DbSet<DailyCandleEntity> DailyCandleEntities { get; set; }
    public DbSet<HourlyCandleEntity> HourlyCandleEntities { get; set; }
    public DbSet<AnalyseResultEntity> AnalyseResultEntities { get; set; }
    public DbSet<OptimizationResultEntity> OptimizationResultEntities { get; set; }
    public DbSet<StrategySignalEntity> StrategySignalEntities { get; set; }
    public DbSet<BacktestResultEntity> BacktestResultEntities { get; set; }
    public DbSet<InstrumentEntity> InstrumentEntities { get; set; }
    public DbSet<SpreadEntity> SpreadEntities { get; set; }
    public DbSet<ForecastTargetEntity> ForecastTargetEntities { get; set; }
    public DbSet<ForecastConsensusEntity> ForecastConsensusEntities { get; set; }
    public DbSet<MarketEventEntity> MarketEventEntities { get; set; }
    public DbSet<AssetReportEventEntity> AssetReportEventEntities { get; set; }
    public DbSet<FearGreedIndexEntity> FearGreedIndexEntities { get; set; }
    
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
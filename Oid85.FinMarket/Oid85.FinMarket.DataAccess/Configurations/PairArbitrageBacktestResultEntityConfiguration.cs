using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class PairArbitrageBacktestResultEntityConfiguration : EntityConfigurationBase<PairArbitrageBacktestResultEntity>
{
    public override void Configure(EntityTypeBuilder<PairArbitrageBacktestResultEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("pair_arbitrage_backtest_results", KnownDatabaseSchemas.Default);
        
        builder
            .Property(x => x.StrategyParams)
            .HasColumnType("jsonb");
    }
}
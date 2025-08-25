using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class StatisticalArbitrageBacktestResultEntityConfiguration : EntityConfigurationBase<StatisticalArbitrageBacktestResultEntity>
{
    public override void Configure(EntityTypeBuilder<StatisticalArbitrageBacktestResultEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("statistical_arbitrage_backtest_results", KnownDatabaseSchemas.Default);
        
        builder
            .Property(x => x.StrategyParams)
            .HasColumnType("jsonb");
    }
}
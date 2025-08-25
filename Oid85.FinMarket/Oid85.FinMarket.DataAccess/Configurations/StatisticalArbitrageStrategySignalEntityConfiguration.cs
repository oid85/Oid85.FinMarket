using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class StatisticalArbitrageStrategySignalEntityConfiguration : EntityConfigurationBase<StatisticalArbitrageStrategySignalEntity>
{
    public override void Configure(EntityTypeBuilder<StatisticalArbitrageStrategySignalEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("statistical_arbitrage_strategy_signals", KnownDatabaseSchemas.Default);
    }
}
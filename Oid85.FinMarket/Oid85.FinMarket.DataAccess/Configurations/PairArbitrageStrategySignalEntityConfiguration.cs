using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class PairArbitrageStrategySignalEntityConfiguration : EntityConfigurationBase<PairArbitrageStrategySignalEntity>
{
    public override void Configure(EntityTypeBuilder<PairArbitrageStrategySignalEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("pair_arbitrage_strategy_signals", KnownDatabaseSchemas.Default);
    }
}
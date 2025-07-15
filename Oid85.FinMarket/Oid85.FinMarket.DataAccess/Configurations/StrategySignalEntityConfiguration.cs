using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class StrategySignalEntityConfiguration : EntityConfigurationBase<StrategySignalEntity>
{
    public override void Configure(EntityTypeBuilder<StrategySignalEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("strategy_signals", KnownDatabaseSchemas.Default);
    }
}
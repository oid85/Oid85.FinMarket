using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class MarketEventEntityConfiguration : EntityConfigurationBase<MarketEventEntity>
{
    public override void Configure(EntityTypeBuilder<MarketEventEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("market_events", KnownDatabaseSchemas.Default);
    }
}
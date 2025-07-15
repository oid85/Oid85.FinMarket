using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class HourlyCandleEntityConfiguration : EntityConfigurationBase<HourlyCandleEntity>
{
    public override void Configure(EntityTypeBuilder<HourlyCandleEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("hourly_candles", KnownDatabaseSchemas.Default);
        builder.HasIndex(x => x.InstrumentId);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class FiveMinuteCandleEntityConfiguration : EntityConfigurationBase<FiveMinuteCandleEntity>
{
    public override void Configure(EntityTypeBuilder<FiveMinuteCandleEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("five-minute-candles", KnownDatabaseSchemas.Storage);
        builder.HasIndex(x => x.InstrumentId);
    }
}
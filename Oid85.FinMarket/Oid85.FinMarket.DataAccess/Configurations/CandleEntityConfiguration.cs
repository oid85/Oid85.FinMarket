using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class CandleEntityConfiguration : EntityConfigurationBase<CandleEntity>
{
    public override void Configure(EntityTypeBuilder<CandleEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("candles", KnownDatabaseSchemas.Storage);
        builder.HasIndex(x => x.Ticker);
    }
}
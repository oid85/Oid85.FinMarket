using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class TickerEntityConfiguration : EntityConfigurationBase<TickerEntity>
{
    public override void Configure(EntityTypeBuilder<TickerEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("tickers", KnownDatabaseSchemas.Dictionary);
    }
}
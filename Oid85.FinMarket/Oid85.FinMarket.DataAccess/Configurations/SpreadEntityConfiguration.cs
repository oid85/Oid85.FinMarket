using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class SpreadEntityConfiguration : EntityConfigurationBase<SpreadEntity>
{
    public override void Configure(EntityTypeBuilder<SpreadEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("spreads", KnownDatabaseSchemas.Storage);
    }
}
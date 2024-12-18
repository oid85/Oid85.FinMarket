using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class IndicativeEntityConfiguration : EntityConfigurationBase<IndicativeEntity>
{
    public override void Configure(EntityTypeBuilder<IndicativeEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("indicatives", KnownDatabaseSchemas.Default);
    }
}
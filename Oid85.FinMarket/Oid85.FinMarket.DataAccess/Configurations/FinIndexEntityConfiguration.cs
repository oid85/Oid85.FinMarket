using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class FinIndexEntityConfiguration : EntityConfigurationBase<FinIndexEntity>
{
    public override void Configure(EntityTypeBuilder<FinIndexEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("fin_indexes", KnownDatabaseSchemas.Default);
    }
}
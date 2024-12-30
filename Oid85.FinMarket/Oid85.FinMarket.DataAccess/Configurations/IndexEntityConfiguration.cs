using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class IndexEntityConfiguration : EntityConfigurationBase<IndexEntity>
{
    public override void Configure(EntityTypeBuilder<IndexEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("indexes", KnownDatabaseSchemas.Default);
    }
}
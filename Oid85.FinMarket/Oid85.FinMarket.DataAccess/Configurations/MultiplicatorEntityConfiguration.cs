using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class MultiplicatorEntityConfiguration : EntityConfigurationBase<MultiplicatorEntity>
{
    public override void Configure(EntityTypeBuilder<MultiplicatorEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("multiplicators", KnownDatabaseSchemas.Default);
    }
}
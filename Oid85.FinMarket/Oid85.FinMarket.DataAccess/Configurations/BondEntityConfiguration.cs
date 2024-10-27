using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class BondEntityConfiguration : EntityConfigurationBase<BondEntity>
{
    public override void Configure(EntityTypeBuilder<BondEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("bonds", KnownDatabaseSchemas.Default);
    }
}
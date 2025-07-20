using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class CorrelationEntityConfiguration : EntityConfigurationBase<CorrelationEntity>
{
    public override void Configure(EntityTypeBuilder<CorrelationEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("correlations", KnownDatabaseSchemas.Default);
    }
}
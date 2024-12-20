using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class CurrencyEntityConfiguration : EntityConfigurationBase<CurrencyEntity>
{
    public override void Configure(EntityTypeBuilder<CurrencyEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("currencies", KnownDatabaseSchemas.Default);
    }
}
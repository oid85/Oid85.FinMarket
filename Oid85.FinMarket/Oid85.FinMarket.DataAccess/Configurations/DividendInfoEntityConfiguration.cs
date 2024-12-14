using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class DividendInfoEntityConfiguration : EntityConfigurationBase<DividendInfoEntity>
{
    public override void Configure(EntityTypeBuilder<DividendInfoEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("dividend_infos", KnownDatabaseSchemas.Default);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class ShareMultiplicatorEntityConfiguration : EntityConfigurationBase<ShareMultiplicatorEntity>
{
    public override void Configure(EntityTypeBuilder<ShareMultiplicatorEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("share_multiplicators", KnownDatabaseSchemas.Default);
    }
}
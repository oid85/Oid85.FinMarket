using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class AssetFundamentalEntityConfiguration : EntityConfigurationBase<AssetFundamentalEntity>
{
    public override void Configure(EntityTypeBuilder<AssetFundamentalEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("asset_fundamentals", KnownDatabaseSchemas.Default);
    }
}
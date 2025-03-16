using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class FearGreedIndexEntityConfiguration : EntityConfigurationBase<FearGreedIndexEntity>
{
    public override void Configure(EntityTypeBuilder<FearGreedIndexEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("fear_greed_index", KnownDatabaseSchemas.Storage);
    }
}
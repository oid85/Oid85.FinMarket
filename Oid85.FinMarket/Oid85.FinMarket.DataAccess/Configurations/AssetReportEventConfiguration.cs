using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class AssetReportEventEntityConfiguration : EntityConfigurationBase<AssetReportEventEntity>
{
    public override void Configure(EntityTypeBuilder<AssetReportEventEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("asset_report_events", KnownDatabaseSchemas.Default);
        builder.HasIndex(x => x.InstrumentId);
    }
}
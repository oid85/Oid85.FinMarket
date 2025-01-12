using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class ForecastTargetEntityConfiguration : EntityConfigurationBase<ForecastTargetEntity>
{
    public override void Configure(EntityTypeBuilder<ForecastTargetEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("forecast_targets", KnownDatabaseSchemas.Default);
    }
}
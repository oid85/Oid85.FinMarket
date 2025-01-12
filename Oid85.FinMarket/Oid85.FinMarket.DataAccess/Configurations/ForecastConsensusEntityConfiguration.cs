using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class ForecastConsensusEntityConfiguration : EntityConfigurationBase<ForecastConsensusEntity>
{
    public override void Configure(EntityTypeBuilder<ForecastConsensusEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("forecast_consensuses", KnownDatabaseSchemas.Default);
    }
}
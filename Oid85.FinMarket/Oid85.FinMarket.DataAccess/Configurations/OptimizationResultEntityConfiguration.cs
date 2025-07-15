using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class OptimizationResultEntityConfiguration : EntityConfigurationBase<OptimizationResultEntity>
{
    public override void Configure(EntityTypeBuilder<OptimizationResultEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("optimization_results", KnownDatabaseSchemas.Default);
        
        builder
            .Property(x => x.StrategyParams)
            .HasColumnType("jsonb");
    }
}
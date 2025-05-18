using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Common.KnownConstants;
using Oid85.FinMarket.DataAccess.Entities;

namespace Oid85.FinMarket.DataAccess.Configurations;

internal class BacktestResultEntityConfiguration : EntityConfigurationBase<BacktestResultEntity>
{
    public override void Configure(EntityTypeBuilder<BacktestResultEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("backtest_results", KnownDatabaseSchemas.Storage);
        
        builder
            .Property(x => x.StrategyParams)
            .HasColumnType("jsonb");
    }
}
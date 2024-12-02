using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Logging.DataAccess.Entities;
using Oid85.FinMarket.Logging.KnownConstants;

namespace Oid85.FinMarket.Logging.DataAccess.Configurations;

internal class LogRecordEntityConfiguration : EntityConfigurationBase<LogRecordEntity>
{
    public override void Configure(EntityTypeBuilder<LogRecordEntity> builder)
    {
        base.Configure(builder);
        
        builder.ToTable("logs", KnownDatabaseSchemas.Log);
    }
}
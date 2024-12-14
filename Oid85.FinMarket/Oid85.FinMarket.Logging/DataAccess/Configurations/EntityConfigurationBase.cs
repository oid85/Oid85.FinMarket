using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Oid85.FinMarket.Logging.DataAccess.Entities;
using Oid85.FinMarket.Logging.DataAccess.Schemas;

namespace Oid85.FinMarket.Logging.DataAccess.Configurations;

internal abstract class EntityConfigurationBase<TEntity> : IEntityTypeConfiguration<TEntity>, ILogSchema 
    where TEntity : LogRecordEntity
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");
    }
}